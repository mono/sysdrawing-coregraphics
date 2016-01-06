using System.Drawing.Drawing2D;
using System.Drawing;

namespace MTExample4_2 {
	public class BarStyle {
		Color fillColor = Color.Black;
		Color borderColor = Color.Black;
		float borderThickness = 1f;
		float barWidth = .8f;
		DashStyle borderPattern = DashStyle.Solid;

		public float BarWidth {
			get {
				return barWidth;
			}
			set {
				barWidth = value;
			}
		}

		virtual public DashStyle BorderPattern {
			get {
				return borderPattern;
			}
			set {
				borderPattern = value;
			}
		}

		public float BorderThickness {
			get {
				return borderThickness;
			}
			set {
				borderThickness = value;
			}
		}

		virtual public Color FillColor {
			get {
				return fillColor;
			}
			set {
				fillColor = value;
			}
		}

		virtual public Color BorderColor {
			get {
				return borderColor;
			}
			set {
				borderColor = value;
			}
		}
	}
}


