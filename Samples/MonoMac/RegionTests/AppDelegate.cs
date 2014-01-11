using System;
using System.DrawingNative;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace RegionTests
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		MainWindowController mainWindowController;

		public AppDelegate ()
		{
		}

		public override void FinishedLaunching (NSObject notification)
		{
			mainWindowController = new MainWindowController ();
			mainWindowController.Window.MakeKeyAndOrderFront (this);
			mainWindowController.Window.ContentView.AddSubview(new DrawingView(mainWindowController.Window.ContentView.Bounds));

		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}
}

