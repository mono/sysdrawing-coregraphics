using System;
using CoreGraphics;
using System.Drawing.Drawing2D;
using UIKit;
using System.Drawing;

namespace MTExample3_7 {
	public class DrawingView : UIView, IForm {
		SubChart sc;
		DataCollection dc1;
		DataCollection dc2;
		DataCollection dc3;
		DataCollection dc4;
		ChartStyle cs1;
		ChartStyle cs2;
		ChartStyle cs3;
		ChartStyle cs4;
		Legend lg;

		public DrawingView (CGRect rect) : base(rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackColor = Color.Wheat;

			sc = new SubChart (this);
			sc.TotalChartBackColor = Color.White;
			sc.Margin = 20;
			sc.Rows = 2;
			sc.Cols = 2;

			dc1 = new DataCollection ();
			cs1 = new ChartStyle (this);
			cs1.TickFont = new Font ("Arial", 7f, FontStyle.Regular);
			cs1.TitleFont = new Font ("Arial", 10f, FontStyle.Regular);
			cs1.XLimMin = 0f;
			cs1.XLimMax = 7f;
			cs1.YLimMin = -1.5f;
			cs1.YLimMax = 1.5f;
			cs1.XTick = 1f;
			cs1.YTick = .5f;
			cs1.Title = "Sin(x)";

			// Sub-chart 2 (0, 1):
			dc2 = new DataCollection ();
			cs2 = new ChartStyle (this);
			cs2.TickFont = new Font ("Arial", 7f, FontStyle.Regular);
			cs2.TitleFont = new Font ("Arial", 10f, FontStyle.Regular);
			cs2.XLimMin = 0f;
			cs2.XLimMax = 7f;
			cs2.YLimMin = -1.5f;
			cs2.YLimMax = 1.5f;
			cs2.XTick = 1f;
			cs2.YTick = .5f;
			cs2.Title = "Cos(x)";

			// Sub-chart 3 (1, 0):
			dc3 = new DataCollection ();
			cs3 = new ChartStyle (this);
			cs3.TickFont = new Font ("Arial", 7f, FontStyle.Regular);
			cs3.TitleFont = new Font ("Arial", 10f, FontStyle.Regular);
			cs3.XLimMin = 0f;
			cs3.XLimMax = 7f;
			cs3.YLimMin = -.5f;
			cs3.YLimMax = 1.5f;
			cs3.XTick = 1f;
			cs3.YTick = .5f;
			cs3.Title = "Sin(x)^2";

			// Sub-chart 4 (1, 1):
			dc4 = new DataCollection ();
			cs4 = new ChartStyle (this);
			cs4.IsY2Axis = true;
			cs4.IsXGrid = false;
			cs4.IsYGrid = false;
			cs4.TickFont = new Font ("Arial", 7f, FontStyle.Regular);
			cs4.TitleFont = new Font ("Arial", 10f, FontStyle.Regular);
			cs4.XLimMin = 0f;
			cs4.XLimMax = 30f;
			cs4.YLimMin = -20f;
			cs4.YLimMax = 20f;
			cs4.XTick = 5f;
			cs4.YTick = 5f;
			cs4.Y2LimMin = 100f;
			cs4.Y2LimMax = 700f;
			cs4.Y2Tick = 100f;
			cs4.XLabel = "X Axis";
			cs4.YLabel = "Y Axis";
			cs4.Y2Label = "Y2 Axis";
			cs4.Title = "With Y2 Axis";
			lg = new Legend ();
			lg.IsLegendVisible = true;
			lg.LegendPosition = Legend.LegendPositionEnum.SouthEast;
		}

		void AddData (Graphics g)
		{
			float x = 0f;
			float y = 0f;

			// Add Sin(x) data to sub-chart 1:
			dc1.DataSeriesList.Clear ();
			var ds = new DataSeries ();
			ds.LineStyle.LineColor = Color.Red;
			ds.LineStyle.Thickness = 2f;
			ds.LineStyle.Pattern = DashStyle.Dash;
			for (int i = 0; i < 50; i++) {
				x = i / 5f;
				y = (float)Math.Sin (x);
				ds.AddPoint (new CGPoint (x, y));
			}
			dc1.Add (ds);

			// Add Cos(x) data sub-chart 2:
			dc2.DataSeriesList.Clear ();
			ds = new DataSeries ();
			ds.LineStyle.LineColor = Color.Blue;
			ds.LineStyle.Thickness = 1f;
			ds.SymbolStyle.SymbolType = SymbolStyle.SymbolTypeEnum.OpenDiamond;
			for (int i = 0; i < 50; i++) {
				x = i / 5f;
				y = (float)Math.Cos (x);
				ds.AddPoint (new CGPoint (x, y));
			}
			dc2.Add (ds);

			// Add Sin(x)^2 data to sub-chart 3:
			dc3.DataSeriesList.Clear ();
			ds = new DataSeries ();
			ds.LineStyle.IsVisible = false;
			ds.SymbolStyle.SymbolType = SymbolStyle.SymbolTypeEnum.Dot;
			ds.SymbolStyle.FillColor = Color.Yellow;
			ds.SymbolStyle.BorderColor = Color.DarkCyan;
			for (int i = 0; i < 50; i++) {
				x = i / 5f;
				y = (float)Math.Sin (x);
				ds.AddPoint (new CGPoint (x, y * y));
			}
			dc3.Add (ds);

			// Add y1 and y2 data to sub-chart 4:
			dc4.DataSeriesList.Clear ();
			// Add y1 data:
			ds = new DataSeries ();
			ds.LineStyle.LineColor = Color.Red;
			ds.LineStyle.Thickness = 2f;
			ds.LineStyle.Pattern = DashStyle.Dash;
			ds.SeriesName = "x1*cos(x1)";
			for (int i = 0; i < 20; i++) {
				float x1 = 1f * i;
				float y1 = x1 * (float)Math.Cos (x1);
				ds.AddPoint(new CGPoint (x1, y1));
			}
			dc4.Add (ds);
			// Add y2 data:
			ds = new DataSeries();
			ds.LineStyle.LineColor = Color.Blue;
			ds.LineStyle.Thickness = 2f;
			ds.SeriesName = "100 + 20*x2";
			ds.IsY2Data = true;
			for (int i = 5; i < 30; i++) {
				float x2 = 1f * i;
				float y2 = 100f + 20f * x2;
				ds.AddPoint (new CGPoint (x2, y2));
			}
			dc4.Add (ds);
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
				BackgroundColor = UIColor.FromRGBA (bgc.R, bgc.G, bgc.B, bgc.A);
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
			// Re-define TotalChartArea for resiz-redraw:
			sc.TotalChartArea = ClientRectangle;

			// Add data for all sub-charts:
			AddData (g);

			// Create sub-chart layout:
			Rectangle[,] subchart = sc.SetSubChart (g);

			// Create sub-chart 1:
			cs1.ChartArea = subchart[0, 0];
			cs1.AddChartStyle (g);
			dc1.AddLines (g, cs1);

			// Create sub-chart 2:
			cs2.ChartArea = subchart[0, 1];
			cs2.AddChartStyle (g);
			dc2.AddLines (g, cs2);

			// Create sub-chart 3:
			cs3.ChartArea = subchart[1, 0];
			cs3.AddChartStyle (g);
			dc3.AddLines (g, cs3);

			// Create sub-chart 4:
			cs4.ChartArea = subchart[1, 1];
			cs4.AddChartStyle (g);
			dc4.AddLines (g, cs4);
			lg.AddLegend (g, dc4, cs4);
			g.Dispose ();
		}
	}
}

public interface IForm {
	Rectangle ClientRectangle { get; }

	Color BackColor { get; set; }

	Font Font { get; set; }
}
