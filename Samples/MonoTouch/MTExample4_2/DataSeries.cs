using System.Collections;
using CoreGraphics;

namespace MTExample4_2 {
	public class DataSeries {
		ArrayList pointList;
		BarStyle barStyle;
		string seriesName = string.Empty;
		bool isColorMap = false;
		bool isSingleColorMap = false;
		int[,] cmap;

		public DataSeries()
		{
			barStyle = new BarStyle ();
			pointList = new ArrayList ();
		}

		public int[,] CMap {
			get {
				return cmap;
			}
			set {
				cmap = value;
			}
		}

		public bool IsColorMap {
			get {
				return isColorMap;
			}
			set {
				isColorMap = value;
			}
		}

		public bool IsSingleColorMap {
			get {
				return isSingleColorMap;
			}
			set {
				isSingleColorMap = value;
			}
		}

		public BarStyle BarStyle {
			get {
				return barStyle;
			}
			set {
				barStyle = value;
			}
		}

		public ArrayList PointList {
			get {
				return pointList;
			}
			set {
				pointList = value;
			}
		}

		public void AddPoint (CGPoint pt)
		{
			pointList.Add (pt);
		}

		public string SeriesName {
			get {
				return seriesName;
			}
			set {
				seriesName = value;
			}
		}
	}
}


