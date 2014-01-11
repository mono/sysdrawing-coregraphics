using System;
using System.DrawingNative;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;

namespace Example4_1
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
			RectangleF bounds = mainWindowController.Window.ContentView.Bounds;
			bounds.Inflate(-50,-50);
			//mainWindowController.Window.ContentView.AddSubview(new ChartCanvas(mainWindowController.Window.ContentView.Bounds));
			mainWindowController.Window.ContentView.AddSubview(new ChartCanvas(bounds));
		}
		
		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}
}

