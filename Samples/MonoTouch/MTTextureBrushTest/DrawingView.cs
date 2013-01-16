using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace MTTextureBrushTest
{
	public class DrawingView : UIView {
		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			var bgc = Color.Wheat;
			BackgroundColor = UIColor.FromRGBA(bgc.R,bgc.G,bgc.B, bgc.A);
		}

		float Width = 0;

		public override void Draw (RectangleF rect)
		{
			Graphics g = new Graphics();
			
			var mainBundle = NSBundle.MainBundle;
			var filePath = mainBundle.PathForResource("CocoaMono","png");
			
			var bitmap = Image.FromFile(filePath);
			
			filePath = mainBundle.PathForResource("HouseAndTree","gif");
			
			Image texture = new Bitmap(filePath);
			g.DrawImage(texture, new Point(20,20));
			
			var textureBrush = new TextureBrush(texture);
			Pen blackPen = new Pen(Color.Black);
			var rect2 = new RectangleF(20,100,200,200);
			
			//textureBrush.WrapMode = WrapMode.TileFlipXY;
			
			g.FillRectangle(textureBrush, rect2);
			g.DrawRectangle(blackPen, rect2);
			
			var cocoaMonoTexture = new Bitmap(bitmap, texture.Size);
			
			g.DrawImage(cocoaMonoTexture, new Point(300,20));
			
			var textureBrush2 = new TextureBrush(cocoaMonoTexture);
			Pen bluePen = new Pen(Color.Blue);
			var rectt = new RectangleF(300,100,200,200);
			
			textureBrush2.WrapMode = WrapMode.TileFlipXY;
			
			g.FillRectangle(textureBrush2, rectt);
			g.DrawRectangle(bluePen, rectt);
			
			g.Dispose();
		}
		

	}
}