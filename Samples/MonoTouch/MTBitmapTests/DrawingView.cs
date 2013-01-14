using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace MTBitmapTests
{
	public class DrawingView : UIView {
		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			var color = Color.Wheat;
			var wheat = new UIColor(color.R / 255f, color.G/255f, color.B/255f, color.A/255f );
			BackgroundColor = wheat;
		}

		public override void Draw (RectangleF rect)
		{
			Graphics g = new Graphics();
			
			g.Clear(Color.Wheat);
			
			var mainBundle = NSBundle.MainBundle;
			var filePath = mainBundle.PathForResource("CocoaMono","png");
			
			var bitmap = Image.FromFile(filePath);
			
			filePath = mainBundle.PathForResource("tiger-Q300","png");
			
			var tiger = Image.FromFile(filePath);
			
			var ig = Graphics.FromImage(bitmap);
			//ig.Clear(Color.Red);
			var pen = new Pen(Brushes.Yellow,20);
			var rec = new SizeF(200,200);
			var recp = new PointF(bitmap.Width - rec.Width - pen.Width / 2, bitmap.Height - rec.Height - pen.Width / 2);
			ig.DrawEllipse(pen, new RectangleF(recp, rec));
			
			g.DrawImage(bitmap, new Point(50,50));
			g.DrawImage(tiger, new Point(200,200));

			// To test Save uncomment the following lines
//			var destopDirectory = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
//			var finalPath = System.IO.Path.Combine (destopDirectory, "cocoa-mono-withcircle.jpg");
//
//			bitmap.Save(finalPath);
			

			using (SolidBrush brush = new SolidBrush(BACKCOLOR))
			{
				Image pic = GetCircleImage(); //get circle image
				Size newSize = new Size(pic.Size.Width * _scale, pic.Size.Height * _scale);//calculate new size of circle
				g.FillEllipse(brush, new Rectangle(_circleLocation, newSize));//draw the shape background
				g.DrawImage(pic, new Rectangle(_circleLocation, newSize));//draw the hatch style
			}
			
			g.Dispose();
			
		}
		
		/// <summary>
		/// Get the initial image
		/// </summary>
		/// <returns></returns>
		private Image GetCircleImage()
		{
			if (_circleImage == null)
			{
				//draw the initial image programmatically
				_circleImage = new Bitmap(WIDTH, HEIGHT);
				Graphics g = Graphics.FromImage(_circleImage);
				
				//draw the shape hatch style, not draw backgound
				using (HatchBrush brush = new HatchBrush(HatchStyle.Wave, FORECOLOR, Color.Transparent))
				{
					g.FillEllipse(brush, new Rectangle(Point.Empty, new Size(WIDTH, HEIGHT)));
				}
				g.Dispose();
			}
			
			return _circleImage;
		}
		
		private Image _circleImage = null;
		private Point _circleLocation = new Point(0, 0);
		private int _scale = 4;
		
		private const int HEIGHT = 100;
		private const int WIDTH = 100;
		
		private Color BACKCOLOR = Color.LightCoral;
		private Color FORECOLOR = Color.Blue;		


	}
}