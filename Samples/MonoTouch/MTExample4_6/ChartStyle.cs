using CoreGraphics;

namespace MTExample4_6 {
	public class ChartStyle {
		ChartCanvas form1;
		int offset;

		public ChartStyle (ChartCanvas fm1)
		{
			form1 = fm1;
		}

		public int Offset {
			get {
				return offset;
			}
			set {
				offset = value;
			}
		}

		public CGRect SetPieArea ()
		{
			Offset = form1.PlotPanel.Width / 10;
			int height = form1.PlotPanel.Height - 2 * Offset;
			int width = height;
			var rect = new CGRect (Offset, Offset, width, height);
			return rect;
		}
	}
}

