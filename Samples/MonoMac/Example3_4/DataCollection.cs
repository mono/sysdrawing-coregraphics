using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Example3_4
{
    public class DataCollection
    {
        private ArrayList dataSeriesList;
        private int dataSeriesIndex = 0;

        public DataCollection()
        {
            dataSeriesList = new ArrayList();
        }

        public ArrayList DataSeriesList
        {
            get { return dataSeriesList; }
            set { dataSeriesList = value; }
        }
        public int DataSeriesIndex
        {
            get { return dataSeriesIndex; }
            set { dataSeriesIndex = value; }
        }

        public void Add(DataSeries ds)
        {
            dataSeriesList.Add(ds);
            if (ds.SeriesName == "Default Name")
            {
                ds.SeriesName = "DataSeries" + dataSeriesList.Count.ToString();
            }
        }

        public void Insert(int dataSeriesIndex, DataSeries ds)
        {
            dataSeriesList.Insert(dataSeriesIndex, ds);
            if (ds.SeriesName == "Default Name")
            {
                dataSeriesIndex = dataSeriesIndex + 1;
                ds.SeriesName = "DataSeries" + dataSeriesIndex.ToString();
            }
        }

        public void Remove(string dataSeriesName)
        {
            if (dataSeriesList != null)
            {
                for (int i = 0; i < dataSeriesList.Count; i++)
                {
                    DataSeries ds = (DataSeries)dataSeriesList[i];
                    if (ds.SeriesName == dataSeriesName)
                    {
                        dataSeriesList.RemoveAt(i);
                    }
                }
            }
        }

        public void RemoveAll()
        {
            dataSeriesList.Clear();
        }

        public void AddLines(Graphics g, ChartStyle cs)
        {
            // Plot lines:
            foreach (DataSeries ds in DataSeriesList)
            {
                if (ds.LineStyle.IsVisible == true)
                {
                    Pen aPen = new Pen(ds.LineStyle.LineColor, ds.LineStyle.Thickness);
                    aPen.DashStyle = ds.LineStyle.Pattern;
                    for (int i = 1; i < ds.PointList.Count; i++)
                    {
                        g.DrawLine(aPen, cs.Point2D((PointF)ds.PointList[i - 1]),
                                         cs.Point2D((PointF)ds.PointList[i]));
                    }
                    aPen.Dispose();
                }
            }
        }
    }
}
