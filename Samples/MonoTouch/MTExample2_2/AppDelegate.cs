using Foundation;
using UIKit;

namespace MTExample2_2 {
	[Register ("AppDelegate")]
	public  class AppDelegate : UIApplicationDelegate {
		UIWindow window;
		MTExample2_2ViewController viewController;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new MTExample2_2ViewController ();
			viewController.View.AddSubview (new DrawingView (window.Bounds));
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}

