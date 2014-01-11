using System;
using System.Collections.Generic;
using System.DrawingNative;
using System.DrawingNative.Drawing2D;
using System.Text;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;
namespace Example4_9
{
    public class LineStyle
    {
        private DashStyle linePattern = DashStyle.Solid;
        private Color lineColor = Color.Black;
        private float LineThickness = 1.0f;
        private PlotLinesMethodEnum pltLineMethod = PlotLinesMethodEnum.Lines;
        private bool isVisible = true;

        public LineStyle()
        {
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public PlotLinesMethodEnum PlotMethod
        {
            get { return pltLineMethod; }
            set { pltLineMethod = value; }
        }

        virtual public DashStyle Pattern
        {
            get { return linePattern; }
            set { linePattern = value; }
        }

        public float Thickness
        {
            get { return LineThickness; }
            set { LineThickness = value; }
        }

        virtual public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        public enum PlotLinesMethodEnum
        {
            Lines = 0,
            Splines = 1
        }
    }
}


