using Foundation;
using UIKit;

namespace MTExample4_7 {
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate {
		UIWindow window;
		MTExample4_7ViewController viewController;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new MTExample4_7ViewController ();
			viewController.View.AddSubview (new ChartCanvas (window.Bounds));
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}

