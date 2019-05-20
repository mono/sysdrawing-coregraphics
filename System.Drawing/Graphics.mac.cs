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
using CoreImage;
using ObjCRuntime;

namespace System.Drawing
{
	public partial class Graphics
	{
		NSView focusedView;
		internal object nativeObject;

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

		public static Graphics FromHwnd (IntPtr hwnd, bool client)
		{
			if (hwnd == IntPtr.Zero) {
				var scaleFactor = NSScreen.MainScreen.BackingScaleFactor;
				var context = new CGBitmapContext (IntPtr.Zero, 1, 1, 8, 4, CGColorSpace.CreateDeviceRGB (), CGImageAlphaInfo.PremultipliedFirst);
				context.ScaleCTM (scaleFactor, -scaleFactor);
				return new Graphics (context);
			}

			Graphics g;
			var obj = ObjCRuntime.Runtime.GetNSObject (hwnd);
			var view = obj as NSView;
			if (view == null && obj is NSWindow && ((NSWindow)obj).GraphicsContext != null) {
				g = new Graphics (((NSWindow)obj).GraphicsContext);
			} else if (NSView.FocusView () == view) {
				if (NSGraphicsContext.CurrentContext == null)
					return FromHwnd (IntPtr.Zero, false);
				g = Graphics.FromCurrentContext ();
			} else if (view.LockFocusIfCanDraw ()) {
				if (NSGraphicsContext.CurrentContext == null)
					return FromHwnd (IntPtr.Zero, false);
				g = Graphics.FromCurrentContext ();
				g.focusedView = view;
			} else if (view.Window != null && view.Window.GraphicsContext != null) {
				g = new Graphics (view.Window.GraphicsContext);
			} else {
				return Graphics.FromImage (new Bitmap (1, 1));
			}

			if (client) {
				if (view is IClientView clientView) {
					var clientBounds = clientView.ClientBounds;
					g.context.ClipToRect (clientBounds);
					g.context.TranslateCTM (clientBounds.Left, clientBounds.Top);
					g.context.SaveState ();
					g.hasClientTransform = true;
				}
			}

			g.screenScaleTexture = g.context.GetCTM().xx; // view.Layer?.ContentsScale ?? 1f;

			return g;
		}

		public static Graphics FromHwnd (IntPtr hwnd)
		{
			return FromHwnd (hwnd, true);
		}

		void PlatformDispose ()
		{
			if (focusedView != null) {
				focusedView.UnlockFocus ();
				focusedView = null;
			}
		}

		void InitializeImagingContext ()
		{
			if (ciContext == null)
				ciContext = CIContext.FromContext (context);
		}

	}
}
