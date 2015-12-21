using System;
using CoreGraphics;
using Foundation;
using AppKit;
using ObjCRuntime;

namespace ClippingTests
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		MainWindowController mainWindowController;

		public AppDelegate ()
		{
		}

		public override void DidFinishLaunching (NSNotification notification)
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

