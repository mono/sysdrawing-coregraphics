using Foundation;
using UIKit;

namespace MTGraphicsPathTests {
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate {
		UIWindow window;
		MTGraphicsPathTestsViewController viewController;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new MTGraphicsPathTestsViewController ();
			viewController.View.AddSubview (new DrawingView (window.Bounds));
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}

