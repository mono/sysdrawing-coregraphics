using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample4_2
{
	public class ChartCanvas : UIView {

		private ChartStyle cs;
		private DataCollection dc;
		private DataSeries ds;
		private ColorMap cm;

		public PlotPanel PlotPanel;

		public ChartCanvas (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			this.BackColor = Color.Wheat;

			PlotPanel = new PlotPanel(rect);

			this.AddSubview(PlotPanel);

			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint +=
				new PaintEventHandler(PlotPanelPaint);
			
			cs = new ChartStyle(this);
			dc = new DataCollection();
			cm = new ColorMap(100, 180);
			// Specify chart style parameters:
			cs.Title = "Bar Chart";
			cs.XLimMin = 0f;
			cs.XLimMax = 5f;
			cs.YLimMin = 0f;
			cs.YLimMax = 10f;
			cs.XTick = 1f;
			cs.YTick = 2f;
			cs.BarType = ChartStyle.BarTypeEnum.Vertical;
		}
		
		private void AddData(Graphics g)
		{
			float x, y;
			// Add first data series:
			dc.DataSeriesList.Clear();
			ds = new DataSeries();
			ds.BarStyle.BorderColor = Color.Red;
			//ds.IsColorMap = true;
			ds.IsColorMap = true;
			ds.CMap = cm.Jet();
			for (int i = 0; i < 5; i++)
			{
				x = i + 1;
				y = 2.0f * x;
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
			Graphics g = Graphics.FromCurrentContext();
			cs.ChartArea = this.ClientRectangle;
			cs.SetChartArea(g);
		}

		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			AddData(g);
			cs.PlotPanelStyle(g);
			dc.AddBars(g, cs, dc.DataSeriesList.Count, ds.PointList.Count);
		}
		

	}
}

