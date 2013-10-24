using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Drawing
{
	internal static class GeomTransformUtils
	{
		static double[] coeffs = new double[8];

		public static void WarpPath(GraphicsPath path, PointF[] destPoints, RectangleF srcRect, Matrix matrix = null, WarpMode warpMode = WarpMode.Perspective, float flatness = 0.25f)
		{

			if (path.PointCount == 0)
				return;

			path.Flatten(matrix, flatness);

			var pathData = path.PathData;
			var pnts = path.PathPoints;

			var srcPoints = new PointF[] { new PointF(srcRect.Left, srcRect.Top),
				new PointF(srcRect.Right, srcRect.Top),
				new PointF(srcRect.Left, srcRect.Bottom),
				new PointF(srcRect.Right, srcRect.Bottom) };

			var count = pnts.Length;
			float x1, y1;
			int i;

			if (warpMode == WarpMode.Perspective)
			{
				CalcProjectiveXformCoeffs(srcPoints, destPoints, out coeffs);

				for (i = 0; i < count; i++)
				{
					x1 = pnts[i].X;
					y1 = pnts[i].Y;

					var factor = 1.0f / (coeffs[6] * x1 + coeffs[7] * y1 + 1.0f);
					pnts[i].X = (float)(factor * (coeffs[0] * x1 + coeffs[1] * y1 + coeffs[2]));
					pnts[i].Y = (float)(factor * (coeffs[3] * x1 + coeffs[4] * y1 + coeffs[5]));
				}
			}
			else
			{
				CalcBilinearXformCoeffs(srcPoints, destPoints, out coeffs);

				for (i = 0; i < count; i++)
				{
					x1 = pnts[i].X;
					y1 = pnts[i].Y;

					pnts[i].X = (float)(coeffs[0] * x1 + coeffs[1] * y1 + coeffs[2] * x1 * y1 + coeffs[3]);
					pnts[i].Y = (float)(coeffs[4] * x1 + coeffs[5] * y1 + coeffs[6] * x1 * y1 + coeffs[7]);
				}

			}

			GraphicsPath warpedPath = new GraphicsPath(pnts, pathData.Types);
			if (warpedPath != null)
			{
				FillMode fm = path.FillMode;
				path.Reset();
				path.FillMode = fm;

				path.AddPath(warpedPath, true);
				warpedPath.Dispose();
			}
		}


		/*-------------------------------------------------------------*
         *                Projective coordinate transformation         *
         * http://tpgit.github.io/Leptonica/projective_8c_source.html  *
         *-------------------------------------------------------------*/
		/*!
         *  CalcProjectiveXformCoeffs()
         *
         *      Input:  srcPoints  (source 4 points; unprimed)
         *              destPoints  (transformed 4 points; primed)
         *              out transformCoeffs   (<return> vector of coefficients of transform)
         *
         *  We have a set of 8 equations, describing the projective
         *  transformation that takes 4 points (srcPoints) into 4 other
         *  points (desPoints).  These equations are:
         *
         *          x1' = (c[0]*x1 + c[1]*y1 + c[2]) / (c[6]*x1 + c[7]*y1 + 1)
         *          y1' = (c[3]*x1 + c[4]*y1 + c[5]) / (c[6]*x1 + c[7]*y1 + 1)
         *          x2' = (c[0]*x2 + c[1]*y2 + c[2]) / (c[6]*x2 + c[7]*y2 + 1)
         *          y2' = (c[3]*x2 + c[4]*y2 + c[5]) / (c[6]*x2 + c[7]*y2 + 1)
         *          x3' = (c[0]*x3 + c[1]*y3 + c[2]) / (c[6]*x3 + c[7]*y3 + 1)
         *          y3' = (c[3]*x3 + c[4]*y3 + c[5]) / (c[6]*x3 + c[7]*y3 + 1)
         *          x4' = (c[0]*x4 + c[1]*y4 + c[2]) / (c[6]*x4 + c[7]*y4 + 1)
         *          y4' = (c[3]*x4 + c[4]*y4 + c[5]) / (c[6]*x4 + c[7]*y4 + 1)
         *
         *  Multiplying both sides of each eqn by the denominator, we get
         *
         *           AC = B
         *
         *  where B and C are column vectors
         *
         *         B = [ x1' y1' x2' y2' x3' y3' x4' y4' ]
         *         C = [ c[0] c[1] c[2] c[3] c[4] c[5] c[6] c[7] ]
         *
         *  and A is the 8x8 matrix
         *
         *             x1   y1     1     0   0    0   -x1*x1'  -y1*x1'
         *              0    0     0    x1   y1   1   -x1*y1'  -y1*y1'
         *             x2   y2     1     0   0    0   -x2*x2'  -y2*x2'
         *              0    0     0    x2   y2   1   -x2*y2'  -y2*y2'
         *             x3   y3     1     0   0    0   -x3*x3'  -y3*x3'
         *              0    0     0    x3   y3   1   -x3*y3'  -y3*y3'
         *             x4   y4     1     0   0    0   -x4*x4'  -y4*x4'
         *              0    0     0    x4   y4   1   -x4*y4'  -y4*y4'
         *
         *  These eight equations are solved here for the coefficients C.
         *
         *  These eight coefficients can then be used to find the mapping
         *  (x,y) --> (x',y'):
         *
         *           x' = (c[0]x + c[1]y + c[2]) / (c[6]x + c[7]y + 1)
         *           y' = (c[3]x + c[4]y + c[5]) / (c[6]x + c[7]y + 1)
         *
         */
		static void CalcProjectiveXformCoeffs(PointF[] srcPoints,
		                                      PointF[] destPoints,
		                                      out double[] transformCoeffs)
		{
			int i;
			float x1, y1, x2, y2, x3, y3, x4, y4;
			double[] b = new double[8];   // vector of primed coords X'; coeffs returned in transformCoeffs 
			double[][] a = new double[8][];  // 8x8 matrix A  

			x1 = srcPoints[0].X;
			y1 = srcPoints[0].Y;
			x2 = srcPoints[1].X;
			y2 = srcPoints[1].Y;
			x3 = srcPoints[2].X;
			y3 = srcPoints[2].Y;
			x4 = srcPoints[3].X;
			y4 = srcPoints[3].Y;

			b[0] = destPoints[0].X;
			b[1] = destPoints[0].Y;
			b[2] = destPoints[1].X;
			b[3] = destPoints[1].Y;
			b[4] = destPoints[2].X;
			b[5] = destPoints[2].Y;
			b[6] = destPoints[3].X;
			b[7] = destPoints[3].Y;

			for (i = 0; i < 8; i++)
			{
				a[i] = new double[8];
			}

			a[0][0] = x1;
			a[0][1] = y1;
			a[0][2] = 1.0f;
			a[0][6] = -x1 * b[0];
			a[0][7] = -y1 * b[0];
			a[1][3] = x1;
			a[1][4] = y1;
			a[1][5] = 1;
			a[1][6] = -x1 * b[1];
			a[1][7] = -y1 * b[1];
			a[2][0] = x2;
			a[2][1] = y2;
			a[2][2] = 1.0f;
			a[2][6] = -x2 * b[2];
			a[2][7] = -y2 * b[2];
			a[3][3] = x2;
			a[3][4] = y2;
			a[3][5] = 1;
			a[3][6] = -x2 * b[3];
			a[3][7] = -y2 * b[3];
			a[4][0] = x3;
			a[4][1] = y3;
			a[4][2] = 1.0f;
			a[4][6] = -x3 * b[4];
			a[4][7] = -y3 * b[4];
			a[5][3] = x3;
			a[5][4] = y3;
			a[5][5] = 1;
			a[5][6] = -x3 * b[5];
			a[5][7] = -y3 * b[5];
			a[6][0] = x4;
			a[6][1] = y4;
			a[6][2] = 1.0f;
			a[6][6] = -x4 * b[6];
			a[6][7] = -y4 * b[6];
			a[7][3] = x4;
			a[7][4] = y4;
			a[7][5] = 1;
			a[7][6] = -x4 * b[7];
			a[7][7] = -y4 * b[7];

			GaussJordan(a, b);  // Solve the equation

			transformCoeffs = b;
		}


		/*-------------------------------------------------------------*
         *                Bilinear coordinate transformation           *
         * http://tpgit.github.io/Leptonica/bilinear_8c_source.html    *               
         *-------------------------------------------------------------*/
		/*!
         *  CalcBilinearXformCoeffs()
         *
         *      Input:  srcPoints  (source 4 points; unprimed)
         *              destPoints  (transformed 4 points; primed)
         *              out transformCoeffs   (<return> vector of coefficients of transform)
         *
         *  We have a set of 8 equations, describing the bilinear
         *  transformation that takes 4 points (ptas) into 4 other
         *  points (ptad).  These equations are:
         *
         *          x1' = c[0]*x1 + c[1]*y1 + c[2]*x1*y1 + c[3]
         *          y1' = c[4]*x1 + c[5]*y1 + c[6]*x1*y1 + c[7]
         *          x2' = c[0]*x2 + c[1]*y2 + c[2]*x2*y2 + c[3]
         *          y2' = c[4]*x2 + c[5]*y2 + c[6]*x2*y2 + c[7]
         *          x3' = c[0]*x3 + c[1]*y3 + c[2]*x3*y3 + c[3]
         *          y3' = c[4]*x3 + c[5]*y3 + c[6]*x3*y3 + c[7]
         *          x4' = c[0]*x4 + c[1]*y4 + c[2]*x4*y4 + c[3]
         *          y4' = c[4]*x4 + c[5]*y4 + c[6]*x4*y4 + c[7]
         *
         *  This can be represented as
         *
         *           AC = B
         *
         *  where B and C are column vectors
         *
         *         B = [ x1' y1' x2' y2' x3' y3' x4' y4' ]
         *         C = [ c[0] c[1] c[2] c[3] c[4] c[5] c[6] c[7] ]
         *
         *  and A is the 8x8 matrix
         *
         *             x1   y1   x1*y1   1   0    0      0     0
         *              0    0     0     0   x1   y1   x1*y1   1
         *             x2   y2   x2*y2   1   0    0      0     0
         *              0    0     0     0   x2   y2   x2*y2   1
         *             x3   y3   x3*y3   1   0    0      0     0
         *              0    0     0     0   x3   y3   x3*y3   1
         *             x4   y4   x4*y4   1   0    0      0     0
         *              0    0     0     0   x4   y4   x4*y4   1
         *
         *  These eight equations are solved here for the coefficients C.
         *
         *  These eight coefficients can then be used to find the mapping
         *  (x,y) --> (x',y'):
         *
         *           x' = c[0]x + c[1]y + c[2]xy + c[3]
         *           y' = c[4]x + c[5]y + c[6]xy + c[7]
         *
         */
		static void CalcBilinearXformCoeffs(PointF[] srcPoints,
		                                    PointF[] destPoints,
		                                    out double[] transformCoeffs)
		{
			int i;
			float x1, y1, x2, y2, x3, y3, x4, y4;
			double[] b = new double[8];   // vector of primed coords X'; coeffs returned in transformCoeffs
			double[][] a = new double[8][];  // 8x8 matrix A  

			x1 = srcPoints[0].X;
			y1 = srcPoints[0].Y;
			x2 = srcPoints[1].X;
			y2 = srcPoints[1].Y;
			x3 = srcPoints[2].X;
			y3 = srcPoints[2].Y;
			x4 = srcPoints[3].X;
			y4 = srcPoints[3].Y;

			b[0] = destPoints[0].X;
			b[1] = destPoints[0].Y;
			b[2] = destPoints[1].X;
			b[3] = destPoints[1].Y;
			b[4] = destPoints[2].X;
			b[5] = destPoints[2].Y;
			b[6] = destPoints[3].X;
			b[7] = destPoints[3].Y;

			for (i = 0; i < 8; i++)
			{
				a[i] = new double[8];
			}

			a[0][0] = x1;
			a[0][1] = y1;
			a[0][2] = x1 * y1;
			a[0][3] = 1.0f;
			a[1][4] = x1;
			a[1][5] = y1;
			a[1][6] = x1 * y1;
			a[1][7] = 1.0f;
			a[2][0] = x2;
			a[2][1] = y2;
			a[2][2] = x2 * y2;
			a[2][3] = 1.0f;
			a[3][4] = x2;
			a[3][5] = y2;
			a[3][6] = x2 * y2;
			a[3][7] = 1.0f;
			a[4][0] = x3;
			a[4][1] = y3;
			a[4][2] = x3 * y3;
			a[4][3] = 1.0f;
			a[5][4] = x3;
			a[5][5] = y3;
			a[5][6] = x3 * y3;
			a[5][7] = 1.0f;
			a[6][0] = x4;
			a[6][1] = y4;
			a[6][2] = x4 * y4;
			a[6][3] = 1.0f;
			a[7][4] = x4;
			a[7][5] = y4;
			a[7][6] = x4 * y4;
			a[7][7] = 1.0f;

			GaussJordan(a, b);

			transformCoeffs = b;
		}



		// Full explanation of the following can be found in the book
		// Technical Java: Developing Scientific and Engineering Applications
		// Chapter 19. Solving Systems of equations
		// and conververted to C#
		private static void PartialPivot(double[][] a, double[] b, int[][] index)
		{
			double temp;
			double[] tempRow;
			int i, j, m;
			int numRows = a.Length;
			int numCols = a[0].Length;
			double[] scale = new double[numRows];

			//  Determine the scale factor (the largest element)
			//  for each row to use with implicit pivoting.
			//  Initialize the index[][] array for an unmodified
			//  array.

			for (i = 0; i < numRows; ++i)
			{
				index[i][0] = i;
				index[i][1] = i;
				for (j = 0; j < numCols; ++j)
				{
					scale[i] = Math.Max(scale[i], Math.Abs(a[i][j]));
				}
			}

			//  Determine the pivot element for each column and
			//  rearrange the rows accordingly. The m variable
			//  stores the row number that has the maximum
			//  scaled value below the diagonal for each column.
			//  The index[][] array stores the history of the row
			//  swaps and is used by the Gauss-Jordan method to
			//  unscramble the inverse a[][] matrix

			for (j = 0; j < numCols - 1; ++j)
			{
				m = j;
				for (i = j + 1; i < numRows; ++i)
				{
					if (Math.Abs(a[i][j]) / scale[i] >
					    Math.Abs(a[m][j]) / scale[m])
					{
						m = i;
					}
				}
				if (m != j)
				{
					index[j][0] = j;
					index[j][1] = m;

					tempRow = a[j];
					a[j] = a[m];
					a[m] = tempRow;

					temp = b[j];
					b[j] = b[m];
					b[m] = temp;

					temp = scale[j];
					scale[j] = scale[m];
					scale[m] = temp;
				}
			}
			return;
		}

		// Full explanation of the following can be found in the book
		// Technical Java: Developing Scientific and Engineering Applications
		// Chapter 19. Solving Systems of equations
		// and conververted to C#
		static void GaussJordan(double[][] a, double[] b)
		{
			int i, j, k, m;
			double temp;

			int numRows = a.Length;
			int numCols = a[0].Length;
			int[][] index = new int[numRows][];// {new int[2], new int[2] };

			for (int ix = 0; ix < numRows; ix++)
			{
				index[ix] = new int[2];
			}

			//  Perform an implicit partial pivoting of the
			//  a[][] array and b[]  array.

			PartialPivot(a, b, index);

			//  Perform the elimination row by row. First dividing
			//  the current row and b element by a[i][i]

			for (i = 0; i < numRows; ++i)
			{
				temp = a[i][i];
				for (j = 0; j < numCols; ++j)
				{
					a[i][j] /= temp;
				}
				b[i] /= temp;
				a[i][i] = 1.0 / temp;

				//  Reduce the other rows by subtracting a multiple
				//  of the current row from them. Don't reduce the
				//  current row. As each column of the a[][] matrix
				//  is reduced its elements are replaced with the
				//  inverse a[][] matrix.

				for (k = 0; k < numRows; ++k)
				{
					if (k != i)
					{
						temp = a[k][i];
						for (j = 0; j < numCols; ++j)
						{
							a[k][j] -= temp * a[i][j];
						}
						b[k] -= temp * b[i];
						a[k][i] = -temp * a[i][i];
					}
				}
			}

			//  Unscramble the inverse a[][] matrix.
			//  The columns are swapped in the opposite order
			//  that the rows were during the pivoting.

			for (j = numCols - 1; j >= 0; --j)
			{
				k = index[j][0];
				m = index[j][1];
				if (k != m)
				{
					for (i = 0; i < numRows; ++i)
					{
						temp = a[i][m];
						a[i][m] = a[i][k];
						a[i][k] = temp;
					}
				}
			}

			return;
		}

	}
}
