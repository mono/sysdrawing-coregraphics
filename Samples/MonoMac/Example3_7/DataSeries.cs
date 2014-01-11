using System;
using System.Collections;
using System.DrawingNative;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;

namespace Example3_7
{
    public class DataSeries
    {
        private ArrayList pointList;
        private LineStyle lineStyle;
        private SymbolStyle symbolStyle;
        private string seriesName = "";
        private bool isY2Data = false;

        public DataSeries()
        {
            lineStyle = new LineStyle();
            SymbolStyle = new SymbolStyle();
            pointList = new ArrayList();
        }

        public bool IsY2Data
        {
            get { return isY2Data; }
            set { isY2Data = value; }
        }

        public LineStyle LineStyle
        {
            get { return lineStyle; }
            set { lineStyle = value; }
        }

        public SymbolStyle SymbolStyle
        {
            get { return symbolStyle; }
            set { symbolStyle = value; }
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


