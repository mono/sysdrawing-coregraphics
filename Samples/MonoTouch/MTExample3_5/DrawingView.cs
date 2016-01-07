using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using CoreGraphics;
using UIKit;

namespace MTExample3_5 {
	public class DrawingView : UIView, Form {
		DataCollection dc;
		ChartStyle cs;
		Legend lg;

		public DrawingView (CGRect rect) : base(rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackColor = Color.Wheat;
			cs = new ChartStyle(this);
			cs.XLimMin = 0f;
			cs.XLimMax = 6f;
			cs.YLimMin = -1.5f;
			cs.YLimMax = 1.5f;
			cs.XTick = 1f;
			cs.YTick = .5f;
			cs.XLabel = "This is X axis";
			cs.YLabel = "This is Y axis";
			cs.Title = "Sine and Cosine Chart";
			lg = new Legend {
				IsLegendVisible = true
			};

			dc = new DataCollection ();
		}

		void AddData (Graphics g)
		{
			cs.XLimMin = 0f;
			cs.XLimMax = 6f;
			cs.YLimMin = -1.5f;
			cs.YLimMax = 1.5f;
			cs.XTick = 1f;
			cs.YTick = .5f;
			cs.XLabel = "This is X axis";
			cs.YLabel = "This is Y axis";
			cs.Title = "Sine and Cosine Chart";

			dc.DataSeriesList.Clear ();

			// Add Sine data with 7 data point:
			var ds1 = new DataSeries ();
			ds1.LineStyle.LineColor = Color.Red;
			ds1.LineStyle.Thickness = 2f;
			ds1.LineStyle.Pattern = DashStyle.Dash;
			ds1.LineStyle.PlotMethod = LineStyle.PlotLinesMethodEnum.Lines;
			ds1.SeriesName = "Sine";
			ds1.SymbolStyle.SymbolType = SymbolStyle.SymbolTypeEnum.Diamond;
			ds1.SymbolStyle.BorderColor = Color.Red;
			ds1.SymbolStyle.FillColor = Color.Yellow;
			ds1.SymbolStyle.BorderThickness = 1f;
			for (int i = 0; i < 7; i++)
				ds1.AddPoint(new CGPoint (1f * i, (float)Math.Sin(1f * i)));
			dc.Add (ds1);

			// Add Cosine data with 7 data point:
			var ds2 = new DataSeries ();
			ds2.LineStyle.LineColor = Color.Blue;
			ds2.LineStyle.Thickness = 1f;
			ds2.LineStyle.Pattern = DashStyle.Solid;
			ds2.LineStyle.PlotMethod = LineStyle.PlotLinesMethodEnum.Splines;
			ds2.SeriesName = "Cosine";
			ds2.SymbolStyle.SymbolType = SymbolStyle.SymbolTypeEnum.Triangle;
			ds2.SymbolStyle.BorderColor = Color.Blue;
			for (int i = 0; i < 40; i++)
				ds2.AddPoint (new CGPoint (1f * i, (float)Math.Cos (1f * i)));
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
					font = new Font("Helvetica", 12);
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
			SetPlotArea (g);
			AddData (g);
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
			if (cs.Title.ToUpper () == "NO TITLE") {
				titleFontSize.Width = 8f;
				titleFontSize.Height = 8f;
			}
			float xSpacing = xOffset / 3f;
			float ySpacing = yOffset / 3f;
			CGSize tickFontSize = g.MeasureString ("A", cs.TickFont);
			float tickSpacing = 2f;
			CGSize yTickSize = g.MeasureString(cs.YLimMin.ToString(), cs.TickFont);
			for (float yTick = cs.YLimMin; yTick <= cs.YLimMax; yTick += cs.YTick) {
				CGSize tempSize = g.MeasureString(yTick.ToString(), cs.TickFont);
				if (yTickSize.Width < tempSize.Width)
					yTickSize = tempSize;
			}

			var leftMargin = (float)(xOffset + labelFontSize.Width + xSpacing + yTickSize.Width + tickSpacing);
			float rightMargin = 2 * xOffset;
			var topMargin = (float)(yOffset + titleFontSize.Height + ySpacing);
			var bottomMargin = (float)(yOffset + labelFontSize.Height + ySpacing + tickSpacing + tickFontSize.Height);

			// Define the plot area with one Y axis:
			int plotX = cs.ChartArea.X + (int)leftMargin;
			int plotY = cs.ChartArea.Y + (int)topMargin;
			int plotWidth = cs.ChartArea.Width - (int)leftMargin - (int)rightMargin;
			int plotHeight = cs.ChartArea.Height - (int)topMargin - (int)bottomMargin;
			cs.PlotArea = new Rectangle (plotX, plotY, plotWidth, plotHeight);
		}
	}
}

public interface Form {
	Rectangle ClientRectangle { get; }

	Color BackColor { get; set; }

	Font Font { get; set; }
}
