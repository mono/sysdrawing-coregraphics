using System.Collections;
using CoreGraphics;

namespace MTExample3_5 {
	public class DataSeries {
		ArrayList pointList;
		LineStyle lineStyle;
		SymbolStyle symbolStyle;
		string seriesName = string.Empty;

		public DataSeries ()
		{
			lineStyle = new LineStyle ();
			SymbolStyle = new SymbolStyle ();
			pointList = new ArrayList ();
		}

		public LineStyle LineStyle {
			get {
				return lineStyle;
			}
			set {
				lineStyle = value;
			}
		}

		public SymbolStyle SymbolStyle {
			get {
				return symbolStyle;
			}
			set {
				symbolStyle = value;
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


