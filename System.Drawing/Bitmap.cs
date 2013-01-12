//
// System.Drawing.Bitmap.cs
//
// Copyright (C) 2002 Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
// Copyright 2011 Xamarin Inc.
//
// Authors: 
//	Alexandre Pigolkine (pigolkine@gmx.de)
//	Christian Meyer (Christian.Meyer@cs.tum.edu)
//	Miguel de Icaza (miguel@ximian.com)
//	Jordi Mas i Hernandez (jmas@softcatala.org)
//	Ravindra (rkumar@novell.com)
//	Sebastien Pouliot  <sebastien@xamarin.com>
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

using System;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.ComponentModel;

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
using MonoTouch.MobileCoreServices;
#endif

namespace System.Drawing {
	
	[Serializable]
	public sealed class Bitmap : Image {
		// if null, we created the bitmap in memory, otherwise, the backing file.

		internal IntPtr bitmapBlock;

		// we will default this to one for now until we get some tests for other image types
		int imageCount = 1;

		public Bitmap (string filename)
		{
			// Use Image IO
			CGDataProvider prov = new CGDataProvider(filename);
			var cg = CGImageSource.FromDataProvider(prov).CreateImage(0, null);
			InitWithCGImage(cg);
		}
		
		public Bitmap (Stream stream, bool useIcm)
		{
			// false: stream is owned by user code
			//nativeObject = InitFromStream (stream);
			// TODO
		}

		public Bitmap (int width, int height) : 
			this (width, height, PixelFormat.Format32bppArgb)
		{
		}
		
		public Bitmap (Image original, int width, int height) : 
			this (width, height, PixelFormat.Format32bppArgb)
		{
			using (Graphics graphics = Graphics.FromImage (this)) {
				graphics.DrawImage (original, 0, 0, width, height);
			}
		}
		
		public Bitmap (int width, int height, PixelFormat format)
		{
			int bitsPerComponent, bytesPerRow;
			CGColorSpace colorSpace;
			CGBitmapFlags bitmapInfo;
			bool premultiplied = false;
			int bitsPerPixel = 0;

			// Don't forget to set the Image width and height for size.
			imageSize.Width = width;
			imageSize.Height = height;

			switch (format){
			case PixelFormat.Format32bppPArgb:
				premultiplied = true;
				colorSpace = CGColorSpace.CreateDeviceRGB ();
				bitsPerComponent = 8;
				bitsPerPixel = 32;
				bitmapInfo = CGBitmapFlags.PremultipliedFirst;
				break;
			case PixelFormat.Format32bppArgb:
				colorSpace = CGColorSpace.CreateDeviceRGB ();
				bitsPerComponent = 8;
				bitsPerPixel = 32;
				bitmapInfo = CGBitmapFlags.PremultipliedFirst;
				break;
			case PixelFormat.Format32bppRgb:
				colorSpace = CGColorSpace.CreateDeviceRGB ();
				bitsPerComponent = 8;
				bitsPerPixel = 32;
				bitmapInfo = CGBitmapFlags.None;
				break;
			default:
				throw new Exception ("Format not supported: " + format);
			}
			bytesPerRow = width * bitsPerPixel/bitsPerComponent;
			int size = bytesPerRow * height;

			bitmapBlock = Marshal.AllocHGlobal (size);
			var bitmap = new CGBitmapContext (bitmapBlock, 
			                              width, height, 
			                              bitsPerComponent, 
			                              bytesPerRow,
			                              colorSpace,
			                              CGImageAlphaInfo.PremultipliedLast);
			// This works for now but we need to look into initializing the memory area itself
			// TODO: Look at what we should do if the image does not have alpha channel
			bitmap.ClearRect (new RectangleF (0,0,width,height));

			var provider = new CGDataProvider (bitmapBlock, size, true);
			NativeCGImage = new CGImage (width, height, bitsPerComponent, bitsPerPixel, bytesPerRow, colorSpace, bitmapInfo, provider, null, false, CGColorRenderingIntent.Default);

		}

		private void InitWithCGImage (CGImage image)
		{
			int	width, height;
			CGBitmapContext bitmap = null;
			bool hasAlpha;
			CGImageAlphaInfo info;

			int bitsPerComponent, bytesPerRow;
			CGBitmapFlags bitmapInfo;
			bool premultiplied = false;
			int bitsPerPixel = 0;

			
			if (image == null) {
				throw new ArgumentException (" image is invalid! " );
			}
			
			info = image.AlphaInfo;
			hasAlpha = ((info == CGImageAlphaInfo.PremultipliedLast) || (info == CGImageAlphaInfo.PremultipliedFirst) || (info == CGImageAlphaInfo.Last) || (info == CGImageAlphaInfo.First) ? true : false);
			
			imageSize.Width = image.Width;
			imageSize.Height = image.Height;

			width = image.Width;
			height = image.Height;

			bitmapInfo = image.BitmapInfo;
			bitsPerComponent = image.BitsPerComponent;
			bitsPerPixel = image.BitsPerPixel;
			bytesPerRow = width * bitsPerPixel/bitsPerComponent;
			int size = bytesPerRow * height;

			bitmapBlock = Marshal.AllocHGlobal (size);
			bitmap = new CGBitmapContext (bitmapBlock, 
			                              image.Width, image.Width, 
			                              bitsPerComponent, 
			                              bytesPerRow,
			                              image.ColorSpace,
			                              CGImageAlphaInfo.PremultipliedLast);
			//CGImageAlphaInfo.PremultipliedLast);

			bitmap.ClearRect (new RectangleF (0,0,width,height));

			// We need to flip the Y axis to go from right handed to lefted handed coordinate system
			var transform = new CGAffineTransform(1, 0, 0, -1, 0, image.Height);
			bitmap.ConcatCTM(transform);
			bitmap.DrawImage(new RectangleF (0, 0, image.Width, image.Height), image);

			var provider = new CGDataProvider (bitmapBlock, size, true);
			NativeCGImage = new CGImage (width, height, bitsPerComponent, 
			                             bitsPerPixel, bytesPerRow, image.ColorSpace,
			                             CGImageAlphaInfo.PremultipliedLast,
			                             //bitmapInfo, 
			                             provider, null, false, CGColorRenderingIntent.Default);

			bitmap.Dispose ();
		}

		/*
		  * perform an in-place swap from Quadrant 1 to Quadrant III format
		  * (upside-down PostScript/GL to right side up QD/CG raster format)
		  * We do this in-place, which requires more copying, but will touch
		  * only half the pages.  (Display grabs are BIG!)
		  *
		  * Pixel reformatting may optionally be done here if needed.
		  * 
		  * NOTE: Not used right now
		*/
		private void flipImageYAxis ()
		{
			
//			long top, bottom;
//			byte[] buffer;
//			long topP;
//			long bottomP;
//			long rowBytes;
//			
//			top = 0;
//			bottom = mHeight - 1;
//			rowBytes = mByteWidth;
//			buffer = new byte[rowBytes];
//			
//			while (top < bottom) {
//				topP = top * rowBytes;
//				bottomP = bottom * rowBytes;
//				
//				/*
//				 * Save and swap scanlines.
//				 *
//				 * This code does a simple in-place exchange with a temp buffer.
//				 * If you need to reformat the pixels, replace the first two Array.Copy
//				 * calls with your own custom pixel reformatter.
//				 */
//				Array.Copy (mData, topP, buffer, 0, rowBytes);
//				Array.Copy (mData, bottomP, mData, topP, rowBytes);
//				Array.Copy (buffer, 0, mData, bottomP, rowBytes);
//				
//				++top;
//				--bottom;
//				
//			}
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing){
				if (NativeCGImage != null){
					NativeCGImage.Dispose ();
					NativeCGImage = null;
				}
				//Marshal.FreeHGlobal (bitmapBlock);
				bitmapBlock = IntPtr.Zero;
				Console.WriteLine("Bitmap Dispose");
			}
			base.Dispose (disposing);
		}
		
		public Color GetPixel (int x, int y)
		{
			// TODO
			return Color.White;
		}
		
		public void SetResolution (float xDpi, float yDpi)
		{
			throw new NotImplementedException ();
		}
		
		public void Save (string path, ImageFormat format)
		{
			if (path == null)
				throw new ArgumentNullException ("path");
			
			if (NativeCGImage == null)
				throw new ObjectDisposedException ("cgimage");

			// With MonoTouch we can use UTType from CoreMobileServices but since
			// MonoMac does not have that yet (or at least can not find it) I will 
			// use the string version of those for now.  I did not want to add another
			// #if #else in here.


			// for now we will just default this to png
			var typeIdentifier = "public.png";

			// Get the correct type identifier
			if (format == ImageFormat.Bmp)
				typeIdentifier = "com.microsoft.bmp";
//			else if (format == ImageFormat.Emf)
//				typeIdentifier = "image/emf";
//			else if (format == ImageFormat.Exif)
//				typeIdentifier = "image/exif";
			else if (format == ImageFormat.Gif)
				typeIdentifier = "com.compuserve.gif";
			else if (format == ImageFormat.Icon)
				typeIdentifier = "com.microsoft.ico";
			else if (format == ImageFormat.Jpeg)
				typeIdentifier = "public.jpeg";
			else if (format == ImageFormat.Png)
				typeIdentifier = "public.png";
			else if (format == ImageFormat.Tiff)
				typeIdentifier = "public.tiff";
			else if (format == ImageFormat.Wmf)
				typeIdentifier = "com.adobe.pdf";

			// Not sure what this is yet
			else if (format == ImageFormat.MemoryBmp)
				throw new NotImplementedException("ImageFormat.MemoryBmp not supported");

			// Obtain a URL file path to be passed
			NSUrl url = NSUrl.FromFilename(path);

			// * NOTE * we only support one image for right now.

			// Create an image destination that saves into the path that is passed in
			CGImageDestination dest = CGImageDestination.FromUrl (url, typeIdentifier, imageCount, null); 

			// Add an image to the destination
			dest.AddImage(NativeCGImage, null);

			// Finish the export
			bool success = dest.Close ();
//                        if (success == false)
//                                Console.WriteLine("did not work");
//                        else
//                                Console.WriteLine("did work: " + path);
			dest.Dispose();
			dest = null;

		}



		public void Save (string path)
		{
			if (path == null)
				throw new ArgumentNullException ("path");
			var format = ImageFormat.Png;
			
			var p = path.LastIndexOf (".");
			if (p != -1 && p < path.Length){
				switch (path.Substring (p + 1)){
				case "png": break;
				case "jpg": format = ImageFormat.Jpeg; break;
				case "tiff": format = ImageFormat.Tiff; break;
				case "bmp": format = ImageFormat.Bmp; break;
				}
			}
			Save (path, format);
		}
		
		public BitmapData LockBits (RectangleF rect, ImageLockMode flags, PixelFormat format)
		{
			throw new NotImplementedException ();			
		}
		
		public void UnlockBits (BitmapData data)
		{
			throw new NotImplementedException ();
		}
		
	}
}
