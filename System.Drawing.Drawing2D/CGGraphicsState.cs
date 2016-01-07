// 
// CGGraphicsState.cs: Stores the internal state of our Graphics
//
// Authors:
//      Kenneth J. Pouncey ( kjpou@pt.lu )
//
//     
// Copyright 2011-2013 Xamarin Inc.
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
using System.Drawing;

using System.Drawing.Text;

namespace System.Drawing.Drawing2D
{
	internal class CGGraphicsState {

		// TODO: set the rest of the states
		// These are just off the top of my head for right now as am sure there are
		// many more
		internal Pen lastPen { get; set; }
		internal Brush lastBrush { get; set; }
		internal Matrix model { get; set; }
		internal Matrix view { get; set; }
        internal CompositingQuality compositingQuality { get; set; }
        internal CompositingMode compositingMode { get; set; }
        internal InterpolationMode interpolationMode { get; set; }
        internal float pageScale { get; set; }
        internal GraphicsUnit pageUnit {get;set;}
        internal PixelOffsetMode pixelOffsetMode { get; set; }
		internal PointF renderingOrigin { get; set; }
		internal SmoothingMode smoothingMode { get; set; }
		internal Region clipRegion { get; set; }
        internal int textContrast { get; set; }
        internal TextRenderingHint textRenderingHint { get; set; }

	}

}

