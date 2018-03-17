using CoreGraphics;
using System;

namespace System.Drawing.Printing
{
	public partial class PageSettings
	{
		public PageSettings (PrinterSettings printerSettings)
		{
			this.PrinterSettings = printerSettings;
		}

		PageSettings (PageSettings pageSettings)
		{
			this.PrinterSettings = pageSettings.PrinterSettings;
		}

		public Rectangle Bounds => Rectangle.Empty;

		public bool Landscape { get; set; }
		public PaperSize PaperSize { get; set; }
		public Margins Margins { get; set; }

	}
}
