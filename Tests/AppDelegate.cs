using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.NUnit.UI;

namespace Tests
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		TouchRunner runner;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			runner = new TouchRunner (window);

			runner.Add (System.Reflection.Assembly.GetExecutingAssembly ());

			window.RootViewController = new UINavigationController (runner.GetViewController ());
			
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

