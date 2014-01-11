using System;
using System.DrawingNative;
using System.DrawingNative.Drawing2D;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;

namespace Example1_9
{
	public class PointC
	{
		public PointF pointf = new PointF();
		public float C = 0;
		public int[] ARGBArray = new int[4];
		
		public PointC()
		{
		}
		
		public PointC(PointF ptf, float c)
		{
			pointf = ptf;
			C = c;
		}
		
		public PointC(PointF ptf, float c, int[] argbArray)
		{
			pointf = ptf;
			C = c;
			ARGBArray = argbArray;
		}
	}
}

