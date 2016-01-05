
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

        void PlatformInitialize()
        {

            // Load our painting view methods.
            paintViewActions = new Action<Graphics>[]
                {
                    GraphicsSave1,
                    GraphicsSave2,
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

        void GraphicsSave1(Graphics g)
        {
            GraphicsState state1, state2;

            g.RotateTransform(30.0f);
            state1 = g.Save();
            g.TranslateTransform(100.0f, 0.0f, MatrixOrder.Append);
            state2 = g.Save();
            g.ScaleTransform(1.0f, 3.0f, MatrixOrder.Append);

            // Draw an ellipse. 
            // Three transformations apply: rotate, then translate, then scale.
            Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0));
            g.DrawEllipse(redPen, 0, 0, 100, 20);

            // Restore to state2 and draw the ellipse again. 
            // Two transformations apply: rotate then translate.
            g.Restore(state2);
            Pen greenPen = new Pen(Color.FromArgb(255, 0, 255, 0));
            g.DrawEllipse(greenPen, 0, 0, 100, 20);

            // Restore to state1 and draw the ellipse again. 
            // Only the rotation transformation applies.
            g.Restore(state1);
            Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255));
            g.DrawEllipse(bluePen, 0, 0, 100, 20);

        }

        void GraphicsSave2(Graphics graphics)
        {
            GraphicsState state1, state2;

            graphics.RotateTransform(30.0f);
            state1 = graphics.Save();
            graphics.TranslateTransform(100.0f, 0.0f, MatrixOrder.Append);
            state2 = graphics.Save();
            graphics.ScaleTransform(1.0f, 3.0f, MatrixOrder.Append);

            // Draw an ellipse. 
            // Three transformations apply: rotate, then translate, then scale.
            Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0));
            graphics.DrawEllipse(redPen, 0, 0, 100, 20);

            // Restore to state1 and draw the ellipse again. 
            // Only the rotation transformation applies.
            graphics.Restore(state1);
            Pen greenPen = new Pen(Color.FromArgb(255, 0, 255, 0));
            graphics.DrawEllipse(greenPen, 0, 0, 100, 20);

            // The information block identified by state2 has been lost.
            // The following call to Restore has no effect because
            // Restore(state1) removed from the stack the
            // information blocks identified by state1 and state2.
            graphics.Restore(state2);

            // The Graphics object is still in the state identified by state1.
            // The following code draws a blue ellipse on top of the previously
            // drawn green ellipse.
            Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255));
            graphics.DrawEllipse(bluePen, 0, 0, 100, 20);

        }


	}
}

