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

using System.IO;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.ComponentModel;

#if MONOMAC
using MonoMac.CoreGraphics;
using MonoMac.Foundation;
using MonoMac.AppKit;
#else
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#endif

namespace System.Drawing {
	
	[Serializable]
	public sealed class Bitmap : Image {
		// if null, we created the bitmap in memory, otherwise, the backing file.
#if MONOMAC
		NSImage uiimage;
#else
		UIImage uiimage;
#endif
		internal CGImage NativeCGImage;
		internal IntPtr bitmapBlock;
		
		public Bitmap (string filename)
		{
			//
			// This is a quick hack: we should use ImageIO to load the data into
			// memory, so we always have the bitmapBlock availabe
			//
			//uiimage.AsCGImage()

#if MONOMAC
			var image = new NSImage(filename);
			NativeCGImage = image.AsCGImage(RectangleF.Empty, null, null);
#else
			var uiimage = UIImage.FromFileUncached (filename);
			NativeCGImage = uiimage.CGImage;
#endif
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

			var provider = new CGDataProvider (bitmapBlock, size, true);
			NativeCGImage = new CGImage (width, height, bitsPerComponent, bitsPerPixel, bytesPerRow, colorSpace, bitmapInfo, provider, null, false, CGColorRenderingIntent.Default);
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing){
				if (NativeCGImage != null){
					NativeCGImage.Dispose ();
					NativeCGImage = null;
				}
				bitmapBlock = IntPtr.Zero;
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

#if MONOMAC
			using (var uiimage = new NSImage (NativeCGImage, new SizeF(NativeCGImage.Width, NativeCGImage.Height))){
				NSError error;
				//  get an NSBitmapImageRep of the CGImage
				NSBitmapImageRep rep = new NSBitmapImageRep (NativeCGImage);

				if (format == ImageFormat.Jpeg){
					using (var data = rep.RepresentationUsingTypeProperties (NSBitmapImageFileType.Jpeg, new NSDictionary ())){
						if (data.Save (path, NSDataWritingOptions.Atomic, out error))
							return;
						
						throw new IOException ("Saving the file " + path + " " + error);
					}
				} else if (format == ImageFormat.Png){
					using (var data = rep.RepresentationUsingTypeProperties (NSBitmapImageFileType.Png, new NSDictionary ())){
						if (data.Save (path, NSDataWritingOptions.Atomic, out error))
							return;
						
						throw new IOException ("Saving the file " + path + " " + error);
					}
				} else
					throw new ArgumentException ("Unsupported format, only Jpeg and Png are supported", "format");
			}
#else
			using (var uiimage = new UIImage (NativeCGImage)){

				NSError error;
				
				if (format == ImageFormat.Jpeg){
					using (var data = uiimage.AsJPEG ()){
						if (data.Save (path, NSDataWritingOptions.Atomic, out error))
							return;
						
						throw new IOException ("Saving the file " + path + " " + error);
					}
				} else if (format == ImageFormat.Png){
					using (var data = uiimage.AsPNG ()){
						if (data.Save (path, NSDataWritingOptions.Atomic, out error))
							return;
						
						throw new IOException ("Saving the file " + path + " " + error);
					}
				} else
					throw new ArgumentException ("Unsupported format, only Jpeg and Png are supported", "format");
			}
#endif
			
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
