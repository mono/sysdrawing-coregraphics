
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;

namespace RegionTests
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


			// We need to set these two view properites so that when we clear an infinite region
			// it actually clears to the background color.
			// If not set this may set the rectangle to Black depending on the context
			// passed.  
			this.WantsLayer = true;
			Layer.BackgroundColor = new CGColor (0, 0, 0, 0);


			var mainBundle = NSBundle.MainBundle;
//			var filePath = mainBundle.PathForResource("bitmap50","png");
//			bmp = Bitmap.FromFile(filePath);
//
//			filePath = mainBundle.PathForResource("bitmap25","png");
//			bmp2 = Bitmap.FromFile(filePath);
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


		Rectangle regionRect1 = new Rectangle(50, 50, 100, 100);
		RectangleF regionRectF1 = new RectangleF(110, 60, 100, 100);
		Rectangle regionRect2 = new Rectangle(110, 60, 100, 100);
		RectangleF regionRectF2 = new RectangleF(110, 60, 100, 100);


		int currentView = 0;
		int totalViews = 20;

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
			Graphics g = new Graphics();
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

			}

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
		void DrawRegionTranslateClip(Graphics g)
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
			title = "DrawRegionTranslateClip";
		}

		void DrawRegionIntersectClip(Graphics g)
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

//			var unionRect = complementRect.UnionWith (regionRect);
//			g.DrawRectangle(Pens.Green,
//			                Rectangle.Round(unionRect));
//
//			g.Clip = myRegion;
//
//			g.DrawImage(bmp2, unionRect);

			title = "DrawImageIntersetClip";
		}

	}
}

