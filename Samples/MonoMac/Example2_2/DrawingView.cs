
using System;
using System.Collections.Generic;
using System.Linq;
using System.DrawingNative.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using System.DrawingNative;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;

namespace Example2_2
{
	public partial class DrawingView : MonoMac.AppKit.NSView
	{

		#region Constructors
		
		// Called when created from unmanaged code
		public DrawingView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public DrawingView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
		}

		public DrawingView (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion

		Font Font = new System.DrawingNative.Font("Helvetica",12);

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			Graphics g = Graphics.FromCurrentContext();
			int offset = 20;
			
			// Scale:
			Matrix m = new Matrix(1, 2, 3, 4, 0, 1);
			g.DrawString("Original Matrix:", this.Font, 
			             Brushes.Black, 10, 10);
			DrawMatrix(m, g, 10, 10 + offset);
			g.DrawString("Scale - Prepend:", this.Font, 
			             Brushes.Black, 10, 10 + 2 * offset);
			m.Scale(1, 0.5f, MatrixOrder.Prepend);
			DrawMatrix(m, g, 10, 10 + 3 * offset);
			g.DrawString("Scale - Append:", this.Font, 
			             Brushes.Black, 10, 10 + 4 * offset);
			// Reset m to the original matrix:
			m = new Matrix(1, 2, 3, 4, 0, 1);
			m.Scale(1, 0.5f, MatrixOrder.Append);
			DrawMatrix(m, g, 10, 10 + 5 * offset);
			
			// Translation:
			m = new Matrix(1, 2, 3, 4, 0, 1);
			g.DrawString("Translation - Prepend:", this.Font, 
			             Brushes.Black, 10, 10 + 6 * offset);
			m.Translate(1, 0.5f, MatrixOrder.Prepend);
			DrawMatrix(m, g, 10, 10 + 7 * offset);
			g.DrawString("Translation - Append:", this.Font, 
			             Brushes.Black, 10, 10 + 8 * offset);
			// Reset m to the original matrix:
			m = new Matrix(1, 2, 3, 4, 0, 1);
			m.Translate(1, 0.5f, MatrixOrder.Append);
			DrawMatrix(m, g, 10, 10 + 9 * offset);
			
			// Rotation:
			m = new Matrix(1, 2, 3, 4, 0, 1);
			g.DrawString("Rotation - Prepend:", this.Font, 
			             Brushes.Black, 10, 10 + 10 * offset);
			m.Rotate(45, MatrixOrder.Prepend);
			DrawMatrix(m, g, 10, 10 + 11 * offset);
			g.DrawString("Rotation - Append:", this.Font, 
			             Brushes.Black, 10, 10 + 12 * offset);
			// Reset m to the original matrix:
			m = new Matrix(1, 2, 3, 4, 0, 1);
			m.Rotate(45, MatrixOrder.Append);
			DrawMatrix(m, g, 10, 10 + 13 * offset);
			
			// Rotation at (x = 1, y = 2):
			m = new Matrix(1, 2, 3, 4, 0, 1);
			g.DrawString("Rotation At - Prepend:", this.Font, 
			             Brushes.Black, 10, 10 + 14 * offset);
			m.RotateAt(45, new PointF(1, 2), MatrixOrder.Prepend);
			DrawMatrix(m, g, 10, 10 + 15 * offset);
			g.DrawString("Rotation At - Append:", this.Font, 
			             Brushes.Black, 10, 10 + 16 * offset);
			// Reset m to the original matrix:
			m = new Matrix(1, 2, 3, 4, 0, 1);
			m.RotateAt(45, new PointF(1, 2), MatrixOrder.Append);
			DrawMatrix(m, g, 10, 10 + 17 * offset);
			
			// Shear:
			m = new Matrix(1, 2, 3, 4, 0, 1);
			g.DrawString("Shear - Prepend:", this.Font, 
			             Brushes.Black, 10, 10 + 18 * offset);
			m.Shear(1, 2, MatrixOrder.Prepend);
			DrawMatrix(m, g, 10, 10 + 19 * offset);
			g.DrawString("Shear - Append:", this.Font, 
			             Brushes.Black, 10, 10 + 20 * offset);
			// Reset m to the original matrix:
			m = new Matrix(1, 2, 3, 4, 0, 1);
			m.Shear(1, 2, MatrixOrder.Append);
			DrawMatrix(m, g, 10, 10 + 21 * offset);
		}
		
		private void DrawMatrix(Matrix m, Graphics g, int x, int y)
		{
			string str = null;
			for (int i = 0; i < m.Elements.Length; i++)
			{
				double dd = Math.Round(m.Elements[i], 3);
				str += dd.ToString();
				str += ",  ";
			}
			g.DrawString(str, this.Font, Brushes.Black, x, y);
		}
	}
}

