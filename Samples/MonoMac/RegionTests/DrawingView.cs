
using System;
using System.Collections.Generic;
using System.Linq;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace DrawingShared
{
	public partial class DrawingView 
	{

		Font anyKeyFont = new Font("Chalkduster", 18, FontStyle.Bold);
		Font clipFont = new Font("Helvetica",12, FontStyle.Bold);

		Image bmp;
		Image bmp2;


		Rectangle regionRect1 = new Rectangle(50, 50, 100, 100);
		RectangleF regionRectF1 = new RectangleF(110, 60, 100, 100);
		Rectangle regionRect2 = new Rectangle(110, 60, 100, 100);
		RectangleF regionRectF2 = new RectangleF(110, 60, 100, 100);

        void PlatformInitialize()
        {

            // Load our painting view methods.
            paintViewActions = new Action<Graphics>[]
                {


                    FillRegionInfinite,
                    FillRegionEmpty,
                    FillRegion1,
                    FillRegionIntersect,
                    FillRegionUnion,
                    FillRegionExclude,
                    FillRegionXor,
                    FillRegionInfiniteIntersect,
                    FillRegionInfiniteUnion,
                    FillRegionInfiniteExclude,
                    FillRegionInfiniteXor,
                    FillRegionEmptyIntersect,
                    FillRegionEmptyUnion,
                    FillRegionEmptyExclude,
                    FillRegionEmptyXor,
                    TranslateRegion,
                    TransformRegion,
                    RegionIsVisibleRectangleF,
                    UnionGraphicsPath,
                    XorGraphicsPath,
                    IntersectGraphicsPath,
                    ExcludeGraphicsPath1,
                    ExcludeGraphicsPath2,
                    GraphicsPathAndRegions,
                    ClipUsingRegions1,
                    ClipUsingRegions2,


                };
        }

        protected void OnPaint(PaintEventArgs e)
        {
            Graphics g = Graphics.FromCurrentContext();
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            paintViewActions[currentView].Invoke(g);
            if (saveCurrentView)
                SavePaintView(paintViewActions[currentView]);

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

            #if __MAC__
            var anyKey = "Press any key to continue.";
            #endif
            #if __IOS__
            var anyKey = "Tap Screen to continue.";
            #endif

            var anyKeySize = g.MeasureString(anyKey, anyKeyFont);
            anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
            anyKeyPoint.Y = ClientRectangle.Height - (anyKeySize.Height + 10);
            g.DrawString(anyKey, anyKeyFont, sBrush, anyKeyPoint );

            var title = paintViewActions[currentView].Method.Name;
            anyKeySize = g.MeasureString(title, anyKeyFont);
            anyKeyPoint.X = (ClientRectangle.Width / 2) - (anyKeySize.Width / 2);
            anyKeyPoint.Y -= anyKeySize.Height;
            g.DrawString(title, anyKeyFont, sBrush, anyKeyPoint );

            g.Dispose();

        }

        Pen penBluish = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);

		void FillRegionInfinite(Graphics g)
		{

			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in myPen color.
			g.DrawRectangle(penBluish, regionRect1);

			// Create a region using the first rectangle.
			Region myRegion = new Region();

			g.FillRegion(myBrush, myRegion);

		}

		void FillRegionEmpty(Graphics g)
		{

			SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

			// Create the first rectangle and draw it to the screen in myPen color.
			g.DrawRectangle(penBluish, regionRect1);

			// Create a region using the first rectangle.
			Region myRegion = new Region(regionRect1);
			myRegion.MakeEmpty ();

			// Fill the region which basically clears the screen to the background color.
			g.FillRegion(myBrush, myRegion);

		}

		void FillRegion1(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();

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

		}

		void FillRegionIntersect(Graphics g)
		{

			// Greyish blue
            Pen myPen = (Pen)penBluish.Clone();
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

		}

		void FillRegionUnion(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		void FillRegionExclude(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		void FillRegionXor(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		void FillRegionInfiniteIntersect(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		void FillRegionInfiniteUnion(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		
		void FillRegionInfiniteExclude(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		void FillRegionInfiniteXor(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		
		void FillRegionEmptyIntersect(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		void FillRegionEmptyUnion(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}


		void FillRegionEmptyExclude(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		void FillRegionEmptyXor(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		public void TranslateRegion(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

		public void TransformRegion(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

        public void RegionIsVisibleRectangleF(Graphics g)
		{

            Pen myPen = (Pen)penBluish.Clone();
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

		}

        void UnionGraphicsPath(Graphics g)
        {
            Rectangle r1 = new Rectangle(10, 10, 50, 50);
            Rectangle r2 = new Rectangle(40, 40, 50, 50);
            Region r = new Region(r1);
            r.Union(r2);

            g.FillRegion(Brushes.Red, r);
            GraphicsPath path = new GraphicsPath(new Point[] {new Point(45, 45),
                new Point(145, 55),
                new Point(200, 150),
                new Point(75, 150),
                new Point(45, 45)
            }, new byte[] {  (byte)PathPointType.Start,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Line
            });

            g.FillRegion(Brushes.Purple, new Region(path));

            r.Union(path);
            g.FillRegion(Brushes.Blue, r);

        }

        void XorGraphicsPath(Graphics g)
        {
            Rectangle r1 = new Rectangle(10, 10, 50, 50);
            Rectangle r2 = new Rectangle(40, 40, 50, 50);
            Region r = new Region(r1);
            r.Union(r2);

            g.FillRegion(Brushes.Red, r);
            GraphicsPath path = new GraphicsPath(new Point[] {new Point(45, 45),
                new Point(145, 55),
                new Point(200, 150),
                new Point(75, 150),
                new Point(45, 45)
            }, new byte[] {  (byte)PathPointType.Start,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Line
            });

            g.FillRegion(Brushes.Purple, new Region(path));

            r.Xor(path);
            g.FillRegion(Brushes.Blue, r);

        }

        void IntersectGraphicsPath(Graphics g)
        {
            Rectangle r1 = new Rectangle(10, 10, 50, 50);
            Rectangle r2 = new Rectangle(40, 40, 50, 50);
            Region r = new Region(r1);
            r.Union(r2);

            g.FillRegion(Brushes.Red, r);
            GraphicsPath path = new GraphicsPath(new Point[] {new Point(45, 45),
                new Point(145, 55),
                new Point(200, 150),
                new Point(75, 150),
                new Point(45, 45)
            }, new byte[] {  (byte)PathPointType.Start,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Line
            });

            g.FillRegion(Brushes.Purple, new Region(path));

            r.Intersect(path);
            g.FillRegion(Brushes.Blue, r);

        }

        void ExcludeGraphicsPath1(Graphics g)
        {
            Rectangle r1 = new Rectangle(10, 10, 50, 50);
            Rectangle r2 = new Rectangle(40, 40, 50, 50);
            Region r = new Region(r1);
            r.Union(r2);

            g.FillRegion(Brushes.Red, r);
            GraphicsPath path = new GraphicsPath(new Point[] {new Point(45, 45),
                new Point(145, 55),
                new Point(200, 150),
                new Point(75, 150),
                new Point(45, 45)
            }, new byte[] {  (byte)PathPointType.Start,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Line
            });

            r.Exclude(path);
            g.FillRegion(Brushes.Blue, r);

        }

        void ExcludeGraphicsPath2(Graphics g)
        {
            Rectangle r1 = new Rectangle(10, 10, 50, 50);
            Rectangle r2 = new Rectangle(40, 40, 50, 50);
            Region r = new Region(r1);
            r.Union(r2);

            g.FillRegion(Brushes.Red, r);
            GraphicsPath path = new GraphicsPath(new Point[] {new Point(45, 45),
                new Point(145, 55),
                new Point(200, 150),
                new Point(75, 150),
                new Point(45, 45)
            }, new byte[] {  (byte)PathPointType.Start,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Bezier,
                (byte)PathPointType.Line
            });

            g.FillRegion(Brushes.Purple, new Region(path));

            r.Exclude(path);
            g.FillRegion(Brushes.Blue, r);

        }

        void GraphicsPathAndRegions(Graphics g)
        {
            g.TranslateTransform(50, 100);
            GraphicsPath gp = new GraphicsPath();
            gp.AddPolygon(new Point[]{
                new Point(0,30),
                new Point(30,0),
                new Point(60,0),
                new Point(73,15),
                new Point(88,0),
                new Point(115,0),
                new Point(140,30),
                new Point(140,53),
                new Point(108,90),
                new Point(82,90),
                new Point(67,73),
                new Point(50,90),
                new Point(30,90),
                new Point(0,60)
            });


            gp.AddPolygon(new Point[]{
                new Point(30,30),
                new Point(20,40),
                new Point(32,55),
                new Point(43,43)
            });
            gp.AddPolygon(new Point[]{
                new Point(110,30),
                new Point(125,45),
                new Point(115,55),
                new Point(100,40)
            });

            g.DrawPath(Pens.Black, gp);

            //Creates a new region from the path
            Region rgn = new Region(gp);
            LinearGradientBrush brush =
                new LinearGradientBrush(new Point(0, 0),
                    new Point(100, 100),
                    Color.LightSteelBlue,
                    Color.CornflowerBlue);
            //Fills the region with the gradient brush
            g.FillRegion(brush, rgn);
            brush.Dispose();

            //Creates the path around the red region
            GraphicsPath gpr = new GraphicsPath();
            gpr.AddPolygon(new Point[]{
                new Point(88,0),
                new Point(115,0),
                new Point(115,25),
                new Point(50,90),
                new Point(30,90),
                new Point(30,55)
            });

            var colorToLighten = Color.Red;
            float correctionFactor = 0.25f;
            float red = (255 - colorToLighten.R) * correctionFactor + colorToLighten.R;
            float green = (255 - colorToLighten.G) * correctionFactor + colorToLighten.G;
            float blue = (255 - colorToLighten.B) * correctionFactor + colorToLighten.B;
            Color lighterColor = Color.FromArgb(colorToLighten.A, (int)red, (int)green, (int)blue);

            LinearGradientBrush rBrush =
                new LinearGradientBrush(new Point(0, 0),
                    new Point(100, 100),
                    lighterColor,
                    Color.Red);
            g.FillRegion(rBrush, new Region(gpr));
            rBrush.Dispose();
            g.DrawPath(Pens.Black, gp);
        }

        void ClipUsingRegions1(Graphics g)
        {

            //Creates a new graphics path and adds a circle figure
            GraphicsPath c1 = new GraphicsPath();
            c1.AddEllipse(0, 0, 100, 100);
            //Create the gradient brush
            LinearGradientBrush brush =
                new LinearGradientBrush(new Point(0, 0), new Point(150, 120),
                    Color.Red, Color.Black);

            var rect = ClientRectangle;
            //Fills the rectangle of the control with the given brush
            g.FillRectangle(brush, rect);
            //Draws a diagonal line over the circle
            g.DrawLine(Pens.Lime, 0, 0, 150, 120);
            //Draws the circle to make it visible.
            g.DrawPath(Pens.White, c1);
            brush.Dispose();

            Region clip = new Region(c1);
            //Replaces the current clipping region to the region of the circle
            g.SetClip(clip, CombineMode.Replace);

            //Draws a diagonal line.
            g.DrawLine(Pens.Magenta, 0, 0, 250, 200);

        }
        void ClipUsingRegions2(Graphics g)
        {

            //Creates a new graphics path and adds a circle figure
            GraphicsPath c1 = new GraphicsPath();
            c1.AddEllipse(0, 0, 100, 100);
            //Creates a new graphics path and adds a circle figure
            GraphicsPath c2 = new GraphicsPath();
            c2.AddEllipse(50, 0, 100, 100);
            Region r1 = new Region(c1);
            //Unions r1 with c2 and removes their intersection
            r1.Xor(c2);
            LinearGradientBrush brush =
                new LinearGradientBrush(new Point(0, 0), new Point(150, 120),
                    Color.Black, Color.Red);
            //fills the ellipse xor region with the brush created above
            g.FillRegion(brush, r1);
            //Clips the rendering area so it excludes r1
            g.SetClip(r1, CombineMode.Exclude);
            LinearGradientBrush fillBrush =
                new LinearGradientBrush(new Point(0, 0),
                    new Point(150, 120),
                    Color.Red,
                    Color.Black);
            //Fills the rectangle of the control with the given brush
            g.FillRectangle(fillBrush, ClientRectangle);
            //Draws a diagonal line.
            g.DrawLine(Pens.Lime, 0, 0, 150, 120);
        }

	}
}

