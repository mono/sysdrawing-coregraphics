using System.Drawing.Drawing2D;
using System.Drawing;

using CoreGraphics;
using Foundation;
using UIKit;

namespace MTBitmapTests {
	public class DrawingView : UIView {
		const int SCALE = 4;
		const int HEIGHT = 100;
		const int WIDTH = 100;

		Image circleImage = null;
		CGPoint circleLocation = new CGPoint(0, 0);

		Color BACKCOLOR = Color.LightCoral;
		Color FORECOLOR = Color.Blue;

		public DrawingView (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			var color = Color.Wheat;
			var wheat = new UIColor (color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
			BackgroundColor = wheat;
		}

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext ();

			g.Clear (Color.Wheat);

			var mainBundle = NSBundle.MainBundle;
			var filePath = mainBundle.PathForResource ("CocoaMono", "png");

			var bitmap = Image.FromFile (filePath);

			filePath = mainBundle.PathForResource ("tiger-Q300", "png");

			var tiger = Image.FromFile (filePath);

			using (var ig = Graphics.FromImage (bitmap)) {
				var pen = new Pen (Brushes.Yellow, 20);
				var rec = new CGSize (200, 200);
				var recp = new CGPoint (bitmap.Width - rec.Width - pen.Width / 2, bitmap.Height - rec.Height - pen.Width / 2);
				ig.DrawEllipse (pen, (RectangleF)new CGRect(recp, rec));
			}

			g.DrawImage (bitmap, 50f, 50f);
			g.DrawImage (tiger, 200f, 200f);

			using (var brush = new SolidBrush (BACKCOLOR)) {
				Image pic = GetCircleImage (); //get circle image
				var newSize = new CGSize (pic.Size.Width * SCALE, pic.Size.Height * SCALE); //calculate new size of circle
				g.FillEllipse (brush, (RectangleF)new CGRect (circleLocation, newSize)); //draw the shape background
				g.DrawImage (pic, (RectangleF)new CGRect (circleLocation, newSize)); //draw the hatch style
			}

			g.Dispose ();

		}

		/// <summary>
		/// Get the initial image
		/// </summary>
		/// <returns></returns>
		Image GetCircleImage ()
		{
			if (circleImage == null) {
				//draw the initial image programmatically
				circleImage = new Bitmap (WIDTH, HEIGHT);
				Graphics g = Graphics.FromImage (circleImage);

				//draw the shape hatch style, not draw backgound
				using (HatchBrush brush = new HatchBrush (HatchStyle.Wave, FORECOLOR, Color.Transparent))
					g.FillEllipse (brush, 0, 0, WIDTH, HEIGHT);

				g.Dispose ();
			}

			return circleImage;
		}
	}
}
