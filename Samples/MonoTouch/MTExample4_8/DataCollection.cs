using System;
using System.Collections;
using System.Drawing;
using CoreGraphics;

namespace MTExample4_8 {
	public class DataCollection {
		ArrayList dataSeriesList;
		int dataSeriesIndex = 0;

		public DataCollection ()
		{
			dataSeriesList = new ArrayList ();
		}

		public ArrayList DataSeriesList {
			get {
				return dataSeriesList;
			}
			set {
				dataSeriesList = value;
			}
		}

		public int DataSeriesIndex {
			get {
				return dataSeriesIndex;
			}
			set {
				dataSeriesIndex = value;
			}
		}

		public void Add (DataSeries ds)
		{
			dataSeriesList.Add (ds);
			if (ds.SeriesName == string.Empty)
				ds.SeriesName = "DataSeries" + dataSeriesList.Count.ToString ();
		}

		public void Insert (int dataSeriesIndex, DataSeries ds)
		{
			dataSeriesList.Insert (dataSeriesIndex, ds);
			if (ds.SeriesName == string.Empty) {
				dataSeriesIndex = dataSeriesIndex + 1;
				ds.SeriesName = "DataSeries" + dataSeriesIndex.ToString ();
			}
		}

		public void Remove (string dataSeriesName)
		{
			if (dataSeriesList != null) {
				for (int i = 0; i < dataSeriesList.Count; i++) {
					var ds = (DataSeries)dataSeriesList [i];
					if (ds.SeriesName == dataSeriesName)
						dataSeriesList.RemoveAt (i);
				}
			}
		}

		public void RemoveAll ()
		{
			dataSeriesList.Clear ();
		}

		public void AddPolar (Graphics g, ChartStyle cs)
		{
			CGRect rect = cs.SetPolarArea ();
			var xc = (float)(rect.X + rect.Width / 2);
			var yc = (float)(rect.Y + rect.Height / 2);
			// Plot lines:
			foreach (DataSeries ds in DataSeriesList) {
				var aPen = new Pen (ds.LineStyle.LineColor, ds.LineStyle.Thickness);
				aPen.DashStyle = ds.LineStyle.Pattern;
				nfloat r = ((CGPoint)ds.PointList [0]).Y;
				nfloat theta = ((CGPoint)ds.PointList [0]).X;
				float x = cs.RNorm ((float)r) * (float)Math.Cos (theta * Math.PI / 180) + xc;
				float y = cs.RNorm ((float)r) * (float)Math.Sin (theta * Math.PI / 180) + yc;
 
				if (ds.LineStyle.IsVisible == true) {
					var ptStart = new PointF (x, y);
					var ptEnd = new PointF (x, y);
					for (int i = 1; i < ds.PointList.Count; i++) {
						r = ((CGPoint)ds.PointList [i - 1]).Y;
						theta = ((CGPoint)ds.PointList [i - 1]).X;
						if (cs.AngleDirection == ChartStyle.AngleDirectionEnum.CounterClockWise) {
							theta = -theta;
						}
						x = cs.RNorm ((float)r) * (float)Math.Cos (theta * Math.PI / 180) + xc;
						y = cs.RNorm ((float)r) * (float)Math.Sin (theta * Math.PI / 180) + yc;
						ptStart = new PointF (x, y);
						r = ((CGPoint)ds.PointList [i]).Y;
						theta = ((CGPoint)ds.PointList [i]).X;
						if (cs.AngleDirection == ChartStyle.AngleDirectionEnum.CounterClockWise) {
							theta = -theta;
						}
						x = cs.RNorm ((float)r) * (float)Math.Cos (theta * Math.PI / 180) + xc;
						y = cs.RNorm ((float)r) * (float)Math.Sin (theta * Math.PI / 180) + yc;
						ptEnd = new PointF (x, y);
						g.DrawLine (aPen, ptStart, ptEnd);
					}
				}
				aPen.Dispose ();
			}
		}
	}
}
