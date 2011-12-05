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
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace System.Drawing.Drawing2D {
	public sealed class GraphicsPath : ICloneable, IDisposable {
		List<PointF> points;
		List<byte> types;
		CGPath cgpath;
		FillMode fillMode;
		bool start_new_fig;
		
		public GraphicsPath () : this (FillMode.Alternate)
		{
		}

		public GraphicsPath (FillMode fillMode)
		{
			cgpath = new CGPath ();
			this.fillMode = fillMode;
			points = new List<PointF> ();
			types = new List<PointF> ();
		}

		public GraphicsPath (PointF [] pts, byte [] types) : this (pts, types, FillMode.Alternate) { }
		public GraphicsPath (Point [] pts, byte [] types) : this (pts, types, FillMode.Alternate) { }

		public GraphicsPath (PointF [] pts, byte [] types, FillMode fillMode)
		{
			if (pts == null)
				throw ArgumentNullException ("pts");
			if (types == null)
				throw ArgumentNullException ("types");
			if (pts.Length != types.Length)
				throw ArgumentException ("Invalid parameter passed. Number of points and types must be same.");

			cgpath = new CGPath ();
			this.fillMode = fillMode;
			
			foreach (int type in types){
				if (type == 0 || type == 1 || type == 3 || type == 16 || type == 32 || type == 128)
					continue;
				throw new ArgumentException ("The pts array contains an invalid value for PathPointType: " + type);
			}
			
			this.points = new List<PointF> (pts);
			this.types = new List<PointF> (types);
		}

		public GraphicsPath (Point [] pts, byte [] types, FillMode fillMode)
		{
			if (pts == null)
				throw ArgumentNullException ("pts");
			if (types == null)
				throw ArgumentNullException ("types");
			if (pts.Length != types.Length)
				throw ArgumentException ("Invalid parameter passed. Number of points and types must be same.");

			cgpath = new CGPath ();
			this.fillMode = fillMode;
			foreach (int type in types){
				if (type == 0 || type == 1 || type == 3 || type == 16 || type == 32 || type == 128)
					continue;
				throw new ArgumentException ("The pts array contains an invalid value for PathPointType: " + type);
			}
			
			this.points = new List<PointF> ();
			foreach (var point in pts)
				points.Add (new PointF (point.X, point.Y));
			
			this.types = new List<PointF> (types);
		}

		void Append (float x, float y, PathPointType type, bool compress)
		{
			byte t = (byte) type;
			PointF pt;

			/* in some case we're allowed to compress identical points */
			if (compress && (points.count > 0)) {
				/* points (X, Y) must be identical */
				PointF lastPoint = points [points.Count-1];
				if ((lastPoint.X == x) && (lastPoint.Y == y)) {
					/* types need not be identical but must handle closed subpaths */
					PathPointType last_type = types [types.Count-1];
					if ((last_type & PathPointType.CloseSubpath) != PathPointTypeCloseSubpath)
						return;
				}
			}

			if (path.start_new_fig)
				t = (byte) PathPointType.Start;

			/* if we closed a subpath, then start new figure and append */
			else if (path.count > 0) {
				type = types [types.Count-1];
				if ((type & PathPointType.CloseSubpath) != 0)
					t = PathPointType.Start;
			}

			pt.X = x;
			pt.Y = y;

			points.Add (pt);
			types.Add (t);
			path->start_new_fig = false;
		}
		
		public void AddEllipse (RectangleF rect)
		{
			const float C1 = 0.552285;
			float rx = rect.Width / 2;
			double ry = rect.Height / 2;
			double cx = rect.X + rx;
			double cy = rect.Y + ry;

			/* origin */
			Append (cx + rx, cy, PathPointType.Start, false);

			/* quadrant I */
			AppendBezier (path,
				       cx + rx, cy - C1 * ry,
				       cx + C1 * rx, cy - ry,
				       cx, cy - ry);

			/* quadrant II */
			AppendBezier (path,
				       cx - C1 * rx, cy - ry,
				       cx - rx, cy - C1 * ry,
				       cx - rx, cy);

			/* quadrant III */
			AppendBezier (path,
				       cx - rx, cy + C1 * ry,
				       cx - C1 * rx, cy + ry,
				       cx, cy + ry);

			/* quadrant IV */
			AppendBezier (path,
				       cx + C1 * rx, cy + ry,
				       cx + rx, cy + C1 * ry,
				       cx + rx, cy);

			/* close the path */
			GdipClosePathFigure (path);
.		}
		       
		public void Dispose ()
		{
		}
	}
}