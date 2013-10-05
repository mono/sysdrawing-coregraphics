
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoTouch.CoreGraphics;

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

		Rectangle src = new Rectangle(0, 0, 50, 50);
		RectangleF srcF = new Rectangle(0, 0, 50, 50);
		Rectangle dst = new Rectangle(170, 170, 100, 100);
		RectangleF dstF = new Rectangle(270, 270, 100, 100);

		int currentView = 0;
		int totalViews = 1;

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

			g.Clear(Color.Wheat);
			//g.SmoothingMode = SmoothingMode.None;
			switch (currentView) 
			{
			case 0:
				DrawRegionTranslateClip (g);
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

