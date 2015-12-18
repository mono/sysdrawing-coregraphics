using System;
using CoreGraphics;
using System.Drawing.Drawing2D;
using System.Drawing;

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

