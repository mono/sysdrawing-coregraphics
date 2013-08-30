//
// System.Drawing.Image.cs
//
// Authors: 	Christian Meyer (Christian.Meyer@cs.tum.edu)
// 		Alexandre Pigolkine (pigolkine@gmx.de)
//		Jordi Mas i Hernandez (jordi@ximian.com)
//		Sanjay Gupta (gsanjay@novell.com)
//		Ravindra (rkumar@novell.com)
//		Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright (C) 2002 Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004, 2007 Novell, Inc (http://www.novell.com)
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

using System;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

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
	[TypeConverter (typeof (ImageConverter))]
	public abstract class Image : MarshalByRefObject, IDisposable , ICloneable, ISerializable {

		// This is obtained from a Bitmap
		// Right now that is all we support
		internal CGImage NativeCGImage;

		// This is obtained from a PDF file.  Not supported right now.
		internal CGPDFDocument nativeMetafile;
		string tag = string.Empty;

		protected PixelFormat pixelFormat;

		// From microsoft documentation an image can also be described by a metafile which in
		// Quartz2D is a PDF file.  Quartz2D for Mac OSX Developers provides more information
		// on that but for right now only Bitmap will be supported.
		internal enum ImageClass 
		{
			Bitmap,		// Concrete Pixel based class of this abstract class
			PDFDocument	// Concrete PDF representation based class of this abstract class
		}

		internal ImageClass Implementaion { get; set; }


		~Image ()
		{
			Dispose (false);
		}
		
		[DefaultValue (false)]
		[Browsable (false)]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
		public int Height {
			get {
				var b = this as Bitmap;
				return b == null ? 0 : b.NativeCGImage.Height;
				return 0;
			}
		}
		
		public PixelFormat PixelFormat {
			get {			
				return pixelFormat;
			}

			protected set 
			{
				pixelFormat = value;
			}
		}
		
		public ImageFormat RawFormat {
			get {

				// TODO
				return new ImageFormat (new Guid ());			
			}
		}
		
		[DefaultValue (false)]
		[Browsable (false)]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
		public int Width {
			get {
				var b = this as Bitmap;
				return b == null ? 0 : b.NativeCGImage.Width;
			}
		}

		protected float dpiWidth = 0;
		protected float dpiHeight = 0;

		/// <summary>
		/// Gets the horizontal resolution, in pixels per inch, of this Image.
		/// </summary>
		/// <value>The horizontal resolution.</value>
		public float HorizontalResolution 
		{ 
			get { return dpiWidth; }

		}

		/// <summary>
		/// Gets the vertical resolution, in pixels per inch, of this Image.
		/// </summary>
		/// <value>The vertical resolution.</value>
		public float VerticalResolution 
		{ 
			get { return dpiHeight; }
		}

		protected Size imageSize = Size.Empty;
		public Size Size 
		{ 
			get { 
				return imageSize;
			}
		}

		protected SizeF physicalSize = SizeF.Empty;
		/// <summary>
		/// Gets the width and height of this image.
		/// </summary>
		/// <value>A SizeF structure that represents the width and height of this Image.</value>
		public SizeF PhysicalSize
		{
			get { return physicalSize; }
		}


		/// <summary>
		/// Creates an exact copy of this Image.
		/// </summary>
		public object Clone ()
		{
			var bitmap = new Bitmap (this);
			return bitmap;
		}

		/// <summary>
		/// Creates a copy of the section of this Bitmap defined by Rectangle structure and with a specified PixelFormat enumeration.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="pixelFormat">Pixel format.</param>
		public object Clone (Rectangle rect, PixelFormat pixelFormat)
		{
			if (rect.Width == 0 || rect.Height == 0)
				throw new ArgumentException ("Width or Height of rect is 0.");

			var width = rect.Width;
			var height = rect.Height;

			var tmpImg = new Bitmap (width, height, pixelFormat);
			using (Graphics g = Graphics.FromImage (tmpImg)) {
				g.DrawImage (this, new Rectangle(0,0, width, height), rect, GraphicsUnit.Pixel );
			}
			return tmpImg;
		}


		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
			Console.WriteLine("Image Dispose");
		}

		protected virtual void Dispose (bool disposing)
		{
			// TODO
		}
		
		public static Image FromStream (Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException ("stream");
			throw new NotImplementedException ();
			return null;
		}

		public void Save (Stream stream, ImageFormat format)
		{
			throw new NotImplementedException ();
		}
		
		public void Save (Stream stream)
		{
			throw new NotImplementedException ();
		}
		
		public void Save (string filename)
		{
			var b = this as Bitmap;
			if (b != null)
				b.Save(filename);
		}

		public static Bitmap FromFile (string filename)
		{
			return new Bitmap(filename);
		}
		
		void ISerializable.GetObjectData (SerializationInfo si, StreamingContext context)
		{
			using (MemoryStream ms = new MemoryStream ()) {
				// Icon is a decoder-only codec
				if (RawFormat.Equals (ImageFormat.Icon)) {
					Save (ms, ImageFormat.Png);
				} else {
					Save (ms, RawFormat);
				}
				si.AddValue ("Data", ms.ToArray ());
			}
		}

		[TypeConverterAttribute(typeof(StringConverter))]
		//[BindableAttribute(true)]
		public Object Tag { 
			get { 
				return tag;
			}
				
			set{
				tag = value.ToString();
			}
		}
	}
}
