using System;
using CoreGraphics;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MTExample4_8 {
	public class ChartStyle {
		ChartCanvas form1;
		int offset;
		float angleStep = 30;
		AngleDirectionEnum angleDirection = AngleDirectionEnum.CounterClockWise;
		float rMin = 0;
		float rMax = 1;
		int nTicks = 4;
		Font tickFont;
		Color tickFontColor = Color.Black;
		Color radiusColor = Color.Black;
		float radiusThickness = 1f;
		DashStyle radiusPattern = DashStyle.Dash;
		Color angleColor = Color.Black;
		float angleThickness = 1f;
		DashStyle anglePattern = DashStyle.Dash;

		public ChartStyle (ChartCanvas fm1)
		{
			form1 = fm1;
			tickFont = form1.Font;
		}

		public AngleDirectionEnum AngleDirection {
			get {
				return angleDirection;
			}
			set {
				angleDirection = value;
			}
		}

		public Color TickFontColor {
			get {
				return tickFontColor;
			}
			set {
				tickFontColor = value;
			}
		}

		public Font TickFont {
			get {
				return tickFont;
			}
			set {
				tickFont = value;
			}
		}

		public DashStyle AnglePattern {
			get {
				return anglePattern;
			}
			set {
				anglePattern = value;
			}
		}

		public float AngleThickness {
			get {
				return angleThickness;
			}
			set {
				angleThickness = value;
			}
		}

		public Color AngleColor {
			get {
				return angleColor;
			}
			set {
				angleColor = value;
			}
		}

		public DashStyle RadiusPattern {
			get {
				return radiusPattern;
			}
			set {
				radiusPattern = value;
			}
		}

		public float RadiusThickness {
			get {
				return radiusThickness;
			}
			set {
				radiusThickness = value;
			}
		}

		public Color RadiusColor {
			get {
				return radiusColor;
			}
			set {
				radiusColor = value;
			}
		}

		public int NTicks {
			get {
				return nTicks;
			}
			set {
				nTicks = value;
			}
		}

		public float RMax {
			get {
				return rMax;
			}
			set {
				rMax = value;
			}
		}

		public float RMin {
			get {
				return rMin;
			}
			set {
				rMin = value;
			}
		}

		public float AngleStep {
			get {
				return angleStep;
			}
			set {
				angleStep = value;
			}
		}

		public int Offset {
			get {
				return offset;
			}
			set {
				offset = value;
			}
		}

		public enum AngleDirectionEnum {
			CounterClockWise = 0,
			ClockWise = 1
		}

		public CGRect SetPolarArea ()
		{
			Offset = form1.PlotPanel.Width / 10;
			int height = 0;
			if (form1.PlotPanel.Height < form1.PlotPanel.Width)
				height = form1.PlotPanel.Height - 2 * Offset;
			else
				height = form1.PlotPanel.Height - 7 * Offset;
			int width = height;
			var rect = new CGRect (Offset, Offset, width, height);
			return rect;
		}

		public void SetPolarAxes (Graphics g)
		{
			var aPen = new Pen (AngleColor, AngleThickness);
			var aBrush = new SolidBrush (TickFontColor);
			var sFormat = new StringFormat ();
			CGRect rect = SetPolarArea ();
			var xc = (float)(rect.X + rect.Width / 2);
			var yc = (float)(rect.Y + rect.Height / 2);

			// Draw circles:
			float dr = RNorm (RMax / NTicks) - RNorm (RMin / nTicks);
			aPen.DashStyle = AnglePattern;
			for (int i = 0; i < NTicks; i++) {
				var rect1 = new RectangleF (xc - (i + 1) * dr, yc - (i + 1) * dr, 2 * (i + 1) * dr, 2 * (i + 1) * dr);
				g.DrawEllipse (aPen, rect1);
			}

			// Draw radii:
			aPen = new Pen (RadiusColor, RadiusThickness);
			aPen.DashStyle = RadiusPattern;
			for (int i = 0; i < 360 / AngleStep; i++) {
				float x = RNorm (RMax) * (float)Math.Cos (i * AngleStep * 
					Math.PI / 180) + xc;
				float y = RNorm (RMax) * (float)Math.Sin (i * AngleStep * 
					Math.PI / 180) + yc;
				g.DrawLine (aPen, xc, yc, x, y);
			}

			// Draw the radius labels:
			for (int i = 1; i <= nTicks; i++) {
				float rlabel = RMin + i * (RMax - RMin) / NTicks;
				sFormat.Alignment = StringAlignment.Near;
				g.DrawString (rlabel.ToString (), TickFont, aBrush, new PointF (xc, yc - i * dr + 5), sFormat);
			}

			// Draw the angle labels:
			CGSize tickFontSize = g.MeasureString ("A", TickFont);
			float angleLabel = 0;
			for (int i = 0; i < 360 / AngleStep; i++) {
				if (AngleDirection == AngleDirectionEnum.ClockWise) {
					angleLabel = i * AngleStep;
				} else if (AngleDirection == AngleDirectionEnum.CounterClockWise) {
					angleLabel = 360 - i * AngleStep;
					if (i == 0)
						angleLabel = 0;
				}
				sFormat.Alignment = StringAlignment.Center;
				var x = (float)((RNorm(RMax) + 1.2f * tickFontSize.Width) * Math.Cos(i * AngleStep * Math.PI / 180) + xc);
				var y = (float)((RNorm(RMax) + 1.2f * tickFontSize.Width) * Math.Sin(i * AngleStep * Math.PI / 180) + yc);
				g.DrawString (angleLabel.ToString (), TickFont, aBrush, new PointF (x, (float)(y - tickFontSize.Height / 2)), sFormat);
			}
		}

		public float RNorm (float r)
		{
			float rNorm;
			CGRect rect = SetPolarArea ();
			if (r < RMin || r > RMax)
				r = float.NaN;
			rNorm = (float)((r - RMin) * rect.Width / 2 / (RMax - RMin));
			return rNorm;
		}
	}
}

