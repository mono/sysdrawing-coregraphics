using System;
using System.Drawing;
using CoreGraphics;
using UIKit;

namespace MTExample4_9 {
	public class ChartCanvas : UIView {
		ChartStyle cs;
		DataCollection dc;

		public PlotPanel PlotPanel;

		public ChartCanvas (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackColor = Color.Wheat;

			PlotPanel = new PlotPanel(rect);

			AddSubview (PlotPanel);

			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint += PlotPanelPaint;
			
			cs = new ChartStyle (this);
			dc = new DataCollection (this);
			// Specify chart style parameters:
			cs.Title = "Chart of GE Stock";
			cs.XTickOffset = 1;
			cs.XLimMin = -1f;
			cs.XLimMax = 20f;
			cs.YLimMin = 32f;
			cs.YLimMax = 36f;
			cs.XTick = 2f;
			cs.YTick = .5f;
			dc.StockChartType = DataCollection.StockChartTypeEnum.Candle;
		}
		
		void AddData ()
		{
			dc.DataSeriesList.Clear ();
			var tfr = new TextFileReader ();
			var ds = new DataSeries ();

			// Add GE stock data from a text data file:
			ds = new DataSeries();
			ds.DataString = tfr.ReadTextFile ("GE.txt");
			ds.LineStyle.LineColor = Color.DarkBlue;
			dc.Add (ds);
		}

		void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			AddData ();
			cs.PlotPanelStyle (g);
			dc.AddStockChart (g, cs);
		}

		#region Form interface
		public Rectangle ClientRectangle {
			get {
				return new Rectangle ((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height);
			}
		}

		public Color BackColor {
			get {
				nfloat red;
				nfloat green;
				nfloat blue;
				nfloat alpha;
				BackgroundColor.GetRGBA (out red, out green, out blue, out alpha);
				return Color.FromArgb ((int)alpha, (int)red, (int)green, (int)blue);
			}
			set {
				var bgc = value;
				BackgroundColor = UIColor.FromRGBA (bgc.R,bgc.G,bgc.B, bgc.A);

			}
		}

		Font font;
		public Font Font {
			get {
				if (font == null)
					font = new Font ("Helvetica", 12f);
				return font;
			}
			set {
				font = value;
			}
		}
		#endregion


		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext();
			cs.ChartArea = ClientRectangle;
			cs.SetChartArea(g);
		}

	}
}

