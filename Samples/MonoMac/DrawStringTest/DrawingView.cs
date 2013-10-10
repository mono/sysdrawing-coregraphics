
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using MonoMac.CoreText;

namespace DrawStringTest
{
	public partial class DrawingView : MonoMac.AppKit.NSView, Form
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
			BackColor = Color.Wheat;
			// Set Form1 size:
			//			this.Width = 350;
			//			this.Height = 300;

		}

		public DrawingView (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion
		#region Form interface
		public Rectangle ClientRectangle 
		{
			get {
				return new Rectangle((int)Bounds.X,
				                     (int)Bounds.Y,
				                     (int)Bounds.Width,
				                     (int)Bounds.Height);
			}
		}

		Color backColor = Color.White;
		public Color BackColor 
		{
			get {
				return backColor;
			}
			
			set {
				backColor = value;
			}
		}

		Font font;
		public Font Font
		{
			get {
				if (font == null)
					font = new Font("Lucida Grande",20, FontStyle.Italic | FontStyle.Bold | FontStyle.Underline);
				return font;
			}
			set 
			{
				font = value;
			}
		}
		#endregion

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{

			var g = Graphics.FromCurrentContext();

			//g.Clear(backColor);
			PointF point = new PointF(50,50);
			var rect = new Rectangle(50,50,20,20);
			//g.RotateTransform(-20);
			//g.TranslateTransform(5,5);
			var size = g.MeasureString("A", this.Font);
			rect.Height = (int)size.Height;
			rect.Width = (int)size.Width*20;

			int nextLine = (int)(size.Height * 1.2);

			g.DrawRectangle(Pens.Red, rect);
			g.DrawString("Test2 without line feed", this.Font, Brushes.Blue, point);

			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;

			rect.Y += nextLine;
			rect.Height = (int)size.Height;
			rect.Width = (int)size.Width*20;
			g.DrawRectangle(Pens.Red, rect);
			g.DrawString("Test2 Centered", this.Font, Brushes.Blue, rect, format);

			format.Alignment = StringAlignment.Far;
			rect.Y += nextLine;
			rect.Height = (int)size.Height;
			rect.Width = (int)size.Width*20;
			g.DrawRectangle(Pens.Red, rect);
			g.DrawString("Test2 Far", this.Font, Brushes.Blue, rect, format);

			format.Alignment = StringAlignment.Far;
			rect.Y += nextLine;
			rect.Height = (int)size.Height;
			rect.Width = (int)size.Width*3;
			g.DrawRectangle(Pens.Red, rect);
			g.DrawString("Test2 Far", this.Font, Brushes.Blue, rect, format);

			g.Dispose();
		}

//				public override bool IsFlipped {
//					get {
//						//return base.IsFlipped;
//						return true;
//					}
//				}
	}


}

public interface Form
{
	Rectangle ClientRectangle { get; }
	
	Color BackColor { get; set; } 
	
	Font Font { get; set; }
}