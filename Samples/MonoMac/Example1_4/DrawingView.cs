
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using Foundation;
using AppKit;
using CoreGraphics;
using CoreText;
using System.Drawing;

namespace Example1_4
{
	public partial class DrawingView : MonoMac.AppKit.NSView
	{

		// Define the drawing area
		private Rectangle drawingRectangle;
		// Unit defined in world coordinate system:
		private float xMin = 4f;
		private float xMax = 6f;
		private float yMin = 3f;
		private float yMax = 6f;
		// Define offset in unit of pixel:
		private int offset = 30;

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
		
		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			// This is here for testing and nothing else
			// Will take this out after I get things working again
//			var context = NSGraphicsContext.CurrentContext.GraphicsPort;
//			RectangleF contextRect = dirtyRect;
//
//			float w, h;
//			w = contextRect.Size.Width;
//			h = contextRect.Size.Height;
//
//			var ctfont = new CTFont("Helvetica",12);
//			var traits = ctfont.SymbolicTraits;
//			var isBold = (traits & CTFontSymbolicTraits.Bold) == CTFontSymbolicTraits.Bold;
//			var isItalic = (traits & CTFontSymbolicTraits.Italic) == CTFontSymbolicTraits.Italic;
//
//			var tMask = CTFontSymbolicTraits.Bold;
//			tMask |= CTFontSymbolicTraits.Italic;
//
//			tMask &= ~CTFontSymbolicTraits.Bold;
//			//tMask &= ~CTFontSymbolicTraits.Italic;
//
//			ctfont = ctfont.WithSymbolicTraits(h/20,tMask,tMask);
//
//			traits = ctfont.SymbolicTraits;
//			isBold = (traits & CTFontSymbolicTraits.Bold) == CTFontSymbolicTraits.Bold;
//			isItalic = (traits & CTFontSymbolicTraits.Italic) == CTFontSymbolicTraits.Italic;
//
//
//			context.SaveState();
//			context.SelectFont (
//				ctfont.PostScriptName,
//				ctfont.Size,
//				CGTextEncoding.MacRoman);
//			context.SetCharacterSpacing(10); // 4
//			context.SetTextDrawingMode(CGTextDrawingMode.FillStroke); // 5
//			
//			
//			var viewMatrix = new CGAffineTransform(1,0,0,-1,0,h);
//			//			var scmat = CGAffineTransform.MakeScale(2f,2f);
//			//			scmat.Multiply(viewMatrix);
//			
//			var modelMatrix = CGAffineTransform.MakeIdentity();
//			var tmat = CGAffineTransform.MakeTranslation(w/2, h / 2);
//			tmat.Multiply(modelMatrix);
//			modelMatrix = tmat;
//			var rotMat = CGAffineTransform.MakeRotation(-90 * (float)Math.PI / 180);
//			rotMat.Multiply(modelMatrix);
//			modelMatrix = rotMat;
//			
//			var modelView = CGAffineTransform.Multiply(modelMatrix, viewMatrix);
//			
//			context.ConcatCTM(modelView);
//			
//			context.SetFillColor(0f, 1f, 0f, .5f); 
//			context.SetStrokeColor(0f, 0f, 1f, 1f); 
//			context.TextMatrix = CGAffineTransform.MakeScale(1f,-1f); // 9
//			context.ShowTextAtPoint(0, 0, "Quartz 2D", 9); 
//			context.RestoreState();
//			context.Dispose();
			// End of testing ---------------------



			Graphics g = Graphics.FromCurrentContext();
			// Create a pen object:
			Pen aPen = new Pen(Color.Blue, 4);

			// Set line caps and dash style:
			aPen.StartCap = LineCap.Flat;
			aPen.EndCap = LineCap.ArrowAnchor;
			aPen.DashStyle = DashStyle.Dot;
			aPen.DashOffset = 50;

			//draw straight line:
			g.DrawLine(aPen, 50, 30, 200, 30);
			// define point array to draw a curve:
			Point point1 = new Point(50, 200);
			Point point2 = new Point(100, 75);
			Point point3 = new Point(150, 60);
			Point point4 = new Point(200, 160);
			Point point5 = new Point(250, 250);
			Point[] Points ={ point1, point2, point3, point4, point5};
			g.DrawCurve(aPen, Points);
			
			aPen.Dispose();
			//g.Dispose();
			
			//Text rotation:
			RectangleF ClientRectangle = dirtyRect;
			string s = "A simple text string";

			Rectangle rect = new Rectangle((int)ClientRectangle.X, (int)ClientRectangle.Y, 
			                               (int)ClientRectangle.Width, (int)ClientRectangle.Height);
			drawingRectangle = new Rectangle(rect.Location, rect.Size);
			Size sz = new Size(rect.Width, rect.Height);
			var font = new Font("Arial",20.0f,FontStyle.Italic | FontStyle.Bold | FontStyle.Underline);
			SizeF stringSize = g.MeasureString(s, font);
			Point Middle = new Point(sz.Width / 30,
			                        sz.Height / 2 - (int)stringSize.Height / 2);
			g.DrawLine(Pens.Black, new Point(0, rect.Height/2), new Point(rect.Width, rect.Height/2));
			g.DrawLine(Pens.Black, new Point(rect.Width / 2, 0), new Point(rect.Width / 2, rect.Height));
			g.TranslateTransform(Middle.X, Middle.Y);
			g.RotateTransform (-90);
			//StringFormat format = new StringFormat(StringFormatFlags.NoClip);
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			//format.LineAlignment = StringAlignment.Center;
			g.DrawString (s, font, Brushes.Black, 0, 0, format);

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

