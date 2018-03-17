using CoreGraphics;
using System;
using AppKit;
using System.Drawing.Mac;

namespace System.Drawing.Printing
{
	public partial class PageSettings
	{
		internal NSPrintInfo print_info;

		public PageSettings (PrinterSettings printerSettings)
		{
			this.PrinterSettings = printerSettings;
			print_info = new NSPrintInfo (NSPrintInfo.SharedPrintInfo.Dictionary);
			print_info.Printer = printerSettings.printer;
			paper_size = new PaperSize (print_info.PaperName, (int)print_info.PaperSize.Width, (int)print_info.PaperSize.Height);
		}

		PageSettings (PageSettings pageSettings)
		{
			this.PrinterSettings = pageSettings.PrinterSettings;
			print_info = new NSPrintInfo (pageSettings.print_info.Dictionary);
			print_info.Printer = PrinterSettings.printer;
			paper_size = pageSettings.PaperSize;
		}

		public Rectangle Bounds => print_info.ImageablePageBounds.ToRectangle ();

		public bool Landscape {
			get {
				return print_info.Orientation == NSPrintingOrientation.Landscape;
			}
			set {
				print_info.Orientation = value ? NSPrintingOrientation.Landscape : NSPrintingOrientation.Portrait;
			}
		}

		public PaperSize PaperSize {
			get { return paper_size; }
			set {
				paper_size = value;
				print_info.PaperName = paper_size.PaperName;
				print_info.PaperSize = new CGSize (paper_size.Width, paper_size.Height);
			}
		}

		public Margins Margins {
			get {
				return new Margins ((int)print_info.LeftMargin, (int)print_info.RightMargin, (int)print_info.TopMargin, (int)print_info.BottomMargin);
			}
			set {
				print_info.LeftMargin = value.Left;
				print_info.RightMargin = value.Right;
				print_info.TopMargin = value.Top;
				print_info.BottomMargin = value.Bottom;
			}
		}

	}
}
