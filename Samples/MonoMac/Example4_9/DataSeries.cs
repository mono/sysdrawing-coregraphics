using System;
using CoreGraphics;

namespace Example4_9
{
    public class DataSeries
    {
        private string[,] dataString;
        private LineStyle lineStyle;
        private string seriesName = "";

        public DataSeries()
        {
            lineStyle = new LineStyle();
        }

        public LineStyle LineStyle
        {
            get { return lineStyle; }
            set { lineStyle = value; }
        }

        public string[,] DataString
        {
            get { return dataString; }
            set { dataString = value; }
        }

        public string SeriesName
        {
            get { return seriesName; }
            set { seriesName = value; }
        }
    }
}


