using System;
using System.Collections;
using System.Drawing;

namespace Example4_8
{
	public class DataSeries
	{
		private ArrayList pointList;
		private LineStyle lineStyle;
		private string seriesName = "";

		public DataSeries ()
		{
			lineStyle = new LineStyle ();
			pointList = new ArrayList ();
		}

		public LineStyle LineStyle {
			get { return lineStyle; }
			set { lineStyle = value; }
		}

		public ArrayList PointList {
			get { return pointList; }
			set { pointList = value; }
		}

		public void AddPoint (PointF pt)
		{
			pointList.Add (pt);
		}

		public string SeriesName {
			get { return seriesName; }
			set { seriesName = value; }
		}
	}
}


