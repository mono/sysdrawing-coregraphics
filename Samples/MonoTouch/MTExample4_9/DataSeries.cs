namespace MTExample4_9 {
	public class DataSeries {
		string[,] dataString;
		LineStyle lineStyle;
		string seriesName = string.Empty;

		public DataSeries ()
		{
			lineStyle = new LineStyle ();
		}

		public LineStyle LineStyle {
			get {
				return lineStyle;
			}
			set {
				lineStyle = value;
			}
		}

		public string[,] DataString {
			get {
				return dataString;
			}
			set {
				dataString = value;
			}
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


