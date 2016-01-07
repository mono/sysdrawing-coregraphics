using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using CoreGraphics;
using UIKit;

namespace MTExample2_2 {
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
			
			// Scale:
			var m = new Matrix(1, 2, 3, 4, 0, 1);
			g.DrawString ("Original Matrix:", Font, Brushes.Black, 10, 10);
			DrawMatrix (m, g, 10, 10 + offset);
			g.DrawString ("Scale - Prepend:", Font, Brushes.Black, 10, 10 + 2 * offset);
			m.Scale (1, 0.5f, MatrixOrder.Prepend);
			DrawMatrix (m, g, 10, 10 + 3 * offset);
			g.DrawString ("Scale - Append:", Font, Brushes.Black, 10, 10 + 4 * offset);
			m = new Matrix (1, 2, 3, 4, 0, 1);
			m.Scale (1, 0.5f, MatrixOrder.Append);
			DrawMatrix (m, g, 10, 10 + 5 * offset);
			
			// Translation:
			m = new Matrix (1, 2, 3, 4, 0, 1);
			g.DrawString ("Translation - Prepend:", Font, Brushes.Black, 10, 10 + 6 * offset);
			m.Translate (1, 0.5f, MatrixOrder.Prepend);
			DrawMatrix (m, g, 10, 10 + 7 * offset);
			g.DrawString ("Translation - Append:", Font, Brushes.Black, 10, 10 + 8 * offset);
			// Reset m to the original matrix:
			m = new Matrix(1, 2, 3, 4, 0, 1);
			m.Translate (1, 0.5f, MatrixOrder.Append);
			DrawMatrix (m, g, 10, 10 + 9 * offset);

			m = new Matrix (1, 2, 3, 4, 0, 1);
			g.DrawString ("Rotation - Prepend:", Font, Brushes.Black, 10, 10 + 10 * offset);
			m.Rotate (45, MatrixOrder.Prepend);
			DrawMatrix (m, g, 10, 10 + 11 * offset);
			g.DrawString ("Rotation - Append:", Font, Brushes.Black, 10, 10 + 12 * offset);
			// Reset m to the original matrix:
			m = new Matrix (1, 2, 3, 4, 0, 1);
			m.Rotate (45, MatrixOrder.Append);
			DrawMatrix (m, g, 10, 10 + 13 * offset);
			
			// Rotation at (x = 1, y = 2):
			m = new Matrix (1, 2, 3, 4, 0, 1);
			g.DrawString ("Rotation At - Prepend:", Font, Brushes.Black, 10, 10 + 14 * offset);
			m.RotateAt (45, new Point (1, 2), MatrixOrder.Prepend);
			DrawMatrix (m, g, 10, 10 + 15 * offset);
			g.DrawString ("Rotation At - Append:", Font, Brushes.Black, 10, 10 + 16 * offset);
			m = new Matrix (1, 2, 3, 4, 0, 1);
			m.RotateAt (45, new Point (1, 2), MatrixOrder.Append);
			DrawMatrix(m, g, 10, 10 + 17 * offset);
			
			// Shear:
			m = new Matrix (1, 2, 3, 4, 0, 1);
			g.DrawString ("Shear - Prepend:", Font, Brushes.Black, 10, 10 + 18 * offset);
			m.Shear (1, 2, MatrixOrder.Prepend);
			DrawMatrix (m, g, 10, 10 + 19 * offset);
			g.DrawString ("Shear - Append:", Font, Brushes.Black, 10, 10 + 20 * offset);
			// Reset m to the original matrix:
			m = new Matrix (1, 2, 3, 4, 0, 1);
			m.Shear (1, 2, MatrixOrder.Append);
			DrawMatrix (m, g, 10, 10 + 21 * offset);
		}
		
		void DrawMatrix (Matrix m, Graphics g, int x, int y)
		{
			string str = null;
			for (int i = 0; i < m.Elements.Length; i++) {
				double dd = Math.Round (m.Elements[i], 3);
				str += dd.ToString ();
				str += ",  ";
			}

			g.DrawString (str, Font, Brushes.Black, x, y);
		}
	}
}
