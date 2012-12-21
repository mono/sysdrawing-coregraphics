
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;

namespace Example1_5a
{
	public partial class DrawingView : MonoMac.AppKit.NSView
	{

		#region Constructors
		
		// Called when created from unmanaged code
		public DrawingView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public DrawingView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
		}

		public DrawingView (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion
		// This is basically the same as Example1_5 except that it uses
		// a page unit of Inches
		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			Graphics g = new Graphics();

			g.Clear(Color.White);

			// Create a pen object:
			Pen aPen = new Pen(Color.Blue, 1 / g.DpiX);
			// Create a brush object with a transparent red color:
			SolidBrush aBrush = new SolidBrush(Color.Red);

			g.PageUnit = GraphicsUnit.Inch;
			g.PageScale = 2;

			// Draw a rectangle:
			g.DrawRectangle(aPen, .20f, .20f, 1.00f, .50f);
			// Draw a filled rectangle:
			g.FillRectangle(aBrush, .20f, .90f, 1.00f, .50f);
			// Draw ellipse:
			g.DrawEllipse(aPen, new RectangleF(.20f, 1.60f, 1.00f, .50f));
			// Draw filled ellipse:
			g.FillEllipse(aBrush, new RectangleF(1.70f, .20f, 1.00f, .50f));
			// Draw arc:
			g.DrawArc(aPen, new RectangleF(1.70f, .90f, 1.00f, .50f), -90, 180);

			// Draw filled pie pieces
			g.FillPie(aBrush, 1.70f, 1.60f, 1.00f, 1.00f, -90, 90);
			g.FillPie(Brushes.Green, 1.70f, 1.60f, 1.00f, 1.00f, -90, -90);

			g.Dispose();
		}
		
		//		public override bool IsFlipped {
		//			get {
		//				//return base.IsFlipped;
		//				return false;
		//			}
		//		}

	}
}

