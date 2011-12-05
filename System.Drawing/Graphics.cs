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

		void FillBrush (Brush brush)
		{
			brush.Setup (this, true);
			context.FillPath ();
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

		void RectanglePath (float x1, float y1, float x2, float y2)
		{
			MoveTo (x1, y1);
			LineTo (x1, y2);
			LineTo (x2, y2);
			LineTo (x1, y2);
			context.ClosePath ();
		}
			
		public void DrawRectangle (Pen pen, float x1, float y1, float x2, float y2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			RectanglePath (x1, y1, x2, y2);
			StrokePen (pen);
		}
		
		public void DrawRectangle (Pen pen, int x1, int y1, int x2, int y2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			RectanglePath (x1, y1, x2, y2);
			StrokePen (pen);
		}
		
		public void DrawRectangle (Pen pen, RectangleF rect)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			RectanglePath (rect.X, rect.Y, rect.Right, rect.Bottom);
			StrokePen (pen);
		}

		public void DrawRectangle (Pen pen, Rectangle rect)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			RectanglePath (rect.X, rect.Y, rect.Right, rect.Bottom);
			StrokePen (pen);
		}

		public void FillRectangle (Brush brush, float x1, float y1, float x2, float y2)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			RectanglePath (x1, y1, x2, y2);
			FillBrush (brush);
		}

		public void FillRectangle (Brush brush, Rectangle rect)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			RectanglePath (rect.X, rect.Y, rect.Right, rect.Bottom);
			FillBrush (brush);
		}
		
		public void FillRectangle (Brush brush, int x1, int y1, int x2, int y2)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			RectanglePath (x1, y1, x2, y2);
			FillBrush (brush);
		}

		public void DrawEllipse (Pen pen, RectangleF rect)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			pen.Setup (this, false);
			context.StrokeEllipseInRect (rect);
		}

		public void DrawEllipse (Pen pen, int x1, int y1, int x2, int y2)
		{
			DrawEllipse (pen, new RectangleF (x1, y1, x2, y2));
		}

		public void FillEllipse (Brush brush, RectangleF rect)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			brush.Setup (this, true);
			context.FillEllipseInRect (rect);
		}

		public void FillEllipse (Brush brush, int x1, int y1, int x2, int y2)
		{
			FillEllipse (brush, new RectangleF (x1, y1, x2, y2));
		}
		
		public void ResetTransform ()
		{
			// Since there is no context.SetCTM, only ConcatCTM
			// get the current transform, invert it, and concat this to
			// obtain the identity.   Then we concatenate the value passed
			var transform = context.GetCTM ();
			transform.Invert ();
			context.ConcatCTM (transform);
		}
		
		public Matrix Transform {
			get {
				return new Matrix (context.GetCTM ());
			}
			set {
				ResetTransform ();
				// Set the new value
				context.ConcatCTM (value.transform);
			}
		}

		public void RotateTransform (float angle)
		{
			context.RotateCTM (angle);
		}

		public void TranslateTransform (float tx, float ty)
		{
			context.TranslateCTM (tx, ty);
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