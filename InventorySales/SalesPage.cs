using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Couchbase.Lite;

namespace InventorySales
{
	public class SalesPage : ContentPage
	{
		Entry employee;
		Entry location;
		Entry quantity;
		Entry sku;
		Entry price;
		Button sell;
		Database db;
		Couchbase.Lite.View viewSold;
		ListView salesActivity;

		public SalesPage ()
		{
			Title = "Sales";
			Padding = new Thickness (10, Device.OnPlatform (20, 0, 0), 10, 5);

			setupCouchbase ();
			setupForm ();
			setupActivity ();

			Content = new StackLayout {
				Children = {
					new Label {
						XAlign = TextAlignment.Center,
						Text = "Sell stuff!"
					},
					employee,
					location,
					quantity,
					sku,
					price,
					sell,
					new Label {
						XAlign = TextAlignment.Center,
						Text = "Sales per employee"
					},
					salesActivity
				}
			};
		}

		void setupCouchbase () {
			db = Manager.SharedInstance.GetDatabase ("inventory");
			setupSoldView ();
			setupViewWatcher ();

		}

		void setupViewWatcher () {
			var query = viewSold.CreateQuery().ToLiveQuery();
			query.GroupLevel = 1;
			query.Changed += (sender, e) => {
				salesActivity.ItemsSource = query.Rows;
			};
			query.Start();
		}

		void setupSoldView(){
			viewSold = db.GetView ("sold-by-employee");

			var mapBlock = new MapDelegate ((doc, emit) => 
				{
					object type;
					doc.TryGetValue ("type", out type);
					if (type != null) {
						string t = type.ToString ();
						if (t == "sale") {
							object name;
							doc.TryGetValue ("employee", out name);

							object quantity;
							doc.TryGetValue ("quantity", out quantity);
							string q = quantity.ToString ();
							int quant;
							var ok = Int32.TryParse(q, out quant);

							object price;
							doc.TryGetValue ("price", out price);
							string ps = price.ToString ();
							Single pnum;
							var ok2 = Single.TryParse(ps, out pnum);

							if (ok && ok2 && name != null)
								emit (name, quant * pnum);
						}	
					}
				});

			viewSold.SetMapReduce (mapBlock, Couchbase.Lite.Views.BuiltinReduceFunctions.Sum, "1.3");
		}

		void setupActivity() {
			salesActivity = new ListView {
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
			price = new Entry {
				Placeholder = "Price"
			};
			sell = new Button {
				Text = "Sell!"	
			};
			sell.Clicked += SellClicked;
		}

		void SellClicked(object sender, EventArgs e)
		{
			var sold = new Dictionary<string, object>
			{
				{"sku", sku.Text},
				{"type", "sale"},
				{"employee", employee.Text},
				{"location", location.Text},
				{"price", price.Text},
				{"quantity", quantity.Text}
			};
			Document doc = db.CreateDocument ();
			doc.PutProperties (sold);
		}
	}
}

