using System.Drawing;
using System.Drawing.Drawing2D;

namespace MTExample4_8 {
	public class LineStyle {
		DashStyle linePattern = DashStyle.Solid;
		Color lineColor = Color.Black;
		float LineThickness = 1f;
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

		virtual public DashStyle Pattern {
			get {
				return linePattern; }
			set {
				linePattern = value; }
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
	}
}


