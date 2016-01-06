using Foundation;
using UIKit;

namespace MTExample5_6 {
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate {
		// class-level declarations
		UIWindow window;
		MTExample5_6ViewController viewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new MTExample5_6ViewController ();
			viewController.View.AddSubview (new ChartCanvas (window.Bounds));
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}

