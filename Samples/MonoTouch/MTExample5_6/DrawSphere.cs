using System.Drawing;
using CoreGraphics;

namespace MTExample5_6 {
	public class DrawSphere {
		ChartCanvas form1;
		float r = 10f;
		float xc = 0f;
		float yc = 0f;
		float zc = 0f;

		public DrawSphere (ChartCanvas fm1)
		{
			form1 = fm1;
		}

		public DrawSphere (ChartCanvas fm1, float r1, float xc1, float yc1, float zc1)
		{
			form1 = fm1;
			r = r1;
			xc = xc1;
			yc = yc1;
			zc = zc1;
		}

		public Point3[,] SphereCoordinates()
		{
			Point3[,] pts = new Point3[30, 20];
			var m = new Matrix3 ();
			var mt = Matrix3.Translate3 (xc, yc, zc);

			for (int i = 0; i < pts.GetLength(0); i++) {
				for (int j = 0; j < pts.GetLength(1); j++) {
					pts[i, j] = m.Spherical(r, i * 180 / (pts.GetLength(0) - 1),
						j * 360 / (pts.GetLength(1) - 1));
					pts[i, j].Transform(mt);
				}
			}
			return pts;
		}

		public void DrawIsometricView (Graphics g)
		{
			Matrix3 m = Matrix3.Axonometric(35.26f, -45);
			Point3[,] pts = SphereCoordinates();
			PointF[,] pta = new PointF[pts.GetLength(0), pts.GetLength(1)];
			for (int i = 0; i < pts.GetLength(0); i++) {
				for (int j = 0; j < pts.GetLength(1); j++) {
					pts[i, j].Transform(m);
					pta[i, j] = Point2D (new CGPoint(pts[i, j].X, pts[i, j].Y));
				}
			}

			var ptf = new PointF[4];
			for (int i = 1; i < pts.GetLength(0); i++) {
				for (int j = 1; j < pts.GetLength(1); j++) {
					ptf[0] = pta[i - 1, j - 1];
					ptf[1] = pta[i, j - 1];
					ptf[2] = pta[i, j];
					ptf[3] = pta[i - 1, j];
					g.FillPolygon(Brushes.White, ptf);
					g.DrawPolygon(Pens.Black, ptf);
				}
			}
		}

		PointF Point2D (CGPoint pt)
		{
			var aPoint = new PointF ();
			aPoint.X = (float)(form1.panel1.Width / 2 + pt.X);
			aPoint.Y = (float)(form1.panel1.Height / 2 - pt.Y);
			return aPoint;
		}
	}
}
