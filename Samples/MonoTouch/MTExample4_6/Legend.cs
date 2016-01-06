using System.Drawing;
using CoreGraphics;

namespace MTExample4_6 {
	public class Legend {
		ChartCanvas form1;
		bool isLegendVisible;
		Color textColor;
		bool isBorderVisible;
		Color legendBackColor;
		Color legendBorderColor;
		Font legendFont;

		public Legend (ChartCanvas fm1)
		{
			form1 = fm1;
			textColor = Color.Black;
			isLegendVisible = false;
			isBorderVisible = true;
			legendBackColor = Color.White;
			legendBorderColor = Color.Black;
			legendFont = new Font ("Arial", 8, FontStyle.Regular);
		}

		public Font LegendFont {
			get {
				return legendFont;
			}
			set {
				legendFont = value;
			}
		}

		public Color LegendBackColor {
			get {
				return legendBackColor;
			}
			set {
				legendBackColor = value;
			}
		}

		public Color LegendBorderColor {
			get {
				return legendBorderColor;
			}
			set {
				legendBorderColor = value;
			}
		}

		public bool IsBorderVisible {
			get {
				return isBorderVisible;
			}
			set {
				isBorderVisible = value;
			}
		}

		public Color TextColor {
			get {
				return textColor;
			}
			set {
				textColor = value;
			}
		}

		public bool IsLegendVisible {
			get {
				return isLegendVisible;
			}
			set {
				isLegendVisible = value;
			}
		}

		public void AddLegend (Graphics g, DataSeries ds, ChartStyle cs)
		{
			if (ds.DataList.Count < 1)
				return;
			if (!IsLegendVisible)
				return;
			int numberOfDataValues = ds.DataList.Count;
			string[] legendLabels = new string[ds.LabelList.Count];
			for (int i = 0; i < ds.LabelList.Count; i++)
				legendLabels[i] = (string)ds.LabelList[i];
			float xc = 0f;
			float yc = 0f;
			CGSize size = g.MeasureString (legendLabels[0], LegendFont);
			var legendWidth = (float)size.Width;
			for (int i = 0; i < legendLabels.Length; i++) {
				size = g.MeasureString (legendLabels[i], LegendFont);
				var tempWidth = (float)size.Width;
				if (legendWidth < tempWidth)
					legendWidth = tempWidth;
			}

			legendWidth = legendWidth + 35f;
			float hWidth = legendWidth / 2;
			float legendHeight = 18f * numberOfDataValues;
			float hHeight = legendHeight / 2;

			CGRect rect = cs.SetPieArea ();
			xc = (float)(rect.X + rect.Width + cs.Offset + 20 + hWidth / 2);
			yc = (float)(rect.Y + rect.Height / 2);
			DrawLegend (g, xc, yc, hWidth, hHeight, ds, cs);
		}

		void DrawLegend (Graphics g, float xCenter, float yCenter, float hWidth, float hHeight, DataSeries ds, ChartStyle cs)
		{
			float spacing = 8.0f;
			float textHeight = 8.0f;
			float htextHeight = textHeight / 2.0f;
			float lineLength = 12.0f;
			float hlineLength = lineLength / 2.0f;
			Rectangle legendRectangle;
			var aPen = new Pen (LegendBorderColor, 1f);
			var aBrush = new SolidBrush (LegendBackColor);

			if (isLegendVisible) {
				legendRectangle = new Rectangle ((int)xCenter - (int)hWidth,
					(int)yCenter - (int)hHeight,
					(int)(2.0f * hWidth), (int)(2.0f * hHeight));
				g.FillRectangle(aBrush, legendRectangle);
				if (IsBorderVisible)
					g.DrawRectangle (aPen, legendRectangle);

				for (int i = 0; i < ds.DataList.Count; i++) {
					float xSymbol = legendRectangle.X + spacing + hlineLength;
					float xText = legendRectangle.X + 2 * spacing + lineLength;
					float yText = legendRectangle.Y + (i + 1) * spacing +
						(2 * i + 1) * htextHeight;
					aPen = new Pen (ds.BorderColor, ds.BorderThickness);
					Color fillColor = Color.FromArgb (ds.CMap[i, 0], ds.CMap[i, 1],
								ds.CMap[i, 2], ds.CMap[i, 3]);
					aBrush = new SolidBrush (fillColor);
					// Draw symbols:
					float hsize = 5f;
					var pts = new PointF[4];
					pts[0] = new PointF (xSymbol - hsize, yText - hsize);
					pts[1] = new PointF (xSymbol + hsize, yText - hsize);
					pts[2] = new PointF (xSymbol + hsize, yText + hsize);
					pts[3] = new PointF (xSymbol - hsize, yText + hsize);
					g.FillPolygon (aBrush, pts);
					g.DrawPolygon (aPen, pts);
					// Draw text:
					var sFormat = new StringFormat {
						Alignment = StringAlignment.Near
					};
					g.DrawString ((string)ds.LabelList[i], LegendFont, new SolidBrush (TextColor), new PointF (xText, yText - 8), sFormat);
				}
			}
			aPen.Dispose ();
			aBrush.Dispose ();
		}
	}
}
