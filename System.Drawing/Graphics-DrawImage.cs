using System;
using System.Drawing.Imaging;


#if MONOMAC
using MonoMac.CoreGraphics;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ImageIO;
#else
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.ImageIO;
#endif

namespace System.Drawing
{
	public partial class Graphics {
		public delegate bool DrawImageAbort (IntPtr callbackData);

		/// <summary>
		/// Draws the specified Image at the specified location and with the specified size.
		/// </summary>
		/// <param name="image">Image.</param>
		/// <param name="rect">Rect.</param>
		public void DrawImage (Image image, RectangleF rect)
		{
			if (image == null)
				throw new ArgumentNullException ("image");

			// we are getting an error somewhere and not sure where
			// I think the image bitmapBlock is being corrupted somewhere
			try {
				context.DrawImage(rect, image.NativeCGImage);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
		}

		/// <summary>
		/// Draws the specified Image, using its original physical size, at the specified location.
		/// </summary>
		/// <param name="image">Image.</param>
		/// <param name="point">Point.</param>
		public void DrawImage (Image image, PointF point)
		{
			if (image == null)
				throw new ArgumentNullException ("image");

			DrawImage(image, point.X, point.Y);
		}

		/// <summary>
		/// Draws the specified Image at the specified location and with the specified shape and size.
		/// 
		/// The destPoints parameter specifies three points of a parallelogram. The three Point structures 
		/// represent the upper-left, upper-right, and lower-left corners of the parallelogram. The fourth 
		/// point is extrapolated from the first three to form a parallelogram.  
		/// 
		/// The image represented by the image parameter is scaled and sheared to fit the shape of the 
		/// parallelogram specified by the destPoints parameters.
		/// </summary>
		/// <param name="image">Image.</param>
		/// <param name="destPoints">Destination points.</param>
		public void DrawImage (Image image, Point [] destPoints)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");

			if (destPoints.Length < 3)
				throw new ArgumentException ("Destination points must be an array with a length of 3 or 4. " +
					"A length of 3 defines a parallelogram with the upper-left, upper-right, " +
					"and lower-left corners. A length of 4 defines a quadrilateral with the " +
					"fourth element of the array specifying the lower-right coordinate.");

			// Windows throws a Not Implemented error if the points are more than 3
			if (destPoints.Length > 3)
				throw new NotImplementedException ();

			context.SaveState ();


			var rect = new RectangleF (0,0, destPoints [1].X - destPoints [0].X, destPoints [2].Y - destPoints [0].Y);

			// We need to give this some perspective so we will manipulate the transform matrix
			// associated to the context
			var affine = GeomUtilities.CreateGeometricTransform (rect, destPoints);
			context.ConcatCTM (affine);

			context.DrawImage(rect, image.NativeCGImage);

			context.RestoreState ();

		}

		/// <summary>
		/// Draws the specified Image, using its original physical size, at the specified location.
		/// 
		/// </summary>
		/// <param name="image">Image.</param>
		/// <param name="point">Point.</param>
		public void DrawImage (Image image, Point point)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			DrawImage (image, point.X, point.Y);
		}

		/// <summary>
		/// Draws the specified Image at the specified location and with the specified size.
		/// 
		/// The image represented by the image object is scaled to the dimensions of the rect rectangle.
		/// </summary>
		/// <param name="image">Image.</param>
		/// <param name="rect">Rect.</param>
		public void DrawImage (Image image, Rectangle rect)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			DrawImage (image, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Draws the specified Image at the specified location and with the specified shape and size.
		/// 
		/// The destPoints parameter specifies three points of a parallelogram. The three PointF structures 
		/// represent the upper-left, upper-right, and lower-left corners of the parallelogram. The fourth point 
		/// is extrapolated from the first three to form a parallelogram.
		/// 
		/// The image represented by the image object is scaled and sheared to fit the shape of the parallelogram 
		/// specified by the destPoints parameter.
		/// </summary>
		/// <param name="image">Image.</param>
		/// <param name="destPoints">Destination points.</param>
		public void DrawImage (Image image, PointF [] destPoints)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			if (destPoints.Length < 3)
				throw new ArgumentException ("Destination points must be an array with a length of 3 or 4. " +
				                             "A length of 3 defines a parallelogram with the upper-left, upper-right, " +
				                             "and lower-left corners. A length of 4 defines a quadrilateral with the " +
				                             "fourth element of the array specifying the lower-right coordinate.");

			// Windows throws a Not Implemented error if the points are more than 3
			if (destPoints.Length > 3)
				throw new NotImplementedException ();

			context.SaveState ();


			var rect = new RectangleF (0,0, destPoints [1].X - destPoints [0].X, destPoints [2].Y - destPoints [0].Y);

			// We need to give this some perspective so we will manipulate the transform matrix
			// associated to the context
			var affine = GeomUtilities.CreateGeometricTransform (rect, destPoints);
			context.ConcatCTM (affine);

			context.DrawImage(rect, image.NativeCGImage);

			context.RestoreState ();

		}

		public void DrawImage (Image image, int x, int y)
		{
			DrawImage (image, x, y, image.Width, image.Height);
		}

		public void DrawImage (Image image, float x, float y)
		{
			DrawImage (image, new PointF (x, y));
		}

		public void DrawImage (Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectRectI (nativeObject, image.NativeObject,
			//	destRect.X, destRect.Y, destRect.Width, destRect.Height,
			//	srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height,
			//	srcUnit, IntPtr.Zero, null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}
		
		public void DrawImage (Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
		{			
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectRect (nativeObject, image.NativeObject,
			//	destRect.X, destRect.Y, destRect.Width, destRect.Height,
			//	srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height,
			//	srcUnit, IntPtr.Zero, null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, Point [] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			
			//throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointsRectI (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y, 
			//	srcRect.Width, srcRect.Height, srcUnit, IntPtr.Zero, 
			//	null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);

			context.DrawImage (srcRect, image.NativeCGImage);


		}

		public void DrawImage (Image image, PointF [] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			
			//throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointsRect (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y, 
			//	srcRect.Width, srcRect.Height, srcUnit, IntPtr.Zero, 
			//	null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
			context.DrawImage (srcRect, image.NativeCGImage);
		}

		public void DrawImage (Image image, Point [] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, 
                                ImageAttributes imageAttr)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointsRectI (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y,
			//	srcRect.Width, srcRect.Height, srcUnit,
			//	imageAttr != null ? imageAttr.NativeObject : IntPtr.Zero, null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}
		
		public void DrawImage (Image image, float x, float y, float width, float height)
		{
			DrawImage (image, new RectangleF (x, y, width, height));
		}

		public void DrawImage (Image image, PointF [] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, 
                                ImageAttributes imageAttr)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointsRect (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y,
			//	srcRect.Width, srcRect.Height, srcUnit, 
			//	imageAttr != null ? imageAttr.NativeObject : IntPtr.Zero, null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
		{			
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointRectI(nativeObject, image.NativeObject, x, y, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit);
			//GDIPlus.CheckStatus (status);
		}
		
		public void DrawImage (Image image, int x, int y, int width, int height)
		{
			if (image == null)
				throw new ArgumentNullException ("image");

			//throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectI (nativeObject, image.nativeObject, x, y, width, height);
			//GDIPlus.CheckStatus (status);
			DrawImage(image, new RectangleF(x,y,width, height));
		}

		public void DrawImage (Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
		{			
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointRect (nativeObject, image.nativeObject, x, y, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, PointF [] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointsRect (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y,
			//	srcRect.Width, srcRect.Height, srcUnit, 
			//	imageAttr != null ? imageAttr.NativeObject : IntPtr.Zero, callback, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, Point [] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointsRectI (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y,
			//	srcRect.Width, srcRect.Height, srcUnit, 
			//	imageAttr != null ? imageAttr.NativeObject : IntPtr.Zero, callback, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, Point [] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");

			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointsRectI (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y, 
			//	srcRect.Width, srcRect.Height, srcUnit, 
			//	imageAttr != null ? imageAttr.NativeObject : IntPtr.Zero, callback, (IntPtr) callbackData);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectRect (nativeObject, image.NativeObject,
            //                    destRect.X, destRect.Y, destRect.Width, destRect.Height,
            //           		srcX, srcY, srcWidth, srcHeight, srcUnit, IntPtr.Zero, 
            //           		null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status); 					
		}
		
		public void DrawImage (Image image, PointF [] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
		{
			//Status status = GDIPlus.GdipDrawImagePointsRect (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y,
			//	srcRect.Width, srcRect.Height, srcUnit, 
			//	imageAttr != null ? imageAttr.NativeObject : IntPtr.Zero, callback, (IntPtr) callbackData);
			//GDIPlus.CheckStatus (status);
			throw new NotImplementedException ();
		}

		public void DrawImage (Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			//Status status = GDIPlus.GdipDrawImageRectRectI (nativeObject, image.NativeObject,
            //                    destRect.X, destRect.Y, destRect.Width, destRect.Height,
            //           		srcX, srcY, srcWidth, srcHeight, srcUnit, IntPtr.Zero, 
            //           		null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
			throw new NotImplementedException ();
		}

		public void DrawImage (Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectRect (nativeObject, image.NativeObject,
            //                    destRect.X, destRect.Y, destRect.Width, destRect.Height,
            //           		srcX, srcY, srcWidth, srcHeight, srcUnit,
			//	imageAttrs != null ? imageAttrs.NativeObject : IntPtr.Zero, null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}
		
		public void DrawImage (Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{			
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectRectI (nativeObject, image.NativeObject, 
            //                            destRect.X, destRect.Y, destRect.Width, 
			//		destRect.Height, srcX, srcY, srcWidth, srcHeight,
			//		srcUnit, imageAttr != null ? imageAttr.NativeObject : IntPtr.Zero, null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}
		
		public void DrawImage (Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectRectI (nativeObject, image.NativeObject, 
            //                            destRect.X, destRect.Y, destRect.Width, 
			//		destRect.Height, srcX, srcY, srcWidth, srcHeight,
			//		srcUnit, imageAttr != null ? imageAttr.NativeObject : IntPtr.Zero, callback,
			//		IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}
		
		public void DrawImage (Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectRect (nativeObject, image.NativeObject, 
            //                            destRect.X, destRect.Y, destRect.Width, 
			//		destRect.Height, srcX, srcY, srcWidth, srcHeight,
			//		srcUnit, imageAttrs != null ? imageAttrs.NativeObject : IntPtr.Zero, 
			//		callback, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectRect (nativeObject, image.NativeObject, 
			//	destRect.X, destRect.Y, destRect.Width, destRect.Height,
			//	srcX, srcY, srcWidth, srcHeight, srcUnit, 
			//	imageAttrs != null ? imageAttrs.NativeObject : IntPtr.Zero, callback, callbackData);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectRect (nativeObject, image.NativeObject, 
            //           		destRect.X, destRect.Y, destRect.Width, destRect.Height,
			//	srcX, srcY, srcWidth, srcHeight, srcUnit,
			//	imageAttrs != null ? imageAttrs.NativeObject : IntPtr.Zero, callback, callbackData);
			//GDIPlus.CheckStatus (status);
		}		
		public void DrawImageUnscaled (Image image, Point point)
		{
			DrawImageUnscaled (image, point.X, point.Y);
		}
		
		public void DrawImageUnscaled (Image image, Rectangle rect)
		{
			DrawImageUnscaled (image, rect.X, rect.Y, rect.Width, rect.Height);
		}
		
		public void DrawImageUnscaled (Image image, int x, int y)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			DrawImage (image, x, y, image.Width, image.Height);
		}

		public void DrawImageUnscaled (Image image, int x, int y, int width, int height)
		{
			if (image == null)
				throw new ArgumentNullException ("image");

			// avoid creating an empty, or negative w/h, bitmap...
			if ((width <= 0) || (height <= 0))
				return;

			using (Image tmpImg = new Bitmap (width, height)) {
				using (Graphics g = FromImage (tmpImg)) {
					g.DrawImage (image, 0, 0, image.Width, image.Height);
					DrawImage (tmpImg, x, y, width, height);
				}
			}
		}

		public void DrawImageUnscaledAndClipped (Image image, Rectangle rect)
		{
			if (image == null)
				throw new ArgumentNullException ("image");

			int width = (image.Width > rect.Width) ? rect.Width : image.Width;
			int height = (image.Height > rect.Height) ? rect.Height : image.Height;

			DrawImageUnscaled (image, rect.X, rect.Y, width, height);			
		}

	}
}

