// 
// GraphicsPath.cs: stores a path
//
// Authors:
//      Miguel de Icaza (miguel@gnome.org)
//      Duncan Mak (duncan@xamarin.com)
//      Ravindra (rkumar@novell.com)
//      Sebastien Pouliot  <spouliot@xamarin.com>
//      Jordi Mas
//
//     
// Copyright 2011 Xamarin Inc.
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
using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace System.Drawing.Drawing2D {
	public sealed class GraphicsPath : ICloneable, IDisposable {
		List<PointF> points;
		List<byte> types;
		FillMode fillMode;
		bool start_new_fig;

		enum CurveType { Open, Close }
		const int CURVE_MIN_TERMS = 1;
		const int CURVE_MAX_TERMS = 7;
			
		public GraphicsPath () : this (FillMode.Alternate)
		{
		}

		public GraphicsPath (FillMode fillMode)
		{
			this.fillMode = fillMode;
			points = new List<PointF> ();
			types = new List<byte> ();
		}

		public GraphicsPath (PointF [] pts, byte [] types) : this (pts, types, FillMode.Alternate) { }
		public GraphicsPath (Point [] pts, byte [] types) : this (pts, types, FillMode.Alternate) { }

		public GraphicsPath (PointF [] pts, byte [] types, FillMode fillMode)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");
			if (types == null)
				throw new ArgumentNullException ("types");
			if (pts.Length != types.Length)
				throw new ArgumentException ("Invalid parameter passed. Number of points and types must be same.");

			this.fillMode = fillMode;
			
			foreach (int type in types){
				if (type == 0 || type == 1 || type == 3 || type == 16 || type == 32 || type == 128)
					continue;
				throw new ArgumentException ("The pts array contains an invalid value for PathPointType: " + type);
			}
			
			this.points = new List<PointF> (pts);
			this.types = new List<byte> (types);
		}

		public GraphicsPath (Point [] pts, byte [] types, FillMode fillMode)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");
			if (types == null)
				throw new ArgumentNullException ("types");
			if (pts.Length != types.Length)
				throw new ArgumentException ("Invalid parameter passed. Number of points and types must be same.");

			this.fillMode = fillMode;
			foreach (int type in types){
				if (type == 0 || type == 1 || type == 3 || type == 16 || type == 32 || type == 128)
					continue;
				throw new ArgumentException ("The pts array contains an invalid value for PathPointType: " + type);
			}
			
			this.points = new List<PointF> ();
			foreach (var point in pts)
				points.Add (new PointF (point.X, point.Y));
			
			this.types = new List<byte> (types);
		}

		void Append (float x, float y, PathPointType type, bool compress)
		{
			byte t = (byte) type;
			PointF pt;

			/* in some case we're allowed to compress identical points */
			if (compress && (points.Count > 0)) {
				/* points (X, Y) must be identical */
				PointF lastPoint = points [points.Count-1];
				if ((lastPoint.X == x) && (lastPoint.Y == y)) {
					/* types need not be identical but must handle closed subpaths */
					PathPointType last_type = (PathPointType) types [types.Count-1];
					if ((last_type & PathPointType.CloseSubpath) != PathPointType.CloseSubpath)
						return;
				}
			}

			if (start_new_fig)
				t = (byte) PathPointType.Start;
			/* if we closed a subpath, then start new figure and append */
			else if (points.Count > 0) {
				type = (PathPointType) types [types.Count-1];
				if ((type & PathPointType.CloseSubpath) != 0)
					t = (byte)PathPointType.Start;
			}

			pt.X = x;
			pt.Y = y;

			points.Add (pt);
			types.Add (t);
			start_new_fig = false;
		}

		void AppendBezier (float x1, float y1, float x2, float y2, float x3, float y3)
		{
			Append (x1, y1, PathPointType.Bezier3, false);
			Append (x2, y2, PathPointType.Bezier3, false);
			Append (x3, y3, PathPointType.Bezier3, false);
		}

		void AppendArc (bool start, float x, float y, float width, float height, float startAngle, float endAngle)
		{
			float delta, bcp;
			float sin_alpha, sin_beta, cos_alpha, cos_beta;
			
			float rx = width / 2;
			float ry = height / 2;
			
			/* center */
			float cx = x + rx;
			float cy = y + ry;
			
			/* angles in radians */        
			float alpha = (float)(startAngle * Math.PI / 180);
			float beta = (float)(endAngle * Math.PI / 180);
			
			/* adjust angles for ellipses */
			alpha = (float) Math.Atan2 (rx * Math.Sin (alpha), ry * Math.Cos (alpha));
			beta = (float) Math.Atan2 (rx * Math.Sin (beta), ry * Math.Cos (beta));
			
			if (Math.Abs (beta - alpha) > Math.PI){
				if (beta > alpha)
					beta -= (float) (2 * Math.PI);
				else
					alpha -= (float) (2 * Math.PI);
			}
			
			delta = beta - alpha;
			// http://www.stillhq.com/ctpfaq/2001/comp.text.pdf-faq-2001-04.txt (section 2.13)
			bcp = (float)(4.0 / 3 * (1 - Math.Cos (delta / 2)) / Math.Sin (delta / 2));
			
			sin_alpha = (float)Math.Sin (alpha);
			sin_beta = (float)Math.Sin (beta);
			cos_alpha = (float)Math.Cos (alpha);
			cos_beta = (float)Math.Cos (beta);
			
			// move to the starting point if we're not continuing a curve 
			if (start) {
				// starting point
				float sx = cx + rx * cos_alpha;
				float sy = cy + ry * sin_alpha;
				Append (sx, sy, PathPointType.Line, false);
			}
			
			AppendBezier (cx + rx * (cos_alpha - bcp * sin_alpha),
				      cy + ry * (sin_alpha + bcp * cos_alpha),
				      cx + rx * (cos_beta  + bcp * sin_beta),
				      cy + ry * (sin_beta  - bcp * cos_beta),
				      cx + rx *  cos_beta,
				      cy + ry *  sin_beta);
		}

		static bool NearZero (float value)
		{
			return (value >= -0.0001f) && (value <= 0.0001f);
		}
		
		void AppendArcs (float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			float drawn = 0;
			int increment;
			float endAngle;
			bool enough = false;

			if (Math.Abs (sweepAngle) >= 360) {
				AddEllipse (x, y, width, height);
				return;
			}

			endAngle = startAngle + sweepAngle;
			increment = (endAngle < startAngle) ? -90 : 90;

			// i is the number of sub-arcs drawn, each sub-arc can be at most 90 degrees.
			// there can be no more then 4 subarcs, ie. 90 + 90 + 90 + (something less than 90) 
			for (int i = 0; i < 4; i++) {
				float current = startAngle + drawn;
				float additional;

				if (enough)
					return;
                
				additional = endAngle - current; /* otherwise, add the remainder */
				if (Math.Abs (additional) > 90) {
					additional = increment;
				} else {
					// a near zero value will introduce bad artefact in the drawing (Novell #78999)
					if (NearZero (additional))
						return;

					enough = true;
				}

				/* only move to the starting pt in the 1st iteration */
				AppendArc ((i == 0),
					   x, y, width, height,      /* bounding rectangle */
					   current, current + additional);
				drawn += additional;
			}
		}

		void AppendPoint (PointF point, PathPointType type, bool compress)
		{
			Append (point.X, point.Y, type, compress);
		}
		
		void AppendCurve (PointF [] points, PointF [] tangents, int offset, int length, CurveType type)
		{
			PathPointType ptype = ((type == CurveType.Close) || (points.Length == 0)) ? PathPointType.Start : PathPointType.Line;
			int i;
			
			AppendPoint (points [offset], ptype, true);
			for (i = offset; i < offset + length; i++) {
				int j = i + 1;

				float x1 = points [i].X + tangents [i].X;
				float y1 = points [i].Y + tangents [i].Y;

				float x2 = points [j].X - tangents [j].X;
				float y2 = points [j].Y - tangents [j].Y;

				float x3 = points [j].X;
				float y3 = points [j].Y;

				AppendBezier (x1, y1, x2, y2, x3, y3);
			}

				/* complete (close) the curve using the first point */
			if (type == CurveType.Close) {
				float x1 = points [i].X + tangents [i].X;
				float y1 = points [i].Y + tangents [i].Y;

				float x2 = points [0].X - tangents [0].X;
				float y2 = points [0].Y - tangents [0].Y;

				float x3 = points [0].X;
				float y3 = points [0].Y;

				AppendBezier (x1, y1, x2, y2, x3, y3);
				ClosePathFigure ();
			}
		}

		static PointF [] ToFloat (Point [] points)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			PointF []f = new PointF [points.Length];
			for (int i = 0; i < points.Length; i++)
				f [i] = new PointF (points [i].X, points [i].Y);
			return f;
		}
		
		public void AddCurve (Point[] points, int offset, int numberOfSegments, float tension)
		{
			AddCurve (ToFloat (points), offset, numberOfSegments, tension);
		}
		
		public void AddCurve (PointF[] points, int offset, int numberOfSegments, float tension)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			if (numberOfSegments < 1)
				throw new ArgumentException ("numberOfSegments");

			int count = points.Length;
			// we need 3 points for the first curve, 2 more for each curves 
			// and it's possible to use a point prior to the offset (to calculate) 
			if (offset == 0 && numberOfSegments == 1 && count < 3)
				throw new ArgumentException ("invalid parameters");
			if (numberOfSegments >= points.Length - offset)
				throw new ArgumentException ("offset");

			var tangents = OpenCurveTangents (CURVE_MIN_TERMS, points, count, tension);
			AppendCurve (points, tangents, offset, numberOfSegments, CurveType.Open);
		}

		public void AddCurve (Point [] points)
		{
			AddCurve (ToFloat (points), 0.5f);
		}

		public void AddCurve (PointF [] points)
		{
			AddCurve (points, 0.5f);
		}

		public void AddCurve (PointF [] points, float tension)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			if (points.Length < 2)
				throw new ArgumentException ("not enough points for polygon", "points");
			
			var tangents = OpenCurveTangents (CURVE_MIN_TERMS, points, points.Length, tension);
			AppendCurve (points, tangents, 0, points.Length-1, CurveType.Open);
		}
				      
		public void AddPolygon (Point [] points)
		{
			AddPolygon (ToFloat (points));
		}
		
		public void AddPolygon (PointF [] points)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			if (points.Length < 3)
				throw new ArgumentException ("not enough points for polygon", "points");
			AppendPoint (points [0], PathPointType.Start, false);
			for (int i = 1; i < points.Length; i++)
				AppendPoint (points [i], PathPointType.Line, false);

			// Add a line from the last point back to the first point if
			// they're not the same
			var last = points [points.Length-1];
			if (points [0] != last)
				AppendPoint (points [0], PathPointType.Line, false);
        
			/* close the path */
			ClosePathFigure ();
		}
		
		void ClosePathFigure ()
		{
			if (points.Count > 0)
				types [types.Count-1] = (byte) (types [types.Count-1] | (byte) PathPointType.CloseSubpath);
			start_new_fig = true;
		}

		public void AddEllipse (RectangleF rect)
		{
			const float C1 = 0.552285f;
			float rx = rect.Width / 2;
			float ry = rect.Height / 2;
			float cx = rect.X + rx;
			float cy = rect.Y + ry;

			/* origin */
			Append (cx + rx, cy, PathPointType.Start, false);

			/* quadrant I */
			AppendBezier (cx + rx, cy - C1 * ry,
				      cx + C1 * rx, cy - ry,
				      cx, cy - ry);

			/* quadrant II */
			AppendBezier (cx - C1 * rx, cy - ry,
				      cx - rx, cy - C1 * ry,
				      cx - rx, cy);
			
			/* quadrant III */
			AppendBezier (cx - rx, cy + C1 * ry,
				      cx - C1 * rx, cy + ry,
				      cx, cy + ry);

			/* quadrant IV */
			AppendBezier (cx + C1 * rx, cy + ry,
				      cx + rx, cy + C1 * ry,
				      cx + rx, cy);

			ClosePathFigure ();
		}
		       
			
		public void AddEllipse (float x, float y, float width, float height)
		{
			AddEllipse (new RectangleF (x, y, width, height));
		}

		public void AddEllipse (int x, int y, int width, int height)
		{
			AddEllipse (new RectangleF (x, y, width, height));
		}

		public void AddEllipse (Rectangle rect)
		{
			AddEllipse (new RectangleF (rect.X, rect.Y, rect.Width, rect.Height));
		}

		public void AddLine (float x1, float y1, float x2, float y2)
		{
			Append (x1, y1, PathPointType.Line, true);
			Append (x2, y2, PathPointType.Line, false);
		}

		public void AddLine (int x1, int y1, int x2, int y2)
		{
			Append (x1, y1, PathPointType.Line, true);
			Append (x2, y2, PathPointType.Line, false);
		}

		public void AddLine (Point pt1, Point pt2)
		{
			Append (pt1.X, pt1.Y, PathPointType.Line, true);
			Append (pt2.X, pt2.Y, PathPointType.Line, false);
		}

		public void AddLine (PointF pt1, PointF pt2)
		{
			Append (pt1.X, pt1.Y, PathPointType.Line, true);
			Append (pt2.X, pt2.Y, PathPointType.Line, false);
		}

		public void AddPie (float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			float sin_alpha, cos_alpha;

			float rx = width / 2;
			float ry = height / 2;

			/* center */
			float cx = x + rx;
			float cy = y + ry;        

			/* angles in radians */        
			float alpha = (float)(startAngle * Math.PI / 180);

			/* adjust angle for ellipses */
			alpha = (float) Math.Atan2 (rx * Math.Sin (alpha), ry * Math.Cos (alpha));

			sin_alpha = (float)Math.Sin (alpha);
			cos_alpha = (float)Math.Cos (alpha);

			/* move to center */
			Append (cx, cy, PathPointType.Start, false);

			/* draw pie edge */
			if (Math.Abs (sweepAngle) < 360)
				Append (cx + rx * cos_alpha, cy + ry * sin_alpha, PathPointType.Line, false);

			/* draw the arcs */
			AppendArcs (x, y, width, height, startAngle, sweepAngle);
        
			/* draw pie edge */
			if (Math.Abs (sweepAngle) < 360)
				Append (cx, cy, PathPointType.Line, false);

			ClosePathFigure ();
		}

		public void AddPie(Rectangle rect, float startAngle, float sweepAngle)
		{
			AddPie (rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		public void AddPie(int x, int y, int width, int height, float startAngle, float sweepAngle)
		{
			AddPie (x, y, width, height, startAngle, sweepAngle);
		}

		static PointF [] OpenCurveTangents (int terms, PointF [] points, int count, float tension)
		{
			float coefficient = tension / 3f;
			PointF [] tangents = new PointF [count];
			
			if (count <= 2)
				return tangents;

			for (int i = 0; i < count; i++) {
				int r = i + 1;
				int s = i - 1;

				if (r >= count)
					r = count - 1;
				if (s < 0)
					s = 0;

				tangents [i].X += (coefficient * (points [r].X - points [s].X));
				tangents [i].Y += (coefficient * (points [r].Y - points [s].Y));
			}
			
			return tangents;        
			
		}

		public void Dispose ()
		{
		}

		public object Clone ()
		{
			var copy = new GraphicsPath (fillMode);
			copy.points = new List<PointF> (points);
			copy.types = new List<byte> (types);
			copy.start_new_fig = start_new_fig;

			return copy;
		}
	}
}