using System;
using CoreGraphics;
using System.Drawing.Drawing2D;
using UIKit;
using Foundation;
using System.Drawing;

namespace MTTextureBrushTest
{
	public class DrawingView : UIView {
		public DrawingView (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			var bgc = Color.Wheat;
			BackgroundColor = UIColor.FromRGBA(bgc.R,bgc.G,bgc.B, bgc.A);
		}

		float Width = 0;

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext();
			
			var mainBundle = NSBundle.MainBundle;
			var filePath = mainBundle.PathForResource("CocoaMono","png");
			
			var bitmap = Image.FromFile(filePath);
			
			filePath = mainBundle.PathForResource("HouseAndTree","gif");
			
			Image texture = new Bitmap(filePath);
			g.DrawImage(texture, new Point(20,20));
			
			var textureBrush = new TextureBrush(texture);
			Pen blackPen = new Pen(Color.Black);
			var rect2 = new Rectangle(20,100,200,200);
			
			//textureBrush.WrapMode = WrapMode.TileFlipXY;
			
			g.FillRectangle(textureBrush, rect2);
			g.DrawRectangle(blackPen, rect2.X, rect2.Y, rect2.Width, rect2.Height);
			
			var cocoaMonoTexture = new Bitmap(bitmap, texture.Size);
			
			g.DrawImage(cocoaMonoTexture, new Point(300,20));
			
			var textureBrush2 = new TextureBrush(cocoaMonoTexture);
			Pen bluePen = new Pen(Color.Blue);
			var rectt = new Rectangle(300,100,200,200);
			
			textureBrush2.WrapMode = WrapMode.TileFlipXY;
			
			g.FillRectangle(textureBrush2, rectt);
			g.DrawRectangle(bluePen, rectt.X, rectt.Y, rectt.Width, rectt.Height);

			// Scale the large image down to 75 x 75 before tiling
			TextureBrush tBrush = new TextureBrush(bitmap);
			tBrush.Transform = new Matrix(
				75.0f / bitmap.Width,
				0.0f,
				0.0f,
				75.0f / bitmap.Height,
				0.0f,
				0.0f);
			g.FillEllipse(tBrush, new Rectangle(300, 320, 150, 250));

			g.Dispose();
		}
		

	}
}
