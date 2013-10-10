using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample3_3
{
	public class DrawingView : UIView, Form {


		private DataCollection dc;
		private ChartStyle cs;
		
		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			this.BackColor = Color.White;
			
			// Set Form1 size:
//			this.Width = 350;
//			this.Height = 300;
			dc = new DataCollection();
			cs = new ChartStyle(this);
			cs.XLimMin = 0f;
			cs.XLimMax = 6f;
			cs.YLimMin = -1.5f;
			cs.YLimMax = 1.5f;
			cs.XTick = 1f;
			cs.YTick = 0.5f;
			cs.TickFont = new Font("Arial", 7, FontStyle.Regular);
			cs.XLabel = "X Axis";
			cs.YLabel = "Y Axis";
			cs.Title = "Sine & Cosine Plot";
			cs.TitleFont = new Font("Arial", 10, FontStyle.Regular);
		}
		
		public void AddData()
		{
			dc.DataSeriesList.Clear();
			// Add Sine data with 20 data point:
			DataSeries ds1 = new DataSeries();
			ds1.LineStyle.LineColor = Color.Red;
			ds1.LineStyle.Thickness = 2f;
			ds1.LineStyle.Pattern = DashStyle.Dash;
			for (int i = 0; i < 20; i++)
			{
				ds1.AddPoint(new PointF(i / 5.0f, (float)Math.Sin(i / 5.0f)));
			}
			dc.Add(ds1);
			
			// Add Cosine data with 40 data point:
			DataSeries ds2 = new DataSeries();
			ds2.LineStyle.LineColor = Color.Blue;
			ds2.LineStyle.Thickness = 1f;
			ds2.LineStyle.Pattern = DashStyle.Solid;
			for (int i = 0; i < 40; i++)
			{
				ds2.AddPoint(new PointF(i / 5.0f, (float)Math.Cos(i / 5.0f)));
			}
			dc.Add(ds2);
		}
	
		#region Form interface
		public Rectangle ClientRectangle 
		{
			get {
				return new Rectangle((int)Bounds.X,
				                      (int)Bounds.Y,
				                      (int)Bounds.Width,
				                      (int)Bounds.Height);
			}
		}

		public Color BackColor 
		{
			get {
				float red;
				float green;
				float blue;
				float alpha;
				BackgroundColor.GetRGBA(out red, out green, out blue, out alpha);
				return Color.FromArgb((int)alpha, (int)red, (int)green, (int)blue);
			}

			set {
				var bgc = value;
				BackgroundColor = UIColor.FromRGBA(bgc.R,bgc.G,bgc.B, bgc.A);

			}
		}

		Font font;
		public Font Font
		{
			get {
				if (font == null)
					font = new Font("Helvetica",12);
				return font;
			}
			set 
			{
				font = value;
			}
		}

		#endregion


		public override void Draw (RectangleF dirtyRect)
		{
			Graphics g = Graphics.FromCurrentContext();

			cs.ChartArea = this.ClientRectangle;
			AddData();
			SetPlotArea(g);
			cs.AddChartStyle(g);
			dc.AddLines(g, cs);
			g.Dispose();
		}
		
		private void SetPlotArea(Graphics g)
		{
			// Set PlotArea:
			float xOffset = cs.ChartArea.Width / 30.0f;
			float yOffset = cs.ChartArea.Height / 30.0f;
			SizeF labelFontSize = g.MeasureString("A", cs.LabelFont);
			SizeF titleFontSize = g.MeasureString("A", cs.TitleFont);
			if (cs.Title.ToUpper() == "NO TITLE")
			{
				titleFontSize.Width = 8f;
				titleFontSize.Height = 8f;
			}
			float xSpacing = xOffset / 3.0f;
			float ySpacing = yOffset / 3.0f;
			SizeF tickFontSize = g.MeasureString("A", cs.TickFont);
			float tickSpacing = 2f;
			SizeF yTickSize = g.MeasureString(cs.YLimMin.ToString(), cs.TickFont);
			for (float yTick = cs.YLimMin; yTick <= cs.YLimMax; yTick += cs.YTick)
			{
				SizeF tempSize = g.MeasureString(yTick.ToString(), cs.TickFont);
				if (yTickSize.Width < tempSize.Width)
				{
					yTickSize = tempSize;
				}
			}
			float leftMargin = xOffset + labelFontSize.Width +
				xSpacing + yTickSize.Width + tickSpacing;
			float rightMargin = 2 * xOffset;
			float topMargin = yOffset + titleFontSize.Height + ySpacing;
			float bottomMargin = yOffset + labelFontSize.Height +
				ySpacing + tickSpacing + tickFontSize.Height;
			
			// Define the plot area with one Y axis:
			int plotX = cs.ChartArea.X + (int)leftMargin;
			int plotY = cs.ChartArea.Y + (int)topMargin;
			int plotWidth = cs.ChartArea.Width - (int)leftMargin - (int)rightMargin;
			int plotHeight = cs.ChartArea.Height - (int)topMargin - (int)bottomMargin;
			cs.PlotArea = new Rectangle(plotX, plotY, plotWidth, plotHeight);
		}
	}
}

public interface Form
{
	Rectangle ClientRectangle { get; }

	Color BackColor { get; set; } 

	Font Font { get; set; }
}