using System;
using System.Collections;
using System.Drawing;

using CoreGraphics;

namespace MTExample4_1 {
	public class DataCollection {
		ArrayList dataSeriesList;
		int dataSeriesIndex = 0;

		public DataCollection()
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
			dataSeriesList.Insert(dataSeriesIndex, ds);
			if (ds.SeriesName == string.Empty) {
				dataSeriesIndex = dataSeriesIndex + 1;
				ds.SeriesName = "DataSeries" + dataSeriesIndex.ToString ();
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
			dataSeriesList.Clear ();
		}

		public void AddBars (Graphics g, ChartStyle cs, int numberOfDataSeries, int numberOfPoints)
		{
			// Draw bars:
			var temp = new ArrayList ();
			float[] tempy = new float[numberOfPoints];
			var temppt = new CGPoint ();
			int n = 0;
			foreach (DataSeries ds in DataSeriesList) {
				var aPen = new Pen (ds.BarStyle.BorderColor, ds.BarStyle.BorderThickness);
				var aBrush = new SolidBrush (ds.BarStyle.FillColor);
				aPen.DashStyle = ds.BarStyle.BorderPattern;
				var pts = new PointF[4];
				CGPoint pt;
				float width;
				if (cs.BarType == ChartStyle.BarTypeEnum.Vertical) {
					if (numberOfDataSeries == 1) {
						width = cs.XTick * ds.BarStyle.BarWidth;
						for (int i = 0; i < ds.PointList.Count; i++) {
							pt = (CGPoint)ds.PointList[i];
							nfloat x = pt.X - cs.XTick / 2;
							pts[0] = cs.Point2D (new CGPoint (x - width / 2, 0));
							pts[1] = cs.Point2D (new CGPoint (x + width / 2, 0));
							pts[2] = cs.Point2D (new CGPoint (x + width / 2, pt.Y));
							pts[3] = cs.Point2D (new CGPoint (x - width / 2, pt.Y));
							g.FillPolygon (aBrush, pts);
							g.DrawPolygon (aPen, pts);
						}
					} else if (numberOfDataSeries > 1) {
						width = 0.7f * cs.XTick;
						for (int i = 0; i < ds.PointList.Count; i++) {
							pt = (CGPoint)ds.PointList[i];
							float w1 = width / numberOfDataSeries;
							float w = ds.BarStyle.BarWidth * w1;
							float space = (w1 - w) / 2;
							nfloat x = pt.X - cs.XTick / 2;
							pts[0] = cs.Point2D(new CGPoint(
								x - width / 2 + space + n * w1, 0));
							pts[1] = cs.Point2D(new CGPoint(
								x - width / 2 + space + n * w1 + w, 0));
							pts[2] = cs.Point2D(new CGPoint(
								x - width / 2 + space + n * w1 + w, pt.Y));
							pts[3] = cs.Point2D(new CGPoint(
								x - width / 2 + space + n * w1, pt.Y));
							g.FillPolygon(aBrush, pts);
							g.DrawPolygon(aPen, pts);
						}
					}
				} else if (cs.BarType == ChartStyle.BarTypeEnum.VerticalOverlay && numberOfDataSeries > 1) {
					width = cs.XTick * ds.BarStyle.BarWidth;
					width = width / (float)Math.Pow(2, n);
					for (int i = 0; i < ds.PointList.Count; i++) {
						pt = (CGPoint)ds.PointList[i];
						nfloat x = pt.X - cs.XTick / 2;
						pts[0] = cs.Point2D(new CGPoint(x - width / 2, 0));
						pts[1] = cs.Point2D(new CGPoint(x + width / 2, 0));
						pts[2] = cs.Point2D(new CGPoint(x + width / 2, pt.Y));
						pts[3] = cs.Point2D(new CGPoint(x - width / 2, pt.Y));
						g.FillPolygon(aBrush, pts);
						g.DrawPolygon(aPen, pts);
					}
				} else if (cs.BarType == ChartStyle.BarTypeEnum.VerticalStack && numberOfDataSeries > 1) {
					width = cs.XTick * ds.BarStyle.BarWidth;
					for (int i = 0; i < ds.PointList.Count; i++) {
						pt = (CGPoint)ds.PointList[i];
						if (temp.Count > 0)
							tempy[i] = (float)(tempy[i] + ((CGPoint)temp[i]).Y);
						nfloat x = pt.X - cs.XTick / 2;
						pts[0] = cs.Point2D (new CGPoint (x - width / 2, 0 + tempy[i]));
						pts[1] = cs.Point2D (new CGPoint (x + width / 2, 0 + tempy[i]));
						pts[2] = cs.Point2D (new CGPoint (x + width / 2, pt.Y + tempy[i]));
						pts[3] = cs.Point2D (new CGPoint (x - width / 2, pt.Y + tempy[i]));

						g.FillPolygon (aBrush, pts);
						g.DrawPolygon (aPen, pts);
					}
					temp = ds.PointList;
				} else if (cs.BarType == ChartStyle.BarTypeEnum.Horizontal) {
					if (numberOfDataSeries == 1) {
						width = cs.YTick * ds.BarStyle.BarWidth;
						for (int i = 0; i < ds.PointList.Count; i++) {
							temppt = (CGPoint)ds.PointList[i];
							pt = new CGPoint(temppt.Y, temppt.X);
							nfloat y = pt.Y - cs.YTick / 2;
							pts[0] = cs.Point2D (new CGPoint (0, y - width / 2));
							pts[1] = cs.Point2D (new CGPoint (0, y + width / 2));
							pts[2] = cs.Point2D (new CGPoint (pt.X, y + width / 2));
							pts[3] = cs.Point2D (new CGPoint (pt.X, y - width / 2));
							g.FillPolygon (aBrush, pts);
							g.DrawPolygon (aPen, pts);
						}
					} else if (numberOfDataSeries > 1) {
						width = 0.7f * cs.YTick;
						for (int i = 0; i < ds.PointList.Count; i++) {
							temppt = (CGPoint)ds.PointList[i];
							pt = new CGPoint(temppt.Y, temppt.X);
							float w1 = width / numberOfDataSeries;
							float w = ds.BarStyle.BarWidth * w1;
							float space = (w1 - w) / 2;
							nfloat y = pt.Y - cs.YTick / 2;
							pts[0] = cs.Point2D(new CGPoint(0,
								y - width / 2 + space + n * w1));
							pts[1] = cs.Point2D(new CGPoint(0,
								y - width / 2 + space + n * w1 + w));
							pts[2] = cs.Point2D(new CGPoint(pt.X,
								y - width / 2 + space + n * w1 + w));
							pts[3] = cs.Point2D(new CGPoint(pt.X,
								y - width / 2 + space + n * w1));
							g.FillPolygon(aBrush, pts);
							g.DrawPolygon(aPen, pts);
						}
					}
				} else if (cs.BarType == ChartStyle.BarTypeEnum.HorizontalOverlay && numberOfDataSeries > 1) {
					width = cs.YTick * ds.BarStyle.BarWidth;
					width = width / (float)Math.Pow(2, n);
					for (int i = 0; i < ds.PointList.Count; i++) {
						temppt = (CGPoint)ds.PointList[i];
						pt = new CGPoint(temppt.Y, temppt.X);
						nfloat y = pt.Y - cs.YTick / 2;
						pts[0] = cs.Point2D(new CGPoint(0, y - width / 2));
						pts[1] = cs.Point2D(new CGPoint(0, y + width / 2));
						pts[2] = cs.Point2D(new CGPoint(pt.X, y + width / 2));
						pts[3] = cs.Point2D(new CGPoint(pt.X, y - width / 2));
						g.FillPolygon(aBrush, pts);
						g.DrawPolygon(aPen, pts);
					}
				} else if (cs.BarType == ChartStyle.BarTypeEnum.HorizontalStack && numberOfDataSeries > 1) {
					width = cs.YTick * ds.BarStyle.BarWidth;
					for (int i = 0; i < ds.PointList.Count; i++) {
						temppt = (CGPoint)ds.PointList[i];
						pt = new CGPoint(temppt.Y, temppt.X);
						if (temp.Count > 0) {
							temppt = (CGPoint)temp[i];
							tempy[i] = (float)(tempy[i] + temppt.Y);
						}
						nfloat y = pt.Y - cs.YTick / 2;
						pts[0] = cs.Point2D (new CGPoint (0 + tempy[i], y - width / 2));
						pts[1] = cs.Point2D (new CGPoint (0 + tempy[i], y + width / 2));
						pts[2] = cs.Point2D (new CGPoint (pt.X + tempy[i], y + width / 2));
						pts[3] = cs.Point2D (new CGPoint (pt.X + tempy[i], y - width / 2));

						g.FillPolygon (aBrush, pts);
						g.DrawPolygon (aPen, pts);
					}
					temp = ds.PointList;
				}
				n++;
				aPen.Dispose ();
			}
		}
	}
}
