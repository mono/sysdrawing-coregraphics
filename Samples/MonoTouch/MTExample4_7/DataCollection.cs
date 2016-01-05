using System.Collections;
using System.Drawing;
using CoreGraphics;

namespace MTExample4_7 {
	public class DataCollection {
		ArrayList dataSeriesList;
		int dataSeriesIndex = 0;
		int[,] cmap;
		float areaAxis = 0;

		public DataCollection ()
		{
			dataSeriesList = new ArrayList ();
		}

		public float AreaAxis {
			get {
				return areaAxis;
			}
			set {
				areaAxis = value;
			}
		}

		public int[,] CMap {
			get {
				return cmap;
			}
			set {
				cmap = value;
			}
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

		public void AddAreas (Graphics g, ChartStyle cs, int nSeries, int nPoints)
		{
			float[] ySum = new float[nPoints];
			var pts = new Point[2 * nPoints];
			CGPoint[] pt0 = new CGPoint[nPoints];
			CGPoint[] pt1 = new CGPoint[nPoints];
			for (int i = 0; i < nPoints; i++) {
				ySum [i] = AreaAxis;
			}

			int n = 0;
			foreach (DataSeries ds in DataSeriesList) {
				var aPen = new Pen (ds.LineStyle.LineColor, ds.LineStyle.Thickness);
				aPen.DashStyle = ds.LineStyle.Pattern;
				Color fillColor = Color.FromArgb (CMap [n, 0], CMap [n, 1], CMap [n, 2], CMap [n, 3]);
				var aBrush = new SolidBrush (fillColor);
				// Draw lines and areas:
				if (ds.LineStyle.PlotMethod == LineStyle.PlotLinesMethodEnum.Lines) {
					for (int i = 0; i < nPoints; i++) {
						pt0 [i] = new CGPoint (((CGPoint)ds.PointList [i]).X, ySum [i]);
						ySum [i] = (float)(ySum[i] + ((CGPoint)ds.PointList[i]).Y);
						pt1 [i] = new CGPoint (((CGPoint)ds.PointList [i]).X, ySum [i]);
						pts [i] = cs.Point2D (pt0 [i]);
						pts [2 * nPoints - 1 - i] = cs.Point2D (pt1 [i]);
					}
					g.FillPolygon (aBrush, pts);
					g.DrawPolygon (Pens.Black, pts);
				}
				n++;
			}
		}
	}
}
