using System;
using System.Collections;
using CoreGraphics;
//using System.Windows.Forms;
using System.Drawing;

namespace Example4_8
{
	public class DataCollection
	{
		private ArrayList dataSeriesList;
		private int dataSeriesIndex = 0;

		public DataCollection ()
		{
			dataSeriesList = new ArrayList ();
		}

		public ArrayList DataSeriesList {
			get { return dataSeriesList; }
			set { dataSeriesList = value; }
		}

		public int DataSeriesIndex {
			get { return dataSeriesIndex; }
			set { dataSeriesIndex = value; }
		}

		public void Add (DataSeries ds)
		{
			dataSeriesList.Add (ds);
			if (ds.SeriesName == "") {
				ds.SeriesName = "DataSeries" + dataSeriesList.Count.ToString ();
			}
		}

		public void Insert (int dataSeriesIndex, DataSeries ds)
		{
			dataSeriesList.Insert (dataSeriesIndex, ds);
			if (ds.SeriesName == "") {
				dataSeriesIndex = dataSeriesIndex + 1;
				ds.SeriesName = "DataSeries" + dataSeriesIndex.ToString ();
			}
		}

		public void Remove (string dataSeriesName)
		{
			if (dataSeriesList != null) {
				for (int i = 0; i < dataSeriesList.Count; i++) {
					DataSeries ds = (DataSeries)dataSeriesList [i];
					if (ds.SeriesName == dataSeriesName) {
						dataSeriesList.RemoveAt (i);
					}
				}
			}
		}

		public void RemoveAll ()
		{
			dataSeriesList.Clear ();
		}

		public void AddPolar (Graphics g, ChartStyle cs)
		{
			Rectangle rect = cs.SetPolarArea ();
			float xc = rect.X + rect.Width / 2;
			float yc = rect.Y + rect.Height / 2;
			// Plot lines:
			foreach (DataSeries ds in DataSeriesList) {
				Pen aPen = new Pen (ds.LineStyle.LineColor, ds.LineStyle.Thickness);
				aPen.DashStyle = ds.LineStyle.Pattern;
				float r = ((PointF)ds.PointList [0]).Y;
				float theta = ((PointF)ds.PointList [0]).X;
				float x = cs.RNorm (r) * (float)Math.Cos (theta * Math.PI / 180) + xc;
				float y = cs.RNorm (r) * (float)Math.Sin (theta * Math.PI / 180) + yc;
 
				if (ds.LineStyle.IsVisible == true) {
					PointF ptStart = new PointF (x, y);
					PointF ptEnd = new PointF (x, y);
					for (int i = 1; i < ds.PointList.Count; i++) {
						r = ((PointF)ds.PointList [i - 1]).Y;
						theta = ((PointF)ds.PointList [i - 1]).X;
						if (cs.AngleDirection == ChartStyle.AngleDirectionEnum.CounterClockWise) {
							theta = -theta;
						}
						x = cs.RNorm (r) * (float)Math.Cos (theta * Math.PI / 180) + xc;
						y = cs.RNorm (r) * (float)Math.Sin (theta * Math.PI / 180) + yc;
						ptStart = new PointF (x, y);
						r = ((PointF)ds.PointList [i]).Y;
						theta = ((PointF)ds.PointList [i]).X;
						if (cs.AngleDirection == ChartStyle.AngleDirectionEnum.CounterClockWise) {
							theta = -theta;
						}
						x = cs.RNorm (r) * (float)Math.Cos (theta * Math.PI / 180) + xc;
						y = cs.RNorm (r) * (float)Math.Sin (theta * Math.PI / 180) + yc;
						ptEnd = new PointF (x, y);
						g.DrawLine (aPen, ptStart, ptEnd);
					}
				}
				aPen.Dispose ();
			}
		}
	}
}
