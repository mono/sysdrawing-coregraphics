using Foundation;
using UIKit;

namespace MTFontTest {
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate {
		UIWindow window;
		MTFontTestViewController viewController;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new MTFontTestViewController ();
			viewController.View.AddSubview (new DrawingView (window.Bounds));
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}

