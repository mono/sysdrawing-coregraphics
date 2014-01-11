using System;
using System.DrawingNative;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace Example4_2
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
			//RectangleF bounds = mainWindowController.Window.ContentView.Bounds;
			//bounds.Inflate(-50,-50);
			mainWindowController.Window.ContentView.AddSubview(new ChartCanvas(mainWindowController.Window.ContentView.Bounds));
			//mainWindowController.Window.ContentView.AddSubview(new ChartCanvas(bounds));
		}
		
		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}

	}
}

