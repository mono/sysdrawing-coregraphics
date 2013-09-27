//
// System.Drawing.Region.cs
//
// Authors:
//	Miguel de Icaza (miguel@ximian.com)
//	Jordi Mas i Hernandez (jordi@ximian.com)
//	Sebastien Pouliot  <sebastien@xamarin.com>
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

namespace System.Drawing 
{

	public sealed class Region : MarshalByRefObject, IDisposable 
	{

		internal static RectangleF infinite = new RectangleF(4194304, 4194304, 8388608, 8388608);
		internal object regionObject; 

		// An infinite region would cover the entire device region which is the same as
		// not having a clipping region. Note that this is not the same as having an
		// empty region, which when clipping to it has the effect of excluding the entire
		// device region.
		public Region ()
		{
			// We set the default region to a very large 
			regionObject = infinite;
		}
		
		public Region (Rectangle rect)
		{
			regionObject = (RectangleF)rect;
		}
		
		public Region (RectangleF rect)
		{
			regionObject = rect;
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
	}
}