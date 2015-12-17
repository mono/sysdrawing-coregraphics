using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using UIKit;

namespace MTExample4_8
{
	public class ChartCanvas : UIView {

		private ChartStyle cs;
		private DataCollection dc;
		private Legend lg;

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
			
			dc = new DataCollection();
			cs = new ChartStyle(this);
			cs.RMax = 1f;
			cs.RMin = -5f;
			cs.NTicks = 4;
			cs.AngleStep = 30;
			cs.AngleDirection = ChartStyle.AngleDirectionEnum.CounterClockWise;
			lg = new Legend();
			lg.IsLegendVisible = true;
		}
		
		private void AddData()
		{
			dc.DataSeriesList.Clear();
			// Add log-sine data:
			DataSeries ds = new DataSeries();
			ds.LineStyle.LineColor = Color.Red;
			ds.LineStyle.Thickness = 1f;
			ds.LineStyle.Pattern = DashStyle.Solid;
			ds.SeriesName = "Sin(theta)";
			for (int i = 0; i < 360; i++)
			{
				float theta = 1.0f * i;
				float r = (float)Math.Log(1.001f + Math.Sin(2 * theta * Math.PI / 180));
				ds.AddPoint(new PointF(theta, r));
			}
			dc.Add(ds);
			
			// Add log-cosine data:
			ds = new DataSeries();
			ds.LineStyle.LineColor = Color.Green;
			ds.LineStyle.Thickness = 1f;
			ds.LineStyle.Pattern = DashStyle.Solid;
			ds.SeriesName = "Cos(theta)";
			for (int i = 0; i < 360; i++)
			{
				float theta = 1.0f * i;
				float r = (float)Math.Log(1.001f + Math.Cos(2 * theta * Math.PI / 180));
				ds.AddPoint(new PointF(theta, r));
			}
			dc.Add(ds);
			
			/*ds = new DataSeries();
            ds.LineStyle.LineColor = Color.Red;
            for (int i = 0; i < 360; i++)
            {
                float theta = 1.0f * i;
                float r = (float)Math.Abs(Math.Cos(2*theta * Math.PI / 180) * 
                    Math.Sin(2 * theta * Math.PI / 180));
                ds.AddPoint(new PointF(theta, r));
            }
            dc.Add(ds);*/
		}
		
		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			AddData();
			cs.SetPolarAxes(g);
			dc.AddPolar(g,cs);
			lg.AddLegend(g, dc, cs);
			g.Dispose();
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


//		public override void Draw (RectangleF dirtyRect)
//		{
//			Graphics g = Graphics.FromCurrentContext();
//			cs.ChartArea = this.ClientRectangle;
//			cs.SetChartArea(g);
//		}

	}
}

