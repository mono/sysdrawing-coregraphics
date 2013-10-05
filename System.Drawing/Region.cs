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

		//Here we are scaling all coordinates up by 100 when they're passed to Clipper 
		//via Polygon (or Polygons) objects because Clipper no longer accepts floating  
		//point values. Likewise when Clipper returns a solution in a Polygons object, 
		//we need to scale down these returned values by the same amount before displaying.
		private static float scale = 100; //or 1 or 10 or 10000 etc for lesser or greater precision.

		private struct RegionEntry
		{
			public RegionType regionType;
			public object regionObject;
			public Path regionPath;
			public RegionClipType regionClipType;

			public RegionEntry (RegionType type) :
				this (type, null, null)
			{   }

			public RegionEntry (RegionType type, object obj) : 
				this (type, obj, null)
			{	}

			public RegionEntry (RegionType type, object obj, Path path)
			{

				regionType = type;
				regionObject = obj;
				regionPath = path;
				regionClipType = RegionClipType.None;
			}
		}
		
		private enum RegionType
		{
			Rectangle = 10000,
			Infinity = 10001,
			Empty = 10002,
		}

		private enum RegionClipType { 
			Intersection = ClipType.ctIntersection, 
			Union = ClipType.ctUnion, 
			Difference = ClipType.ctDifference, 
			Xor = ClipType.ctXor,
			None = -1
		};


		internal static RectangleF infinite = new RectangleF(4194304, 4194304, 8388608, 8388608);
		internal object regionObject; 
		List<RegionEntry> regionList = new List<RegionEntry>();

		// An infinite region would cover the entire device region which is the same as
		// not having a clipping region. Note that this is not the same as having an
		// empty region, which when clipping to it has the effect of excluding the entire
		// device region.
		public Region ()
		{
			// We set the default region to a very large 
			regionObject = infinite;
			regionList.Add (new RegionEntry (RegionType.Infinity) );

		}
		
		public Region (Rectangle rect)
		{
			regionObject = (RectangleF)rect;
			regionList.Add (new RegionEntry (RegionType.Rectangle, (RectangleF)rect));
		}
		
		public Region (RectangleF rect)
		{
			regionObject = rect;
			regionList.Add (new RegionEntry (RegionType.Rectangle, rect, RectangleToPath(rect)));
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
			// Right now we only support Rectangular regions
			// we need to fix this when we support other types
			return new Region ((RectangleF)regionObject);
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
			if (regionObject is RectangleF) 
			{
				regionObject = infinite;
				regionList.Clear ();
				regionList.Add (new RegionEntry (RegionType.Infinity, null));
			}

		}

		public void MakeEmpty() 
		{
			if (regionObject is RectangleF) 
			{
				regionObject = RectangleF.Empty;
				regionList.Clear ();
				regionList.Add (new RegionEntry (RegionType.Empty, null));
			}

		}

		public void Transform(Matrix matrix)
		{
			if (!IsEmpty && !IsInfinite) 
			{
				if (regionObject is RectangleF) 
				{
					var rect = (RectangleF)regionObject;
					//GeomUtilities.TransformRectangle (ref rect, matrix);
					regionObject = rect.Transform (matrix);
					//regionObject = rect;
				}
			}
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

			if (regionObject is RectangleF) 
			{
				// if it is infinite then we just replace rectangle.
				// Regions that are empty will still be empty with an intersection.
				if (IsInfinite) 
				{
					regionObject = rect;
				}
				else 
				{
					var iRect = (RectangleF)regionObject;
					iRect.Intersect (rect);
					regionObject = iRect;
				}
			}
		}

		internal RectangleF GetBounds()
		{
			RectangleF? rect = null;
			if (regionObject is RectangleF)
				return (RectangleF)regionObject;

			return infinite;
		}

		internal bool IsInfinite 
		{
			get 
			{
				RectangleF? rect = null;
				if (regionObject is RectangleF)
					rect = (RectangleF)regionObject;
				if (rect == null || rect.Equals (infinite))
					return true;

				return false;
			}
		}

		internal bool IsEmpty
		{
			get 
			{
				RectangleF? rect = null;
				if (regionObject is RectangleF)
					rect = (RectangleF)regionObject;
				if (rect != null && rect.Equals (RectangleF.Empty))
					return true;

				return false;
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
	}
}