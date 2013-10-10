using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample3_2
{
	public class DrawingView : UIView, Form {


		private DataCollection dc;
		private ChartStyle cs;
		
		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			var bgc = Color.Wheat;
			BackgroundColor = UIColor.FromRGBA(bgc.R,bgc.G,bgc.B, bgc.A);

			dc = new DataCollection();
			cs = new ChartStyle(this);
			cs.XLimMin = 0f;
			cs.XLimMax = 6f;
			cs.YLimMin = -1.1f;
			cs.YLimMax = 1.1f;

		}

	
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
		
		private void SetPlotArea(Graphics g)
		{
			// Set PlotArea:
			int xOffset = cs.ChartArea.Width / 10;
			int yOffset = cs.ChartArea.Height / 10;
			// Define the plot area:
			int plotX = cs.ChartArea.X + xOffset;
			int plotY = cs.ChartArea.Y + yOffset;
			int plotWidth = cs.ChartArea.Width - 2 * xOffset;
			int plotHeight = cs.ChartArea.Height - 2 * yOffset;
			cs.PlotArea = new Rectangle(plotX, plotY, plotWidth, plotHeight);
		}
	}
}

public interface Form
{
	Rectangle ClientRectangle { get; }

	Color BackColor { get; set; } 
}