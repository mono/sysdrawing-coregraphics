
using System;
using System.Collections.Generic;
using System.Linq;
using System.DrawingNative.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;

using System.DrawingNative;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;

namespace Example1_9
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

		Font Font = new Font("Helvetica",8);

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			Graphics g = Graphics.FromCurrentContext();
			//g.Clear(Color.White);

			DrawObjects(g);
			g.Dispose();
		}
		
		private void DrawObjects(Graphics g)
		{
			ColorMap cm = new ColorMap();
			int[,] cmap = cm.Jet();
			int x0 = 10;
			int y0 = 10;
			int width = 85;
			int height = 85;
			PointC[,] pts = new PointC[3, 3];
			pts[0, 0] = new PointC(new PointF(x0, y0), 3);
			pts[0, 1] = new PointC(new PointF(x0 + width, y0), 0);
			pts[0, 2] = new PointC(new PointF(x0 + 2 * width, y0), 4);
			pts[1, 0] = new PointC(new PointF(x0, y0 + height), -2);
			pts[1, 1] = new PointC(new PointF(x0 + width, y0 + height), 3);
			pts[1, 2] = new PointC(new PointF(x0 + 2 * width, y0 + height), 1);
			pts[2, 0] = new PointC(new PointF(x0, y0 + 2* height), -1);
			pts[2, 1] = new PointC(new PointF(x0 + width, y0 + 2* height), 2);
			pts[2, 2] = new PointC(new PointF(x0 + 2 * width, y0 + 2 * height), -3);
			float cmin = -3;
			float cmax = 4;
			int colorLength = cmap.GetLength(0);
			
			// Original color map:
			for (int i = 0; i < 3; i++)
			{               
				for (int j = 0; j < 3; j++)
				{
					int cindex = (int)Math.Round((colorLength * (pts[i, j].C - cmin) +
					                              (cmax - pts[i, j].C)) / (cmax - cmin));
					if (cindex < 1)
						cindex = 1;
					if (cindex >  colorLength)
						cindex = colorLength;
					for (int k = 0; k < 4; k++)
					{
						pts[i, j].ARGBArray[k] = cmap[cindex - 1, k];
					}
				}
			}
			
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					SolidBrush aBrush = new SolidBrush(Color.FromArgb(pts[i, j].ARGBArray[0],
					                                                  pts[i, j].ARGBArray[1], pts[i, j].ARGBArray[2],pts[i, j].ARGBArray[3]));
					PointF[] pta = new PointF[4]{pts[i,j].pointf, pts[i+1,j].pointf,
						pts[i+1,j+1].pointf, pts[i,j+1].pointf};
					g.FillPolygon(aBrush, pta);
					aBrush.Dispose();
				}
			}
			g.DrawString("Direct Color Map", this.Font, Brushes.Black, 50, 190);   
			
			// Bilinear interpolation:
			x0 = x0 + 200;
			pts[0, 0] = new PointC(new PointF(x0, y0), 3);
			pts[0, 1] = new PointC(new PointF(x0 + width, y0), 0);
			pts[0, 2] = new PointC(new PointF(x0 + 2 * width, y0), 4);
			pts[1, 0] = new PointC(new PointF(x0, y0 + height), -2);
			pts[1, 1] = new PointC(new PointF(x0 + width, y0 + height), 3);
			pts[1, 2] = new PointC(new PointF(x0 + 2 * width, y0 + height), 1);
			pts[2, 0] = new PointC(new PointF(x0, y0 + 2 * height), -1);
			pts[2, 1] = new PointC(new PointF(x0 + width, y0 + 2 * height), 2);
			pts[2, 2] = new PointC(new PointF(x0 + 2 * width, y0 + 2 * height), -3);
			
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					PointF[] pta = new PointF[4]{pts[i,j].pointf, pts[i+1,j].pointf,
						pts[i+1,j+1].pointf, pts[i,j+1].pointf};
					float[] cdata = new float[4]{pts[i,j].C,pts[i+1,j].C,
						pts[i+1,j+1].C,pts[i,j+1].C};
					Interp(g, pta, cdata, 50);
				}
			}
			g.DrawString("Interpolated Color Map", this.Font, Brushes.Black, 240, 190);   
		}
		
		private void Interp(Graphics g, PointF[] pta, float[] cData, int npoints)
		{
			PointC[,] pts = new PointC[npoints + 1, npoints + 1];
			float x0 = pta[0].X;
			float x1 = pta[3].X;
			float y0 = pta[0].Y;
			float y1 = pta[1].Y;
			float dx = (x1 - x0) / npoints;
			float dy = (y1 - y0) / npoints;
			float C00 = cData[0];
			float C10 = cData[1];
			float C11 = cData[2];
			float C01 = cData[3];
			
			for (int i = 0; i <= npoints; i++)
			{
				float x = x0 + i * dx;
				for (int j = 0; j <= npoints; j++)
				{
					float y = y0 + j * dy;
					float C = (y1 - y) * ((x1 - x) * C00 + 
					                      (x - x0) * C10) / (x1 - x0) / (y1 - y0) +
						(y - y0) * ((x1 - x) * C01 + 
						            (x - x0) * C11) / (x1 - x0) / (y1 - y0);
					pts[j, i] = new PointC(new PointF(x, y),C);
				}
			}
			
			ColorMap cm = new ColorMap();
			int[,] cmap = cm.Jet();
			float cmin = -3;
			float cmax = 4;
			int colorLength = cmap.GetLength(0);
			for (int i = 0; i <= npoints; i++)
			{
				for (int j = 0; j <= npoints; j++)
				{
					int cindex = (int)Math.Round((colorLength * (pts[i, j].C - cmin) +
					                              (cmax - pts[i, j].C)) / (cmax - cmin));
					if (cindex < 1)
						cindex = 1;
					if (cindex > colorLength)
						cindex = colorLength;
					for (int k = 0; k < 4; k++)
					{
						pts[j, i].ARGBArray[k] = cmap[cindex - 1, k];
					}
				}
			}
			
			for (int i = 0; i < npoints; i++)
			{
				for (int j = 0; j < npoints; j++)
				{
					SolidBrush aBrush = new SolidBrush(Color.FromArgb(pts[i, j].ARGBArray[0],
					                                                  pts[i, j].ARGBArray[1], pts[i, j].ARGBArray[2], pts[i, j].ARGBArray[3]));
					PointF[] points = new PointF[4]{pts[i,j].pointf, pts[i+1,j].pointf,
						pts[i+1,j+1].pointf, pts[i,j+1].pointf};
					g.FillPolygon(aBrush, points);
					aBrush.Dispose();
				}
			}
		}
		
		//		public override bool IsFlipped {
		//			get {
		//				//return base.IsFlipped;
		//				return false;
		//			}
		//		}

	}
}

