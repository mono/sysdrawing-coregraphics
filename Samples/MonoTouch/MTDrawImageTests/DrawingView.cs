using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using UIKit;
using Foundation;
using CoreGraphics;

namespace MTDrawImageTests
{
	public class DrawingView : UIView {

		public event PaintEventHandler Paint;

		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			BackColor = Color.Wheat;

			var mainBundle = NSBundle.MainBundle;

			var filePath = mainBundle.PathForResource("bitmap50","png");
			bmp = Bitmap.FromFile(filePath);

			filePath = mainBundle.PathForResource("bitmap25","png");
			bmp2 = Bitmap.FromFile(filePath);
		}

		#region Panel interface
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
					font = new Font("Helvetica",12);
				return font;
			}
			set 
			{
				font = value;
			}
		}
		
		public int Left 
		{
			get { 
				
				return (int)Frame.Left; 
			}
			
			set {
				var location = new PointF(value, Frame.Y);
				Frame = new RectangleF(location, Frame.Size);
			}
			
		}
		
		public int Right 
		{
			get { return (int)Frame.Right; }
			
			set { 
				var size = Frame;
				size.Width = size.X - value;
				Frame = size;
			}
			
		}
		
		public int Top
		{
			get { return (int)Frame.Top; }
			set { 
				var location = new PointF(Frame.X, value);
				Frame = new RectangleF(location, Frame.Size);
				
			}
		}
		
		public int Bottom
		{
			get { return (int)Frame.Bottom; }
			set { 
				var frame = Frame;
				frame.Height = frame.Y - value;
				Frame = frame;
				
			}
		}
		
		public int Width 
		{
			get { return (int)Frame.Width; }
			set { 
				var frame = Frame;
				frame.Width = value;
				Frame = frame;
			}
		}
		
		public int Height
		{
			get { return (int)Frame.Height; }
			set { 
				var frame = Frame;
				frame.Height = value;
				Frame = frame;
			}
		}
#endregion

		public override void Draw (RectangleF dirtyRect)
		{
			Graphics g = Graphics.FromCurrentContext();
			g.Clear(backColor);

			Rectangle clip = new Rectangle((int)dirtyRect.X,
			                               (int)dirtyRect.Y,
			                               (int)dirtyRect.Width,
			                               (int)dirtyRect.Height);

			var args = new PaintEventArgs(g, clip);

			OnPaint(args);

			if(Paint != null)
			{
				Paint(this, args);
			}
		}

		public override void TouchesBegan (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			currentView++;
			currentView %= totalViews;
			//Console.WriteLine("Current View: {0}", currentView);
			MarkDirty();
			//this.NeedsDisplay = true;
			SetNeedsDisplay ();
		}

		Font anyKeyFont = new Font("Chalkduster", 18, FontStyle.Bold);
		Font clipFont = new Font("Helvetica",12, FontStyle.Bold);

		Image bmp;
		Image bmp2;

		Rectangle src = new Rectangle(0, 0, 50, 50);
		RectangleF srcF = new Rectangle(0, 0, 50, 50);
		Rectangle dst = new Rectangle(170, 170, 100, 100);
		RectangleF dstF = new Rectangle(270, 270, 100, 100);

		int currentView = 15;
		int totalViews = 17;

		string title = string.Empty;

		protected void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
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
			case 14:
				DrawImageTranslateClip (g);
				break;
			case 15:
				FillRegionIntersect (g);
				break;
			case 16:
				DrawImageIntersectClip (g);
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
			var anyKey = "Tap screen to continue.";
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

		void DrawImageTranslateClip(Graphics g)
		{
			// Create rectangle for clipping region.
			Rectangle clipRect = new Rectangle(0, 0, 100, 100);

			// Set clipping region of graphics to rectangle.
			g.SetClip(clipRect);

			// Translate clipping region. 
			int dx = 50;
			int dy = 50;
			g.TranslateClip(dx, dy);

			// Fill rectangle to demonstrate translated clip region.
			g.FillRectangle(new SolidBrush(Color.Black), 0, 0, 500, 300);
			title = "DrawImageTranslateClip";
		}

		void FillRegionIntersect(Graphics g)
		{
			// Create the first rectangle and draw it to the screen in black.
			Rectangle regionRect = new Rectangle(20, 20, 100, 100);
			g.DrawRectangle(Pens.Black, regionRect);

			// create the second rectangle and draw it to the screen in red.
			RectangleF complementRect = new RectangleF(90, 30, 100, 100);
			g.DrawRectangle(Pens.Red,
			                Rectangle.Round(complementRect));

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect);

			// Get the area of intersection for myRegion when combined with 

			// complementRect.
			myRegion.Intersect(complementRect);

			// Fill the intersection area of myRegion with blue.
			SolidBrush myBrush = new SolidBrush(Color.Blue);
			g.FillRegion(myBrush, myRegion);

			title = "FillRegionInterset";
		}

		void DrawImageIntersectClip(Graphics g)
		{
			// Create the first rectangle and draw it to the screen in black.
			Rectangle regionRect = new Rectangle(20, 20, 100, 100);
			g.DrawRectangle(Pens.Black, regionRect);

			// create the second rectangle and draw it to the screen in red.
			RectangleF complementRect = new RectangleF(90, 30, 100, 100);
			g.DrawRectangle(Pens.Red,
			                Rectangle.Round(complementRect));

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect);

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Intersect(complementRect);

			var unionRect = complementRect.UnionWith (regionRect);
			g.DrawRectangle(Pens.Green,
			                Rectangle.Round(unionRect));

			g.Clip = myRegion;

			g.DrawImage(bmp2, unionRect);

			title = "DrawImageIntersetClip";
		}
	}
}

public delegate void PaintEventHandler(object sender, PaintEventArgs e);


public class PaintEventArgs : EventArgs, IDisposable
{
	private readonly Rectangle clipRect;
	private Graphics graphics;

	public PaintEventArgs(Graphics graphics, Rectangle clipRect)
	{
		if (graphics == null)
		{
			throw new ArgumentNullException("graphics");
		}
		this.graphics = graphics;
		this.clipRect = clipRect;
	}

	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if ((disposing && (this.graphics != null)))
		{
			this.graphics.Dispose();
		}
	}

	~PaintEventArgs()
	{
		this.Dispose(false);
	}

	public Rectangle ClipRectangle
	{
		get
		{
			return this.clipRect;
		}
	}

	public Graphics Graphics
	{
		get
		{
			return this.graphics;
		}
	}

}
