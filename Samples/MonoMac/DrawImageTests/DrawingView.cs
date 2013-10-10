
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using MonoMac.Foundation;
using MonoMac.AppKit;

namespace DrawImageTests
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
			var mainBundle = NSBundle.MainBundle;

			var filePath = mainBundle.PathForResource("bitmap50","png");
			bmp = Bitmap.FromFile(filePath);

			filePath = mainBundle.PathForResource("bitmap25","png");
			bmp2 = Bitmap.FromFile(filePath);
		}

		public DrawingView (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion

		Font anyKeyFont = new Font("Chalkduster", 18, FontStyle.Bold);
		Font clipFont = new Font("Helvetica",12, FontStyle.Bold);

		Image bmp;
		Image bmp2;

		Rectangle src = new Rectangle(0, 0, 50, 50);
		RectangleF srcF = new Rectangle(0, 0, 50, 50);
		Rectangle dst = new Rectangle(170, 170, 100, 100);
		RectangleF dstF = new Rectangle(270, 270, 100, 100);

		int currentView = 13;
		int totalViews = 17;

		public Rectangle ClientRectangle 
		{
			get {
				return new Rectangle((int)Bounds.X,
				                     (int)Bounds.Y,
				                     (int)Bounds.Width,
				                     (int)Bounds.Height);
			}
		}

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
			Graphics g = Graphics.FromCurrentContext();
			g.InterpolationMode = InterpolationMode.NearestNeighbor;

			g.Clear(Color.Wheat);
			//g.SmoothingMode = SmoothingMode.None;
			switch (currentView) 
			{
			case 0:
				DrawImage1 (g);
				break;
			case 1:
				DrawImage2 (g);
				break;
			case 2:
				DrawImage3 (g);
				break;
			case 3:
				DrawImage4 (g);
				break;
			case 4:
				DrawImage5 (g);
				break;
			case 5:
				DrawImage6 (g);
				break;
			case 6:
				DrawImage7 (g);
				break;
			case 7:
				DrawImage8 (g);
				break;
			case 8:
				DrawImage9 (g);
				break;
			case 9:
				DrawImagePageUnit (g);
				break;
			case 10:
				DrawImagePageUnit_2 (g);
				break;
			case 11:
				DrawImagePageUnit_3 (g);
				break;
			case 12:
				DrawImagePageUnit_4 (g);
				break;
			case 13:
				DrawImagePageUnitClip (g);
				break;

			}

			g.PageUnit = GraphicsUnit.Pixel;
			Brush sBrush = Brushes.Black;

			g.ResetTransform ();

			if (!g.IsClipEmpty) 
			{
				var clipPoint = PointF.Empty;
				var clipString = string.Format("Clip-{0}", g.ClipBounds);
				g.ResetClip ();
				var clipSize = g.MeasureString(clipString, clipFont);
				clipPoint.X = (ClientRectangle.Width / 2) - (clipSize.Width / 2);
				clipPoint.Y = 5;
				g.DrawString(clipString, clipFont, sBrush, clipPoint );

			}

			var anyKeyPoint = PointF.Empty;
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

		public void DrawImage1(Graphics g) {
			g.DrawImage(bmp, new Point[]{new Point(170,10), new Point(250,0), new Point(100,100)}, src, GraphicsUnit.Pixel );
			g.DrawImage(bmp, new PointF[]{new PointF(70,10), new PointF(150,0), new PointF(10,100)}, srcF, GraphicsUnit.Pixel );
			title = "DrawImage1";
		}

		public void DrawImage2(Graphics g) {
			g.DrawImage(bmp, dst, src, GraphicsUnit.Pixel);
			g.DrawImage(bmp, dstF, srcF, GraphicsUnit.Pixel);
			title = "DrawImage2";
		}

		public void DrawImage3(Graphics g) {
			g.DrawImage(bmp, 10.0F, 10.0F, srcF, GraphicsUnit.Pixel);
			g.DrawImage(bmp, 70.0F, 150.0F, 250.0F, 150.0F);
			title = "DrawImage3";
		}
		public void DrawImage4(Graphics g) {
			g.DrawImage(bmp, dst);
			g.DrawImage(bmp, dstF);
			title = "DrawImage4";
		}
		public void DrawImage5(Graphics g) {
			g.SetClip( new Rectangle(70, 0, 20, 200));
			g.DrawImage(bmp, new Point[]{new Point(50,50), new Point(250,30), new Point(100,150)}, src, GraphicsUnit.Pixel );
			title = "DrawImage5";
		}
		public void DrawImage6(Graphics g) {
			g.ScaleTransform(2, 2);
			g.SetClip( new Rectangle(70, 0, 20, 200));
			g.DrawImage(bmp, new Point[]{new Point(50,50), new Point(250,30), new Point(100,150)}, src, GraphicsUnit.Pixel );
			title = "DrawImage6";
		}
		public void DrawImage7(Graphics g) {
			g.DrawImage(bmp, 170, 70, src, GraphicsUnit.Pixel);
			g.DrawImage(bmp, 70, 350, 350, 150);
			title = "DrawImage7";
		}
		public void DrawImage8(Graphics g) {
			g.DrawImage(bmp, new Point[]{new Point(170,10), new Point(250,10), new Point(100,100)} );
			g.DrawImage(bmp, new PointF[]{new PointF(170,100), new PointF(250,100), new PointF(100,190)} );
			title = "DrawImage8";
		}
		public void DrawImage9(Graphics g) {
			g.DrawImage(bmp, 0, 0);
			g.DrawImage(bmp, 200, 200);
			title = "DrawImage9";
		}
		public void DrawImagePageUnit(Graphics g) {
			g.PageUnit = GraphicsUnit.Document;
			//g.ScaleTransform(0.32f, 0.32f);
			Point [] p = new Point[]{
				new Point(100, 100),
				new Point(200, 100),
				new Point(50, 200)
			};

			g.DrawImage(bmp2, p, new Rectangle(100, 100, 100, 100), GraphicsUnit.Pixel);
			title = "DrawImagePageUnit";
		}
		public void DrawImagePageUnit_2(Graphics g) {
			g.PageUnit = GraphicsUnit.Millimeter;
			g.ScaleTransform(0.3f, 0.3f);
			Point [] p = new Point[]{
				new Point(100, 100),
				new Point(200, 100),
				new Point(50, 200)
			};

			g.DrawImage(bmp2, p, new Rectangle(100, 100, 100, 100), GraphicsUnit.Pixel);
			title = "DrawImagePageUnit_2";
		}
	
		public void DrawImagePageUnit_3(Graphics g) {
			g.PageUnit = GraphicsUnit.Millimeter;
			g.ScaleTransform(0.3f, 0.3f);
			g.DrawImage(bmp2, new Rectangle(100, 100, 100, 100));
			title = "DrawImagePageUnit_3";
		}

		public void DrawImagePageUnit_4(Graphics g) {
			g.PageUnit = GraphicsUnit.Millimeter;
			g.ScaleTransform(0.5f, 0.5f);
			g.DrawImage(bmp, 50, 50);
			title = "DrawImagePageUnit_4";
		}

		public void DrawImagePageUnitClip(Graphics g) {
			g.PageUnit = GraphicsUnit.Millimeter;
			g.ScaleTransform(0.3f, 0.3f);

			Point[] p = new Point[]{
			    new Point(100, 100),
			    new Point(200, 100),
			    new Point(50, 200)
			};

			g.SetClip( new Rectangle(120, 120, 50, 100) );

			g.DrawImage(bmp2, p, new Rectangle(100, 100, 100, 100), GraphicsUnit.Pixel);
			title = "DrawImagePageUnitClip";
		}
	}
}

