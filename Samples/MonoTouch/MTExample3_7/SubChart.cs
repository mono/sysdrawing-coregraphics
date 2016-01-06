using System.Drawing;

namespace MTExample3_7 {
	public class SubChart {
		int rows = 1;
		int cols = 1;
		int margin = 0;
		Rectangle totalChartArea;
		Color totalChartBackColor;
		Color totalChartBorderColor;
		IForm form1;

		public SubChart (IForm fm1)
		{
			form1 = fm1;
			TotalChartArea = form1.ClientRectangle;
			totalChartBackColor = fm1.BackColor;
			totalChartBorderColor = fm1.BackColor;
		}

		public int Rows {
			get {
				return rows;
			}
			set {
				rows = value;
			}
		}

		public int Cols {
			get {
				return cols;
			}
			set {
				cols = value;
			}
		}

		public int Margin {
			get {
				return margin;
			}
			set {
				margin = value;
			}
		}

		public Rectangle TotalChartArea {
			get {
				return totalChartArea;
			}
			set {
				totalChartArea = value;
			}
		}

		public Color TotalChartBackColor {
			get {
				return totalChartBackColor;
			}
			set {
				totalChartBackColor = value;
			}
		}

		public Color TotalChartBorderColor {
			get {
				return totalChartBorderColor;
			}
			set {
				totalChartBorderColor = value;
			}
		}

		public Rectangle[,] SetSubChart (Graphics g)
		{
			var subRectangle = new Rectangle [Rows, Cols];
			var subWidth = (TotalChartArea.Width - 2 * Margin) / Cols;
			var subHeight = (TotalChartArea.Height - 2 * Margin) / Rows;
			for (int i = 0; i < Rows; i++) {
				for (int j = 0; j < Cols; j++) {
					int x = TotalChartArea.X + Margin + j * subWidth;
					int y = TotalChartArea.Y + Margin + i * subHeight;
					subRectangle[i, j] = new Rectangle (x, y, subWidth, subHeight);
				}
			}
			// Draw total chart area:
			var aPen = new Pen(TotalChartBorderColor, 1f);
			var aBrush = new SolidBrush(TotalChartBackColor);
			g.FillRectangle (aBrush, TotalChartArea);
			g.DrawRectangle (aPen, TotalChartArea);
			return subRectangle;
		}
	}
}
