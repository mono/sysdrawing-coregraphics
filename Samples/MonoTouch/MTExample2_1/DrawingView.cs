using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample2_1
{
	public class DrawingView : UIView {
		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			BackgroundColor = UIColor.White;
		}

		Font Font = new System.Drawing.Font("Arial",14);

		public override void Draw (RectangleF rect)
		{
			Graphics g = new Graphics();
			int offset = 20;
			
			// Invert matrix:
			Matrix m = new Matrix(1, 2, 3, 4, 0, 0);
			g.DrawString("Original Matrix:", this.Font, Brushes.Black, 10, 10);
			DrawMatrix(m, g, 10, 10 + offset);
			g.DrawString("Inverted Matrix:", this.Font, Brushes.Black, 10, 10 + 2*offset);
			m.Invert();
			DrawMatrix(m, g, 10, 10 + 3 * offset);
			
			// Matrix multiplication - MatrixOrder.Append:
			Matrix m1 = new Matrix(1, 2, 3, 4, 0, 1);
			Matrix m2 = new Matrix(0, 1, 2, 1, 0, 1);
			g.DrawString("Original Matrices:", this.Font, Brushes.Black, 10, 10 + 4 * offset);
			DrawMatrix(m1, g, 10, 10 + 5 * offset);
			DrawMatrix(m2, g, 10 + 100, 10 + 5 * offset);
			m1.Multiply(m2, MatrixOrder.Append);
			g.DrawString("Resultant Matrix - Append:", this.Font, Brushes.Black, 10, 10 + 6 * offset);
			DrawMatrix(m1, g, 10, 10 + 7 * offset);
			
			// Matrix multiplication - MatrixOrder.Prepend:
			m1 = new Matrix(1, 2, 3, 4, 0, 1);
			m1.Multiply(m2, MatrixOrder.Prepend);
			g.DrawString("Resultant Matrix - Prepend:", this.Font, Brushes.Black, 10, 10 + 8 * offset);
			DrawMatrix(m1, g, 10, 10 + 9 * offset);
		}
		
		private void DrawMatrix(Matrix m, Graphics g, int x, int y)
		{
			string str = null;
			for (int i = 0; i < m.Elements.Length; i++)
			{
				str += m.Elements[i].ToString();
				str += ", ";
			}
			g.DrawString(str, this.Font, Brushes.Black, x, y);
		}

	}
}