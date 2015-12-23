using System.Drawing;
using System.Drawing.Drawing2D;

using CoreGraphics;
using UIKit;

namespace MTExample2_1 {
	public class DrawingView : UIView {
		Font Font = new Font ("Helvetica",12);

		public DrawingView (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackgroundColor = UIColor.White;
		}

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext ();
			int offset = 20;
			
			// Invert matrix:
			var m = new Matrix (1, 2, 3, 4, 0, 0);
			g.DrawString ("Original Matrix:", Font, Brushes.Black, 10, 10);
			DrawMatrix (m, g, 10, 10 + offset);
			g.DrawString ("Inverted Matrix:", Font, Brushes.Black, 10, 10 + 2 * offset);
			m.Invert ();
			DrawMatrix (m, g, 10, 10 + 3 * offset);
			
			// Matrix multiplication - MatrixOrder.Append:
			var m1 = new Matrix (1, 2, 3, 4, 0, 1);
			var m2 = new Matrix (0, 1, 2, 1, 0, 1);
			g.DrawString ("Original Matrices:", Font, Brushes.Black, 10, 10 + 4 * offset);
			DrawMatrix (m1, g, 10, 10 + 5 * offset);
			DrawMatrix (m2, g, 10 + 130, 10 + 5 * offset);
			m1.Multiply (m2, MatrixOrder.Append);
			g.DrawString ("Resultant Matrix - Append:", Font, Brushes.Black, 10, 10 + 6 * offset);
			DrawMatrix (m1, g, 10, 10 + 7 * offset);

			// Matrix multiplication - MatrixOrder.Prepend:
			m1 = new Matrix (1, 2, 3, 4, 0, 1);
			m1.Multiply (m2, MatrixOrder.Prepend);
			g.DrawString ("Resultant Matrix - Prepend:", Font, Brushes.Black, 10, 10 + 8 * offset);
			DrawMatrix (m1, g, 10, 10 + 9 * offset);
		}
		
		void DrawMatrix (Matrix m, Graphics g, int x, int y)
		{
			string str = null;
			for (int i = 0; i < m.Elements.Length; i++) {
				str += m.Elements[i].ToString ();
				str += ", ";
			}

			g.DrawString (str, Font, Brushes.Black, x, y);
		}
	}
}
