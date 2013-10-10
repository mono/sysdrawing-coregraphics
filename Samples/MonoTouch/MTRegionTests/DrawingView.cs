using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;

namespace MTRegionTests
{
	public class DrawingView : UIView {

		public event PaintEventHandler Paint;

		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			BackgroundColor = new UIColor (0, 0, 0, 0);
			BackColor = Color.Wheat;

			var mainBundle = NSBundle.MainBundle;

//			var filePath = mainBundle.PathForResource("bitmap50","png");
//			bmp = Bitmap.FromFile(filePath);
//
//			filePath = mainBundle.PathForResource("bitmap25","png");
//			bmp2 = Bitmap.FromFile(filePath);
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
			Graphics g = new Graphics();
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


		Rectangle regionRect1 = new Rectangle(50, 50, 100, 100);
		RectangleF regionRectF1 = new RectangleF(110, 60, 100, 100);
		Rectangle regionRect2 = new Rectangle(110, 60, 100, 100);
		RectangleF regionRectF2 = new RectangleF(110, 60, 100, 100);


		int currentView = 0;
		int totalViews = 20;

		string title = string.Empty;

		protected void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.InterpolationMode = InterpolationMode.NearestNeighbor;

			g.Clear(Color.White);
			//g.SmoothingMode = SmoothingMode.None;
			switch (currentView) 
			{
			case 0:
				FillRegionInfinite (g);
				break;
			case 1:
				FillRegionEmpty (g);
				break;
			case 2:
				FillRegion1 (g);
				break;
			case 3:
				FillRegionIntersect (g);
				break;
			case 4:
				FillRegionUnion (g);
				break;
			case 5:
				FillRegionExclude (g);
				break;
			case 6:
				FillRegionXor(g);
				break;
			case 7:
				FillRegionInfiniteIntersect(g);
				break;
			case 8:
				FillRegionInfiniteUnion(g);
				break;
			case 9:
				FillRegionInfiniteExclude(g);
				break;
			case 10:
				FillRegionInfiniteXor(g);
				break;
			case 11:
				FillRegionEmptyIntersect(g);
				break;
			case 12:
				FillRegionEmptyUnion(g);
				break;
			case 13:
				FillRegionEmptyExclude(g);
				break;
			case 14:
				FillRegionEmptyXor(g);
				break;
			case 15:
				TranslateRegion(g);
				break;
			case 16:
				TransformRegion(g);
				break;
			case 17:
				RegionIsVisibleRectangleF(g);
				break;

			}

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

		void FillRegionInfinite(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in myPen color.
			g.DrawRectangle(myPen, regionRect1);

			// Create a region using the first rectangle.
			Region myRegion = new Region();

			// Fill the region which basically clears the screen to the background color.
			// Take a look at the initialize method of DrawingView.
			// This may set the rectangle to Black depending on the context
			// passed.  On a NSView set WantsLayers and the Layer Background color.
			g.FillRegion(myBrush, myRegion);

			title = "FillInfiniteRegion";
		}

		void FillRegionEmpty(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in myPen color.
			g.DrawRectangle(myPen, regionRect1);

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect1);
			myRegion.MakeEmpty ();

			// Fill the region which basically clears the screen to the background color.
			// Take a look at the initialize method of DrawingView.
			// This may set the rectangle to Black depending on the context
			// passed.  On a NSView set WantsLayers and the Layer Background color.
			g.FillRegion(myBrush, myRegion);

			title = "FillEmptyRegion";
		}

		void FillRegion1(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in myPen color.
			g.DrawRectangle(myPen, regionRect1);


			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect1);

			// Fill the intersection area of myRegion with blue.
			g.FillRegion(myBrush, myRegion);

			title = "FillRegion1";
		}

		void FillRegionIntersect(Graphics g)
		{

			// Greyish blue
			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			// Pinkish
			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect1);

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Intersect(regionRectF2);

			// Greenish
			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionIntersect";
		}

		void FillRegionUnion(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect1);

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Union(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionUnion";
		}

		void FillRegionExclude(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect1);

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Exclude(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionExclude";
		}

		void FillRegionXor(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect1);

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Xor(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionXor";
		}

		
		void FillRegionInfiniteIntersect(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Intersect(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionInfiniteIntersect";
		}

		void FillRegionInfiniteUnion(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Union(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionInfiniteUnion";
		}


		void FillRegionInfiniteExclude(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Exclude(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionInfiniteExclude";
		}

		void FillRegionInfiniteXor(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Xor(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionInfiniteXor";
		}


		void FillRegionEmptyIntersect(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region();
			myRegion.MakeEmpty();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Intersect(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionEmptyIntersect";
		}

		void FillRegionEmptyUnion(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region();
			myRegion.MakeEmpty();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Union(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionEmptyUnion";
		}


		void FillRegionEmptyExclude(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region();
			myRegion.MakeEmpty();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Exclude(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionEmptyExclude";
		}

		void FillRegionEmptyXor(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen,
			                Rectangle.Round(regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			Region myRegion = new Region();
			myRegion.MakeEmpty();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Xor(regionRectF2);

			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

			g.FillRegion(myBrush, myRegion);

			title = "FillRegionEmptyXor";
		}

		
		public void TranslateRegion(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in blue.
			Rectangle regionRect = new Rectangle(100, 50, 100, 100);
			g.DrawRectangle(myPen, regionRect);
			g.FillRectangle (myBrush, regionRect);

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect);

			// Apply the translation to the region.
			myRegion.Translate(150, 100);

			// Fill the transformed region with red and draw it to the screen in red.
			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);
			g.FillRegion(myBrush, myRegion);

			title = "TranslateRegion";
		}

		public void TransformRegion(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in blue.
			Rectangle regionRect = new Rectangle(100, 50, 100, 100);
			g.DrawRectangle(myPen, regionRect);
			g.FillRectangle (myBrush, regionRect);

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect);

			// Create a transform matrix and set it to have a 45 degree 

			// rotation.
			Matrix transformMatrix = new Matrix();
			transformMatrix.RotateAt(45, new Point(100, 50));

			// Apply the transform to the region.
			myRegion.Transform(transformMatrix);

			// Fill the transformed region with red and draw it to the screen 
			// in color.
			myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb(255, 0, 0x33, 0);
			g.FillRegion(myBrush, myRegion);

			title = "TransformRegion";
		}

		public void RegionIsVisibleRectangleF(Graphics g)
		{

			Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in blue.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			// Create the second rectangle and draw it to the screen in red.
			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			g.DrawRectangle(myPen, Rectangle.Round(regionRectF2));
			g.FillRectangle (myBrush, Rectangle.Round (regionRectF2));

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect1);

			// Determine if myRect is contained in the region. 
			bool contained = myRegion.IsVisible(regionRect2);

			// Display the result.
			Font myFont = new Font("Arial", 8);
			SolidBrush txtBrush = new SolidBrush(Color.Black);
			g.DrawString("contained = " + contained.ToString(),
			             myFont,
			             txtBrush,
			             new PointF(regionRectF2.Right + 10, regionRectF2.Top));

			regionRect1.Y += 120;
			regionRectF2.Y += 120;
			regionRectF2.X += 41;

			myPen.Color = Color.FromArgb (196, 0xC3, 0xC9, 0xCF);
			myBrush.Color = Color.FromArgb(127, 0xDD, 0xDD, 0xF0);

			// Create the first rectangle and draw it to the screen in blue.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			// Create the second rectangle and draw it to the screen in red.
			myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

			g.DrawRectangle(myPen, Rectangle.Round(regionRectF2));
			g.FillRectangle (myBrush, Rectangle.Round (regionRectF2));

			// Create a region using the first rectangle.
			myRegion = new Region(regionRect1);

			// Determine if myRect is contained in the region. 
			contained = myRegion.IsVisible(regionRectF2);

			// Display the result.
			g.DrawString("contained = " + contained.ToString(),
			             myFont,
			             txtBrush,
			             new PointF(regionRectF2.Right + 10, regionRectF2.Top));

			// restore defaults
			regionRect1.Y -= 120;
			regionRectF2.Y -= 120;
			regionRectF2.X -= 41;

			title = "RegionIsVisibleRectangleF";

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
