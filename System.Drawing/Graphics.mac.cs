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
using AppKit;
using CoreGraphics;

namespace System.Drawing
{
	public partial class Graphics
	{
		NSView focusedView;

		Graphics () : this (NSGraphicsContext.CurrentContext)
		{ 
		}

		Graphics (NSGraphicsContext context)  
		{
			var gc = context;

			if (gc.IsFlipped)
				gc = NSGraphicsContext.FromGraphicsPort (gc.GraphicsPort, false);

			screenScale = 1;
			nativeObject = gc;
			isFlipped = gc.IsFlipped;
			InitializeContext (gc.GraphicsPort);

		}

		public static Graphics FromContext (NSGraphicsContext context)
		{
			return new Graphics (context);
		}

		public static Graphics FromHwnd (IntPtr hwnd)
		{
		        if (hwnd == IntPtr.Zero)
		                return Graphics.FromImage(new Bitmap (1, 1));
		
		        Graphics g;
		        NSView view = (NSView)ObjCRuntime.Runtime.GetNSObject (hwnd);

		        if (NSView.FocusView () != view && view.LockFocusIfCanDraw())
		                g = new Graphics (view.Window.GraphicsContext) { focusedView = view };
		        else if (view.Window != null && view.Window.GraphicsContext != null)
		                g = new Graphics (view.Window.GraphicsContext);
		        else 
		                g = Graphics.FromImage(new Bitmap (1, 1));
		
		        return g;
		}

		void PlatformDispose ()
		{
			if (focusedView != null) {
				focusedView.UnlockFocus ();
				focusedView = null;
			}
		}
	}
}
