//
// System.Drawing.Icon.cs
//
// Authors:
//   Gary Barnett (gary.barnett.mono@gmail.com)
//   Dennis Hayes (dennish@Raytek.com)
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//   Sanjay Gupta (gsanjay@novell.com)
//   Peter Dennis Bartok (pbartok@novell.com)
//   Sebastien Pouliot  <sebastien@ximian.com>
//   Filip Navara <filip.navara@gmail.com>
//   Jiri Volejnik <aconcagua21@volny.cz>
//
// Copyright (C) 2002 Ximian, Inc. http://www.ximian.com
// Copyright (C) 2004-2008 Novell, Inc (http://www.novell.com)
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

using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using ImageIO;
using CoreGraphics;
using Foundation;

namespace System.Drawing
{
	[Serializable]	
	//[Editor ("System.Drawing.Design.IconEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (System.Drawing.Design.UITypeEditor))]
	[TypeConverter(typeof(IconConverter))]	
	public sealed class Icon : MarshalByRefObject, ISerializable, ICloneable, IDisposable
	{
		ReferencedImageData imageData;
		CGImage image;
		internal bool undisposable;

		public Icon (Icon original, int width, int height)
			: this (original, new Size (width, height))
		{
		}
		
		public Icon (Icon original, Size size)
		{
			if (original == null)
				throw new ArgumentException ("original");

			// The original icon was loaded from multi-image file.
			imageData = original.imageData.Acquire();
			if (!LoadImageWithSize(size))
				image = original.image.Clone();
		}
		
		public Icon (Stream stream) : this (stream, 32, 32) 
		{
		}
		
		public Icon (Stream stream, Size size) : this(stream, size.Width, size.Height)
		{
		}

		public Icon (Stream stream, int width, int height)
		{
			InitFromStreamWithSize (stream, width, height);
		}
		
		public Icon (string fileName)
		{
			using (FileStream fs = File.OpenRead (fileName)) {
				InitFromStreamWithSize (fs, 32, 32);
			}
		}
		
		public Icon (Type type, string resource)
		{
			if (resource == null)
				throw new ArgumentException ("resource");
			
			using (Stream s = type.Assembly.GetManifestResourceStream (type, resource)) {
				if (s == null) {
					string msg = Locale.GetText ("Resource '{0}' was not found.", resource);
					throw new FileNotFoundException (msg);
				}
				InitFromStreamWithSize (s, 32, 32);		// 32x32 is default
			}
		}
		
		private Icon (SerializationInfo info, StreamingContext context)
		{
			MemoryStream dataStream = null;
			int width=0;
			int height=0;
			foreach (SerializationEntry serEnum in info) {
				if (String.Compare(serEnum.Name, "IconData", true) == 0) {
					dataStream = new MemoryStream ((byte []) serEnum.Value);
				}
				if (String.Compare(serEnum.Name, "IconSize", true) == 0) {
					Size iconSize = (Size) serEnum.Value;
					width = iconSize.Width;
					height = iconSize.Height;
				}
			}
			if (dataStream != null) {
				dataStream.Seek (0, SeekOrigin.Begin);
				InitFromStreamWithSize (dataStream, width, height);
			}
		}

		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			MemoryStream ms = new MemoryStream ();
			Save (ms);
			si.AddValue ("IconSize", Size, typeof (Size));
			si.AddValue ("IconData", ms.ToArray ());
		}
		
		public Icon (string fileName, int width, int height)
		{
			using (FileStream fs = File.OpenRead (fileName)) {
				InitFromStreamWithSize (fs, width, height);
			}
		}
		
		public Icon (string fileName, Size size)
		{
			using (FileStream fs = File.OpenRead (fileName)) {
				InitFromStreamWithSize (fs, size.Width, size.Height);
			}
		}
		
		public void Dispose ()
		{
			// SystemIcons requires this
			if (undisposable)
				return;

			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (image != null){
				image.Dispose();
				image = null;
			}
			if (imageData != null) {
				imageData.Release();
				imageData = null;
			}
		}

		public object Clone ()
		{
			return new Icon (this, Size);
		}
		
		public static Icon FromHandle (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentException (nameof(handle));
			throw new NotImplementedException();
		}


		public void Save (Stream outputStream)
		{
			if (outputStream == null)
				throw new NullReferenceException (nameof (outputStream));

			using (var stream = imageData.Data.AsStream())
				stream.CopyTo(outputStream);
		}
			
		public Bitmap ToBitmap ()
		{
			return new Bitmap(image);
		}

		public override string ToString ()
		{
			//is this correct, this is what returned by .Net
			return "<Icon>";			
		}
		
		[Browsable (false)]
		public IntPtr Handle {
			get {
				return IntPtr.Zero;
			}
		}

		[Browsable (false)]
		public int Height {
			get {
				return (int)image.Height;
			}
		}
		
		public Size Size {
			get {
				return new Size(Width, Height);
			}
		}
		
		[Browsable (false)]
		public int Width {
			get {
				return (int)image.Width;
			}
		}
		
		~Icon ()
		{
			Dispose (false);
		}
		
		private void InitFromStreamWithSize (Stream stream, int width, int height)
		{
			var data = NSData.FromStream(stream);
			imageData = new ReferencedImageData(data, CGImageSource.FromData(data));
			if (!LoadImageWithSize(new Size(width, height)))
				throw new ArgumentOutOfRangeException(nameof(stream));
		}

		private bool LoadImageWithSize(Size size)
		{
			var imageSource = imageData.ImageSource;
			int bestIndex = (int)imageSource.ImageCount - 1;
			Size bestSize = Size.Empty;
			for (int imageIndex = 0; imageIndex < imageSource.ImageCount; imageIndex++)
			{
				var properties = imageSource.GetProperties(imageIndex);
				if (properties.PixelWidth.Value <= size.Width && properties.PixelHeight.Value <= size.Height)
				{
					if (properties.PixelWidth.Value > bestSize.Width || properties.PixelHeight.Value > bestSize.Height)
					{
						bestSize = new Size(properties.PixelWidth.Value, properties.PixelHeight.Value);
						bestIndex = imageIndex;
					}
				}
				if (bestSize == size)
					break;
			}
			if (bestIndex >= imageSource.ImageCount)
				return false;
			image = imageSource.CreateImage(bestIndex, null);
			return image != null;
		}

		public static Icon ExtractAssociatedIcon(string filePath)
		{
			return SystemIcons.WinLogo;
		}

		class ReferencedImageData
		{
			public NSData Data { get; private set; }
			public CGImageSource ImageSource { get; private set; }
			int referenceCount;

			public ReferencedImageData(NSData data, CGImageSource imageSource)
			{
				Data = data;
				ImageSource = imageSource;
				referenceCount = 1;
			}

			public void Release()
			{
				if (--referenceCount == 0) {
					if (ImageSource != null) {
						ImageSource.Dispose();
						ImageSource = null;
					}
					if (Data != null) {
						Data.Dispose();
						Data = null;
					}
				}
			}

			public ReferencedImageData Acquire()
			{
				referenceCount++;
				return this;
			}
		}
	}
}
