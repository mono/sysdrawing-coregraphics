using System.Drawing;
using System.Drawing.Drawing2D;

namespace MTExample3_7 {
	public class LineStyle {
		DashStyle linePattern = DashStyle.Solid;
		Color lineColor = Color.Black;
		float LineThickness = 1f;
		PlotLinesMethodEnum pltLineMethod = PlotLinesMethodEnum.Lines;
		bool isVisible = true;

		public LineStyle ()
		{
		}

		public bool IsVisible {
			get {
				return isVisible;
			}
			set {
				isVisible = value;
			}
		}

		public PlotLinesMethodEnum PlotMethod {
			get {
				return pltLineMethod;
			}
			set {
				pltLineMethod = value;
			}
		}

		virtual public DashStyle Pattern {
			get {
				return linePattern;
			}
			set {
				linePattern = value;
			}
		}

		public float Thickness {
			get {
				return LineThickness;
			}
			set {
				LineThickness = value;
			}
		}

		virtual public Color LineColor {
			get {
				return lineColor;
			}
			set {
				lineColor = value;
			}
		}

		public enum PlotLinesMethodEnum {
			Lines = 0,
			Splines = 1
		}
	}
}


