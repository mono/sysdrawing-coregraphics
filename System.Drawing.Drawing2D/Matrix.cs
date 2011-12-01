//
// System.Drawing.Drawing2D.Matrix.cs: CoreGraphics implementation
//
// Authors:
//   Stefan Maierhofer <sm@cg.tuwien.ac.at>
//   Dennis Hayes (dennish@Raytek.com)
//   Duncan Mak (duncan@ximian.com)
//   Ravindra (rkumar@novell.com)
//   Miguel de Icaza (miguel@xamarin.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004, 2006 Novell, Inc (http://www.novell.com)
// Copyright 2011 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Runtime.InteropServices;
using MonoTouch.CoreGraphics;

namespace System.Drawing.Drawing2D
{
	public sealed class Matrix : MarshalByRefObject, IDisposable
	{
		CGAffineTransform transform;
		
		public Matrix ()
		{
		}

		internal Matrix (CGAffineTransform transform)
		{
			this.transform = transform;
		}
		
		public Matrix (Rectangle rect, Point[] plgpts)
		{
			if (plgpts == null)
				throw new ArgumentNullException ("plgpts");
			if (plgpts.Length != 3)
				throw new ArgumentException ("plgpts");

			Point p0 = plgpts [0];
			Point p1 = plgpts [1];
			Point p2 = plgpts [2];

			float m11 = (p1.X - p0.X) / (float)rect.Width;
			float m12 = (p1.Y - p0.Y) / (float)rect.Width;
			float m21 = (p2.X - p0.X) / (float)rect.Height;
			float m22 = (p2.X - p0.X) / (float)rect.Height;

			transform = new CGAffineTransform (m11, m12, m21, m22, p0.X, p0.Y);
			transform.Translate (rect.Width, rect.Height);
		}
	
		public Matrix (RectangleF rect, PointF[] plgpts)
		{
			if (plgpts == null)
				throw new ArgumentNullException ("plgpts");
			if (plgpts.Length != 3)
				throw new ArgumentException ("plgpts");

			PointF p0 = plgpts [0];
			PointF p1 = plgpts [1];
			PointF p2 = plgpts [2];

			float m11 = (p1.X - p0.X) / rect.Width;
			float m12 = (p1.Y - p0.Y) / rect.Width;
			float m21 = (p2.X - p0.X) / rect.Height;
			float m22 = (p2.X - p0.X) / rect.Height;

			transform = new CGAffineTransform (m11, m12, m21, m22, p0.X, p0.Y);
			transform.Translate (rect.Width, rect.Height);
		}

		public Matrix (float m11, float m12, float m21, float m22, float dx, float dy)
		{
			transform = new CGAffineTransform (m11, m12, m21, m22, dx, dy);
		}
	
		// properties
		public float[] Elements {
			get {
				return new float [6] { transform.xx, transform.yx, transform.xy, transform.yy, transform.x0, transform.y0 };
			}
		}
	
		public bool IsIdentity {
			get {
				return transform.IsIdentity;
			}
		}
	
		public bool IsInvertible {
			get {
				var inverted = transform.Invert ();
				if (inverted.xx == transform.xx &&
				    inverted.xy == transform.xy &&
				    inverted.yx == transform.yx &&
				    inverted.yy == transform.yy &&
				    inverted.x0 == transform.x0 &&
				    inverted.y0 == transform.y0)
					return false;
				return true;
			}
		}
	
		public float OffsetX {
			get {
				return transform.x0;
			}
		}
	
		public float OffsetY {
			get {
				return transform.y0;
			}
		}

		public Matrix Clone()
		{
			var copy = new Matrix ();
			copy.transform = transform;

			return copy;
		}
		
	
		public void Dispose ()
		{
		}			
	
		public override bool Equals (object obj)
		{
			Matrix m = obj as Matrix;

			if (m != null) {
				var o = m.transform;
				var t = transform;
				return (o.x0 == t.x0 && o.y0 == t.y0 &&
					o.xx == t.xx && o.yy == t.yy &&
					o.xy == t.xy && o.yx == t.yx);
			} else
				return false;
		}
	
		~Matrix()
		{
		}
		
		public override int GetHashCode ()
		{
			return transform.GetHashCode ();
		}
	
		public void Invert ()
		{
			transform = transform.Invert ();
		}
	
		public void Multiply (Matrix matrix)
		{
			if (matrix == null)
				throw new ArgumentNullException ("matrix");
			transform.Multiply (matrix.transform);
		}
	
		public void Multiply (Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
				throw new ArgumentNullException ("matrix");

			if (order == MatrixOrder.Prepend)
				transform.Multiply (matrix.transform);
			else {
				var mtrans = matrix.transform;
				mtrans.Multiply (transform);
				transform = mtrans;
			}
		}
	
		public void Reset()
		{
			transform = CGAffineTransform.MakeIdentity ();
		}
	
		public void Rotate (float angle)
		{
			transform.Rotate (angle);
		}
	
		public void Rotate (float angle, MatrixOrder order)
		{
			throw new NotImplementedException ();
		}
	
		public void RotateAt (float angle, PointF point)
		{
			RotateAt (angle, point, MatrixOrder.Prepend);
		}
	
		public void RotateAt (float angle, PointF point, MatrixOrder order)
		{
			if ((order < MatrixOrder.Prepend) || (order > MatrixOrder.Append))
				throw new ArgumentException ("order");

			angle *= (float) (Math.PI / 180.0);  // degrees to radians
			float cos = (float) Math.Cos (angle);
			float sin = (float) Math.Sin (angle);
			float e4 = -point.X * cos + point.Y * sin + point.X;
			float e5 = -point.X * sin - point.Y * cos + point.Y;
			float[] m = this.Elements;

			if (order == MatrixOrder.Prepend)
				transform = new CGAffineTransform (cos * m[0] + sin * m[2],
								   cos * m[1] + sin * m[3],
								   -sin * m[0] + cos * m[2],
								   -sin * m[1] + cos * m[3],
								   e4 * m[0] + e5 * m[2] + m[4],
								   e4 * m[1] + e5 * m[3] + m[5]);
			else
				transform = new CGAffineTransform (m[0] * cos + m[1] * -sin,
								   m[0] * sin + m[1] * cos,
								   m[2] * cos + m[3] * -sin,
								   m[2] * sin + m[3] * cos,
								   m[4] * cos + m[5] * -sin + e4,
								   m[4] * sin + m[5] * cos + e5);
		}
	
		public void Scale (float scaleX, float scaleY)
		{
			transform.Scale (scaleX, scaleY);
		}
	
		public void Scale (float scaleX, float scaleY, MatrixOrder order)
		{
			var affine = CGAffineTransform.MakeScale (scaleX, scaleY);
			if (order == MatrixOrder.Prepend)
				transform.Multiply (affine);
			else {
				affine.Multiply (transform);
				transform = affine;
			}
		}
	
		public void Shear (float shearX, float shearY)
		{
			transform.Multiply (new CGAffineTransform (1, shearX, shearY, 1, 0, 0));
		}
	
		public void Shear (float shearX, float shearY, MatrixOrder order)
		{
			var affine = new CGAffineTransform (1, shearX, shearY, 1, 0, 0);
			if (order == MatrixOrder.Prepend)
				transform.Multiply (affine);
			else {
				affine.Multiply (transform);
				transform = affine;
			}
		}
	
		public void TransformPoints (Point[] pts)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");

			for (int i = 0; i < pts.Length; i++){
				var point = pts [i];
				pts [i] = new Point ((int)(transform.xx * point.X + transform.xy * point.Y + transform.x0),
						     (int)(transform.yx * point.X + transform.yy * point.Y + transform.y0));
			}
		}
	
		public void TransformPoints (PointF[] pts)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");

			for (int i = 0; i < pts.Length; i++)
				pts [i] = transform.TransformPoint (pts [i]);
		}
	
		public void TransformVectors (Point[] pts)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");

			for (int i = 0; i < pts.Length; i++){
				var point = pts [i];
				pts [i] = new Point ((int)(transform.xx * point.X + transform.xy * point.Y),
						     (int)(transform.yx * point.X + transform.yy * point.Y));
			}
		}
	
		public void TransformVectors (PointF[] pts)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");

			for (int i = 0; i < pts.Length; i++){
				var point = pts [i];
				pts [i] = new PointF ((float)(transform.xx * point.X + transform.xy * point.Y),
						      (float)(transform.yx * point.X + transform.yy * point.Y));
			}
		}
	
		public void Translate (float offsetX, float offsetY)
		{
			transform.Translate (offsetX, offsetY);
		}
	
		public void Translate (float offsetX, float offsetY, MatrixOrder order)
		{
			var affine = CGAffineTransform.MakeTranslation (offsetX, offsetY);
			if (order == MatrixOrder.Prepend)
				transform.Multiply (affine);
			else {
				affine.Multiply (transform);
				transform = affine;
			}
		}
	
		public void VectorTransformPoints (Point[] pts)
		{
			TransformVectors (pts);
		}
	}
}
