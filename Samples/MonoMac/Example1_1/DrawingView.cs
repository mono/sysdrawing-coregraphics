
using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using CoreGraphics;
using System.Drawing;

namespace sampleMac
{
	public partial class DrawingView : AppKit.NSView
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
		}
		
		#endregion

		public override void DrawRect (CGRect dirtyRect)
		{
			var g = Graphics.FromCurrentContext();

			// NSView does not have a background color so we just use Clear to white here
			g.Clear(Color.White);

			//RectangleF ClientRectangle = this.Bounds;
			CGRect ClientRectangle = dirtyRect;

			// Following codes draw a line from (0, 0) to (1, 1) in unit of inch:
			/*g.PageUnit = GraphicsUnit.Inch;
			Pen blackPen = new Pen(Color.Black, 1/g.DpiX);
			g.DrawLine(blackPen, 0, 0, 1, 1);*/
			
			// Following code shifts the origin to the center of 
			// client area, and then draw a line from (0,0) to (1, 1) inch: 
			g.PageUnit = GraphicsUnit.Inch;
			g.TranslateTransform(((float)ClientRectangle.Width / g.DpiX) / 2,
				((float)ClientRectangle.Height / g.DpiY) / 2);
			Pen greenPen = new Pen(Color.Green, 1 /  g.DpiX);
			g.DrawLine(greenPen, 0, 0, 1, 1);

			g.Dispose ();
		}

//		public override bool IsFlipped {
//			get {
//				//return base.IsFlipped;
//				return false;
//			}
//		}
	}
}

