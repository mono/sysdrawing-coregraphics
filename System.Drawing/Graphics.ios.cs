//
// System.Drawing.Graphics
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

using Foundation;
using System.Drawing.Mac;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;

namespace System.Drawing
{

	public partial class Graphics {
		private Graphics () :
			this (UIGraphics.GetCurrentContext (), UIScreen.MainScreen.Scale)
		{ }

		private Graphics (CGContext context) :
			this (context, UIScreen.MainScreen.Scale)
		{ }

		private Graphics (CGContext context, nfloat screenScale)
		{
			var gc = context;
			this.screenScale = (float)screenScale;
			InitializeContext (gc);
		}

		public static Graphics FromContext (CGContext context)
		{
			return new Graphics (context, UIScreen.MainScreen.Scale);
		}

		public static Graphics FromContext (CGContext context, float screenScale)
		{
			return new Graphics (context, screenScale);
		}

	}
}
