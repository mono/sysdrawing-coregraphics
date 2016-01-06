using Foundation;
using UIKit;

namespace MTExample5_5 {
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate {
		UIWindow window;
		MTExample5_5ViewController viewController;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			viewController = new MTExample5_5ViewController ();
			viewController.View.AddSubview (new ChartCanvas (window.Bounds));
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

