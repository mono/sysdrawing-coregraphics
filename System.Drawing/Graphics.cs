//
// Graphics.cs: The graphics context API from System.Drawing, MonoTouch implementation
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2011 Xamarin Inc
//
using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
#if MONOMAC
using MonoMac.CoreGraphics;
#else
using MonoTouch.CoreGraphics;
#endif

namespace System.Drawing {

	public sealed partial class Graphics : MarshalByRefObject, IDisposable {
		internal CGContext context;

		// Need to keep a transform around, since it is not possible to
		// set the transform on the context, merely to concatenate.
		CGAffineTransform transform;
		
		~Graphics ()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
		}

		internal void Dispose (bool disposing)
		{
			if (disposing){
				if (context != null){
					context.Dispose ();
					context = null;
				}
			}
		}

		// from: gdip_cairo_move_to, inlined to assume converts_unit=true, antialias=true
		void MoveTo (float x, float y)
		{
			// FIXME: Avoid conversion if possible
			// FIXME: Apply AA offset
			context.MoveTo (x, y);
		}

		void LineTo (float x, float y)
		{
			context.AddLineToPoint (x, y);
		}

		void Stroke (Pen pen)
		{
			pen.Setup (this, false);
			context.StrokePath ();
		}

		void StrokePen (Pen pen)
		{
			Stroke (pen);
			// FIXME: draw custom start/end caps
		}
		
		public void DrawImage (Image image, RectangleF rect)
		{
			DrawImage (image, rect.X, rect.Y, rect.Width, rect.Height);
		}
		
		public void DrawImage (Image image, float x, float y, float width, float height)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			// TODO
		}
		
		public void DrawLine (Pen pen, Point pt1, Point pt2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			MoveTo (pt1.X, pt1.Y);
			LineTo (pt2.X, pt2.Y);
			StrokePen (pen);
		}

		public void DrawLine (Pen pen, PointF pt1, PointF pt2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			MoveTo (pt1.X, pt1.Y);
			LineTo (pt2.X, pt2.Y);
			StrokePen (pen);
		}

		public void DrawLine (Pen pen, int x1, int y1, int x2, int y2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			MoveTo (x1, y1);
			LineTo (x2, y2);
			StrokePen (pen);
		}

		public void DrawLine (Pen pen, float x1, float y1, float x2, float y2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			MoveTo (x1, y1);
			LineTo (x2, y2);
			StrokePen (pen);
		}

		public Matrix Transform {
			get {
				return new Matrix (context.GetCTM ());
			}
			set {
				// since there is no set ctm on the context,
				// we need to multiply by the inverse of the last value to get the
				// identity, and then set the new value
			}
		}
		
		CompositingMode compositing_mode;
		public CompositingMode CompositingMode {
			get {
				return compositing_mode;
			}
			set {
				compositing_mode = value;
			}
		}
		
		public static Graphics FromImage (Image image)
		{
			if (image == null) 
				throw new ArgumentNullException ("image");

			if ((image.PixelFormat & PixelFormat.Indexed) != 0)
				throw new Exception (Locale.GetText ("Cannot create Graphics from an indexed bitmap."));
			
			return null; // TODO
		}
	}
}