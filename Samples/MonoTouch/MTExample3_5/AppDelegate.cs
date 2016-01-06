using Foundation;
using UIKit;

namespace MTExample3_5
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		MTExample3_5ViewController viewController;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			viewController = new MTExample3_5ViewController ();
			viewController.View.AddSubview (new DrawingView (window.Bounds));
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

