using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using CoreGraphics;
using Foundation;
using UIKit;

namespace MTClippingTests {
	public class DrawingView : UIView {
		public event PaintEventHandler Paint;

		Font anyKeyFont = new Font("Chalkduster", 18, FontStyle.Bold);
		Font clipFont = new Font("Helvetica", 12, FontStyle.Bold);

		Image bmp;
		Image bmp2;

		Rectangle regionRect1 = new Rectangle (50, 50, 100, 100);
		Rectangle regionRectF1 = new Rectangle (110, 60, 100, 100);
		Rectangle regionRect2 = new Rectangle (110, 60, 100, 100);
		Rectangle regionRectF2 = new Rectangle (110, 60, 100, 100);

		int currentView = 13;
		int totalViews = 20;

		string title = string.Empty;

		#region Panel interface
		public CGRect ClientRectangle {
			get {
				return new CGRect (Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
			}
		}

		public DrawingView (CGRect rect) : base(rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackgroundColor = UIColor.Clear;
			BackColor = Color.Wheat;

			var mainBundle = NSBundle.MainBundle;
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
					font = new Font("Helvetica", 12);
				return font;
			}
			set {
				font = value;
			}
		}

		public int Left {
			get {
				return (int)Frame.Left;
			}
			set {
				var location = new CGPoint (value, Frame.Y);
				Frame = new CGRect (location, Frame.Size);
			}

		}

		public int Right {
			get {
				return (int)Frame.Right;
			}
			set {
				var size = Frame;
				size.Width = size.X - value;
				Frame = size;
			}

		}

		public int Top {
			get {
				return (int)Frame.Top;
			}
			set {
				var location = new CGPoint (Frame.X, value);
				Frame = new CGRect (location, Frame.Size);

			}
		}

		public int Bottom {
			get {
				return (int)Frame.Bottom;
			}
			set {
				var frame = Frame;
				frame.Height = frame.Y - value;
				Frame = frame;

			}
		}

		public int Width {
			get {
				return (int)Frame.Width;
			}
			set {
				var frame = Frame;
				frame.Width = value;
				Frame = frame;
			}
		}

		public int Height {
			get {
				return (int)Frame.Height;
			}
			set {
				var frame = Frame;
				frame.Height = value;
				Frame = frame;
			}
		}
		#endregion

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext();
			g.Clear(backColor);

			var clip = new CGRect((int)rect.X, (int)rect.Y, (int)rect.Width, rect.Height);
			var args = new PaintEventArgs(g, clip);

			OnPaint(args);
			Paint?.Invoke(this, args);
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			currentView++;
			currentView %= totalViews;
			MarkDirty();
			SetNeedsDisplay();
		}

		protected void OnPaint (PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.InterpolationMode = InterpolationMode.NearestNeighbor;

			g.Clear(Color.White);
			switch (currentView) {
				case 0:
					ClipRegionInfinite(g);
					break;
				case 1:
					ClipRegionEmpty(g);
					break;
				case 2:
					ClipRegion1(g);
					break;
				case 3:
					ClipRegionIntersect(g);
					break;
				case 4:
					ClipRegionUnion(g);
					break;
				case 5:
					ClipRegionExclude(g);
					break;
				case 6:
					ClipRegionXor(g);
					break;
				case 7:
					ClipRegionInfiniteIntersect(g);
					break;
				case 8:
					ClipRegionInfiniteUnion(g);
					break;
				case 9:
					ClipRegionInfiniteExclude(g);
					break;
				case 10:
					ClipRegionInfiniteXor(g);
					break;
				case 11:
					ClipRegionEmptyIntersect(g);
					break;
				case 12:
					ClipRegionEmptyUnion(g);
					break;
				case 13:
					ClipRegionEmptyExclude(g);
					break;
				case 14:
					ClipRegionEmptyXor(g);
					break;
				case 15:
					IntersectClipRectangle(g);
					break;
				case 16:
					ExcludeClipRectangle(g);
					break;
				case 17:
					TranslateClip(g);
					break;
			}

			Brush sBrush = Brushes.Black;

			g.ResetTransform ();

			var clipPoint = CGPoint.Empty;
			var clipString = string.Format ("Clip-{0}", g.ClipBounds);
			g.ResetClip ();
			var clipSize = g.MeasureString (clipString, clipFont);
			clipPoint.X = (ClientRectangle.Width / 2) - (clipSize.Width / 2);
			clipPoint.Y = 5;
			g.DrawString (clipString, clipFont, sBrush, (PointF)clipPoint);

			var anyKeyPoint = CGPoint.Empty;
			var anyKey = "Tap screen to continue.";
			var anyKeySize = g.MeasureString (anyKey, anyKeyFont);
			anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
			anyKeyPoint.Y = ClientRectangle.Height - (anyKeySize.Height + 10);
			g.DrawString (anyKey, anyKeyFont, sBrush, (PointF)anyKeyPoint);

			anyKeySize = g.MeasureString (title, anyKeyFont);
			anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
			anyKeyPoint.Y -= anyKeySize.Height;
			g.DrawString (title, anyKeyFont, sBrush, (PointF)anyKeyPoint);

			g.Dispose ();
		}

		void ClipRegionInfinite (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush(Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in myPen color.
			g.DrawRectangle (myPen, regionRect1);

			// Create a region using the first rectangle.
			var myRegion = new Region ();

			g.Clip = myRegion;
			g.Clear (Color.Red);

			title = "ClipInfiniteRegion";
		}

		void ClipRegionEmpty (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in myPen color.
			g.DrawRectangle (myPen, regionRect1);

			// Create a region using the first rectangle.
			var myRegion = new Region (regionRect1);
			myRegion.MakeEmpty ();

			g.Clip = myRegion;
			g.Clear (Color.Red);
			title = "ClipEmptyRegion";
		}

		void ClipRegion1 (Graphics g)
		{
			var myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush(Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in myPen color.
			g.DrawRectangle (myPen, regionRect1);

			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, regionRectF2);

			// Create a region using the first rectangle.
			var myRegion = new Region (regionRect1);
			g.Clip = myRegion;

			// Greenish
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);

			// Clear intersection area of myRegion with blue.
			g.Clear (myBrush.Color);
			g.SetClip (regionRect2);

			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);
			g.FillRectangle(myBrush, new Rectangle (regionRect2.X + 50, regionRect2.Y + 50, 250, 300));
			title = "ClipRegion1";
		}

		void ClipRegionIntersect (Graphics g)
		{
			// Greyish blue
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			// Pinkish
			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region (regionRect1);

			// Get the area of intersection for myRegion when combined with complementRect.
			myRegion.Intersect (regionRectF2);

			// Greenish
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);

			g.Clip = myRegion;
			g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionIntersect";
		}

		void ClipRegionUnion (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region (regionRect1);

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Union (regionRectF2);
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);

			g.Clip = myRegion;
			g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionUnion";
		}

		void ClipRegionExclude (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round(regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region (regionRect1);

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Exclude (regionRectF2);

			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);

			g.Clip = myRegion;
			g.FillRectangle(myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionExclude";
		}

		void ClipRegionXor (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region (regionRect1);

			// Get the area of intersection for myRegion when combined with complementRect.
			myRegion.Xor (regionRectF2);

			g.Clip = myRegion;

			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);
			g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionXor";
		}

		void ClipRegionInfiniteIntersect (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region ();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Intersect (regionRectF2);

			g.Clip = myRegion;
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);

			g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionInfiniteIntersect";
		}

		void ClipRegionInfiniteUnion (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region ();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Union (regionRectF2);
			g.Clip = myRegion;
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);

			g.FillRectangle(myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionInfiniteUnion";
		}

		void ClipRegionInfiniteExclude (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round(regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region ();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Exclude (regionRectF2);

			g.Clip = myRegion;
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);

			g.FillRectangle(myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionInfiniteExclude";
		}

		void ClipRegionInfiniteXor (Graphics g)
		{
			var myPen = new Pen(Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle(myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle(myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region ();

			// Get the area of intersection for myRegion when combined with complementRect.
			myRegion.Xor(regionRectF2);
			g.Clip = myRegion;

			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);

			g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionInfiniteXor";
		}

		void ClipRegionEmptyIntersect (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle(myPen, regionRect1);
			g.FillRectangle(myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round(regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region ();
			myRegion.MakeEmpty ();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Intersect (regionRectF2);

			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);
			g.FillRegion (myBrush, myRegion);
			title = "ClipRegionEmptyIntersect";
		}

		void ClipRegionEmptyUnion (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region ();
			myRegion.MakeEmpty ();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Union (regionRectF2);
			g.Clip = myRegion;
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);
			g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionEmptyUnion";
		}

		void ClipRegionEmptyExclude (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region ();
			myRegion.MakeEmpty ();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Exclude (regionRectF2);
			g.Clip = myRegion;
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);
			g.FillRectangle(myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionEmptyExclude";
		}

		void ClipRegionEmptyXor (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in black.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			// create the second rectangle and draw it to the screen in red.
			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, regionRect2);

			// Create a region using the first rectangle.
			var myRegion = new Region ();
			myRegion.MakeEmpty ();

			// Get the area of intersection for myRegion when combined with 
			// complementRect.
			myRegion.Xor (regionRectF2);
			g.Clip = myRegion;
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);
			g.FillRectangle(myBrush, new Rectangle (0, 0, 500, 500));
			title = "ClipRegionEmptyXor";
		}

		public void IntersectClipRectangle (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (255, 0, 0x33, 0), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0x66, 0xEF, 0x7F));

			// Set clipping region.
			var clipRect = new Rectangle (0, 0, 200, 200);
			var clipRegion = new Region (clipRect);
			g.SetClip (clipRegion, CombineMode.Replace);

			// Update clipping region to intersection of existing region with specified rectangle.
			var intersectRect = new Rectangle (100, 100, 200, 200);
			var intersectRegion = new Region (intersectRect);
			g.IntersectClip (intersectRegion);

			// Fill rectangle to demonstrate effective clipping region.
			g.FillRectangle (myBrush, 0, 0, 500, 500);

			// Reset clipping region to infinite.
			g.ResetClip ();

			// Draw clipRect and intersectRect to screen.
			myPen.Color = Color.FromArgb (196, 0xC3, 0xC9, 0xCF);
			myBrush.Color = Color.FromArgb (127, 0xDD, 0xDD, 0xF0);
			g.DrawRectangle (myPen, clipRect);
			g.FillRectangle (myBrush, clipRect);

			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);
			g.DrawRectangle (myPen, intersectRect);
			g.FillRectangle (myBrush, intersectRect);
			title = "IntersectClipRectangle";
		}

		public void ExcludeClipRectangle (Graphics g)
		{
			// Create rectangle for exclusion.
			var excludeRect = new Rectangle (100, 100, 200, 200);
			// Set clipping region to exclude rectangle.
			g.ExcludeClip (excludeRect);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0x66, 0xEF, 0x7F));

			// Fill large rectangle to show clipping region.
			g.FillRectangle (myBrush, 0, 0, 500, 500);
			title = "ExcludeClipRectangle";
		}

		public void TranslateClip (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in blue.
			var regionRect = new Rectangle (100, 50, 100, 100);
			g.DrawRectangle (myPen, regionRect);
			g.FillRectangle (myBrush, regionRect);

			// Create a region using the first rectangle.
			var myRegion = new Region (regionRect);
			g.Clip = myRegion;

			// Apply the translation to the region.
			g.TranslateClip (150, 100);

			// Fill the transformed region with red and draw it to the screen in red.
			myBrush.Color = Color.FromArgb (127, 0x66, 0xEF, 0x7F);
			myPen.Color = Color.FromArgb (255, 0, 0x33, 0);
			g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));
			title = "TranslateClip";
		}

		void DrawRegionTranslateClip (Graphics g)
		{
			// Create rectangle for clipping region.
			var clipRect = new Rectangle (0, 0, 100, 100);

			// Set clipping region of graphics to rectangle.
			g.SetClip (clipRect);

			// Translate clipping region. 
			int dx = 50;
			int dy = 50;
			g.TranslateClip (dx, dy);

			// Fill rectangle to demonstrate translated clip region.
			g.FillRectangle (new SolidBrush (Color.Black), 0, 0, 500, 300);
			title = "DrawRegionTranslateClip";
		}

		void DrawRegionIntersectClip (Graphics g)
		{
			// Create the first rectangle and draw it to the screen in black.
			var regionRect = new Rectangle (20, 20, 100, 100);
			g.DrawRectangle (Pens.Black, regionRect);

			// create the second rectangle and draw it to the screen in red.
			var complementRect = new Rectangle (90, 30, 100, 100);
			g.DrawRectangle (Pens.Red, Rectangle.Round (complementRect));

			// Create a region using the first rectangle.
			var myRegion = new Region (regionRect);

			// Get the area of intersection for myRegion when combined with complementRect.
			myRegion.Intersect (complementRect);
			title = "DrawImageIntersetClip";
		}

		public void GraphicsIsVisibleRectangleF (Graphics g)
		{
			var myPen = new Pen (Color.FromArgb (196, 0xC3, 0xC9, 0xCF), .6f);
			var myBrush = new SolidBrush (Color.FromArgb (127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in blue.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			// Create the second rectangle and draw it to the screen in red.
			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, Rectangle.Round (regionRectF2));

			// Create a region using the first rectangle.
			var myRegion = new Region(regionRect1);

			// Determine if myRect is contained in the region. 
			bool contained = myRegion.IsVisible(regionRect2);

			// Display the result.
			var myFont = new Font ("Arial", 8);
			var txtBrush = new SolidBrush (Color.Black);
			g.DrawString ("contained = " + contained.ToString (), myFont, txtBrush, new Point (regionRectF2.Right + 10, regionRectF2.Top));

			regionRect1.Y += 120;
			regionRectF2.Y += 120;
			regionRectF2.X += 41;

			myPen.Color = Color.FromArgb (196, 0xC3, 0xC9, 0xCF);
			myBrush.Color = Color.FromArgb (127, 0xDD, 0xDD, 0xF0);

			// Create the first rectangle and draw it to the screen in blue.
			g.DrawRectangle (myPen, regionRect1);
			g.FillRectangle (myBrush, regionRect1);

			// Create the second rectangle and draw it to the screen in red.
			myPen.Color = Color.FromArgb (196, 0xF9, 0xBE, 0xA6);
			myBrush.Color = Color.FromArgb (127, 0xFF, 0xE0, 0xE0);

			g.DrawRectangle (myPen, Rectangle.Round (regionRectF2));
			g.FillRectangle (myBrush, Rectangle.Round (regionRectF2));

			// Create a region using the first rectangle.
			myRegion = new Region (regionRect1);

			// Determine if myRect is contained in the region. 
			contained = myRegion.IsVisible (regionRectF2);

			// Display the result.
			g.DrawString ("contained = " + contained.ToString (), myFont, txtBrush, new Point(regionRectF2.Right + 10, regionRectF2.Top));
			title = "GraphicsIsVisibleRectangleF";
		}
	}
}

public delegate void PaintEventHandler(object sender, PaintEventArgs e);


public class PaintEventArgs : EventArgs, IDisposable {
	readonly CGRect clipRect;
	Graphics graphics;

	public CGRect ClipRectangle {
		get {
			return clipRect;
		}
	}

	public Graphics Graphics {
		get {
			return graphics;
		}
	}

	public PaintEventArgs(Graphics graphics, CGRect clipRect)
	{
		if (graphics == null)
			throw new ArgumentNullException ("graphics");
		
		this.graphics = graphics;
		this.clipRect = clipRect;
	}

	public void Dispose()
	{
		Dispose (true);
		GC.SuppressFinalize (this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if ((disposing && (this.graphics != null)))
			this.graphics.Dispose ();
	}

	~PaintEventArgs()
	{
		Dispose (false);
	}

}
