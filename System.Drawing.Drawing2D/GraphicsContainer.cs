using System;

#if MONOMAC
namespace System.DrawingNative.Drawing2D 
#else
namespace System.Drawing.Drawing2D 
#endif
{
	public sealed class GraphicsContainer :  MarshalByRefObject
	{
		public GraphicsContainer ()
		{
		}
	}
}

