using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using CoreGraphics;
using UIKit;

namespace MTExample3_6 {
	public class DrawingView : UIView, IForm {
		DataCollection dc;
		ChartStyle cs;
		Legend lg;

		public DrawingView (CGRect rect) : base(rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackColor = Color.Wheat;

			dc = new DataCollection ();
			cs = new ChartStyle (this);
			lg = new Legend ();
			lg.IsLegendVisible = true;
			lg.LegendPosition = Legend.LegendPositionEnum.NorthWest;
			cs.IsY2Axis = true;
			cs.IsXGrid = false;
			cs.IsYGrid = false;
			cs.TickFont = new Font ("Arial", 7f, FontStyle.Regular);
			cs.TitleFont = new Font ("Arial", 10f, FontStyle.Regular);
			cs.XLimMin = 0f;
			cs.XLimMax = 30f;
			cs.YLimMin = -20f;
			cs.YLimMax = 20f;
			cs.XTick = 5f;
			cs.YTick = 5f;
			cs.Y2LimMin = 100f;
			cs.Y2LimMax = 700f;
			cs.Y2Tick = 100f;
			cs.XLabel = "X Axis";
			cs.YLabel = "Y Axis";
			cs.Y2Label = "Y2 Axis";
			cs.Title = "With Y2 Axis";
		}

		void AddData (Graphics g)
		{
			dc.DataSeriesList.Clear ();
			// Add data points to ds1:
			var ds1 = new DataSeries ();
			ds1.LineStyle.LineColor = Color.Red;
			ds1.LineStyle.Thickness = 2f;
			ds1.LineStyle.Pattern = DashStyle.Dash;
			ds1.SeriesName = "x1*cos(x1)";
			for (int i = 0; i < 20; i++) {
				float x1 = 1.0f * i;
				float y1 = x1 * (float)Math.Cos (x1);
				ds1.AddPoint (new CGPoint (x1, y1));
			}
			dc.Add (ds1);
			// Add data points to ds2:
			var ds2 = new DataSeries ();
			ds2.LineStyle.LineColor = Color.Blue;
			ds2.LineStyle.Thickness = 2f;
			ds2.SeriesName = "100 + 20*x2";
			ds2.IsY2Data = true;
			for (int i = 5; i < 30; i++) {
				float x2 = 1.0f * i;
				float y2 = 100f + 20f * x2;
				ds2.AddPoint (new CGPoint(x2, y2));
			}
			dc.Add (ds2);
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
					font = new Font ("Helvetica", 12);
				return font;
			}
			set {
				font = value;
			}
		}
		#endregion

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext ();
			cs.ChartArea = ClientRectangle;
			AddData (g);
			SetPlotArea (g);
			cs.AddChartStyle (g);
			dc.AddLines (g, cs);
			lg.AddLegend (g, dc, cs);
			g.Dispose ();
		}

		void SetPlotArea (Graphics g)
		{
			// Set PlotArea:
			float xOffset = cs.ChartArea.Width / 30f;
			float yOffset = cs.ChartArea.Height / 30f;
			CGSize labelFontSize = g.MeasureString ("A", cs.LabelFont);
			CGSize titleFontSize = g.MeasureString ("A", cs.TitleFont);
			if (cs.Title.ToUpper() == "NO TITLE") {
				titleFontSize.Width = 8f;
				titleFontSize.Height = 8f;
			}

			float xSpacing = xOffset / 3f;
			float ySpacing = yOffset / 3f;
			CGSize tickFontSize = g.MeasureString ("A", cs.TickFont);
			float tickSpacing = 2f;
			CGSize yTickSize = g.MeasureString (cs.YLimMin.ToString (), cs.TickFont);
			for (float yTick = cs.YLimMin; yTick <= cs.YLimMax; yTick += cs.YTick) {
				CGSize tempSize = g.MeasureString (yTick.ToString (), cs.TickFont);
				if (yTickSize.Width < tempSize.Width)
					yTickSize = tempSize;
			}
			var leftMargin = (float)(xOffset + labelFontSize.Width + xSpacing + yTickSize.Width + tickSpacing);
			float rightMargin = xOffset;
			var topMargin = (float)(yOffset + titleFontSize.Height + ySpacing);
			var bottomMargin = (float)(yOffset + labelFontSize.Height + ySpacing + tickSpacing + tickFontSize.Height);

			if (!cs.IsY2Axis) {
				// Define the plot area with one Y axis:
				int plotX = cs.ChartArea.X + (int)leftMargin;
				int plotY = cs.ChartArea.Y + (int)topMargin;
				int plotWidth = cs.ChartArea.Width - (int)leftMargin - (int)rightMargin;
				int plotHeight = cs.ChartArea.Height - (int)topMargin - (int)bottomMargin;
				cs.PlotArea = new Rectangle (plotX, plotY, plotWidth, plotHeight);
			} else {
				// Define the plot area with Y and Y2 axes:
				CGSize y2TickSize = g.MeasureString (cs.Y2LimMin.ToString (), cs.TickFont);
				for (float y2Tick = cs.Y2LimMin; y2Tick <= cs.Y2LimMax; y2Tick += cs.Y2Tick) {
					CGSize tempSize2 = g.MeasureString (y2Tick.ToString (), cs.TickFont);
					if (y2TickSize.Width < tempSize2.Width)
						y2TickSize = tempSize2;
				}

				rightMargin = (float)(xOffset + labelFontSize.Width + xSpacing + y2TickSize.Width + tickSpacing);
				int plotX = cs.ChartArea.X + (int)leftMargin;
				int plotY = cs.ChartArea.Y + (int)topMargin;
				int plotWidth = cs.ChartArea.Width - (int)leftMargin - (int)rightMargin;
				int plotHeight = cs.ChartArea.Height - (int)topMargin - (int)bottomMargin;
				cs.PlotArea = new Rectangle (plotX, plotY, plotWidth, plotHeight);
			}
		}
	}
}

public interface IForm {
	Rectangle ClientRectangle { get; }

	Color BackColor { get; set; }

	Font Font { get; set; }
}
