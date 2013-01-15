
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;

namespace TextureBrushTest
{
	public partial class DrawingView : MonoMac.AppKit.NSView
	{

		#region Constructors
		
		// Called when created from unmanaged code
		public DrawingView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public DrawingView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
		}

		public DrawingView (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion

		Font Font = new System.Drawing.Font("Helvetica",12);

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			Graphics g = new Graphics();

			g.Clear(Color.Wheat);

			var mainBundle = NSBundle.MainBundle;
			var filePath = mainBundle.PathForResource("CocoaMono","png");

			var bitmap = Image.FromFile(filePath);

			filePath = mainBundle.PathForResource("HouseAndTree","gif");

			Image texture = new Bitmap(filePath);
			g.DrawImage(texture, new Point(20,20));

			var textureBrush = new TextureBrush(texture);
			Pen blackPen = new Pen(Color.Black);
			var rect = new RectangleF(20,100,200,200);

			textureBrush.WrapMode = WrapMode.TileFlipXY;

			g.FillRectangle(textureBrush, rect);
			g.DrawRectangle(blackPen, rect);


			g.Dispose();
			
		}
	}
}

