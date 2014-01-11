using System;
using System.Collections;
using System.DrawingNative;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;
namespace Example4_1
{
    public class DataSeries
    {
        private ArrayList pointList;
        private BarStyle barStyle;
        private string seriesName = "";

        public DataSeries()
        {
            barStyle = new BarStyle();
            pointList = new ArrayList();
        }

        public BarStyle BarStyle
        {
            get { return barStyle; }
            set { barStyle = value; }
        }

        public ArrayList PointList
        {
            get { return pointList; }
            set { pointList = value; }
        }

        public void AddPoint(PointF pt)
        {
            pointList.Add(pt);
        }

        public string SeriesName
        {
            get { return seriesName; }
            set { seriesName = value; }
        }
    }
}


