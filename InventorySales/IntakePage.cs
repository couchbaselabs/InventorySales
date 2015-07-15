using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Couchbase.Lite;

namespace InventorySales
{
	public class IntakePage : ContentPage
	{
		Entry employee;
		Entry location;
		Entry quantity;
		Entry sku;
		Button scan;
		Database db;
		Couchbase.Lite.View viewScanned;
		ListView scanActivity;

		public IntakePage ()
		{
			Title = "Intake";
			Padding = new Thickness (10, Device.OnPlatform (20, 0, 0), 10, 5);

			setupCouchbase ();
			setupForm ();
			setupActivity ();

			Content = new StackLayout {
				Children = {
					new Label {
						XAlign = TextAlignment.Center,
						Text = "Scan Inventory Arrivals"
					},
					employee,
					location,
					quantity,
					sku,
					scan,
					new Label {
						XAlign = TextAlignment.Center,
						Text = "Employee Scan Activity"
					},
					scanActivity
				}
			};
		}

		void setupCouchbase () {
			db = Manager.SharedInstance.GetDatabase ("inventory");
			setupScannedView ();
			setupViewWatcher ();

		}

		void setupViewWatcher () {
			var query = viewScanned.CreateQuery().ToLiveQuery();
			query.Descending = true;
			query.GroupLevel = 1;
			query.Changed += (sender, e) => {
				scanActivity.ItemsSource = query.Rows;
			};
			query.Start();
		}

		void setupScannedView(){
			viewScanned = db.GetView ("scanned-by-employee");

			var mapBlock = new MapDelegate ((doc, emit) => 
				{
					object name;
					doc.TryGetValue ("employee", out name);

					object quantity;
					doc.TryGetValue ("quantity", out quantity);
					string q = quantity.ToString ();
					int quant;
					var ok = Int32.TryParse(q, out quant);

					if (ok && name != null)
						emit (name, quant);
				});
					
			viewScanned.SetMapReduce (mapBlock, Couchbase.Lite.Views.BuiltinReduceFunctions.Sum, "1.1");
		}

		void setupActivity() {
			scanActivity = new ListView {
				ItemTemplate = new DataTemplate (typeof(DetailCell)),
				ItemsSource = null
			};
		}

		void setupForm() {
			employee = new Entry {
				Placeholder = "Employee"
			};

			location = new Entry {
				Placeholder = "Location"
			};

			quantity = new Entry {
				Placeholder = "Quantity"
			};

			sku = new Entry {
				Placeholder = "SKU"
			};
			scan = new Button {
				Text = "Scan!"	
			};
			scan.Clicked += OnButtonClicked;
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			var scanned = new Dictionary<string, object>
			{
				{"sku", sku.Text},
				{"type", "intake"},
				{"employee", employee.Text},
				{"location", location.Text},
				{"created_at", DateTime.Now},
				{"quantity", quantity.Text}
			};
			Document doc = db.CreateDocument ();
			doc.PutProperties (scanned);
		}
	}
}

