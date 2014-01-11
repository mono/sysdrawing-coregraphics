using System;
using System.Collections;
using System.DrawingNative;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;
namespace Example4_9
{
	public class DataCollection
	{
		private ChartCanvas form1;
		private ArrayList dataSeriesList;
		private int dataSeriesIndex = 0;
		private StockChartTypeEnum stockChartType = StockChartTypeEnum.HiLoOpenClose;

		public DataCollection (ChartCanvas fm1)
		{
			dataSeriesList = new ArrayList ();
			form1 = fm1;
		}

		public StockChartTypeEnum StockChartType {
			get { return stockChartType; }
			set { stockChartType = value; }
		}

		public enum StockChartTypeEnum
		{
			HiLo = 0,
			HiLoOpenClose = 1,
			Candle = 2
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

		public void AddStockChart (Graphics g, ChartStyle cs)
		{
			foreach (DataSeries ds in DataSeriesList) {
				Pen aPen = new Pen (ds.LineStyle.LineColor, ds.LineStyle.Thickness);
				aPen.DashStyle = ds.LineStyle.Pattern;
				SolidBrush aBrush = new SolidBrush (ds.LineStyle.LineColor);
				SolidBrush whiteBrush = new SolidBrush (Color.White);
				float barLength = form1.PlotPanel.Width / (5 * ds.DataString.GetLength (1));
				for (int i = 0; i < ds.DataString.GetLength(1); i++) {
					float[] stockdata = new float[4];
					for (int j = 0; j < stockdata.Length; j++) {
						stockdata [j] = Convert.ToSingle (ds.DataString [j + 1, i]);
					}
					PointF ptHigh = cs.Point2D (new PointF (i, stockdata [1]));
					PointF ptLow = cs.Point2D (new PointF (i, stockdata [2]));
					PointF ptOpen = cs.Point2D (new PointF (i, stockdata [0]));
					PointF ptCLose = cs.Point2D (new PointF (i, stockdata [3]));
					PointF ptOpen1 = new PointF (ptOpen.X - barLength, ptOpen.Y);
					PointF ptClose1 = new PointF (ptCLose.X + barLength, ptCLose.Y);
					PointF ptOpen2 = new PointF (ptOpen.X + barLength, ptOpen.Y);
					PointF ptClose2 = new PointF (ptCLose.X - barLength, ptCLose.Y);
                    

					// Draw Hi-Lo stock chart:
					if (StockChartType == StockChartTypeEnum.HiLo) {
						g.DrawLine (aPen, ptHigh, ptLow);
					}

                    // Draw Hi-Li-Open-Close chart:
                    else if (StockChartType == StockChartTypeEnum.HiLoOpenClose) {
						g.DrawLine (aPen, ptHigh, ptLow);
						g.DrawLine (aPen, ptOpen, ptOpen1);
						g.DrawLine (aPen, ptCLose, ptClose1);
					}

                    // Draw candle chart:
                    else if (stockChartType == StockChartTypeEnum.Candle) {
						PointF[] pts = new PointF[4];
						pts [0] = ptOpen1;
						pts [1] = ptOpen2;
						pts [2] = ptClose1;
						pts [3] = ptClose2;
						g.DrawLine (aPen, ptHigh, ptLow);
						if (stockdata [0] > stockdata [3]) {
							g.FillPolygon (aBrush, pts);
						} else if (stockdata [0] < stockdata [3]) {
							g.FillPolygon (whiteBrush, pts);
						}
						g.DrawPolygon (aPen, pts);
					}
				}
				aPen.Dispose ();
				aBrush.Dispose ();
				whiteBrush.Dispose ();
			}
		}
	}
}
