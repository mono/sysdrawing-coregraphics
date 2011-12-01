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

namespace System.Drawing {
	
	[Serializable]
	public sealed class Bitmap : Image {

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
			// TODO
		}
		
		public Color GetPixel (int x, int y)
		{
			// TODO
			return Color.White;
		}
		
		public void SetResolution (float xDpi, float yDpi)
		{
		}
	}
}
