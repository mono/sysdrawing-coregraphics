using System;
using System.Drawing.Imaging;

namespace System.Drawing
{
	public partial class Graphics {
		public delegate bool DrawImageAbort (IntPtr callbackData);
		public void DrawImage (Image image, RectangleF rect)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			
			throw new NotImplementedException ();
		}

		public void DrawImage (Image image, PointF point)
		{
			if (image == null)
				throw new ArgumentNullException ("image");

			throw new NotImplementedException ();
		}

		public void DrawImage (Image image, Point [] destPoints)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			
			//Status status = GDIPlus.GdipDrawImagePointsI (nativeObject, image.NativeObject, destPoints, destPoints.Length);
			//GDIPlus.CheckStatus (status);
			throw new NotImplementedException ();
		}

		public void DrawImage (Image image, Point point)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			DrawImage (image, point.X, point.Y);
		}

		public void DrawImage (Image image, Rectangle rect)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			DrawImage (image, rect.X, rect.Y, rect.Width, rect.Height);
		}

		public void DrawImage (Image image, PointF [] destPoints)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePoints (nativeObject, image.NativeObject, destPoints, destPoints.Length);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, int x, int y)
		{
			DrawImage (image, new Point (x, y));
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
			
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointsRectI (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y, 
			//	srcRect.Width, srcRect.Height, srcUnit, IntPtr.Zero, 
			//	null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
		}

		public void DrawImage (Image image, PointF [] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");
			
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImagePointsRect (nativeObject, image.NativeObject,
			//	destPoints, destPoints.Length , srcRect.X, srcRect.Y, 
			//	srcRect.Width, srcRect.Height, srcUnit, IntPtr.Zero, 
			//	null, IntPtr.Zero);
			//GDIPlus.CheckStatus (status);
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
			throw new NotImplementedException ();
			//Status status = GDIPlus.GdipDrawImageRectI (nativeObject, image.nativeObject, x, y, width, height);
			//GDIPlus.CheckStatus (status);
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

	}
}

