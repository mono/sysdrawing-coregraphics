//
// System.Drawing.Region.cs
//
// Authors:
//	Miguel de Icaza (miguel@ximian.com)
//	Jordi Mas i Hernandez (jordi@ximian.com)
//	Sebastien Pouliot  <sebastien@xamarin.com>
//	Kenneth J. Pouncey  <kenneth.pouncey@xamarin.com>
//
// Copyright (C) 2003 Ximian, Inc. http://www.ximian.com
// Copyright (C) 2004,2006 Novell, Inc. http://www.novell.com
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

using System.Drawing.Drawing2D;
using System.Collections.Generic;

#if MONOMAC
using MonoMac.CoreGraphics;
//using MonoMac.AppKit;
//using MonoMac.Foundation;
//using MonoMac.CoreText;
#else
using MonoTouch.CoreGraphics;
//using MonoTouch.UIKit;
//using MonoTouch.Foundation;
//using MonoTouch.CoreText;
#endif

// Polygon Clipping Library
using ClipperLib;


namespace System.Drawing 
{

	// Clipper lib definitions
	using Path = List<IntPoint>;
	using Paths = List<List<IntPoint>>;

	public sealed class Region : MarshalByRefObject, IDisposable 
	{
		internal static RectangleF infinite = new RectangleF(-4194304, -4194304, 8388608, 8388608);
		internal object regionObject; 
		internal List<RegionEntry> regionList = new List<RegionEntry>();
		internal CGPath regionPath;
		internal RectangleF regionBounds;


		//Here we are scaling all coordinates up by 100 when they're passed to Clipper 
		//via Polygon (or Polygons) objects because Clipper no longer accepts floating  
		//point values. Likewise when Clipper returns a solution in a Polygons object, 
		//we need to scale down these returned values by the same amount before displaying.
		private static float scale = 100; //or 1 or 10 or 10000 etc for lesser or greater precision.

		internal Paths solution = new Paths();


		internal struct RegionEntry
		{
			public RegionType regionType;
			public object regionObject;
			public Paths regionPath;
			public RegionClipType regionClipType;

			public RegionEntry (RegionType type) :
				this (type, null, new Paths(), RegionClipType.None)
			{   }

			public RegionEntry (RegionType type, object obj) : 
				this (type, obj, new Paths(), RegionClipType.None)
			{	}

			public RegionEntry (RegionType type, object obj, Path path) :
				this (type, obj, path, RegionClipType.None)
			{ 	}

			public RegionEntry (RegionType type, object obj, Path path, RegionClipType clipType) //:
				//this (type, obj, new Paths<List<IntPoint>>(path), clipType)
			{	
				regionType = type;
				regionObject = obj;
				regionPath = new Paths() { path };
				regionClipType = clipType;
			}

			public RegionEntry (RegionType type, object obj, Paths path, RegionClipType clipType)
			{

				regionType = type;
				regionObject = obj;
				regionPath = path;
				regionClipType = clipType;
			}

//			public RegionEntry (RegionType type, object obj, Paths path, RegionClipType clipType)
//			{
//
//				regionType = type;
//				regionObject = obj;
//				regionPath = path;
//				regionClipType = clipType;
//			}
		}
		
		internal enum RegionType
		{
			Rectangle = 10000,
			Infinity = 10001,
			Empty = 10002,
			Path = 10003,
		}

		internal enum RegionClipType { 
			Intersection = ClipType.ctIntersection, 
			Union = ClipType.ctUnion, 
			Difference = ClipType.ctDifference, 
			Xor = ClipType.ctXor,
			None = -1
		};


		// An infinite region would cover the entire device region which is the same as
		// not having a clipping region. Note that this is not the same as having an
		// empty region, which when clipping to it has the effect of excluding the entire
		// device region.
		public Region ()
		{
			// We set the default region to a very large 
			regionObject = infinite;

			var path = RectangleToPath (infinite);
			solution.Add (path);

			regionList.Add (new RegionEntry (RegionType.Infinity, infinite, path));

			regionPath = new CGPath ();
			regionPath.MoveToPoint (infinite.Left, infinite.Top);
			regionPath.AddLineToPoint (infinite.Right, infinite.Top);
			regionPath.AddLineToPoint (infinite.Right, infinite.Bottom);
			regionPath.AddLineToPoint (infinite.Left, infinite.Bottom);

			regionBounds = infinite;
		}
		
		public Region (Rectangle rect) : 
			this ((RectangleF)rect)
		{ }
		
		public Region (RectangleF rect)
		{
			regionObject = rect;
			var path = RectangleToPath (rect);
			solution.Add (path);
			regionList.Add (new RegionEntry (RegionType.Rectangle, rect, path));
			regionPath = new CGPath ();
			regionPath.MoveToPoint (rect.Left, rect.Top);
			regionPath.AddLineToPoint (rect.Right, rect.Top);
			regionPath.AddLineToPoint (rect.Right, rect.Bottom);
			regionPath.AddLineToPoint (rect.Left, rect.Bottom);

			regionBounds = rect;
		}

		~Region ()
		{
			Dispose (false);
		}
		
		public bool Equals(Region region, Graphics g)
		{
			if (region == null)
				throw new ArgumentNullException ("region");
			if (g == null)
				throw new ArgumentNullException ("g");

			throw new NotImplementedException ();
		}
		
		public Region Clone ()
		{


			var region = new Region ();
			region.solution = this.solution;
			region.regionPath = this.regionPath;
			region.regionList = this.regionList;
			region.regionObject = this.regionObject;
			region.regionBounds = this.regionBounds;

			return region;
		}

		public void Dispose ()
		{
			Dispose (true);
			System.GC.SuppressFinalize (this);
		}

		void Dispose (bool disposing)
		{
		}
		
		public RectangleF GetBounds (Graphics g)
		{
			if (g == null)
				throw new ArgumentNullException ();
			throw new NotImplementedException ();
		}

//		public bool IsInfinite(Graphics g)
//		{
//		}

		public void MakeInfinite() 
		{
			regionObject = infinite;

			var path = RectangleToPath (infinite);

			// clear out our containers.
			regionList.Clear ();
			solution.Clear ();

			solution.Add (path);
			regionList.Add (new RegionEntry (RegionType.Infinity, infinite, path));

			regionPath = new CGPath ();
			regionPath.MoveToPoint (infinite.Left, infinite.Top);
			regionPath.AddLineToPoint (infinite.Right, infinite.Top);
			regionPath.AddLineToPoint (infinite.Right, infinite.Bottom);
			regionPath.AddLineToPoint (infinite.Left, infinite.Bottom);

			regionBounds = regionPath.BoundingBox;
		}

		public void MakeEmpty() 
		{
			regionObject = RectangleF.Empty;

			var path = RectangleToPath (RectangleF.Empty);

			// clear out our containers.
			regionList.Clear ();
			solution.Clear ();

			solution.Add (path);
			regionList.Add (new RegionEntry (RegionType.Empty, RectangleF.Empty, path));

			regionPath = new CGPath ();

			regionBounds = Rectangle.Empty;
		}

		public void Transform(Matrix matrix)
		{
			if (!IsEmpty && !IsInfinite) 
			{
				foreach (var path in solution) 
				{
					for (int p = 0; p < path.Count; p++) 
					{
						var point = path [p];
						TransformIntPoint (ref point, matrix);
						path [p] = point;
					}
				}

				PathsToInternalPath (solution);

			}
		}

		/// <summary>
		/// Transform the specified Rectangle by the matrix that is passed.
		/// </summary>
		/// <param name="matrix">Matrix.</param>
		private static void TransformIntPoint (ref IntPoint point, Matrix matrix) 
		{
			var transform = matrix.transform;
			var x = point.X / scale;
			var y = point.Y / scale;

			point.X = (long)((transform.xx * x + transform.xy * y + transform.x0) * scale);
			point.Y = (long)((transform.yx * x + transform.yy * y + transform.y0) * scale);

		}


		public void Translate(int dx,int dy)
		{
			Translate ((float)dx, (float)dy);

		}

		public void Translate(float dx, float dy)
		{
			var translateMatrix = new Matrix(CGAffineTransform.MakeTranslation(dx, dy));
			Transform (translateMatrix);
		}

		public void Intersect(Rectangle rect)
		{
			Intersect ((RectangleF)rect);
		}

		public void Intersect(RectangleF rect)
		{

			regionList.Add(new RegionEntry(RegionType.Rectangle, rect, RectangleToPath(rect), RegionClipType.Intersection));
			calculateRegionPath (ClipType.ctIntersection);
		}

		public void Intersect(Region region)
		{

			regionList.Add(new RegionEntry(RegionType.Path, region.solution, region.solution, RegionClipType.Intersection));
			calculateRegionPath (ClipType.ctIntersection);
		}


		public void Union(Rectangle rect)
		{
			Union ((RectangleF)rect);
		}

		public void Union(RectangleF rect)
		{
			regionList.Add(new RegionEntry(RegionType.Rectangle, rect, RectangleToPath(rect), RegionClipType.Union));
			calculateRegionPath (ClipType.ctUnion);
		}

		public void Union(Region region)
		{

			regionList.Add(new RegionEntry(RegionType.Path, region.solution, region.solution, RegionClipType.Union));
			calculateRegionPath (ClipType.ctUnion);
		}

		public void Xor(Rectangle rect)
		{
			Xor ((RectangleF)rect);
		}

		public void Xor(RectangleF rect)
		{
			regionList.Add(new RegionEntry(RegionType.Rectangle, rect, RectangleToPath(rect), RegionClipType.Xor));
			calculateRegionPath (ClipType.ctXor);
		}

		public void Xor(Region region)
		{

			regionList.Add(new RegionEntry(RegionType.Path, region.solution, region.solution, RegionClipType.Xor));
			calculateRegionPath (ClipType.ctXor);
		}

		public void Exclude(Rectangle rect)
		{
			Exclude ((RectangleF)rect);
		}

		public void Exclude(RectangleF rect)
		{
			regionList.Add(new RegionEntry(RegionType.Rectangle, rect, RectangleToPath(rect), RegionClipType.Difference));
			calculateRegionPath (ClipType.ctDifference);
		}

		public void Exclude(Region region)
		{

			regionList.Add(new RegionEntry(RegionType.Path, region.solution, region.solution, RegionClipType.Difference));
			calculateRegionPath (ClipType.ctDifference);
		}

		void calculateRegionPath (ClipType clipType)
		{
			Clipper c = new Clipper();

			var subjects = solution;
			//subjects.Add (solution);

			var clips = new Paths ();

			foreach (var path in regionList [regionList.Count - 1].regionPath)
				clips.Add (path);


			c.AddPolygons(subjects, PolyType.ptSubject);
			c.AddPolygons(clips, PolyType.ptClip);

			solution.Clear();

			bool succeeded = c.Execute(clipType, solution, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

			if (succeeded) 
			{
				PathsToInternalPath (solution);

				// Not sure what this is returning
//				var bounds = c.GetBounds ();
//				regionBounds.X = bounds.left / scale;
//				regionBounds.Y = bounds.top / scale;
//				regionBounds.Width = (bounds.right - bounds.left) / scale;
//				regionBounds.Height = (bounds.bottom - bounds.top) / scale;

				if (regionPath.IsEmpty)
					regionBounds = RectangleF.Empty;
				else
					regionBounds = regionPath.BoundingBox;

			}

		}

		void PathsToInternalPath(Paths paths)
		{

			regionPath = new CGPath ();

			foreach (var poly in solution)
			{
				regionPath.MoveToPoint(IntPointToPointF(poly[0]));

				for (var p =1; p < poly.Count; p++) 
				{
					regionPath.AddLineToPoint (IntPointToPointF (poly [p]));
				}
			}

		}

		internal RectangleF GetBounds()
		{

			return regionBounds;
		}

		internal bool IsInfinite 
		{
			get 
			{
				return regionBounds.Equals (infinite);
			}
		}

		internal bool IsEmpty
		{
			get 
			{
				return regionBounds.Equals (RectangleF.Empty);
			}
		}

		static Path RectangleToPath (RectangleF rect)
		{
			Path path = new Path ();

			path.Add(new IntPoint(rect.Left * scale, rect.Top * scale));
			path.Add(new IntPoint(rect.Right * scale, rect.Top * scale));
			path.Add(new IntPoint(rect.Right * scale, rect.Bottom * scale));
			path.Add(new IntPoint(rect.Left * scale, rect.Bottom * scale));
			path.Add(new IntPoint(rect.Left * scale, rect.Top * scale));

			return path;
		}
						
		static PointF IntPointToPointF (IntPoint point)
		{
			return new PointF (point.X / scale, point.Y / scale);
		}
	}

}