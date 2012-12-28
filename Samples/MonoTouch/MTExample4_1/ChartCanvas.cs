using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample4_1
{
	public class ChartCanvas : UIView {

		private ChartStyle cs;
		private DataCollection dc;
		private DataSeries ds;

		public PlotPanel PlotPanel;

		public ChartCanvas (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			this.BackColor = Color.Wheat;

			PlotPanel = new PlotPanel(rect);

			this.AddSubview(PlotPanel);

			// Set Form1 size:
//			this.Width = 350;
//			this.Height = 300;
			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint +=
				new PaintEventHandler(PlotPanelPaint);
			
			cs = new ChartStyle(this);
			dc = new DataCollection();
			// Specify chart style parameters:
			cs.Title = "Bar Chart";

			// Vertical
			cs.XLimMin = 0f;
			cs.XLimMax = 5f;
			cs.YLimMin = 0f;
			cs.YLimMax = 25f;
			cs.XTick = 1f;
			cs.YTick = 5f;

			cs.BarType = ChartStyle.BarTypeEnum.VerticalOverlay;
			//cs.BarType = ChartStyle.BarTypeEnum.VerticalStack;


			// Horizontal
			cs.XLimMin = 0f;
			cs.XLimMax = 25f;
			cs.YLimMin = 0f;
			cs.YLimMax = 5f;
			cs.XTick = 5f;
			cs.YTick = 1f;

			//cs.BarType = ChartStyle.BarTypeEnum.HorizontalOverlay;
			cs.BarType = ChartStyle.BarTypeEnum.HorizontalStack;
		}
		
		private void AddData(Graphics g)
		{
			float x, y;
			// Add first data series:
			dc.DataSeriesList.Clear();
			ds = new DataSeries();
			ds.BarStyle.BorderColor = Color.Red;
			ds.BarStyle.FillColor = Color.Green;
			for (int i = 0; i < 5; i++)
			{
				x = i + 1;
				y = 2.0f * x;
				ds.AddPoint(new PointF(x, y));
			}
			dc.Add(ds);
			
			// Add second data series:
			ds = new DataSeries();
			ds.BarStyle.BorderColor = Color.Red;
			ds.BarStyle.FillColor = Color.Yellow;
			for (int i = 0; i < 5; i++)
			{
				x = i + 1;
				y = 1.5f * x;
				ds.AddPoint(new PointF(x, y));
			}
			dc.Add(ds);
			
			// Add third data series:
			ds = new DataSeries();
			ds.BarStyle.BorderColor = Color.Red;
			ds.BarStyle.FillColor = Color.Blue;
			for (int i = 0; i < 5; i++)
			{
				x = i + 1;
				y = 1.0f * x;
				ds.AddPoint(new PointF(x, y));
			}
			dc.Add(ds);
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
			Graphics g = new Graphics();
			cs.ChartArea = this.ClientRectangle;
			cs.SetChartArea(g);
		}

		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			AddData(g);
			cs.PlotPanelStyle(g);
			dc.AddBars(g, cs, dc.DataSeriesList.Count,
			           ds.PointList.Count);
		}

	}
}

