using Foundation;
using UIKit;

namespace MTBitmapTests {
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate {
		UIWindow window;
		MTBitmapTestsViewController viewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow(UIScreen.MainScreen.Bounds);

			viewController = new MTBitmapTestsViewController ();
			viewController.View.AddSubview (new DrawingView(window.Bounds));
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}

