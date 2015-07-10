using System;
using Xamarin.Forms;

namespace InventorySales
{
	public class SalesPage : ContentPage
	{
		public SalesPage ()
		{
			Title = "Sales";
			Padding = new Thickness (10, Device.OnPlatform (20, 0, 0), 10, 5);
			Content = new StackLayout {
				Children = {
					new Label {
						XAlign = TextAlignment.Center,
						Text = "Sell stuff!"
					},
					new Entry {
						Placeholder = "Employee"
					},
					new Entry {
						Placeholder = "Location"
					},
					new Entry {
						Placeholder = "Quantity"
					},
					new Entry {
						Placeholder = "SKU"
					},
					new Entry {
						Placeholder = "Price"
					},
					new Button {
						Text = "Sell"	
					},
					new Label {
						XAlign = TextAlignment.Center,
						Text = "Sales per employee"
					},
					new ListView {
						ItemTemplate = new DataTemplate (typeof(DetailCell)),
						ItemsSource = new TextCell[] { // todo iterate per employee with totals
							new TextCell {
								Text = "jchris",
								Detail = "$202.44",
							},
							new TextCell {
								Text = "zack",
								Detail = "$3207.50",
							},
							new TextCell {
								Text = "jim",
								Detail = "$1100.01",
							}
						}
					}
				}
			};
		}
	}
}

