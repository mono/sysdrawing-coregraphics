using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using CoreGraphics;
using System.Drawing;

namespace Example3_5
{
    public class DataSeries
    {
        private ArrayList pointList;
        private LineStyle lineStyle;
        private SymbolStyle symbolStyle;
        private string seriesName = "";

        public DataSeries()
        {
            lineStyle = new LineStyle();
            SymbolStyle = new SymbolStyle();
            pointList = new ArrayList();
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


