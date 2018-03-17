//
// System.Drawing.SystemIcons.cs
//
// Authors:
//   Dennis Hayes (dennish@Raytek.com)
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//   Sebastien Pouliot  <sebastien@ximian.com>
//   Filip Navara <filip.navara@gmail.com>
//   	
// (C) 2002 Ximian, Inc
// Copyright (C) 2004, 2006 Novell, Inc (http://www.novell.com)
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
namespace System.Drawing
{

	public sealed partial class SystemIcons
	{
		static Icon FromResource (string name)
		{
			using (var stream = typeof (Icon).Assembly.GetManifestResourceStream (name))
				return new Icon (stream) { undisposable = true };			
		}

		SystemIcons ()
		{
		}

		// note: same as WinLogo (for Mono)
		public static Icon Application {
			get { return WinLogo; }
		}

		// note: same as Information
		public static Icon Asterisk {
			get { return Information; }
		}

		// note: same as Hand
		public static Icon Error {
			get;
			private set;
		}

		// same as Warning
		public static Icon Exclamation {
			get { return Warning; }
		}

		// note: same as Error
		public static Icon Hand {
			get { return Error; }
		}

		// note: same as Asterisk
		public static Icon Information {
			get;
			private set;
		}

		public static Icon Question {
			get;
			private set;
		}

		// note: same as Exclamation
		public static Icon Warning {
			get;
			private set;
		}

		// note: same as Application (for Mono)
		public static Icon WinLogo {
			get;
			private set;
		}

		public static Icon Shield {
			get;
			private set;
		}
	}
}
