using System;
using System.Collections.Generic;
using System.DrawingNative;
using System.DrawingNative.Drawing2D;
using System.Text;

namespace Example3_3
{
    public class LineStyle
    {
        private DashStyle linePattern = DashStyle.Solid;
        private Color lineColor = Color.Black;
        private float LineThickness = 1.0f;
        private bool isVisible = true;

        public LineStyle()
        {
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
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
    }
}


