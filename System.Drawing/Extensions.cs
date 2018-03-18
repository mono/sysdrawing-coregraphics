//
// Authors:
//   Jiri Volejnik <aconcagua21@volny.cz>
//   Miguel de Icaza <miguel@microsoft.com>
//   Filip Navara <filip.navara@gmail.com>
//
using System.Text;

using CoreGraphics;
using CoreText;
using Foundation;

namespace System.Drawing.Mac
{
	public static partial class Extensions
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

		public static NSData ToNSData (this Image image, Imaging.ImageFormat format)
		{
			using (var stream = new IO.MemoryStream ()) {
				image.Save (stream, format);
				return NSData.FromArray (stream.ToArray ());
			}
		}

		public static Bitmap ToBitmap (this CGImage cgImage)
		{
			return new Bitmap (cgImage);
		}		
	}
}
