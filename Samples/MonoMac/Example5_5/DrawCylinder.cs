using System;
using CoreGraphics;
//using System.Windows.Forms;
using System.Drawing;

namespace Example5_5
{
	public class DrawCylinder
	{
		private ChartCanvas form1;
		private float r = 10;
		private float h = 10;

		public DrawCylinder (ChartCanvas fm1)
		{
			form1 = fm1;
		}

		public DrawCylinder (ChartCanvas fm1, float r1, float h1)
		{
			form1 = fm1;
			r = r1;
			h = h1;
		}

		public Point3[] CircleCoordinates (float y)
		{
			Point3[] pts = new Point3[30];
			Matrix3 m = new Matrix3 ();
			for (int i = 0; i < pts.Length; i++) {
				pts [i] = m.Cylindrical (r, i * 360 / (pts.Length - 1), y);
			}
			return pts;
		}
        
		public void DrawIsometricView (Graphics g)
		{
			Point3[] ptsBottom = CircleCoordinates (-h / 2);
			PointF[] ptaBottom = new PointF[ptsBottom.Length];
			Point3[] ptsTop = CircleCoordinates (h / 2);
			PointF[] ptaTop = new PointF[ptsTop.Length];
			Matrix3 m = Matrix3.Axonometric (35.26f, -45);
			for (int i = 0; i < ptsBottom.Length; i++) {
				ptsBottom [i].Transform (m);
				ptaBottom [i] = Point2D (new PointF (ptsBottom [i].X, ptsBottom [i].Y));
				ptsTop [i].Transform (m);
				ptaTop [i] = Point2D (new PointF (ptsTop [i].X, ptsTop [i].Y));
			}

			PointF[] ptf = new PointF[4];
			for (int i = 1; i < ptsTop.Length; i++) {
				ptf [0] = ptaBottom [i - 1];
				ptf [1] = ptaTop [i - 1];
				ptf [2] = ptaTop [i];
				ptf [3] = ptaBottom [i];
				if (i < 5 || i > ptsTop.Length - 12) {
					g.FillPolygon (Brushes.White, ptf);
					g.DrawPolygon (Pens.Black, ptf);
				}
			}

			g.FillPolygon (Brushes.White, ptaTop);
			g.DrawPolygon (Pens.Black, ptaTop);
		}

		private PointF Point2D (PointF pt)
		{
			PointF aPoint = new PointF ();
			aPoint.X = form1.panel1.Width / 2 + pt.X;
			aPoint.Y = form1.panel1.Height / 2 - pt.Y;
			return aPoint;
		}
	}
}
