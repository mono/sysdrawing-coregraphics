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
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreText;
#else
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreText;
#endif

namespace System.Drawing {

	public sealed partial class Graphics : MarshalByRefObject, IDisposable {
		internal CGContext context;
		internal Pen LastPen;
		internal Brush LastBrush;
		internal SizeF contextUserSpace;
		internal RectangleF boundingBox;
		internal GraphicsUnit quartzUnit = GraphicsUnit.Point;
		internal object nativeObject;
		internal bool isFlipped;
		internal InterpolationMode interpolationMode;
		// Need to keep a transform around, since it is not possible to
		// set the transform on the context, merely to concatenate.
		CGAffineTransform transform;
		internal SmoothingMode smoothingMode;

		// Text Layout
		internal Color lastBrushColor;

		// Clipping state variables
		int clipSet = 0;

		// User Space variables
		internal Matrix modelMatrix;
		internal Matrix viewMatrix;
		internal Matrix modelViewMatrix;
		float userspaceScaleX = 1, userspaceScaleY = 1;
		private GraphicsUnit graphicsUnit = GraphicsUnit.Display;
		private float pageScale = 1;
		private PointF renderingOrigin = PointF.Empty;
		private RectangleF subviewClipOffset = RectangleF.Empty;
		private Region clipRegion;
		private float screenScale;

		public Graphics (CGContext context, bool flipped = true)
		{
			if (context == null)
				throw new ArgumentNullException ("context");
			isFlipped = flipped;
			screenScale = 1;
			InitializeContext(context);
		}

#if MONOTOUCH
		public Graphics() {

			var gc = UIGraphics.GetCurrentContext ();
			nativeObject = gc;
			screenScale = UIScreen.MainScreen.Scale;
			InitializeContext(gc);
		}
#endif

#if MONOMAC
		private Graphics() :
			this (NSGraphicsContext.CurrentContext)
		{	}

		private Graphics (NSGraphicsContext context)
		{
			var gc = context;

			if (gc.IsFlipped)
				gc = NSGraphicsContext.FromGraphicsPort (gc.GraphicsPort, false);

			// testing for now
			//			var attribs = gc.Attributes;
			//			attribs = NSScreen.MainScreen.DeviceDescription;
			//			NSValue asdf = (NSValue)attribs["NSDeviceResolution"];
			//			var size = asdf.SizeFValue;
			// ----------------------
			screenScale = 1;
			nativeObject = gc;

			isFlipped = gc.IsFlipped;

			InitializeContext(gc.GraphicsPort);

		}

		public static Graphics FromContext (NSGraphicsContext context)
		{
			return new Graphics (context);
		}
#endif

		public static Graphics FromCurrentContext()
		{
			return new Graphics ();
		}

		private void InitializeContext(CGContext context) 
		{
			this.context = context;

			modelMatrix = new Matrix();
			viewMatrix = new Matrix();

			ResetTransform();

			boundingBox = context.GetClipBoundingBox();

			// We are going to try this here and it may cause problems down the road.
			// This seems to only happen with Mac and not iOS
			// What is happening is that sub views are offset by their relative location
			// within the window.  That means our drawing locations are also offset by this 
			// value as well.  So what we need to do is translate our view by this offset as well.
			subviewClipOffset = context.GetClipBoundingBox();

			PageUnit = GraphicsUnit.Pixel;
			PageScale = 1;

			// Set anti-aliasing
			SmoothingMode = SmoothingMode.Default;

			clipRegion = new Region ();
		}

		private void initializeMatrix(ref Matrix matrix, bool isFlipped) 
		{
			if (!isFlipped) 
			{
				//				matrix.Reset();
				//				matrix.Translate(0, boundingBox.Height, MatrixOrder.Append);
				//				matrix.Scale(1,-1, MatrixOrder.Append);
				matrix = new Matrix(
					1, 0, 0, -1, 0, boundingBox.Height);
				
			}
			else {
				matrix.Reset();
			}
			
			// It looks like we really do not need to determin if it is flipped or not.
			// So far this following is working no matter which coordinate system is being used
			// on both Mac and iOS.  
			// I will leave the previous commented out code there just in case.  When first implementing 
			// DrawString the flipped coordinates were causing problems.  Now after implementing with 
			// CoreText it seems to all be working.  Fingers Crossed.
//			matrix = new Matrix(
//				1, 0, 0, -1, 0, boundingBox.Height);
		}

		internal float GraphicsUnitConvertX (float x)
		{
			return ConversionHelpers.GraphicsUnitConversion(PageUnit, GraphicsUnit.Pixel, DpiX, x);
		}

		internal float GraphicsUnitConvertY (float y)
		{
			return ConversionHelpers.GraphicsUnitConversion(PageUnit, GraphicsUnit.Pixel, DpiY, y);
		}

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
			// First we call the Pen with a fill of false so the brush can setup the stroke 
			// For LinearGradientBrush this will setup a TransparentLayer so the gradient can
			// be filled at the end.  See comments.
			pen.Setup (this, false);
			context.DrawPath(CGPathDrawingMode.Stroke);
			// Next we call the Pen with a fill of true so the brush can draw the Gradient. 
			// For LinearGradientBrush this will draw the Gradient and end the TransparentLayer.
			// See comments.
			pen.Setup (this, true);
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

			DrawEllipticalArc(x, y, width, height, startAngle, sweepAngle, false);
			StrokePen (pen);
		}

		// Microsoft documentation states that the signature for this member should be
		// public void DrawArc( Pen pen,  int x,  int y,  int width,  int height,   int startAngle,
   		// int sweepAngle. However, GdipDrawArcI uses also float for the startAngle and sweepAngle params
   		public void DrawArc (Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			DrawEllipticalArc(x, y, width, height, startAngle, sweepAngle, false);
			StrokePen (pen);
		}

		public void DrawLine (Pen pen, Point pt1, Point pt2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

#if MONOTOUCH
			// DrawLine is throwing an assertion error on MonoTouch
			// Assertion failed: (CGFloatIsValid(x) && CGFloatIsValid(y))
			// , function void CGPathAddLineToPoint(CGMutablePathRef, const CGAffineTransform *, CGFloat, CGFloat)
			// What we will do here is not draw the line at all if any of the points are Single.NaN
			if (!float.IsNaN(pt1.X) && !float.IsNaN(pt1.Y) &&
			    !float.IsNaN(pt2.X) && !float.IsNaN(pt2.Y)) 
			{
				MoveTo (pt1.X, pt1.Y);
				LineTo (pt2.X, pt2.Y);
			}
#else

			MoveTo (pt1.X, pt1.Y);
			LineTo (pt2.X, pt2.Y);
#endif

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

#if MONOTOUCH
			// DrawLine is throwing an assertion error on MonoTouch
			// Assertion failed: (CGFloatIsValid(x) && CGFloatIsValid(y))
			// , function void CGPathAddLineToPoint(CGMutablePathRef, const CGAffineTransform *, CGFloat, CGFloat)
			// What we will do here is not draw the line at all if any of the points are Single.NaN
			if (!float.IsNaN(pt1.X) && !float.IsNaN(pt1.Y) &&
			    !float.IsNaN(pt2.X) && !float.IsNaN(pt2.Y)) 
			{
				MoveTo (pt1.X, pt1.Y);
				LineTo (pt2.X, pt2.Y);
				StrokePen (pen);
			}
#else

			MoveTo (pt1.X, pt1.Y);
			LineTo (pt2.X, pt2.Y);
			StrokePen (pen);
#endif
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
			LineTo (x2, y1);
			context.ClosePath ();
		}
			
		void RectanglePath (RectangleF rectangle) 
		{
			MoveTo (rectangle.Location);
			context.AddRect(rectangle);
			context.ClosePath();
		}

		public void DrawRectangle (Pen pen, float x1, float y1, float x2, float y2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			RectanglePath (new RectangleF(x1, y1, x2, y2));
			StrokePen (pen);
		}
		
		public void DrawRectangle (Pen pen, int x1, int y1, int x2, int y2)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			RectanglePath (new RectangleF(x1, y1, x2, y2));
			StrokePen (pen);
		}
		
		public void DrawRectangle (Pen pen, RectangleF rect)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			RectanglePath (rect);
			StrokePen (pen);

		}

		public void DrawRectangle (Pen pen, Rectangle rect)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			RectanglePath (new RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
			StrokePen (pen);

		}

		public void FillRectangle (Brush brush, float x1, float y1, float x2, float y2)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			RectanglePath (new RectangleF(x1, y1, x2, y2));
			FillBrush (brush);

		}

		public void FillRectangle (Brush brush, Rectangle rect)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			RectanglePath (new RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
			FillBrush (brush);

		}
		
		public void FillRectangle (Brush brush, RectangleF rect)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			RectanglePath (new RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
			FillBrush (brush);

		}

		public void FillRectangle (Brush brush, int x1, int y1, int x2, int y2)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");

			RectanglePath (new RectangleF(x1, y1, x2, y2));
			FillBrush (brush);

		}

		
		public void FillRegion (Brush brush, Region region)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (region == null)
				throw new ArgumentNullException ("region");

			// We will clear the rectangle of our clipping bounds for Empty
			if (region.regionPath == null) 
			{
				// This may set the rectangle to Black depending on the context
				// passed.  On a NSView set WantsLayers and the Layer Background color.
				context.ClearRect (context.GetClipBoundingBox ());
				return;
			}

			context.AddPath (region.regionPath);
			FillBrush (brush);
		}

		public void DrawEllipse (Pen pen, RectangleF rect)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

			context.AddEllipseInRect(rect);
			StrokePen(pen);
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

			DrawEllipse (pen, new RectangleF (x, y, width, height));

		}

		public void FillEllipse (Brush brush, RectangleF rect)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");

			context.AddEllipseInRect(rect);
			FillBrush(brush);
		}

		public void FillEllipse (Brush brush, int x1, int y1, int x2, int y2)
		{
			FillEllipse (brush, new RectangleF (x1, y1, x2, y2));
		}
		
		public void FillEllipse (Brush brush, float x1, float y1, float x2, float y2)
		{
			FillEllipse (brush, new RectangleF (x1, y1, x2, y2));
		}

		private void applyModelView() {
			
			// Since there is no context.SetCTM, only ConcatCTM
			// get the current transform, invert it, and concat this to
			// obtain the identity.   Then we concatenate the value passed
			context.ConcatCTM (context.GetCTM().Invert());

			var modelView = CGAffineTransform.Multiply(modelMatrix.transform, viewMatrix.transform);

//			Console.WriteLine("------------ apply Model View ------");
//			Console.WriteLine("Model: " + modelMatrix.transform);
//			Console.WriteLine("View: " + viewMatrix.transform);
//			Console.WriteLine("ModelView: " + modelView);
//			Console.WriteLine("------------ end apply Model View ------\n\n");
			// we apply the Model View matrix passed to the context
			context.ConcatCTM (modelView);

		} 

		public void ResetTransform ()
		{
			modelMatrix.Reset();
			applyModelView();
		}
		
		public Matrix Transform {
			get {
				return modelMatrix;
			}
			set {
				modelMatrix = value;
				applyModelView();
			}
		}

		public void RotateTransform (float angle)
		{
			RotateTransform (angle, MatrixOrder.Prepend);
		}

		public void RotateTransform (float angle, MatrixOrder order)
		{
			modelMatrix.Rotate(angle, order);
			applyModelView();
		}
		
		public void TranslateTransform (float tx, float ty)
		{
			TranslateTransform (tx, ty, MatrixOrder.Prepend);
		}
		
		public void TranslateTransform (float tx, float ty, MatrixOrder order)
		{
			//Console.WriteLine ("Currently does not support anything but prepend mode");
			modelMatrix.Translate(tx, ty, order);
			applyModelView();
		}
		
		public void ScaleTransform (float sx, float sy)
		{
			ScaleTransform (sx, sy, MatrixOrder.Prepend);
		}
		
		public void ScaleTransform (float sx, float sy, MatrixOrder order)
		{
			modelMatrix.Scale(sx,sy,order);
			applyModelView();
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
			for (int i = 0; i < len; i++)
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
				switch (compositing_mode) 
				{
				case CompositingMode.SourceCopy:
					context.SetBlendMode (CGBlendMode.Copy);
					break;
				case CompositingMode.SourceOver:
					context.SetBlendMode (CGBlendMode.Overlay);
					break;
				}
			}
		}

		public Color GetNearestColor(Color color)
		{
			// this uses a color pallette which we really do not implement
			// so just return back the color passed for now.
			return Color.FromArgb (color.ToArgb());
		}

		void setupView() 
		{
			initializeMatrix(ref viewMatrix, isFlipped);
			// * NOTE * Here we offset our drawing by the subview Clipping region of the Window
			// this is so that we start at offset 0,0 for all of our graphic operations
			viewMatrix.Translate(subviewClipOffset.Location.X, subviewClipOffset.Y, MatrixOrder.Append);

			// Take into account retina diplays
			viewMatrix.Scale(screenScale, screenScale);

			userspaceScaleX = GraphicsUnitConvertX(1) * pageScale;
			userspaceScaleY = GraphicsUnitConvertY(1) * pageScale;
			viewMatrix.Scale(userspaceScaleX, userspaceScaleY);
			viewMatrix.Translate(renderingOrigin.X * userspaceScaleX, 
			                     -renderingOrigin.Y * userspaceScaleY,MatrixOrder.Append);
			applyModelView();
			
		}

		public GraphicsUnit PageUnit { 
			get {
				return graphicsUnit;
			}
			set {
				graphicsUnit = value;

				setupView();
			} 
		}

		public float PageScale 
		{ 
			get { return pageScale; }
			set {
				// TODO: put some validation in here maybe?  Need to 
				pageScale = value;
				setupView();			
			}
		}

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

			var bitmapContext = b.GetRenderableContext ();

			return new Graphics (bitmapContext, false);
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
			SetClip (new Region (rect), combineMode);
		}

		public void SetClip (Rectangle rect, CombineMode combineMode)
		{
			SetClip ((RectangleF)rect, combineMode);
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
			// We need to reset the clip that is active now so that the graphic
			// states are correct when we set them.
			ResetClip ();

			switch (combineMode) 
			{
			case CombineMode.Replace:
				// Set our clip region by cloning the region that is passed for now
				clipRegion = region.Clone ();
				break;
			case CombineMode.Intersect:

				clipRegion.Intersect (region);

				break;
			case CombineMode.Union:

				clipRegion.Union (region);

				break;
			case CombineMode.Exclude:

				clipRegion.Exclude (region);

				break;
			case CombineMode.Xor:

				clipRegion.Xor (region);

				break;
			default:
				throw new NotImplementedException ("SetClip for CombineMode " + combineMode + " not implemented");
			}

			//Unlike the current path, the current clipping path is part of the graphics state. 
			//Therefore, to re-enlarge the paintable area by restoring the clipping path to a 
			//prior state, you must save the graphics state before you clip and restore the graphics 
			//state after you’ve completed any clipped drawing.
			context.SaveState ();
			if (clipRegion.IsEmpty) {
				context.ClipToRect (RectangleF.Empty);
			} else {
				//context.ClipToRect ((RectangleF)clipRegion.regionObject);
				context.AddPath (clipRegion.regionPath);
				context.ClosePath ();
				context.Clip ();
			}
			clipSet++;

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
		

		// CGContext Anti-Alias:
		// A Boolean value that specifies whether anti-aliasing should be turned on. 
		// Anti-aliasing is turned on by default when a window or bitmap context is created. 
		// It is turned off for other types of contexts.

		// Default, None, and HighSpeed are equivalent and specify rendering without smoothing applied.
		// AntiAlias and HighQuality are equivalent and specify rendering with smoothing applied.
		public SmoothingMode SmoothingMode { 
			get { return smoothingMode; } 
			set 
			{
				// Quartz performs antialiasing for a graphics context if both the allowsAntialiasing parameter 
				// and the graphics state parameter shouldAntialias are true.
				switch (value) 
				{
				case SmoothingMode.AntiAlias:
				case SmoothingMode.HighQuality:
					context.SetAllowsAntialiasing(true);  // This parameter is not part of the graphics state.
					context.SetShouldAntialias(true);
					break;
				default:
					context.SetAllowsAntialiasing(false); // This parameter is not part of the graphics state.
					context.SetShouldAntialias(false);
					break;
				}
			}
		}
		
		public bool IsClipEmpty {
			get {
				return clipRegion.IsEmpty;
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
				return clipRegion;
			}
			set {
				SetClip (value, CombineMode.Replace);
			}
		}
		
		public RectangleF ClipBounds {
			get {
				return clipRegion.GetBounds ();
				//return context.GetClipBoundingBox ();
			}
			set {
				SetClip(value);
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

		/// <summary>
		/// Gets or sets the interpolation mode.  The interpolation mode determines how intermediate values between two endpoints are calculated.
		/// </summary>
		/// <value>The interpolation mode.</value>
		public InterpolationMode InterpolationMode {
			get {
				return interpolationMode;
			}
			set {
				interpolationMode = value;
				switch (value) 
				{
				case InterpolationMode.Low:
					context.InterpolationQuality = CGInterpolationQuality.Low;
					break;
				case InterpolationMode.High:
				case InterpolationMode.HighQualityBicubic:
				case InterpolationMode.HighQualityBilinear:
					context.InterpolationQuality = CGInterpolationQuality.High;
					break;
				case InterpolationMode.NearestNeighbor:
				case InterpolationMode.Bicubic:
				case InterpolationMode.Bilinear:
					context.InterpolationQuality = CGInterpolationQuality.Medium;
					break;
				case InterpolationMode.Invalid:
					context.InterpolationQuality = CGInterpolationQuality.None;
					break;
				default:
					context.InterpolationQuality = CGInterpolationQuality.Default;
					break;
				}

			}
		}

		public PointF RenderingOrigin {
			get {
				return renderingOrigin;
			}
			set {

				renderingOrigin = value;
				setupView();
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
				// We should probably read the NSScreen attributes and get the resolution
				//    but there are problems getting the value from NSValue to a Rectangle
				// We will set this to a fixed value for now
				return 96.0f;
			}
		}

		public float DpiY { 
			get {
				// We should probably read the NSScreen attributes and get the resolution
				//    but there are problems getting the value from NSValue to a Rectangle
				// We will set this to a fixed value for now
				return 96.0f;
			}
		}


		public CompositingQuality CompositingQuality {
			// There is no support for CompositingQuality in CoreGraphics.
			// Instead of throwing a NotImplementedException we will just let
			// things fall through when setting and return Default always.
			get {
				return CompositingQuality.Default;
			}
			set {

			}
		}
		
		public bool IsVisibleClipEmpty { 
			get {
				return clipRegion == null;
			}
		}

		public void TranslateClip (int dx, int dy)
		{
			TranslateClip ((float)dx, (float)dy);
		}
		
		public void TranslateClip (float dx, float dy)
		{
			clipRegion.Translate (dx, dy);
			SetClip (clipRegion, CombineMode.Replace);
		}

		public void ResetClip ()
		{
			if (clipSet > 0) 
			{

				//Unlike the current path, the current clipping path is part of the graphics state. 
				//Therefore, to re-enlarge the paintable area by restoring the clipping path to a 
				//prior state, you must save the graphics state before you clip and restore the graphics 
				//state after you’ve completed any clipped drawing.
				context.EOClip ();
				context.RestoreState ();

				// We are clobbering our transform when we do the restore.
				// there are probably other one as well.
				applyModelView ();
				clipSet--;
			}
		}
		
		public void ExcludeClip (Rectangle rect)
		{
			SetClip ((RectangleF)rect, CombineMode.Exclude);
		}
		
		public void ExcludeClip (RectangleF rect)
		{
			SetClip (rect, CombineMode.Exclude);
		}

		public void ExcludeClip (Region region)
		{
			SetClip (region, CombineMode.Exclude);
		}

		public void IntersectClip (Rectangle rect)
		{
			SetClip ((RectangleF)rect, CombineMode.Intersect);
		}
		
		public void IntersectClip (RectangleF rect)
		{
			SetClip (rect, CombineMode.Intersect);
		}

		public void IntersectClip (Region region)
		{
			SetClip (region, CombineMode.Intersect);
		}

		NSString FontAttributedName = (NSString)"NSFont";
		NSString ForegroundColorAttributedName = (NSString)"NSColor";
		NSString UnderlineStyleAttributeName = (NSString)"NSUnderline";
		NSString ParagraphStyleAttributeName = (NSString)"NSParagraphStyle";
		//NSAttributedString.ParagraphStyleAttributeName
		NSString StrikethroughStyleAttributeName = (NSString)"NSStrikethrough";

		private NSMutableAttributedString buildAttributedString(string text, Font font, 
		                                                Color? fontColor=null) 
		{


			// Create a new attributed string from text
			NSMutableAttributedString atts = 
				new NSMutableAttributedString(text);

			var attRange = new NSRange(0, atts.Length);
			var attsDic = new NSMutableDictionary();
			
			// Font attribute
			NSObject fontObject = new NSObject(font.nativeFont.Handle);
			attsDic.Add(FontAttributedName, fontObject);
			// -- end font 

			if (fontColor.HasValue) {

				// Font color
				var fc = fontColor.Value;
#if MONOMAC
				NSColor color = NSColor.FromDeviceRgba(fc.R / 255f, 
				                                           fc.G / 255f,
				                                           fc.B / 255f,
				                                           fc.A / 255f);
				NSObject colorObject = new NSObject(color.Handle);
				attsDic.Add(ForegroundColorAttributedName, colorObject);
#else
				UIColor color = UIColor.FromRGBA(fc.R / 255f, 
				                                 fc.G / 255f,
				                                 fc.B / 255f,
				                                 fc.A / 255f);
				NSObject colorObject = new NSObject(color.Handle);
				attsDic.Add(ForegroundColorAttributedName, colorObject);
#endif
				// -- end font Color
			}

			if (font.Underline) {
				// Underline
#if MONOMAC
				int single = (int)MonoMac.AppKit.NSUnderlineStyle.Single;
				int solid = (int)MonoMac.AppKit.NSUnderlinePattern.Solid;
				var attss = single | solid;
				var underlineObject = NSNumber.FromInt32(attss);
				//var under = NSAttributedString.UnderlineStyleAttributeName.ToString();
#else
				var underlineObject = NSNumber.FromInt32 (1);
#endif
				attsDic.Add(UnderlineStyleAttributeName, underlineObject);
				// --- end underline
			}


			if (font.Strikeout) {
				// StrikeThrough
//				NSColor bcolor = NSColor.Blue;
//				NSObject bcolorObject = new NSObject(bcolor.Handle);
//				attsDic.Add(NSAttributedString.StrikethroughColorAttributeName, bcolorObject);
#if MONOMAC
				int stsingle = (int)MonoMac.AppKit.NSUnderlineStyle.Single;
				int stsolid = (int)MonoMac.AppKit.NSUnderlinePattern.Solid;
				var stattss = stsingle | stsolid;
				var stunderlineObject = NSNumber.FromInt32(stattss);
#else
				var stunderlineObject = NSNumber.FromInt32 (1);
#endif

				attsDic.Add(StrikethroughStyleAttributeName, stunderlineObject);
				// --- end underline
			}
			

			// Text alignment
			var alignment = CTTextAlignment.Left;
			var alignmentSettings = new CTParagraphStyleSettings();
			alignmentSettings.Alignment = alignment;
			var paragraphStyle = new CTParagraphStyle(alignmentSettings);
			NSObject psObject = new NSObject(paragraphStyle.Handle);

			// end text alignment
			
			attsDic.Add(ParagraphStyleAttributeName, psObject);
			
			atts.SetAttributes(attsDic, attRange);

			return atts;

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
			
			// As per documentation 
			// https://developer.apple.com/library/mac/#documentation/GraphicsImaging/Conceptual/drawingwithquartz2d/dq_text/dq_text.html#//apple_ref/doc/uid/TP30001066-CH213-TPXREF101
			// 
			// * Note * Not sure if we should save off the graphic state, set context transform to identity
			//  and restore state to do the measurement.  Something to be looked into.
//			context.TextPosition = rect.Location;
//			var startPos = context.TextPosition;
//			context.SelectFont ( font.nativeFont.PostScriptName,
//			                    font.SizeInPoints,
//			                    CGTextEncoding.MacRoman);
//			context.SetTextDrawingMode(CGTextDrawingMode.Invisible); 
//			
//			context.SetCharacterSpacing(1); 
//			var textMatrix = CGAffineTransform.MakeScale(1f,-1f);
//			context.TextMatrix = textMatrix;
//
//			context.ShowTextAtPoint(rect.X, rect.Y, textg, textg.Length); 
//			var endPos = context.TextPosition;
//
//			var measure = new SizeF(endPos.X - startPos.X, font.nativeFont.CapHeightMetric);

			var atts = buildAttributedString(textg, font);

			// for now just a line not sure if this is going to work
			CTLine line = new CTLine(atts);
			var lineBounds = line.GetImageBounds(context);
			// Create and initialize some values from the bounds.
			float ascent;
			float descent;
			float leading;
			double lineWidth = line.GetTypographicBounds(out ascent, out descent, out leading);

			var measure = new SizeF((float)lineWidth, ascent + descent);

			return measure;
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
			context.SaveState ();
			context.SetFillColorWithColor(new CGColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));
			context.FillRect(context.GetClipBoundingBox());
			context.RestoreState ();
		}
		
		public void Restore (GraphicsState gstate)
		{
			LastPen = gstate.lastPen;
			LastBrush = gstate.lastBrush;
			modelMatrix = gstate.model;
			viewMatrix = gstate.view;
			renderingOrigin = gstate.renderingOrigin;
			graphicsUnit = gstate.pageUnit;
			pageScale = gstate.pageScale;
			SmoothingMode = gstate.smoothingMode;
			clipRegion = gstate.clipRegion;
			//applyModelView();
			// I do not know if we should use the contexts save/restore state or our own
			// we will do that save state for now
			context.RestoreState();
		}
		
		public GraphicsState Save ()
		{
			var currentState = new GraphicsState();
			currentState.lastPen = LastPen;
			currentState.lastBrush = LastBrush;
			// Make sure we clone the Matrices or we will still modify
			// them after the save as they are the same objects.  Woops!!
			currentState.model = modelMatrix.Clone();
			currentState.view = viewMatrix.Clone();
			currentState.renderingOrigin = renderingOrigin;
			currentState.pageUnit = graphicsUnit;
			currentState.pageScale = pageScale;
			currentState.smoothingMode = smoothingMode;
			currentState.clipRegion = clipRegion;
			// I do not know if we should use the contexts save/restore state or our own
			// we will do that save state for now
			context.SaveState();
			return currentState;
		}
		
		public void DrawClosedCurve (Pen pen, PointF [] points)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			DrawClosedCurve (pen, points, 0.5f, FillMode.Winding);
		}
		
		public void DrawClosedCurve (Pen pen, Point [] points)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			DrawClosedCurve (pen, ConvertPoints (points), 0.5f, FillMode.Winding);
		}
 			
		// according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
		// GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
		public void DrawClosedCurve (Pen pen, Point [] points, float tension, FillMode fillmode)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");

			DrawClosedCurve (pen, ConvertPoints (points), tension, fillmode);
		}

		// according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
		// GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
		public void DrawClosedCurve (Pen pen, PointF [] points, float tension, FillMode fillmode)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			if (points == null)
				throw new ArgumentNullException ("points");

			int count = points.Length;
			if (count == 2)
				DrawPolygon (pen, points);
			else {
				int segments = (count > 3) ? (count-1) : (count-2);

				var tangents = GraphicsPath.OpenCurveTangents (GraphicsPath.CURVE_MIN_TERMS, points, count, tension);
				MakeCurve (points, tangents, 0, segments, CurveType.Close);
				StrokePen (pen);
			}
		}
		
		public void FillClosedCurve (Brush brush, PointF [] points)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			FillClosedCurve(brush,points,FillMode.Alternate);
		}
		
		public void FillClosedCurve (Brush brush, Point [] points)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			FillClosedCurve(brush,ConvertPoints(points),FillMode.Alternate);
		}
 			
		// according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
		// GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
		public void FillClosedCurve (Brush brush, Point [] points, FillMode fillmode, float tension = 0.5f)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			FillClosedCurve(brush,points,FillMode.Alternate);
		}

		// according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
		// GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
		public void FillClosedCurve (Brush brush, PointF [] points, FillMode fillmode, float tension = 0.5f)
		{

			if (brush == null)
				throw new ArgumentNullException ("brush");
			if (points == null)
				throw new ArgumentNullException ("points");
			
			int count = points.Length;
			if (count == 2)
				FillPolygon(brush, points, FillMode.Alternate);
			else {
				int segments = (count > 3) ? (count-1) : (count-2);
				
				var tangents = GraphicsPath.OpenCurveTangents (GraphicsPath.CURVE_MIN_TERMS, points, count, tension);
				MakeCurve (points, tangents, 0, segments, CurveType.Close);
				FillBrush(brush);
			}
		}
#if MONOTOUCH	
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
#endif		
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

		static double radians (float degrees) {
			return degrees * Math.PI/180;
		
		}

		public void DrawPie (Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");
			DrawEllipticalArc(x,y,width,height, startAngle, sweepAngle, true);
			StrokePen(pen);

		}
		public void FillPie (Brush brush, Rectangle rect, float startAngle, float sweepAngle)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			DrawEllipticalArc(rect, startAngle, sweepAngle, true);
			FillBrush(brush);
		}

		public void FillPie (Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			DrawEllipticalArc(x,y,width,height, startAngle, sweepAngle, true);
			FillBrush(brush);
		}

		public void FillPie (Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (brush == null)
				throw new ArgumentNullException ("brush");
			DrawEllipticalArc(x,y,width,height, startAngle, sweepAngle, true);
			FillBrush(brush);
		}

		void PolygonSetup (PointF [] points)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			if (points.Length < 2)
				throw new ArgumentException ("Needs at least two points");
			MoveTo (points [0]);
			for (int i = 0; i < points.Length - 0; i++)
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
			context.ClosePath ();
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

			// TODO: Take into consideration units

			// Not sure we need the Save and Restore around this yet.
			context.SaveState();

			// TextMatrix is not part of the Graphics State and Restore 
			var saveMatrix = context.TextMatrix;
			bool layoutAvailable = true;

//			context.SelectFont ( font.nativeFont.PostScriptName,
//			                    font.SizeInPoints,
//			                    CGTextEncoding.MacRoman);
//
//			context.SetCharacterSpacing(1);
//			context.SetTextDrawingMode(CGTextDrawingMode.Fill); // 5
//			
//			// Setup both the stroke and the fill ?
//			brush.Setup(this, true);
//			brush.Setup(this, false);
//
//			var textMatrix = font.nativeFont.Matrix;
//
//			textMatrix.Scale(1,-1);
//			context.TextMatrix = textMatrix;
//
//			context.ShowTextAtPoint(layoutRectangle.X, 
//			                        layoutRectangle.Y + font.nativeFont.CapHeightMetric, s); 
//
//
			// First we call the brush with a fill of false so the brush can setup the stroke color
			// that the text will be using.
			// For LinearGradientBrush this will setup a TransparentLayer so the gradient can
			// be filled at the end.  See comments.
			brush.Setup(this, false); // Stroke

			// I think we only Fill the text with no Stroke surrounding
			context.SetTextDrawingMode(CGTextDrawingMode.Fill);

			var attributedString = buildAttributedString(s, font, lastBrushColor);

			// Work out the geometry
			RectangleF insetBounds = layoutRectangle;
			if (insetBounds.Size == SizeF.Empty)
			{
				insetBounds.Width = boundingBox.Width;
				insetBounds.Height = boundingBox.Height;
				layoutAvailable = false;
			}

			PointF textPosition = new PointF(insetBounds.X,
			                                 insetBounds.Y);

			float boundsWidth = insetBounds.Width;
			
			// Calculate the lines
			int start = 0;
			int length = attributedString.Length;
			
			var typesetter = new CTTypesetter(attributedString);

			while (start < length && textPosition.Y < insetBounds.Bottom)
			{

				// Now we ask the typesetter to break off a line for us.
				// This also will take into account line feeds embedded in the text.
				//  Example: "This is text \n with a line feed embedded inside it"
				int count = typesetter.SuggestLineBreak(start, boundsWidth);
				var line = typesetter.GetLine(new NSRange(start, count));

				// Create and initialize some values from the bounds.
				float ascent;
				float descent;
				float leading;
				double lineWidth = line.GetTypographicBounds(out ascent, out descent, out leading);

				// Calculate the string format if need be
				var penFlushness = 0.0f;
				if (format != null) 
				{
					if (layoutAvailable) 
					{
						if (format.Alignment == StringAlignment.Far)
							penFlushness = (float)line.GetPenOffsetForFlush(1.0f, boundsWidth);
						else if (format.Alignment == StringAlignment.Center)
							penFlushness = (float)line.GetPenOffsetForFlush(0.5f, boundsWidth);
					}
					else 
					{
						// We were only passed in a point so we need to format based
						// on the point.
						if (format.Alignment == StringAlignment.Far)
							penFlushness -= (float)lineWidth;
						else if (format.Alignment == StringAlignment.Center)
							penFlushness -= (float)lineWidth / 2.0f;


					}

				
				}

				// initialize our Text Matrix or we could get trash in here
				var textMatrix = CGAffineTransform.MakeIdentity();
				// flip us over or things just do not look good
				textMatrix.Scale(1,-1);
				context.TextMatrix = textMatrix;

				// move us to our graphics baseline
				var textViewPos = textPosition;
				textViewPos.Y += (float)Math.Floor(ascent - 1);

				// take into account our justification
				textViewPos.X += penFlushness;

				// setup our text position
				context.TextPosition = textViewPos;
				// and draw the line
				line.Draw(context);

				// Move the index beyond the line break.
				start += count;
				textPosition.Y += (float)Math.Ceiling(ascent + descent + leading + 1); // +1 matches best to CTFramesetter's behavior  
				line.Dispose();
				
			}

			// Now we call the brush with a fill of true so the brush can do the fill if need be 
			// For LinearGradientBrush this will draw the Gradient and end the TransparentLayer.
			// See comments.
			brush.Setup(this, true); // Fill

			context.TextMatrix = saveMatrix;
			context.RestoreState();



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
			return clipRegion.IsVisible (point);
		}

		
		public bool IsVisible (RectangleF rect)
		{
			return clipRegion.IsVisible (rect);
		}

		public bool IsVisible (PointF point)
		{
			return clipRegion.IsVisible (point);
		}
		
		public bool IsVisible (Rectangle rect)
		{
			return clipRegion.IsVisible (rect);
		}
		
		public bool IsVisible (float x, float y)
		{
			return clipRegion.IsVisible (x, y);
		}
		
		public bool IsVisible (int x, int y)
		{
			return clipRegion.IsVisible (x, y);
		}
		
		public bool IsVisible (float x, float y, float width, float height)
		{
			return IsVisible (new RectangleF (x, y, width, height));
		}

		
		public bool IsVisible (int x, int y, int width, int height)
		{
			return IsVisible (new RectangleF (x, y, width, height));
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
				context.ConcatCTM (matrix.transform);
		}
		
		public void TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF [] pts)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");

			Matrix transform = new Matrix();
			ConversionHelpers.GetGraphicsTransform (this, destSpace, srcSpace, ref transform);
			transform.TransformPoints (pts);
		}


		public void TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, Point [] pts)
		{						
			if (pts == null)
				throw new ArgumentNullException ("pts");

			Matrix transform = new Matrix();
			ConversionHelpers.GetGraphicsTransform (this, destSpace, srcSpace, ref transform);
			transform.TransformPoints (pts);

		}

	}
}