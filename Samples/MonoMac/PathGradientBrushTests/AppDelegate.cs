using System;
using System.DrawingNative;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace PathGradientBrushTests
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
			var drawingView = new DrawingView(mainWindowController.Window.ContentView.Bounds);

			mainWindowController.Window.ContentView.AddSubview(drawingView);
			mainWindowController.Window.MakeKeyAndOrderFront (this);
			mainWindowController.Window.MakeFirstResponder(drawingView);

		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}
}

