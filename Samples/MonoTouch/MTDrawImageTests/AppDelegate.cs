using Foundation;
using UIKit;

namespace MTDrawImageTests {
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate {
		UIWindow window;
		MTDrawImageTestsViewController viewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			viewController = new MTDrawImageTestsViewController ();
			viewController.View.AddSubview (new DrawingView (window.Bounds));
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

