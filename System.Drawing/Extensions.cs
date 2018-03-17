//
// Authors:
//   Jiri Volejnik <aconcagua21@volny.cz>
//   Miguel de Icaza <miguel@microsoft.com>
//   Filip Navara <filip.navara@gmail.com>
//
using System.Text;

#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using CoreGraphics;
using CoreText;
using Foundation;

namespace System.Drawing.Mac
{
	public static class Extensions
	{
		public static CGRect ToCGRect (this Rectangle r)
		{
			return new CGRect (r.X, r.Y, r.Width, r.Height);
		}

		public static CGRect ToCGRect (this RectangleF r)
		{
			return new CGRect (r.X, r.Y, r.Width, r.Height);
		}

		public static Rectangle ToRectangle (this CGRect r)
		{
			return new Rectangle ((int)Math.Round (r.X), (int)Math.Round (r.Y), (int)Math.Round (r.Width), (int)Math.Round (r.Height));
		}

		public static CGRect Inflate (this CGRect r, float w, float h)
		{
			return new CGRect (r.X - w, r.Y - h, r.Width + w + w, r.Height + h + h);
		}

		public static CGRect Move (this CGRect r, float dx, float dy)
		{
			return new CGRect (r.X + dx, r.Y + dy, r.Width, r.Height);
		}

		public static CGSize ToCGSize (this Size s)
		{
			return new CGSize (s.Width, s.Height);
		}

		public static Size ToSDSize (this CGSize s)
		{
			return new Size ((int)Math.Round (s.Width), (int)Math.Round (s.Height));
		}

		public static CGSize Inflate (this CGSize s, float w, float h)
		{
			return new CGSize (s.Width + w, s.Height + h);
		}

		public static CTFont ToCTFont (this Font f)
		{
			return f.nativeFont;
		}

#if MONOMAC
		public static NSFont ToNativeFont(this Font f)
		{
			return ObjCRuntime.Runtime.GetNSObject(f.nativeFont.Handle) as NSFont;
		}

		public static CTFont ToCTFont(this NSFont f)
		{
			// CTFont and NSFont are toll-free bridged
			return (CTFont)Activator.CreateInstance(
				typeof(CTFont),
				Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Instance,
				null,
				new object[] { f.Handle },
				null);
		}

		public static NSTextAlignment ToNSTextAlignment (this ContentAlignment a)
		{
			return (NSTextAlignment)ToCTTextAlignment (a);
		}
#else
		public static UIFont ToNativeFont (this Font f)
		{
			return ObjCRuntime.Runtime.GetNSObject (f.nativeFont.Handle) as UIFont;
		}

		public static CTFont ToCTFont (this UIFont f)
		{
			// CTFont and NSFont are toll-free bridged
			return (CTFont)Activator.CreateInstance (
				typeof (CTFont),
				Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Instance,
				null,
				new object [] { f.Handle },
				null);
		}
#endif


		public static CTTextAlignment ToCTTextAlignment (this ContentAlignment a)
		{
			switch (a) {
			default:
				return CTTextAlignment.Left;

			case ContentAlignment.TopLeft:
			case ContentAlignment.MiddleLeft:
			case ContentAlignment.BottomLeft:
				return CTTextAlignment.Left;

			case ContentAlignment.TopRight:
			case ContentAlignment.MiddleRight:
			case ContentAlignment.BottomRight:
				return CTTextAlignment.Right;

			case ContentAlignment.TopCenter:
			case ContentAlignment.MiddleCenter:
			case ContentAlignment.BottomCenter:
				return CTTextAlignment.Center;
			}
		}

		public static CGColor ToCGColor (this Color c)
		{
			return new CGColor (c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
		}

#if MONOMAC
		public static NSColor ToNativeColor (this Color c)
		{
			return NSColor.FromDeviceRgba(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
		}

		public static Color ToSDColor(this NSColor color)
		{
			var convertedColor = color.UsingColorSpace(NSColorSpace.GenericRGBColorSpace);
			if (convertedColor != null)
			{
				nfloat r, g, b, a;
				convertedColor.GetRgba(out r, out g, out b, out a);
				return Color.FromArgb((int)(a * 255), (int)(r * 255), (int)(g * 255), (int)(b * 255));
			}

			var cgColor = color.CGColor; // 10.8+
			if (cgColor != null)
			{
				if (cgColor.NumberOfComponents == 4)
					return Color.FromArgb(
						(int)(cgColor.Components[3] * 255),
						(int)(cgColor.Components[0] * 255),
						(int)(cgColor.Components[1] * 255),
						(int)(cgColor.Components[2] * 255));

				if (cgColor.NumberOfComponents == 2)
					return Color.FromArgb(
						(int)(cgColor.Components[1] * 255),
						(int)(cgColor.Components[0] * 255),
						(int)(cgColor.Components[0] * 255),
						(int)(cgColor.Components[0] * 255));
			}

			return Color.Transparent;
		}

		public static int ToArgb(this NSColor color)
		{
			return color.ToSDColor().ToArgb();
		}
	
		public static uint ToUArgb(this NSColor color)
		{
			return (uint)color.ToSDColor().ToArgb();
		}
#else
		public static UIColor ToNativeColor (this Color c)
		{
			return UIColor.FromRGBA(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
		}

		public static Color ToSDColor (this UIColor color)
		{
			throw new NotImplementedException ();
		}

		public static int ToArgb (this UIColor color)
		{
			return color.ToSDColor ().ToArgb ();
		}

		public static uint ToUArgb (this UIColor color)
		{
			return (uint)color.ToSDColor ().ToArgb ();
		}
#endif
		public static NSData ToNSData (this Image image, Imaging.ImageFormat format)
		{
			using (var stream = new IO.MemoryStream ()) {
				image.Save (stream, format);
				return NSData.FromArray (stream.ToArray ());
			}
		}
#if MONOMAC
		public static NSImage ToNSImage(this Image image)
		{
			if (image.NativeCGImage != null)
				return new NSImage(image.NativeCGImage, CGSize.Empty);

			return new NSImage(image.ToNSData(Imaging.ImageFormat.Png));
		}
#endif
		public static Bitmap ToBitmap (this CGImage cgImage)
		{
			return new Bitmap (cgImage);
		}

#if MONOMAC
		public static CGContext CGContext(this NSGraphicsContext context)
		{
			return context.CGContext;
		}
#endif
	}
}
