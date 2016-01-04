using System.Collections;
using System.Drawing;
using CoreGraphics;

namespace MTExample3_5 {
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

		public void Add(DataSeries ds)
		{
			dataSeriesList.Add(ds);
			if (ds.SeriesName == string.Empty)
				ds.SeriesName = string.Format ("DataSeries{0}", dataSeriesList.Count.ToString ());
		}

		public void Insert (int dataSeriesIndex, DataSeries ds)
		{
			dataSeriesList.Insert(dataSeriesIndex, ds);
			if (ds.SeriesName == string.Empty) {
				dataSeriesIndex = dataSeriesIndex + 1;
				ds.SeriesName = string.Format ("DataSeries{0}", dataSeriesIndex.ToString ());
			}
		}

		public void Remove (string dataSeriesName)
		{
			if (dataSeriesList != null) {
				for (int i = 0; i < dataSeriesList.Count; i++) {
					var ds = (DataSeries)dataSeriesList[i];
					if (ds.SeriesName == dataSeriesName)
						dataSeriesList.RemoveAt (i);
				}
			}
		}

		public void RemoveAll ()
		{
			dataSeriesList.Clear();
		}

		public void AddLines (Graphics g, ChartStyle cs)
		{
			// Plot lines:
			foreach (DataSeries ds in DataSeriesList) {
				if (ds.LineStyle.IsVisible == true) {
					var aPen = new Pen (ds.LineStyle.LineColor, ds.LineStyle.Thickness);
					aPen.DashStyle = ds.LineStyle.Pattern;
					if (ds.LineStyle.PlotMethod == LineStyle.PlotLinesMethodEnum.Lines) {
						for (int i = 1; i < ds.PointList.Count; i++) {
							g.DrawLine (aPen, cs.Point2D ((CGPoint)ds.PointList[i - 1]), cs.Point2D ((CGPoint)ds.PointList[i]));
						}
					} else if (ds.LineStyle.PlotMethod == LineStyle.PlotLinesMethodEnum.Splines) {
						var al = new ArrayList ();
						for (int i = 0; i < ds.PointList.Count; i++) {
							var pt = (CGPoint)ds.PointList[i];
							if (pt.X >= cs.XLimMin && pt.X <= cs.XLimMax && pt.Y >= cs.YLimMin && pt.Y <= cs.YLimMax)
								al.Add (pt);
						}

						var pts = new PointF[al.Count];
						for (int i = 0; i < pts.Length; i++)
							pts[i] = cs.Point2D ((CGPoint)(al[i]));
						
						g.DrawCurve (aPen, pts);
					}
					aPen.Dispose();
				}
			}

			foreach (DataSeries ds in DataSeriesList) {
				for (int i = 0; i < ds.PointList.Count; i++) {
					var pt = (CGPoint)ds.PointList[i];
					if (pt.X >= cs.XLimMin && pt.X <= cs.XLimMax && pt.Y >= cs.YLimMin && pt.Y <= cs.YLimMax)
						ds.SymbolStyle.DrawSymbol (g, cs.Point2D((CGPoint)ds.PointList[i]));
				}
			}
		}
	}
}
