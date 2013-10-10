
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using MonoMac.CoreText;
using System.Drawing;

namespace Example5_5
{
	public partial class ChartCanvas : MonoMac.AppKit.NSView
	{

		public PlotPanel panel1;

		#region Constructors
		
		// Called when created from unmanaged code
		public ChartCanvas (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public ChartCanvas (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
			BackColor = Color.Wheat;

			var panelRect = new RectangleF(Frame.X,Frame.Y,Frame.Width,Frame.Height);
			panelRect.Inflate(-20,-20);
			panel1 = new PlotPanel(panelRect);
			panel1.BackColor = Color.AliceBlue;

			this.AddSubview (panel1);

			// Subscribing to a paint eventhandler to drawingPanel: 
			panel1.Paint +=
				new PaintEventHandler (PlotPanelPaint);
			
		}
		
		public ChartCanvas (RectangleF rect) : base (rect)
		{
			Initialize ();
		}
		
#endregion
		#region Form interface
		public Rectangle ClientRectangle {
			get {
				return new Rectangle ((int)Bounds.X,
				                     (int)Bounds.Y,
				                     (int)Bounds.Width,
				                     (int)Bounds.Height);
			}
		}

		Color backColor = Color.White;

		public Color BackColor {
			get {
				return backColor;
			}
			
			set {
				backColor = value;
			}
		}

		Font font;

		public Font Font {
			get {
				if (font == null)
					font = new Font ("Arial", 12);
				return font;
			}
			set {
				font = value;
			}
		}
		#endregion

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			var g = Graphics.FromCurrentContext() ();
			g.Clear (backColor);
		}
		
		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			var borderPen = new Pen(Brushes.Black,1);
			g.DrawRectangle(borderPen,panel1.Bounds);
			borderPen.Dispose();

			float a = panel1.Height / 4;
			DrawCylinder dc = new DrawCylinder(this, a, 2 * a);
			dc.DrawIsometricView(g);

		}
		


		// Here we make sure we are flipped so our subview PlotPanel size and location
		// are calculated correctly.  If not the positions are calculated on the 0,0 in 
		// the lower left corner instead of upper left.
		public override bool IsFlipped {
			get {
				//return base.IsFlipped;
				return true;
			}
		}

	}
}

