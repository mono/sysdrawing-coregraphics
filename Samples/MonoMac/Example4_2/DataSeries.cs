using System;
using System.Collections;
using System.Drawing;

namespace Example4_2
{
    public class DataSeries
    {
        private ArrayList pointList;
        private BarStyle barStyle;
        private string seriesName = "";
        private bool isColorMap = false;
        private bool isSingleColorMap = false;
        private int[,] cmap;

        public DataSeries()
        {
            barStyle = new BarStyle();
            pointList = new ArrayList();
        }

        public int[,] CMap
        {
            get { return cmap; }
            set { cmap = value; }
        }

        public bool IsColorMap
        {
            get { return isColorMap; }
            set { isColorMap = value; }
        }

        public bool IsSingleColorMap
        {
            get { return isSingleColorMap; }
            set { isSingleColorMap = value; }
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


