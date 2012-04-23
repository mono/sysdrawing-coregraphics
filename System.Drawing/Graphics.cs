//
// Graphics.cs: The graphics context API from System.Drawing, MonoTouch implementation
//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//   Alexandre Pigolkine (pigolkine@gmx.de)
//   Duncan Mak (duncan@xamarin.com)
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2011 Xamarin Inc
// Copyright 2003-2009 Novell, Inc.
//
using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

#if MONOMAC
using MonoMac.CoreGraphics;
#else
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
#endif

namespace System.Drawing {

	public sealed partial class Graphics : MarshalByRefObject, IDisposable {
		internal CGContext context;

		public Graphics (CGContext context)
		{
			if (context == null)
				throw new ArgumentNullException ("context");
			
			this.context = context;
		}
		
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
		
		void MoveTo (PointF point)
		{
			context.MoveTo (point.X, point.Y);
		}

		void LineTo (PointF point)
		{
			context.AddLineToPoint (point.X, point.Y);
		}

		void LineTo (float x, float y)
		{
			context.AddLineToPoint (x, y);
		}
		
		void CurveTo (float x1, float y1, float x2, float y2, float x3, float y3)
		{
			context.AddCurveToPoint (x1, y1, x2, y2, x3, y3);
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

		void FillBrush (Brush brush, FillMode fillMode = FillMode.Alternate)
		{
			brush.Setup (this, true);
			if (fillMode == FillMode.Alternate)
				context.EOFillPath ();
			else
				context.FillPath ();
		}
		
		public void DrawArc (Pen pen, Rectangle rect, float startAngle, float sweepAngle)
		{
			DrawArc (pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		
		public void DrawArc (Pen pen, RectangleF rect, float startAngle, float sweepAngle)
		{
			DrawArc (pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		public void DrawArc (Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			throw new NotImplementedException ();
		}

		// Microsoft documentation states that the signature for this member should be
		// public void DrawArc( Pen pen,  int x,  int y,  int width,  int height,   int startAngle,
   		// int sweepAngle. However, GdipDrawArcI uses also float for the startAngle and sweepAngle params
   		public void DrawArc (Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			throw new NotImplementedException ();
		}

		public void DrawLine (Pen pen, Point pt1, Point pt2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			MoveTo (pt1.X, pt1.Y);
			LineTo (pt2.X, pt2.Y);
			StrokePen (pen);
		}
		
		public void DrawBezier (Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			MoveTo (pt1.X, pt1.Y);
			CurveTo (pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
			StrokePen (pen);
		}

		public void DrawBezier (Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			MoveTo (pt1.X, pt1.Y);
			CurveTo (pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
			StrokePen (pen);
		}

		public void DrawBezier (Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			MoveTo (x1, y1);
			CurveTo (x2, y2, x3, y3, x4, y4);
			StrokePen (pen);
		}
		
		public void DrawBeziers (Pen pen, Point [] points)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");
			
            int length = points.Length;
            if (length < 4)
	            return;

			for (int i = 0; i < length - 1; i += 3) {
	            Point p1 = points [i];
	            Point p2 = points [i + 1];
	            Point p3 = points [i + 2];
	            Point p4 = points [i + 3];

				DrawBezier (pen, p1, p2, p3, p4);
			}
		}

		public void DrawBeziers (Pen pen, PointF [] points)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");
            int length = points.Length;
            if (length < 4)
	            return;

			for (int i = 0; i < length - 1; i += 3) {
	            var p1 = points [i];
	            var p2 = points [i + 1];
	            var p3 = points [i + 2];
	            var p4 = points [i + 3];

				DrawBezier (pen, p1, p2, p3, p4);
			}
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
		
		public void DrawLines (Pen pen, Point [] points)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			if (points == null)
				throw new ArgumentNullException ("points");
			int count = points.Length;
			if (count < 2)
				return;
			
			MoveTo (points [0]);
			for (int i = 1; i < count; i++)
				LineTo (points [i]);
			StrokePen (pen);
		}
		
		public void DrawLines (Pen pen, PointF [] points)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			if (points == null)
				throw new ArgumentNullException ("points");
			int count = points.Length;
			if (count < 2)
				return;
			
			MoveTo (points [0]);
			for (int i = 1; i < count; i++)
				LineTo (points [i]);
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
		
		public void FillRectangle (Brush brush, RectangleF rect)
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

		
		public void FillRegion (Brush brush, Region region)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (region == null)
				throw new ArgumentNullException ("region");

			throw new NotImplementedException ();
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

		public void DrawEllipse (Pen pen, Rectangle rect)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			
			DrawEllipse (pen, new RectangleF (rect.X, rect.Y, rect.Width, rect.Height));
		}

		public void DrawEllipse (Pen pen, float x, float y, float width, float height)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			context.StrokeEllipseInRect (new RectangleF (x, y, width, height));
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
		
		public void FillEllipse (Brush brush, float x1, float y1, float x2, float y2)
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
			RotateTransform (angle, MatrixOrder.Prepend);
		}

		public void RotateTransform (float angle, MatrixOrder order)
		{
			Console.WriteLine ("Currently does not support anything bur prepend mode");
			context.RotateCTM (angle);
		}
		
		public void TranslateTransform (float tx, float ty)
		{
			TranslateTransform (tx, ty, MatrixOrder.Prepend);
		}
		
		public void TranslateTransform (float tx, float ty, MatrixOrder order)
		{
			Console.WriteLine ("Currently does not support anything bur prepend mode");
			context.TranslateCTM (tx, ty);
		}
		
		public void ScaleTransform (float sx, float sy)
		{
			ScaleTransform (sx, sy, MatrixOrder.Prepend);
		}
		
		public void ScaleTransform (float sx, float sy, MatrixOrder order)
		{
			context.ScaleCTM (sx, sy);
		}
		
		void MakeCurve (PointF [] points, PointF [] tangents, int offset, int length, CurveType type)
		{
			MoveTo (points [offset].X, points [offset].Y);
			int i = offset;
			
			for (; i < offset + length; i++) {
				int j = i + 1;

				float x1 = points [i].X + tangents [i].X;
				float y1 = points [i].Y + tangents [i].Y;

				float x2 = points [j].X - tangents [j].X;
				float y2 = points [j].Y - tangents [j].Y;

				float x3 = points [j].X;
				float y3 = points [j].Y;

				context.AddCurveToPoint (x1, y1, x2, y2, x3, y3);
			}

			if (type == CurveType.Close) {
				/* complete (close) the curve using the first point */
				float x1 = points [i].X + tangents [i].X;
				float y1 = points [i].Y + tangents [i].Y;

				float x2 = points [0].X - tangents [0].X;
				float y2 = points [0].Y - tangents [0].Y;

				float x3 = points [0].X;
				float y3 = points [0].Y;

				context.AddCurveToPoint (x1, y1, x2, y2, x3, y3);

				context.ClosePath ();
			}
		}
		
		internal PointF [] ConvertPoints (Point [] points)
		{
			if (points == null)
				return null;
			int len = points.Length;
			var result = new PointF [len];
			for (int i = 0; points.Length < len; i++)
				result [i] = new PointF (points [i].X, points [i].Y);
			return result;
		}
		
		
		public void DrawCurve (Pen pen, PointF[] points, int offset, int numberOfSegments, float tension = 0.5f)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (numberOfSegments < 1)
				throw new ArgumentException ("numberOfSegments");

			int count = points.Length;
			// we need 3 points for the first curve, 2 more for each curves 
			// and it's possible to use a point prior to the offset (to calculate) 
			if (offset == 0 && numberOfSegments == 1 && count < 3)
				throw new ArgumentException ("invalid parameters");
			if (numberOfSegments >= points.Length - offset)
				throw new ArgumentException ("offset");

			var tangents = GraphicsPath.OpenCurveTangents (GraphicsPath.CURVE_MIN_TERMS, points, count, tension);
			MakeCurve (points, tangents, offset, numberOfSegments, CurveType.Open);
			StrokePen (pen);
		}

		public void DrawCurve (Pen pen, Point[] points, int offset, int numberOfSegments, float tension = 0.5f)
		{
			DrawCurve (pen, ConvertPoints (points), offset, numberOfSegments, tension);
		}
		
		public void DrawCurve (Pen pen, Point [] points, float tension = 0.5f)
		{
			DrawCurve (pen, ConvertPoints (points), tension);
		}

		public void DrawCurve (Pen pen, PointF [] points, float tension = 0.5f)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			int count = points.Length;
			if (count == 2)
				DrawLines (pen, points);
			else {
				int segments = (count > 3) ? (count-1) : (count-2);
				
				DrawCurve (pen, points, 0, segments, tension);
			}
		}

		void PlotPath (GraphicsPath path)
		{
			float x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0;
			var points = path.PathPoints;
			var types = path.PathTypes;
			int bidx = 0;
			
			for (int i = 0; i < points.Length; i++){
				var point = points [i];
				var type = (PathPointType) types [i];

				switch (type & PathPointType.PathTypeMask){
				case PathPointType.Start:
					MoveTo (point.X, point.Y);
					break;
					
				case PathPointType.Line:
					LineTo (point.X, point.Y);
					break;
					
				case PathPointType.Bezier3:
					// collect 3 points
					switch (bidx++){
					case 0:
						x1 = point.X;
						y1 = point.Y;
						break;
					case 1:
						x2 = point.X;
						y2 = point.Y;
						break;
					case 2:
						x3 = point.X;
						y3 = point.Y;
						break;
					}
					if (bidx == 3){
						context.AddCurveToPoint (x1, y1, x2, y2, x3, y3);
						bidx = 0;
					}
					break;
				default:
					throw new Exception ("Inconsistent internal state, path type=" + type);
				}
				if ((type & PathPointType.CloseSubpath) != 0)
					context.ClosePath ();
			}
		}
		
		public void DrawPath (Pen pen, GraphicsPath path)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (path == null)
				throw new ArgumentNullException ("path");

			PlotPath (path);
			StrokePen (pen);
		}

		public void FillPath (Brush brush, GraphicsPath path)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (path == null)
				throw new ArgumentNullException ("path");
			PlotPath (path);
			FillBrush (brush);
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
		
		public GraphicsUnit PageUnit { get; set; }
		public float PageScale { get; set; }
		public TextRenderingHint TextRenderingHint { get; set; }
		
		public static Graphics FromImage (Image image)
		{
			if (image == null) 
				throw new ArgumentNullException ("image");

			if ((image.PixelFormat & PixelFormat.Indexed) != 0)
				throw new Exception ("Cannot create Graphics from an indexed bitmap.");
			
			Bitmap b = image as Bitmap;
			if (b == null)
				throw new Exception ("Can not create Graphics contexts from " + image.GetType () + " Images, only Bitmaps are supported");
			var cgimage = b.NativeCGImage;
			
			if (b.bitmapBlock == IntPtr.Zero){
				throw new Exception ("Missing functionality: currently we can not create graphics contexts from bitmaps loaded from disk, need to do some extra work");
			}
			
			// Creates a context using the parameters that were used initially for the bitmap, 
			// reusing the memory address space on it as well.
			return new Graphics (new CGBitmapContext (b.bitmapBlock, cgimage.Width, cgimage.Height, cgimage.BitsPerComponent, cgimage.BytesPerRow, cgimage.ColorSpace, cgimage.BitmapInfo));
		}
		
		public void SetClip (RectangleF rect)
		{
			SetClip (rect, CombineMode.Replace);
		}

		public void SetClip (Rectangle rect)
		{
			SetClip (rect, CombineMode.Replace);
		}

		public void SetClip (GraphicsPath graphicsPath)
		{
			SetClip (graphicsPath, CombineMode.Replace);
		}
		
		public void SetClip (Graphics g)
		{
			SetClip (g, CombineMode.Replace);	
		}

		public void SetClip (RectangleF rect, CombineMode combineMode)
		{
			throw new NotImplementedException ();
		}

		public void SetClip (Rectangle rect, CombineMode combineMode)
		{
			throw new NotImplementedException ();
		}

		public void SetClip (GraphicsPath graphicsPath, CombineMode combineMode)
		{
			throw new NotImplementedException ();
		}
		
		public void SetClip (Graphics g, CombineMode combineMode)
		{
			throw new NotImplementedException ();
		}
		
		public void SetClip (Region region, CombineMode combineMode)
		{
			throw new NotImplementedException ();
		}
		
		public GraphicsContainer BeginContainer ()
		{
			throw new NotImplementedException ();		
		}
		
		public GraphicsContainer BeginContainer (Rectangle dstRect, Rectangle srcRect, GraphicsUnit unit)
		{
			throw new NotImplementedException ();		
		}

		public GraphicsContainer BeginContainer (RectangleF dstRect, RectangleF srcRect, GraphicsUnit unit)
		{
			throw new NotImplementedException ();		
		}

		public void EndContainer (GraphicsContainer container)
		{
			throw new NotImplementedException ();
		}
		
		public SmoothingMode SmoothingMode { get; set; }
		
		public bool IsClipEmpty {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public PixelOffsetMode PixelOffsetMode {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public Region Clip {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public RectangleF ClipBounds {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public RectangleF VisibleClipBounds {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public InterpolationMode InterpolationMode {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public PointF RenderingOrigin {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public int TextContrast {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public float DpiX { 
			get {
				throw new NotImplementedException ();
			}
		}

		public float DpiY { 
			get {
				throw new NotImplementedException ();
			}
		}
		public CompositingQuality CompositingQuality {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public bool IsVisibleClipEmpty { 
			get {
				throw new NotImplementedException ();
			}
		}

		public void TranslateClip (int dx, int dy)
		{
			throw new NotImplementedException ();
		}
		
		public void TranslateClip (float dx, float dy)
		{
			throw new NotImplementedException ();
		}
		
		public void ResetClip ()
		{
			throw new NotImplementedException ();
		}
		
		public void ExcludeClip (Rectangle rect)
		{
			throw new NotImplementedException ();
		}
		
		public void IntersectClip (Rectangle rect)
		{
			throw new NotImplementedException ();
		}
		
		public void IntersectClip (RectangleF rect)
		{
			throw new NotImplementedException ();
		}

		public void IntersectClip (Region region)
		{
			throw new NotImplementedException ();
		}

		public SizeF MeasureString (string text, Font font)
		{
			return MeasureString (text, font, SizeF.Empty);
		}
		
		public SizeF MeasureString (string textg, Font font, int width)
		{
			return MeasureString (textg, font, new RectangleF (0, 0, width, Int32.MaxValue));
		}

		public SizeF MeasureString (string textg, Font font, SizeF layoutArea)
		{
			return MeasureString (textg, font, new RectangleF (new PointF (0, 0), layoutArea));
		}
		
		public SizeF MeasureString (string text, Font font, PointF point, StringFormat stringFormat)
		{
			throw new NotImplementedException ();
		}
		
		public SizeF MeasureString (string textg, Font font, RectangleF rect)
		{
			throw new NotImplementedException ();
		}
		public SizeF MeasureString (string text, Font font, SizeF layoutArea, StringFormat stringFormat)
		{
			throw new NotImplementedException ();
		}

		public SizeF MeasureString (string text, Font font, int width, StringFormat format)
		{
			throw new NotImplementedException ();
		}

		public SizeF MeasureString (string text, Font font, SizeF layoutArea, StringFormat stringFormat, 
			out int charactersFitted, out int linesFilled)
		{	
			charactersFitted = 0;
			linesFilled = 0;

			if ((text == null) || (text.Length == 0))
				return SizeF.Empty;

			if (font == null)
				throw new ArgumentNullException ("font");

			throw new NotImplementedException ();
		}
		
		public void Clear (Color color)
		{
			throw new NotImplementedException ();
		}
		
		public void Restore (GraphicsState gstate)
		{
		}
		
		public GraphicsState Save ()
		{
			throw new NotImplementedException ();
		}
		
		public void DrawClosedCurve (Pen pen, PointF [] points)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			throw new NotImplementedException ();
		}
		
		public void DrawClosedCurve (Pen pen, Point [] points)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			throw new NotImplementedException ();
		}
 			
		// according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
		// GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
		public void DrawClosedCurve (Pen pen, Point [] points, float tension, FillMode fillmode)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			throw new NotImplementedException ();
		}

		// according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
		// GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
		public void DrawClosedCurve (Pen pen, PointF [] points, float tension, FillMode fillmode)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			throw new NotImplementedException ();
		}
		
		public void FillClosedCurve (Brush brush, PointF [] points)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			throw new NotImplementedException ();
		}
		
		public void FillClosedCurve (Brush brush, Point [] points)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			throw new NotImplementedException ();
		}
 			
		// according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
		// GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
		public void FillClosedCurve (Brush brush, Point [] points, FillMode fillmode, float tension = 0.5f)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			throw new NotImplementedException ();
		}

		// according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
		// GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
		public void FillClosedCurve (Brush brush, PointF [] points, FillMode fillmode, float tension = 0.5f)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			throw new NotImplementedException ();
		}
	
		public void DrawIcon (Icon icon, Rectangle targetRect)
		{
			if (icon == null)
				throw new ArgumentNullException ("icon");

			//DrawImage (icon.GetInternalBitmap (), targetRect);
			throw new NotImplementedException ();
		}

		public void DrawIcon (Icon icon, int x, int y)
		{
			if (icon == null)
				throw new ArgumentNullException ("icon");

			//DrawImage (icon.GetInternalBitmap (), x, y);
			throw new NotImplementedException ();
		}
		
		public void DrawIconUnstretched (Icon icon, Rectangle targetRect)
		{
			if (icon == null)
				throw new ArgumentNullException ("icon");

			//DrawImageUnscaled (icon.GetInternalBitmap (), targetRect);
			throw new NotImplementedException ();
		}
		
		void MakePie (float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			float rx, ry, cx, cy, alpha;
			float sin_alpha, cos_alpha;
				
			rx = width / 2;
			ry = height / 2;
		
			/* center */
			cx = x + rx;
			cy = y + ry;
		
			/* angles in radians */        
			alpha = (float) startAngle * (float) Math.PI / 180;
			
	        /* adjust angle for ellipses */
	        alpha = (float) Math.Atan2 (rx * Math.Sin (alpha), ry * Math.Cos (alpha));
		
			sin_alpha = (float) Math.Sin (alpha);
			cos_alpha = (float) Math.Cos (alpha);
		
			/* draw pie edge */
			if (Math.Abs (sweepAngle) >= 360)
				MoveTo (cx + rx * cos_alpha, cy + ry * sin_alpha);
			else {
				MoveTo (cx, cy);
				LineTo (cx + rx * cos_alpha, cy + ry * sin_alpha);
			}
		
			/* draw the arcs */
			//MakeArcs (x, y, width, height, startAngle, sweepAngle);
			throw new NotImplementedException ();
			// 
		
			if (Math.Abs (sweepAngle) >= 360)
				MoveTo (cx, cy);
			else
				LineTo (cx, cy);
		}

		public void DrawPie (Pen pen, Rectangle rect, float startAngle, float sweepAngle)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			DrawPie (pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}
		
		public void DrawPie (Pen pen, RectangleF rect, float startAngle, float sweepAngle)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			DrawPie (pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}
		
		public void DrawPie (Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			// 
			throw new NotImplementedException ();
		}
		public void FillPie (Brush brush, Rectangle rect, float startAngle, float sweepAngle)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			throw new NotImplementedException ();
		}

		public void FillPie (Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			throw new NotImplementedException ();
		}

		public void FillPie (Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			throw new NotImplementedException ();
		}

		void PolygonSetup (PointF [] points)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			if (points.Length < 2)
				throw new ArgumentException ("Needs at least two points");
			MoveTo (points [0]);
			for (int i = 0; i < points.Length; i++)
				LineTo (points [i]);
		}
		
		public void DrawPolygon (Pen pen, Point [] points)
		{
			DrawPolygon (pen, ConvertPoints (points));
		}

		public void DrawPolygon (Pen pen, PointF [] points)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			PolygonSetup (points);
			StrokePen (pen);
		}	

		public void FillPolygon (Brush brush, Point [] points, FillMode fillMode = FillMode.Alternate)
		{
			FillPolygon (brush, ConvertPoints (points), fillMode);
		}

		public void FillPolygon (Brush brush, PointF [] points, FillMode fillMode = FillMode.Alternate)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (points == null)
				throw new ArgumentNullException ("points");

			PolygonSetup (points);
			FillBrush (brush, fillMode);
		}
		
		public void DrawRectangles (Pen pen, RectangleF [] rects)
		{
			if (pen == null)
				throw new ArgumentNullException ("image");
			if (rects == null)
				throw new ArgumentNullException ("rects");

			foreach (var rect in rects)
				RectanglePath (rect.X, rect.Y, rect.Right, rect.Bottom);
			StrokePen (pen);
		}

		public void DrawRectangles (Pen pen, Rectangle [] rects)
		{
			if (pen == null)
				throw new ArgumentNullException ("image");
			if (rects == null)
				throw new ArgumentNullException ("rects");

			foreach (var rect in rects)
				RectanglePath (rect.X, rect.Y, rect.Right, rect.Bottom);
			StrokePen (pen);
		}

		public void FillRectangles (Brush brush, Rectangle [] rects)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (rects == null)
				throw new ArgumentNullException ("rects");

			foreach (var rect in rects)
				RectanglePath (rect.X, rect.Y, rect.Right, rect.Bottom);
			FillBrush (brush);
		}

		public void FillRectangles (Brush brush, RectangleF [] rects)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (rects == null)
				throw new ArgumentNullException ("rects");

			foreach (var rect in rects)
				RectanglePath (rect.X, rect.Y, rect.Right, rect.Bottom);

			FillBrush (brush);
		}

		public void DrawString (string s, Font font, Brush brush, PointF point, StringFormat format = null)
		{
			DrawString(s, font, brush, new RectangleF(point.X, point.Y, 0, 0), format);
		}

		public void DrawString (string s, Font font, Brush brush, float x, float y, StringFormat format = null)
		{
			DrawString (s, font, brush, new RectangleF(x, y, 0, 0), format);
		}

		public void DrawString (string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format = null)
		{
			if (font == null)
				throw new ArgumentNullException ("font");
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (s == null || s.Length == 0)
				return;
			throw new NotImplementedException ();
		}	
		
		public void Flush ()
		{
			Flush (FlushIntention.Flush);
		}

		
		public void Flush (FlushIntention intention)
		{
			if (context == null)
				return;

			throw new NotImplementedException ();
		}
		
		public bool IsVisible (Point point)
		{
			throw new NotImplementedException();
		}

		
		public bool IsVisible (RectangleF rect)
		{
			throw new NotImplementedException ();
		}

		public bool IsVisible (PointF point)
		{
			throw new NotImplementedException ();
		}
		
		public bool IsVisible (Rectangle rect)
		{
			bool isVisible = false;

			throw new NotImplementedException ();
		}
		
		public bool IsVisible (float x, float y)
		{
			return IsVisible (new PointF (x, y));
		}
		
		public bool IsVisible (int x, int y)
		{
			return IsVisible (new Point (x, y));
		}
		
		public bool IsVisible (float x, float y, float width, float height)
		{
			return IsVisible (new RectangleF (x, y, width, height));
		}

		
		public bool IsVisible (int x, int y, int width, int height)
		{
			return IsVisible (new Rectangle (x, y, width, height));
		}
		
		public Region[] MeasureCharacterRanges (string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
		{
			if ((text == null) || (text.Length == 0))
				return new Region [0];

			if (font == null)
				throw new ArgumentNullException ("font");

			if (stringFormat == null)
				throw new ArgumentException ("stringFormat");

			throw new NotImplementedException ();
		}
		
		public void MultiplyTransform (Matrix matrix)
		{
			MultiplyTransform (matrix, MatrixOrder.Prepend);
		}

		public void MultiplyTransform (Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
				throw new ArgumentNullException ("matrix");

			if (order == MatrixOrder.Prepend)
				context.ConcatCTM (matrix.transform);
			else
				throw new NotImplementedException ();
		}
		
		public void TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF [] pts)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");

			throw new NotImplementedException ();
		}


		public void TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, Point [] pts)
		{						
			if (pts == null)
				throw new ArgumentNullException ("pts");
          throw new NotImplementedException ();
		}
		
	}
}