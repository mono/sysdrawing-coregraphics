
using System;
using System.Collections.Generic;
using System.Linq;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using Foundation;

namespace DrawingShared
{

    public partial class DrawingView 
    {

        void PlatformInitialize()
        {

            // Load our painting view methods.
            paintViewActions = new Action<Graphics>[]
                {
                    TextureBrush1,
                };
        }

        protected void OnPaint(PaintEventArgs e)
        {
            Graphics g = Graphics.FromCurrentContext();
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            g.Clear(Color.Wheat);

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

        void TextureBrush1 (Graphics g)
        {

            var mainBundle = NSBundle.MainBundle;
            var filePath = mainBundle.PathForResource("CocoaMono","png");

            var bitmap = Image.FromFile(filePath);

            filePath = mainBundle.PathForResource("HouseAndTree","gif");

            Image texture = new Bitmap(filePath);
            g.DrawImage(texture, new Point(20,20));

            var textureBrush = new TextureBrush(texture);
            Pen blackPen = new Pen(Color.Black);
            var rect = new RectangleF(20,100,200,200);
            //textureBrush.WrapMode = WrapMode.TileFlipXY;

            g.FillRectangle(textureBrush, rect);
            g.DrawRectangle(blackPen, rect.X, rect.Y, rect.Width, rect.Height);

            var cocoaMonoTexture = new Bitmap(bitmap, texture.Size);

            g.DrawImage(cocoaMonoTexture, new Point(300,20));

            var textureBrush2 = new TextureBrush(cocoaMonoTexture);
            Pen bluePen = new Pen(Color.Blue);
            var rectt = new RectangleF(300,100,200,200);

            textureBrush2.WrapMode = WrapMode.TileFlipXY;

            g.FillRectangle(textureBrush2, rectt);
            g.DrawRectangle(bluePen, rectt.X, rectt.Y, rectt.Width, rectt.Height);


            // Scale the large image down to 75 x 75 before tiling
            TextureBrush tBrush = new TextureBrush(bitmap);
            tBrush.Transform = new Matrix(
              75.0f / bitmap.Width,
              0.0f,
              0.0f,
              75.0f / bitmap.Height,
              0.0f,
              0.0f);
            g.FillEllipse(tBrush, new Rectangle(300, 320, 150, 250));



        }

	}
}

