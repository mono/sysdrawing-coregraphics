
using System;
using System.Collections.Generic;
using System.Linq;
using System.DrawingNative;
using System.DrawingNative.Drawing2D;
using System.DrawingNative.Text;

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

		public DrawingView (System.Drawing.RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion
		#region Form interface
		public System.Drawing.Rectangle ClientRectangle 
		{
			get {
				return new System.Drawing.Rectangle((int)Bounds.X,
				                     (int)Bounds.Y,
				                     (int)Bounds.Width,
				                     (int)Bounds.Height);
			}
		}

		System.DrawingNative.Color backColor = System.DrawingNative.Color.White;
		public System.DrawingNative.Color BackColor 
		{
			get {
				return backColor;
			}
			
			set {
				backColor = value;
			}
		}

		System.DrawingNative.Font font;
		public System.DrawingNative.Font Font
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


		System.DrawingNative.Font anyKeyFont = new System.DrawingNative.Font("Chalkduster", 18, FontStyle.Bold);
		System.DrawingNative.Font clipFont = new System.DrawingNative.Font(System.DrawingNative.FontFamily.GenericSansSerif,12, System.DrawingNative.FontStyle.Bold);


		int currentView = 0;
		int totalViews = 20;

		public override bool AcceptsFirstResponder ()
		{
			return true;
		}

		public override void KeyDown (NSEvent theEvent)
		{
			currentView++;
			currentView %= totalViews;
			//Console.WriteLine("Current View: {0}", currentView);
			this.NeedsDisplay = true;
		}

		string title = string.Empty;


		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{

			var g = System.DrawingNative.Graphics.FromCurrentContext();

			g.Clear(backColor);
//			System.Drawing.PointF point = new System.Drawing.PointF(50,50);
//			var rect = new Rectangle(50,50,20,20);
//			//g.RotateTransform(-20);
//			//g.TranslateTransform(5,5);
//			var size = g.MeasureString("A", this.Font);
//			rect.Height = (int)size.Height;
//			rect.Width = (int)size.Width*20;
//
//			int nextLine = (int)(size.Height * 1.2);
//
//			g.DrawRectangle(Pens.Red, rect);
//			g.DrawString("Test2 without line feed", this.Font, Brushes.Blue, point);
//
//			StringFormat format = new StringFormat();
//			format.Alignment = StringAlignment.Center;
//
//			rect.Y += nextLine;
//			rect.Height = (int)size.Height;
//			rect.Width = (int)size.Width*20;
//			g.DrawRectangle(Pens.Red, rect);
//			g.DrawString("Test2 Centered", this.Font, Brushes.Blue, rect, format);
//
//			format.Alignment = StringAlignment.Far;
//			rect.Y += nextLine;
//			rect.Height = (int)size.Height;
//			rect.Width = (int)size.Width*20;
//			g.DrawRectangle(Pens.Red, rect);
//			g.DrawString("Test2 Far", this.Font, Brushes.Blue, rect, format);
//
//			format.Alignment = StringAlignment.Far;
//			rect.Y += nextLine;
//			rect.Height = (int)size.Height;
//			rect.Width = (int)size.Width*3;
//			g.DrawRectangle(Pens.Red, rect);
//			g.DrawString("Test2 Far", this.Font, Brushes.Blue, rect, format);
//
			switch (currentView) 
			{
			case 0:
				//DrawStringPointF (g);
				break;
			case 1:
				DrawStringRectangleF (g);
				break;
			case 2:
				DrawStringPointFFormat (g);
				break;
			case 3:
				DrawStringRectangleFFormat (g);
				break;
			case 4:
				DrawStringFloat (g);
				break;
			case 5:
				DrawStringFloatFormat (g);
				break;
			case 6:
				DrawStringRectangleFFormat1 (g);
				break;
			case 7:
				DrawStringRectangleFFormat2 (g);
				break;
			case 8:
				DrawStringRectangleFFormat3 (g);
				break;
			case 9:
				DrawStringRectangleFFormat4 (g);
				break;
			case 10:
				DrawStringRectangleFFormat5 (g);
				break;
			case 11:
				DrawStringPointFFormat1 (g);
				break;
			case 12:
				DrawStringPointFFormat2(g);
				break;
			case 13:
				DrawStringPointFFormat3 (g);
				break;

			}

			g.ResetTransform ();
			System.DrawingNative.Brush sBrush = System.DrawingNative.Brushes.Black;

			if (!g.IsClipEmpty) 
			{
			var clipPoint = System.Drawing.PointF.Empty;
			var clipString = string.Format("Clip-{0}", g.ClipBounds);
			g.ResetClip ();
			var clipSize = g.MeasureString(clipString, clipFont);
			clipPoint.X = (ClientRectangle.Width / 2) - (clipSize.Width / 2);
			clipPoint.Y = 5;
			g.DrawString(clipString, clipFont, sBrush, clipPoint );

			}

			var anyKeyPoint = System.Drawing.PointF.Empty;
			var anyKey = "Press any key to continue.";
			var anyKeySize = g.MeasureString(anyKey, anyKeyFont);
			anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
			anyKeyPoint.Y = ClientRectangle.Height - (anyKeySize.Height + 10);
			g.DrawString(anyKey, anyKeyFont, sBrush, anyKeyPoint );

			anyKeySize = g.MeasureString(title, anyKeyFont);
			anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
			anyKeyPoint.Y -= anyKeySize.Height;
			g.DrawString(title, anyKeyFont, sBrush, anyKeyPoint );

			
			g.Dispose();
		}


		public void PointF(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			System.DrawingNative.Font drawFont = new System.DrawingNative.Font("Arial", 16);
			System.DrawingNative.SolidBrush drawBrush = new System.DrawingNative.SolidBrush(Color.Black);

			// Create point for upper-left corner of drawing.
			System.Drawing.PointF drawPoint = new System.Drawing.PointF(150.0F, 150.0F);

			var outline = g.MeasureString (drawString, drawFont);

			g.DrawRectangle (System.DrawingNative.Pens.Blue, new System.Drawing.RectangleF (drawPoint, outline));

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawPoint);

			title = "DrawStringSystem.Drawing.PointF";
		}

		public void DrawStringRectangleF(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			Font drawFont = new Font("Arial", 16);
			SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create rectangle for drawing. 
			float x = 150.0F;
			float y = 150.0F;
			float width = 200.0F;
			float height = 50.0F;
			System.Drawing.RectangleF drawRect = new System.Drawing.RectangleF(x, y, width, height);

			// Draw rectangle to screen.
			System.DrawingNative.Pen blackPen = new System.DrawingNative.Pen(Color.Black);
			g.DrawRectangle(blackPen, x, y, width, height);

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawRect);

			title = "DrawStringRectangleF";
		}

		public void DrawStringPointFFormat(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample\n Text";

			// Create font and brush.
			System.DrawingNative.Font drawFont = new Font("Arial", 16);
			System.DrawingNative.SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create point for upper-left corner of drawing.
			System.Drawing.PointF drawPoint = new System.Drawing.PointF(150.0F, 50.0F);

			// Set format of string.
			System.DrawingNative.StringFormat drawFormat = new System.DrawingNative.StringFormat();
			drawFormat.FormatFlags = System.DrawingNative.StringFormatFlags.DirectionVertical;
			g.FillRectangle(System.DrawingNative.Brushes.Red, new System.Drawing.RectangleF(drawPoint.X - 1, drawPoint.Y - 1, 2, 2));

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawPoint, drawFormat);

			title = "DrawStringSystem.Drawing.PointFFormat";
		}

		public void DrawStringRectangleFFormat(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			System.DrawingNative.Font drawFont = new System.DrawingNative.Font("Arial", 16);
			System.DrawingNative.SolidBrush drawBrush = new System.DrawingNative.SolidBrush(Color.Black);

			// Create rectangle for drawing. 
			float x = 150.0F;
			float y = 150.0F;
			float width = 200.0F;
			float height = 50.0F;
			System.Drawing.RectangleF drawRect = new System.Drawing.RectangleF(x, y, width, height);

			// Draw rectangle to screen.
			Pen blackPen = new Pen(Color.Black);
			g.DrawRectangle(blackPen, x, y, width, height);

			// Set format of string.
			StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = StringAlignment.Center;

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);

			title = "DrawStringRectangleFFormat";
		}

		public void DrawStringFloat(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			System.DrawingNative.Font drawFont = new Font("Arial", 16);
			System.DrawingNative.SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create point for upper-left corner of drawing. 
			float x = 150.0F;
			float y = 150.0F;

			var outline = g.MeasureString (drawString, drawFont);

			g.DrawRectangle (Pens.Blue, new System.Drawing.RectangleF (x,y, outline.Width, outline.Height));

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, x, y);

			title = "DrawStringFloat";
		}

		public void DrawStringFloatFormat(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			System.DrawingNative.Font drawFont = new Font("Arial", 16);
			System.DrawingNative.SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create point for upper-left corner of drawing. 
			float x = 150.0F;
			float y =  50.0F;
			g.FillRectangle(Brushes.Red, new System.Drawing.RectangleF(x - 1, y - 1, 2, 2));
			// Set format of string.
			System.DrawingNative.StringFormat drawFormat = new StringFormat();
			drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

			title = "DrawStringFloatFormat";

		}


		public void DrawStringRectangleFFormat1(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			System.DrawingNative.Font drawFont = new Font("Arial", 16);
			SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create rectangle for drawing. 
			float x = 150.0F;
			float y = 150.0F;
			float width = 200.0F;
			float height = 50.0F;
			System.Drawing.RectangleF drawRect = new System.Drawing.RectangleF(x, y, width, height);

			// Draw rectangle to screen.
			System.DrawingNative.Pen blackPen = new Pen(Color.Black);
			g.DrawRectangle(blackPen, x, y, width, height);

			// Set format of string.
			System.DrawingNative.StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = StringAlignment.Center;
			drawFormat.LineAlignment = StringAlignment.Center;

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);

			title = "DrawStringRectangleFFormat1";
		}

		public void DrawStringRectangleFFormat2(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			System.DrawingNative.Font drawFont = new Font("Arial", 16);
			SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create rectangle for drawing. 
			float x = 150.0F;
			float y = 150.0F;
			float width = 200.0F;
			float height = 50.0F;
			System.Drawing.RectangleF drawRect = new System.Drawing.RectangleF(x, y, width, height);

			// Draw rectangle to screen.
			Pen blackPen = new Pen(Color.Black);
			g.DrawRectangle(blackPen, x, y, width, height);

			// Set format of string.
			StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = StringAlignment.Center;
			drawFormat.LineAlignment = StringAlignment.Far;

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);

			title = "DrawStringRectangleFFormat2";
		}


		public void DrawStringRectangleFFormat3(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			System.DrawingNative.Font drawFont = new Font("Arial", 16);
			System.DrawingNative.SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create rectangle for drawing. 
			float x = 150.0F;
			float y = 150.0F;
			float width = 200.0F;
			float height = 50.0F;
			System.Drawing.RectangleF drawRect = new System.Drawing.RectangleF(x, y, width, height);

			// Draw rectangle to screen.
			Pen blackPen = new Pen(Color.Black);
			g.DrawRectangle(blackPen, x, y, width, height);

			// Set format of string.
			StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = StringAlignment.Far;
			//drawFormat.LineAlignment = StringAlignment.Far;

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);

			title = "DrawStringRectangleFFormat3";
		}


		public void DrawStringRectangleFFormat4(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			System.DrawingNative.Font drawFont = new Font("Arial", 16);
			System.DrawingNative.SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create rectangle for drawing. 
			float x = 150.0F;
			float y = 150.0F;
			float width = 200.0F;
			float height = 50.0F;
			System.Drawing.RectangleF drawRect = new System.Drawing.RectangleF(x, y, width, height);

			// Draw rectangle to screen.
			Pen blackPen = new Pen(Color.Black);
			g.DrawRectangle(blackPen, x, y, width, height);

			// Set format of string.
			StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = StringAlignment.Far;
			drawFormat.LineAlignment = StringAlignment.Center;

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);

			title = "DrawStringRectangleFFormat4";
		}


		public void DrawStringRectangleFFormat5(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample Text";

			// Create font and brush.
			Font drawFont = new Font("Arial", 16);
			SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create rectangle for drawing. 
			float x = 150.0F;
			float y = 150.0F;
			float width = 200.0F;
			float height = 50.0F;
			System.Drawing.RectangleF drawRect = new System.Drawing.RectangleF(x, y, width, height);

			// Draw rectangle to screen.
			Pen blackPen = new Pen(Color.Black);
			g.DrawRectangle(blackPen, x, y, width, height);

			// Set format of string.
			StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = StringAlignment.Far;
			drawFormat.LineAlignment = StringAlignment.Far;

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);

			title = "DrawStringRectangleFFormat5";
		}

		public void DrawStringPointFFormat1(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample\n Text";

			// Create font and brush.
			Font drawFont = new Font("Arial", 16);
			SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create point for upper-left corner of drawing.
			System.Drawing.PointF drawPoint = new System.Drawing.PointF(150.0F, 50.0F);

			// Set format of string.
			StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = StringAlignment.Center;
			g.FillRectangle(Brushes.Red, new System.Drawing.RectangleF(drawPoint.X - 1, drawPoint.Y - 1, 2, 2));

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawPoint, drawFormat);

			title = "DrawStringSystem.Drawing.PointFFormat1";
		}


		public void DrawStringPointFFormat2(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample\n Text";

			// Create font and brush.
			Font drawFont = new Font("Arial", 16);
			SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create point for upper-left corner of drawing.
			System.Drawing.PointF drawPoint = new System.Drawing.PointF(150.0F, 50.0F);

			// Set format of string.
			StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = StringAlignment.Far;
			g.FillRectangle(Brushes.Red, new System.Drawing.RectangleF(drawPoint.X - 1, drawPoint.Y - 1, 2, 2));

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawPoint, drawFormat);

			title = "DrawStringSystem.Drawing.PointFFormat2";
		}


		public void DrawStringPointFFormat3(System.DrawingNative.Graphics g)
		{

			// Create string to draw.
			String drawString = "Sample\n Text";

			// Create font and brush.
			Font drawFont = new Font("Arial", 16);
			SolidBrush drawBrush = new SolidBrush(Color.Black);

			// Create point for upper-left corner of drawing.
			System.Drawing.PointF drawPoint = new System.Drawing.PointF(150.0F, 50.0F);

			// Set format of string.
			StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = StringAlignment.Near;
			drawFormat.LineAlignment = StringAlignment.Far;
			g.FillRectangle(Brushes.Red, new System.Drawing.RectangleF(drawPoint.X - 1, drawPoint.Y - 1, 2, 2));

			// Draw string to screen.
			g.DrawString(drawString, drawFont, drawBrush, drawPoint, drawFormat);

			title = "DrawStringSystem.Drawing.PointFFormat3";
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
	System.Drawing.Rectangle ClientRectangle { get; }
	
	System.DrawingNative.Color BackColor { get; set; } 
	
	System.DrawingNative.Font Font { get; set; }
}