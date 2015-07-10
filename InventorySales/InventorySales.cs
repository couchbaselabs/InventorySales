using System;

using Xamarin.Forms;

namespace InventorySales
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new TabbedPage {
				Children = {
					new IntakePage {

					},
					new SalesPage {
					}
				}
			};
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}

	// helper junk for the real time chart placeholder
	class DetailCell : ViewCell
	{
		public DetailCell()
		{
			View = CreateNameLayout();
		}

		static StackLayout CreateNameLayout()
		{
			var nameLabel = new Label
			{
				HorizontalOptions= LayoutOptions.FillAndExpand,
				TextColor = Color.Blue
			};
			nameLabel.SetBinding(Label.TextProperty, "Text");

			var totalLabel = new Label
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			totalLabel.SetBinding(Label.TextProperty, "Detail");

			var nameLayout = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Orientation = StackOrientation.Horizontal,
				Children = { nameLabel, totalLabel }
			};
			return nameLayout;
		}
	}

}

