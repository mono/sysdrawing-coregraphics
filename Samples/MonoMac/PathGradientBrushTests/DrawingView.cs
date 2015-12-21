
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

using Foundation;
using AppKit;
using CoreGraphics;
using CoreGraphics;
using System.Drawing;

namespace PathGradientBrushTests
{
	public partial class DrawingView : AppKit.NSView
	{
		
		public event PaintEventHandler Paint;
		
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
		}
		
		public DrawingView (CGRect rect) : base (rect)
		{
			Initialize();
		}
		
#endregion
		
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
				var location = new CGPoint (value, Frame.Y);
				Frame = new CGRect (location, Frame.Size);
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
				var location = new CGPoint (Frame.X, value);
				Frame = new CGRect (location, Frame.Size);
				
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
				this.NeedsDisplay = true;
				
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

		Font anyKeyFont = new Font("Chalkduster", 18, FontStyle.Bold);
		Font clipFont = new Font("Helvetica",12, FontStyle.Bold);
		string title = string.Empty;

		public override void DrawRect (CGRect dirtyRect)
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

		public override bool AcceptsFirstResponder ()
		{
			return true;
		}

		int currentView = 0;
		int totalViews = 10;

		public override void KeyDown (NSEvent theEvent)
		{
			currentView++;
			currentView %= totalViews;
			//Console.WriteLine("Current View: {0}", currentView);
			this.NeedsDisplay = true;
		}


		protected void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			switch (currentView) 
			{
			case 0:
				PaintView0 (g);
				break;
			case 1:
				PaintView1 (g);
				break;
			case 2:
				GlassSphere (g);
				break;
			case 3:
				BlendingIn (g);
				break;
			case 4:
				PaintView3 (g);
				break;
			case 5:
				PaintViewB (g);
				break;
			case 6:
				PaintView4 (g);
				break;
			case 7:
				PaintView5 (g);
				break;
			case 8:
				PaintView6 (g);
				break;
			case 9:
				PaintView7 (g);
				break;
			};

			g.ResetTransform ();
			Brush sBrush = Brushes.Black;

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
		
		void PaintView0 (Graphics g)
		{
			var trianglePoints = new PointF[]{
				new PointF(this.ClientRectangle.Width/2,0),
				new PointF(0,this.ClientRectangle.Height),
				new PointF(this.ClientRectangle.Width,this.ClientRectangle.Height)};

			PathGradientBrush pgb = new PathGradientBrush(trianglePoints);

			var surroundColors = new Color[] { Color.Red, Color.Green, Color.Blue };

			pgb.SurroundColors = surroundColors;

			pgb.CenterColor = Color.Gray;
			//pgb.FocusScales = new PointF(0.3f, 0.8f);
			g.FillRectangle(pgb, this.ClientRectangle);

			pgb.Dispose();

			title = "Triangle Blend";
		}

		Brush brush1;

		void PaintView1 (Graphics g)
		{
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.CompositingQuality = CompositingQuality.HighQuality;

			if (brush1==null) brush1=createCircularMetalBrush();
			//Graphics g=e.Graphics;
			int cx=Width/2;
			int cy=Height/2;
			int r=cx;
			if (r>cy) r=cy;
			r-=20;	// margin

			g.TranslateTransform(cx, cy);
			g.ScaleTransform(r, r);
			g.FillEllipse(brush1, new RectangleF(-1, -1, 2, 2));

			
			var triangleCenter = ((PathGradientBrush)brush1).CenterPoint;
			g.FillEllipse(Brushes.Red, new RectangleF(triangleCenter.X + cx, triangleCenter.Y+cy, 2*r, 2*r));

			title = "Metal Brush - not Working";

		}

		private static Brush createCircularMetalBrush() {
			List<PointF> points=new List<PointF>();
			List<Color> colors=new List<Color>();
			int darkest=80;
			int lightest=255;

			double rplus=1.03;
			for (int i=0; i<360; i+=5) {
				double angle=i*Math.PI/180;
				double cos=Math.Cos(angle);
				double sin=Math.Sin(angle);
				points.Add(new PointF((float)(rplus*cos), (float)(rplus*sin)));
				angle=(i-45)*Math.PI/180;
				cos=Math.Cos(angle);
				int c=darkest+(int)((lightest-darkest)*cos*cos);
				colors.Add(Color.FromArgb(c, c, c));
			}
			PathGradientBrush brush = new PathGradientBrush(points.ToArray());
			brush.SurroundColors = colors.ToArray();
			brush.CenterColor = Color.White;
			return brush;
		}


		void PaintView2 (Graphics g)
		{
			// Create a LinearGradientBrush.
			Rectangle myRect = new Rectangle(20, 20, 200, 100);
			LinearGradientBrush myLGBrush = new LinearGradientBrush(
				myRect, Color.Blue, Color.Red,  0.0f, true);
			
			// Draw an ellipse to the screen using the LinearGradientBrush.
			g.FillEllipse(myLGBrush, myRect);
			
			// Rotate the LinearGradientBrush.
			myLGBrush.RotateTransform(45.0f, MatrixOrder.Prepend);
			
			// Rejustify the brush to start at the left edge of the ellipse.
			myLGBrush.TranslateTransform(-100.0f, 0.0f);
			
			// Draw a second ellipse to the screen using 
			// the transformed brush.
			g.FillEllipse(myLGBrush, 20, 150, 200, 100);

			title = "PaintView2";

		}

		void PaintViewB(Graphics g)
		{

			GraphicsPath gPath = new GraphicsPath();
			Rectangle rect = new Rectangle(0, 0, Width, Height / 3);
			gPath.AddRectangle(rect);

			PathGradientBrush pathGradientBrush = new PathGradientBrush(gPath);

			pathGradientBrush.CenterColor = Color.Crimson;

			Color[] colors = { Color.SaddleBrown, Color.Green, Color.Orange, Color.DarkViolet };
			pathGradientBrush.SurroundColors = colors;

			Blend blend = new Blend();
			blend.Factors = new float[] { 1, 0, 1, 0, 1 };
			blend.Positions = new float[] { 0, 0.20f, 0.5f, 0.70f, 1f };
			//pathGradientBrush.Blend = blend;

			Rectangle rect2 = new Rectangle(0, 0, Width, Height / 3);

			g.FillRectangle(pathGradientBrush, rect2);

			title = "Rectangle Blend";
		}

		private void GlassSphere(Graphics g)
		{
			Color c = Color.Blue;
			int reduct = 200;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			// Base ellipse
			Rectangle r1 = new Rectangle(new Point(0, 0), new Size(this.Width-reduct, this.Height-reduct));
			Rectangle r2 = new Rectangle(r1.Location, new Size(r1.Size.Width - reduct - 2, r1.Size.Height - reduct - 2));

			GraphicsPath path = new GraphicsPath (FillMode.Winding);
			path.AddEllipse(r2);
			PathGradientBrush br1 = new PathGradientBrush(path);
			br1.CenterColor = c;
			br1.SurroundColors = new Color[] { Color.FromArgb(255, Color.Black) };
			//br1.CenterPoint = new PointF((float)(r1.Width / 1.5), r1.Top - Convert.ToInt16(r1.Height * 2));

			Blend bl1 = new Blend(5);
			bl1.Factors = new float[] { 0.5f, 1.0f, 1.0f, 1.0f, 1.0f };
			bl1.Positions = new float[] { 0.0f, 0.05f, 0.5f, 0.75f, 1.0f };
			br1.Blend = bl1;

			g.FillPath(br1, path);

			br1.Dispose();
			path.Dispose();

			// 1st hilite ellipse
			int r3w = Convert.ToInt16(r2.Width * 0.8);
			int r3h = Convert.ToInt16(r2.Height * 0.6);

			int r3posX = (r2.Width / 2) - (r3w / 2);
			int r3posY = r2.Top + 1;

			Rectangle r3 = new Rectangle(
				new Point(r3posX, r3posY),
				new Size(r3w, r3h));

			Color br3c1 = Color.White;
			Color br3c2 = Color.Transparent;

			LinearGradientBrush br2 = new LinearGradientBrush(r3, br3c1, br3c2, 90);
			br2.WrapMode = WrapMode.TileFlipX;
			g.FillEllipse(br2, r3);

			br2.Dispose();

			// 2nd hilite ellipse
			int r4w = Convert.ToInt16(r2.Width * 0.3);
			int r4h = Convert.ToInt16(r2.Height * 0.2);

			int r4posX = (r2.Width / 2) + (r4w / 2);
			int r4posY = r2.Top + Convert.ToInt16(r2.Height * 0.2);

			Rectangle r4 = new Rectangle(
				new Point(-(int)(r4w / 2), -(int)(r4h / 2)),
				new Size(r4w, r4h));

			LinearGradientBrush br3 = new LinearGradientBrush(r4, br3c1, br3c2, 90, true);
			g.TranslateTransform(r4posX, r4posY);
			g.RotateTransform(30);
			g.FillEllipse(br3, r4);

			br3.Dispose();
			title = "GlassSphere";

		}

		private void BlendingIn(Graphics g)
		{

			PointF[] p=new PointF[11];
			float t;
			bool i;
			int n;
			for(t=0, i=false, n=0; t<=(Math.PI*2); t+=(float)Math.PI/5, i=!i, n++)
			{
				float l=50+(80*(i ? 0 : 1));
				p[n]=new PointF((float)Math.Cos(t-Math.PI/2)*l,(float)Math.Sin(t-Math.PI/2)*l);
			}
			p[n]=p[0];

			GraphicsPath pth=new GraphicsPath();

			pth.AddLines(p);

			PathGradientBrush pgb=new PathGradientBrush(pth);

			pgb.CenterColor=Color.White;
			pgb.SurroundColors=new Color[]{Color.Black};
			Blend b=new Blend();
			b.Factors=new float[]{1,0,1,0,1};
			b.Positions=new float[]{0,0.25f,0.5f,0.75f,1f};

			pgb.Blend=b;

			g.Transform=new Matrix(1,0,0,1,Width/2,Height/2);

			g.FillPath(pgb,pth);

			title = "Blending In";
		}

		private void PaintView3(Graphics g)
		{
			GraphicsPath gp = new GraphicsPath();
			gp.AddEllipse(ClientRectangle);

			PathGradientBrush pgb = new PathGradientBrush(gp);

			pgb.CenterPoint = new PointF(ClientRectangle.Width / 2,
			                             ClientRectangle.Height / 2);
			pgb.CenterPoint = new PointF(ClientRectangle.Width / 4,
				ClientRectangle.Height / 4);
			pgb.CenterColor = Color.White;
			pgb.SurroundColors = new Color[] { Color.Red };

			g.FillRectangle(pgb, this.ClientRectangle);
			pgb.Dispose();
			gp.Dispose();

			title = "Big Red Circle";
		}


		private void PaintView4 (Graphics g)
		{
			// Create a path that consists of a single ellipse.
			GraphicsPath path = new GraphicsPath();
			path.AddEllipse(0, 0, 140, 70);

			// Use the path to construct a brush.
			PathGradientBrush pthGrBrush = new PathGradientBrush(path);

			// Set the color at the center of the path to blue.
			pthGrBrush.CenterColor = Color.FromArgb(255, 0, 0, 255);

			// Set the color along the entire boundary  
			// of the path to aqua.
			Color[] colors = { Color.FromArgb(255, 0, 255, 255) };
			pthGrBrush.SurroundColors = colors;

			g.FillEllipse(pthGrBrush, 0, 0, 140, 70);

			title = "Ellipse";
		}

		private void PaintView5(Graphics g)
		{
			// Put the points of a polygon in an array.
			Point[] points = {
				new Point(75, 0),
				new Point(100, 50),
				new Point(150, 50),
				new Point(112, 75),
				new Point(150, 150),
				new Point(75, 100),
				new Point(0, 150),
				new Point(37, 75),
				new Point(0, 50),
				new Point(50, 50)};

			// Use the array of points to construct a path.
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);

			// Use the path to construct a path gradient brush.
			PathGradientBrush pthGrBrush = new PathGradientBrush(path);

			// Set the color at the center of the path to red.
			pthGrBrush.CenterColor = Color.FromArgb(255, 255, 0, 0);

			// Set the colors of the points in the array.
			Color[] colors = {
				Color.FromArgb(255, 0, 0, 0),
				Color.FromArgb(255, 0, 255, 0),
				Color.FromArgb(255, 0, 0, 255), 
				Color.FromArgb(255, 255, 255, 255),
				Color.FromArgb(255, 0, 0, 0),
				Color.FromArgb(255, 0, 255, 0),
				Color.FromArgb(255, 0, 0, 255),
				Color.FromArgb(255, 255, 255, 255),
				Color.FromArgb(255, 0, 0, 0),  
				Color.FromArgb(255, 0, 255, 0)};

			pthGrBrush.SurroundColors = colors;

			// Fill the path with the path gradient brush.
			g.FillPath(pthGrBrush, path);

			title = "Polygon Blending";
		}

		private void PaintView6(Graphics g)
		{
			// Construct a path gradient brush based on an array of points.
			PointF[] ptsF = {
				new PointF(0, 0), 
				new PointF(160, 0), 
				new PointF(160, 200),
				new PointF(80, 150),
				new PointF(0, 200)};

			PathGradientBrush pBrush = new PathGradientBrush(ptsF);

			// An array of five points was used to construct the path gradient 
			// brush. Set the color of each point in that array.
			Color[] colors = {
				Color.FromArgb(255, 255, 0, 0),  // (0, 0) red
				Color.FromArgb(255, 0, 255, 0),  // (160, 0) green
				Color.FromArgb(255, 0, 255, 0),  // (160, 200) green
				Color.FromArgb(255, 0, 0, 255),  // (80, 150) blue
				Color.FromArgb(255, 255, 0, 0)}; // (0, 200) red

			pBrush.SurroundColors = colors;

			// Set the center color to white.
			pBrush.CenterColor = Color.White;

			// Use the path gradient brush to fill a rectangle.
			g.FillRectangle(pBrush, new Rectangle(0, 0, 160, 200));

			title = "Squarish Polygon Blend";
		}

		private void PaintView7 (Graphics g)
		{
			// Create a path that consists of a single ellipse.
			GraphicsPath path = new GraphicsPath();
			path.AddEllipse(0, 0, 200, 100);

			// Create a path gradient brush based on the elliptical path.
			PathGradientBrush pthGrBrush = new PathGradientBrush(path);

			// Set the color along the entire boundary to blue.
			Color[] color = { Color.Blue };
			pthGrBrush.SurroundColors = color;

			// Set the center color to aqua.
			pthGrBrush.CenterColor = Color.Aqua;

			// Use the path gradient brush to fill the ellipse. 
			g.FillPath(pthGrBrush, path);

			// Set the focus scales for the path gradient brush.
			pthGrBrush.FocusScales = new PointF(0.3f, 0.8f);

			// Use the path gradient brush to fill the ellipse again. 
			// Show this filled ellipse to the right of the first filled ellipse.
			g.TranslateTransform(220.0f, 0.0f);
			g.FillPath(pthGrBrush, path);

			title = "Focus Scales - Not working";

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
