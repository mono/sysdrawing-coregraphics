
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
        Rectangle regionRect1 = new Rectangle(50, 50, 100, 100);
        RectangleF regionRectF1 = new RectangleF(110, 60, 100, 100);
        Rectangle regionRect2 = new Rectangle(110, 60, 100, 100);
        RectangleF regionRectF2 = new RectangleF(110, 60, 100, 100);

        void PlatformInitialize()
        {

            // Load our painting view methods.
            paintViewActions = new Action<Graphics>[]
                {
                    ClipRegionInfinite,
                    ClipRegionEmpty,
                    ClipRegion1,
                    ClipRegionIntersect,
                    ClipRegionUnion,
                    ClipRegionExclude,
                    ClipRegionXor,
                    ClipRegionInfiniteIntersect,
                    ClipRegionInfiniteUnion,
                    ClipRegionInfiniteExclude,
                    ClipRegionInfiniteXor,
                    ClipRegionEmptyIntersect,
                    ClipRegionEmptyUnion,
                    ClipRegionEmptyExclude,
                    ClipRegionEmptyXor,
                    IntersectClipRectangle,
                    ExcludeClipRectangle,
                    TranslateClip,
 
                };
        }

        protected void OnPaint(PaintEventArgs e)
        {
            Graphics g = Graphics.FromCurrentContext();
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            g.Clear(Color.White);

            paintViewActions[currentView].Invoke(g);
            if (saveCurrentView)
                SavePaintView(paintViewActions[currentView]);

            g.ResetTransform ();
            Brush sBrush = Brushes.Black;

//            if (!g.IsClipEmpty) 
//            {
                var clipPoint = PointF.Empty;
                var clipString = string.Format("Clip-{0}", g.ClipBounds);
                g.ResetClip ();
                var clipSize = g.MeasureString(clipString, clipFont);
                clipPoint.X = (ClientRectangle.Width / 2) - (clipSize.Width / 2);
                clipPoint.Y = 5;
                g.DrawString(clipString, clipFont, sBrush, clipPoint );
//            }

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

        void ClipRegionInfinite(Graphics g)
        {

            Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

            // Create the first rectangle and draw it to the screen in myPen color.
            g.DrawRectangle(myPen, regionRect1);

            // Create a region using the first rectangle.
            Region myRegion = new Region();

            g.Clip = myRegion;

            g.Clear (Color.Red);

        }

        void ClipRegionEmpty(Graphics g)
        {

            Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

            // Create the first rectangle and draw it to the screen in myPen color.
            g.DrawRectangle(myPen, regionRect1);

            // Create a region using the first rectangle.
            Region myRegion = new Region(regionRect1);
            myRegion.MakeEmpty ();

            g.Clip = myRegion;

            g.Clear (Color.Red);

        }

        void ClipRegion1(Graphics g)
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

            g.Clip = myRegion;

            // Greenish
            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

            // Clear intersection area of myRegion with blue.
            g.Clear(myBrush.Color);

            g.SetClip (regionRect2);

            myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);
            g.FillRectangle(myBrush, new Rectangle(regionRect2.X + 50,regionRect2.Y + 50,250,300));

        }

        void ClipRegionIntersect(Graphics g)
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

            g.Clip = myRegion;

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }

        void ClipRegionUnion(Graphics g)
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

            g.Clip = myRegion;

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }

        void ClipRegionExclude(Graphics g)
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

            g.Clip = myRegion;

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));


        }

        void ClipRegionXor(Graphics g)
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

            g.Clip = myRegion;

            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }

        void ClipRegionInfiniteIntersect(Graphics g)
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

            g.Clip = myRegion;

            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }

        void ClipRegionInfiniteUnion(Graphics g)
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

            g.Clip = myRegion;

            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }


        void ClipRegionInfiniteExclude(Graphics g)
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

            g.Clip = myRegion;

            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }

        void ClipRegionInfiniteXor(Graphics g)
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

            g.Clip = myRegion;

            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }


        void ClipRegionEmptyIntersect(Graphics g)
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

        }

        void ClipRegionEmptyUnion(Graphics g)
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

            g.Clip = myRegion;

            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }


        void ClipRegionEmptyExclude(Graphics g)
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

            g.Clip = myRegion;

            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }

        void ClipRegionEmptyXor(Graphics g)
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

            g.Clip = myRegion;

            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);

            g.FillRectangle (myBrush, new Rectangle (0, 0, 500, 500));

        }

        public void IntersectClipRectangle(Graphics g)
        {

            Pen myPen = new Pen(Color.FromArgb(255, 0, 0x33, 0), (float)0.6);
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0x66, 0xEF, 0x7F));

            // Set clipping region.
            Rectangle clipRect = new Rectangle(0, 0, 200, 200);
            Region clipRegion = new Region(clipRect);
            g.SetClip(clipRegion, CombineMode.Replace);

            // Update clipping region to intersection of 

            // existing region with specified rectangle.
            Rectangle intersectRect = new Rectangle(100, 100, 200, 200);
            Region intersectRegion = new Region(intersectRect);
            g.IntersectClip(intersectRegion);

            // Fill rectangle to demonstrate effective clipping region.
            g.FillRectangle(myBrush, 0, 0, 500, 500);

            // Reset clipping region to infinite.
            g.ResetClip();

            // Draw clipRect and intersectRect to screen.
            myPen.Color = Color.FromArgb(196, 0xC3, 0xC9, 0xCF);
            myBrush.Color = Color.FromArgb(127, 0xDD, 0xDD, 0xF0);
            g.DrawRectangle(myPen, clipRect);
            g.FillRectangle(myBrush, clipRect);

            myPen.Color = Color.FromArgb(196, 0xF9, 0xBE, 0xA6);
            myBrush.Color = Color.FromArgb(127, 0xFF, 0xE0, 0xE0);

            g.DrawRectangle(myPen, intersectRect);
            g.FillRectangle(myBrush, intersectRect);

        }

        public void ExcludeClipRectangle(Graphics g)
        {

            // Create rectangle for exclusion.
            Rectangle excludeRect = new Rectangle(100, 100, 200, 200);

            // Set clipping region to exclude rectangle.
            g.ExcludeClip(excludeRect);

            var myBrush = new SolidBrush(Color.FromArgb(127, 0x66, 0xEF, 0x7F));

            // Fill large rectangle to show clipping region.
            g.FillRectangle(myBrush, 0, 0, 500, 500);

        }



        public void TranslateClip (Graphics g)
        {

            Pen myPen = new Pen(Color.FromArgb(196, 0xC3, 0xC9, 0xCF), (float)0.6);
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(127, 0xDD, 0xDD, 0xF0));

            // Create the first rectangle and draw it to the screen in blue.
            Rectangle regionRect = new Rectangle(100, 50, 100, 100);
            g.DrawRectangle(myPen, regionRect);
            g.FillRectangle (myBrush, regionRect);

            // Create a region using the first rectangle.
            Region myRegion = new Region(regionRect);

            g.Clip = myRegion;

            // Apply the translation to the region.
            g.TranslateClip(150, 100);

            // Fill the transformed region with red and draw it to the screen in red.
            myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
            myPen.Color = Color.FromArgb(255, 0, 0x33, 0);
            g.FillRectangle(myBrush, new Rectangle(0,0,500,500) );

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

            //          var unionRect = complementRect.UnionWith (regionRect);
            //          g.DrawRectangle(Pens.Green,
            //                          Rectangle.Round(unionRect));
            //
            //          g.Clip = myRegion;
            //
            //          g.DrawImage(bmp2, unionRect);

        }

        public void GraphicsIsVisibleRectangleF(Graphics g)
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

        }
	}
}

