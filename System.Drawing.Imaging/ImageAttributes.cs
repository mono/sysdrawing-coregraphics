using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging {

        [StructLayout(LayoutKind.Sequential)]
        public sealed class ImageAttributes : ICloneable, IDisposable {
		#region ICloneable implementation
		public object Clone ()
		{
			throw new NotImplementedException ();
		}
		#endregion
		
		#region IDisposable implementation
		public void Dispose ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}