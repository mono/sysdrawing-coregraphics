using System;
using System.Collections;
using System.Drawing;

using CoreGraphics;

namespace MTExample4_6 {
	public class DataSeries {
		ArrayList dataList;
		ArrayList labelList;
		ArrayList explodeList;
		int[,] cmap;
		Color borderColor = Color.Black;
		float borderThickness = 1f;

		public DataSeries ()
		{
			dataList = new ArrayList ();
			labelList = new ArrayList ();
			explodeList = new ArrayList ();
		}

		public int[,] CMap {
			get {
				return cmap;
			}
			set {
				cmap = value;
			}
		}

		public Color BorderColor {
			get {
				return borderColor;
			}
			set {
				borderColor = value;
			}
		}

		public float BorderThickness {
			get {
				return borderThickness;
			}
			set {
				borderThickness = value;
			}
		}

		public ArrayList DataList {
			get {
				return dataList;
			}
			set {
				dataList = value;
			}
		}

		public ArrayList LabelList {
			get {
				return labelList;
			}
			set {
				labelList = value;
			}
		}

		public ArrayList ExplodeList {
			get {
				return explodeList;
			}
			set {
				explodeList = value;
			}
		}

		public void AddData (float data)
		{
			dataList.Add (data);
			labelList.Add (data.ToString ());
			explodeList.Add (0);
		}

		public void AddPie (Graphics g, ChartStyle cs)
		{
			var aBrush = new SolidBrush (Color.Black);
			var aPen = new Pen(BorderColor);
			int nData = DataList.Count;
			float fSum = 0;
			for (int i = 0; i < nData; i++)
				fSum = fSum + (float)DataList[i];
			float startAngle = 0;
			float sweepAngle = 0;
			CGRect rect = cs.SetPieArea ();

			for (int i = 0; i < nData; i++) {
				Color fillColor = Color.FromArgb (CMap[i, 0], CMap[i, 1], CMap[i, 2], CMap[i, 3]);
				aBrush = new SolidBrush(fillColor);
				var explode = (int)ExplodeList[i];

				if (fSum < 1) {
					startAngle = startAngle + sweepAngle;
					sweepAngle = 360 * (float)DataList[i];
				} else if (fSum >= 1) {
					startAngle = startAngle + sweepAngle;
					sweepAngle = 360 * (float)DataList[i] / fSum;
				}

				var xshift = (int)(explode * Math.Cos((startAngle + sweepAngle / 2) * Math.PI / 180));
				var yshift = (int)(explode * Math.Sin((startAngle + sweepAngle / 2) * Math.PI / 180));
				var rect1 = new CGRect (rect.X + xshift, rect.Y + yshift, rect.Width, rect.Height);
				g.FillPie (aBrush, (Rectangle)rect1, startAngle, sweepAngle);
				g.DrawPie (aPen, (Rectangle)rect1, startAngle, sweepAngle);
			}
		}
	}
}
