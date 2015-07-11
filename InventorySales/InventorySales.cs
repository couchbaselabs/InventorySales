using System;
using Couchbase.Lite;
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
			setupSync();
		}

		void setupSync() {
			var newRemoteUrl = new Uri ("http://localhost:4984/retail");
			Database db = Manager.SharedInstance.GetDatabase ("inventory");
			var push = db.CreatePushReplication (newRemoteUrl);
			push.Continuous = true;
			push.Start();
			var pull = db.CreatePullReplication (newRemoteUrl);
			pull.Continuous = true;
			string[] channels = { "sale", "intake" };
			pull.Channels = channels;
			pull.Start();
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
			nameLabel.SetBinding(Label.TextProperty, "Key");

			var totalLabel = new Label
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			totalLabel.SetBinding(Label.TextProperty, "Value");

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

