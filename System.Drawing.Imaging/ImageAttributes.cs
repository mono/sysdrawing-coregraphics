using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging 
{

	[StructLayout(LayoutKind.Sequential)]
	public sealed class ImageAttributes : ICloneable, IDisposable 
	{
		internal ColorMatrix colorMatrix;
		internal ColorMatrixFlag colorMatrixFlags;
		internal ColorAdjustType colorAdjustType;

		/// <summary>
		/// Clears the color matrix.
		/// </summary>
		public void ClearColorMatrix()
		{
			colorMatrix = null;
			colorMatrixFlags = ColorMatrixFlag.Default;
			colorAdjustType = ColorAdjustType.Default;
		}

		/// <summary>
		/// Sets the color matrix with the ColorMatrixFlag.Default.
		/// </summary>
		/// <param name="newColorMatrix">New color matrix.</param>
		public void SetColorMatrix(ColorMatrix newColorMatrix)
		{
			SetColorMatrix (newColorMatrix, ColorMatrixFlag.Default);
		}

		/// <summary>
		/// Sets the color matrix with specifed flagsß.
		/// </summary>
		/// <param name="newColorMatrix">New color matrix.</param>
		/// <param name="flags">Flags.</param>
		public void SetColorMatrix(ColorMatrix newColorMatrix, ColorMatrixFlag flags)
		{
			SetColorMatrix (newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
		}

		public void SetColorMatrix(ColorMatrix newColorMatrix, ColorMatrixFlag mode, ColorAdjustType type)
		{
			colorMatrix = newColorMatrix;
			colorMatrixFlags = mode;
			colorAdjustType = type;

		}
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