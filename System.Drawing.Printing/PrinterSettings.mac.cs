//
// System.Drawing.PrinterSettings.cs
//
// Authors:
//   Dennis Hayes (dennish@Raytek.com)
//   Herve Poussineau (hpoussineau@fr.st)
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//   Sebastien Pouliot  <sebastien@xamarin.com>
//   Filip Navara <filip.navara@gmail.com>
//   Jiri Volejnik <aconcagua21@volny.cz>
//
// (C) 2002 Ximian, Inc
// Copyright (C) 2004,2006 Novell, Inc (http://www.novell.com)
// Copyright 2011-2013 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using AppKit;

namespace System.Drawing.Printing
{
	public partial class PrinterSettings : ICloneable
	{
		internal NSPrinter printer;
		string printer_name;

		void InitPrinterSettings ()
		{
			printer = NSPrintInfo.DefaultPrinter;
			printer_name = printer?.Name;
		}

		public string PrinterName {
			get { return printer_name; }
			set {
				printer_name = value;
				printer = NSPrinter.PrinterWithName (value);
			}
		}
		public PrinterSettings.PaperSizeCollection PaperSizes {
			get {
				List<PaperSize> paper_sizes = new List<PaperSize> ();
				if (printer != null) {
					foreach (var paper_name in printer.StringListForKey ("PageSize", "PPD")) {
						var size = printer.PageSizeForPaper (paper_name);
						paper_sizes.Add (new PaperSize (paper_name, (int)size.Width, (int)size.Height));
					}
				}
				return new PaperSizeCollection (paper_sizes.ToArray ());
			}
		}
		public static PrinterSettings.StringCollection InstalledPrinters {
			get {
				return new StringCollection (NSPrinter.PrinterNames); 
			}
		}

		public bool IsValid {
			get { return printer != null; }
		}
	}
}