using System.Collections;

using CoreGraphics;

namespace MTExample4_1 {
	public class DataSeries {
		ArrayList pointList;
		BarStyle barStyle;
		string seriesName = string.Empty;

		public DataSeries ()
		{
			barStyle = new BarStyle ();
			pointList = new ArrayList ();
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


