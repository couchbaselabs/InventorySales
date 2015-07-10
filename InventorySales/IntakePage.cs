using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace InventorySales
{
	public class IntakePage : ContentPage
	{
		Entry employee;
		Entry location;
		Entry quantity;
		Entry sku;
		Button scan;

		public IntakePage ()
		{
			Title = "Intake";
			Padding = new Thickness (10, Device.OnPlatform (20, 0, 0), 10, 5);

			setupForm ();

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
					new ListView {
						ItemTemplate = new DataTemplate (typeof(DetailCell)),
						ItemsSource = new TextCell[] { // todo iterate per employee with totals
							new TextCell {
								Text = "jchris",
								Detail = "5",
							},
							new TextCell {
								Text = "zack",
								Detail = "2",
							},
							new TextCell {
								Text = "jim",
								Detail = "11",
							}
						}
					}
				}
			};
		}

		void setupCouchbase () {
			
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
				Text = "Scan"	
			};
			scan.Clicked += OnButtonClicked;
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			var d = new Dictionary<string, object>
			{
				{"sku", sku.Text},
				{"employee", employee.Text},
				{"location", location.Text},
				{"quantity", quantity.Text}
			};

			
		}
	}
}

