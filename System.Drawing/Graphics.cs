//
// Graphics.cs: The graphics context API from System.Drawing, MonoTouch implementation
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2011 Xamarin Inc
//
using System;
using MonoTouch.CoreGraphics;
using System.Drawing.Drawing2D;

namespace System.Drawing {

	public sealed partial class Graphics : MarshalByRefObject, IDisposable {
		internal CGContext context;
		
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
		}
		
		
		// TODO: clear, we can get the dimensions for some kinds of CGContexts, like bitmaps, but need a general case

		// from: cairo_DrawLine
		public void DrawLine (Pen pen, Point pt1, Point pt2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			MoveTo (pt1.X, pt1.Y);
			LineTo (pt2.X, pt2.Y);
			Stroke (pen);
			// FIXME: draw custom start/end caps
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
	}
}