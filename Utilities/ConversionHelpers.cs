using System;
using System.Drawing;

#if MONOMAC
using MonoMac.CoreGraphics;
#else
using MonoTouch.CoreGraphics;
#endif

namespace System.Drawing
{
	internal static class ConversionHelpers
	{
		internal static float F_PI = (float)Math.PI;
		
		internal static CGColor ToCGColor (this Color color)
		{				
			return new CGColor(color.R / 255f, color.G/255f, color.B/255f, color.A/255f );
		}
		
		internal static float ToRadians (this float degrees) 
		{
			return degrees * F_PI / 180f;
		}
		
		internal static float DegreesToRadians (float degrees) 
		{
			return degrees * F_PI / 180f;
		}
		internal static float GraphicsUnitConversion (GraphicsUnit from, GraphicsUnit to, float dpi, float nSrc)
		{	
			float inchs = 0;
			
			switch (from) {
			case GraphicsUnit.Document:
				inchs = nSrc / 300.0f;
				break;
			case GraphicsUnit.Inch:
				inchs = nSrc;
				break;
			case GraphicsUnit.Millimeter:
				inchs = nSrc / 25.4f;
				break;
			case GraphicsUnit.Display:
				//if (type == gtPostScript) { /* Uses 1/100th on printers */
				//	inchs = nSrc / 100;
				//} else { /* Pixel for video display */
				inchs = nSrc / dpi;
				//}
				break;
			case GraphicsUnit.Pixel:
			case GraphicsUnit.World:
				inchs = nSrc / dpi;
				break;
			case GraphicsUnit.Point:
				inchs = nSrc / 72.0f;
				break;
				//			case GraphicsUnit.Display:
				//				if (type == gtPostScript) { /* Uses 1/100th on printers */
				//					inchs = nSrc / 72.0f;
				//				} else { /* Pixel for video display */
				//					inchs = nSrc / dpi;
				//				}
				//				break;
			default:
				return nSrc;
			}
			
			switch (to) {
			case GraphicsUnit.Document:
				return inchs * 300.0f;
			case GraphicsUnit.Inch:
				return inchs;
			case GraphicsUnit.Millimeter:
				return inchs * 25.4f;
			case GraphicsUnit.Display:
				//if (type == gtPostScript) { /* Uses 1/100th on printers */
				//	return inchs * 100;
				//} else { /* Pixel for video display */
				return inchs * dpi;
				//}
			case GraphicsUnit.Pixel:
			case GraphicsUnit.World:
				return inchs * dpi;
			case GraphicsUnit.Point:
				return inchs * 72.0f;
				//			case GraphicsUnit.Display:
				//				if (type == gtPostScript) { /* Uses 1/100th on printers */
				//					return inchs * 72.0f;
				//				} else { /* Pixel for video display */
				//					return inchs * dpi;
				//				}
			default:
				return nSrc;
			}
		}
	}
}

