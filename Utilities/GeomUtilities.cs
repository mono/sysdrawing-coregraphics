using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#if MONOMAC
using MonoMac.CoreGraphics;
#else
using MonoTouch.CoreGraphics;
#endif

namespace System.Drawing
{
	internal static class GeomUtilities
	{

		/*
		 * The angle of the orientation line determines which corners the starting and ending lines pass through. 
		 * For example, if the angle is between 0 and 90 degrees, the starting line passes through the upper-left corner, 
		 * and the ending line passes through the lower-right corner.
		 * 
		 * Mac NSGradient drawInRect angle Documentation:
		 * 
		 * https://developer.apple.com/library/mac/#documentation/Cocoa/Reference/NSGradient_class/Reference/Reference.html
		 * 
		 * Linear gradient starting points.
		 *   Mac osx coordinates
		 * 	Rotation angle		Start corner
		 * 	0-89 degrees		Lower-left
		 * 	90-179 degrees		Lower-right
		 * 	180-269 degrees		Upper-right
		 * 	270-359 degrees		Upper-left
		 * 
		 *  Think of a rectangle within a circle.  start from one of the quadrants above and find the point on the
		 *  circumference of the circle
		 * 
		 * http://msdn.microsoft.com/en-us/library/ms142563.aspx
		 * All points along any line perpendicular to the orientation line are the same color.
		 * The starting line is perpendicular to the orientation line and passes through one of the corners of 
		 * the rectangle. All points on the starting line are the starting color. Then ending line is perpendicular 
		 * to the orientation line and passes through one of the corners of the rectangle. All points on the 
		 * ending line are the ending color.
		 * 
		 */
		internal static void ComputeOrientationLine (RectangleF rect, float angle, out PointF start, out PointF end)
		{
			start = PointF.Empty;
			end = PointF.Empty;
			
			SizeF tanSize = SizeF.Empty;

			// Clamp to 360 degrees
			angle = angle % 360;

			// We start by breaking the angle up into quadrants
			// as per table in comments
			if (angle < 90) {
				start.X = rect.Location.X;
				start.Y = rect.Location.Y;
				tanSize.Width = rect.Size.Width;
				tanSize.Height = rect.Size.Height;
			} else if (angle < 180) {
				start.X = rect.Location.X + rect.Size.Width;
				start.Y = rect.Location.Y;
				
				tanSize.Width = -rect.Size.Width;
				tanSize.Height = rect.Size.Height;
			} else if (angle < 270) {
				start.X = rect.Location.X + rect.Size.Width;
				start.Y = rect.Location.Y + rect.Size.Height;
				
				tanSize.Width = -rect.Size.Width;
				tanSize.Height = -rect.Size.Height;
			} else {
				start.X = rect.Location.X;
				start.Y = rect.Location.Y + rect.Size.Height;
				
				tanSize.Width = rect.Size.Width;
				tanSize.Height = -rect.Size.Height;
			}
			
			
			float radAngle = angle.ToRadians ();
			// Distance formula to get the circle radius - http://en.wikipedia.org/wiki/Distance
			float radius = (float)Math.Sqrt (rect.Size.Width * rect.Size.Width + rect.Size.Height * rect.Size.Height);
			// get the slope angle
			float slopeAngle = (float)Math.Atan2 (tanSize.Height, tanSize.Width);
			//  Compute the distance
			float distance = (float)Math.Cos (slopeAngle - radAngle) * radius;
			
			// Parametric Equation for a circle
			// a = angle in radians
			// ( cos(a) * d + x, sin(a) * d + y)
			end.X = (float)Math.Cos (radAngle) * distance + start.X;
			end.Y = (float)Math.Sin (radAngle) * distance + start.Y;
			
		}

		/*
		 * Linear Interpoloation helper function
		 */
		internal static float Lerp (float value1, float value2, float amount)
		{
			return value1 + (value2 - value1) * amount;
		}

		/*
		 * Based on libgdiplus lineargradientbrush.c implementation with a couple of small modifications
		 * for calculating the Normal Distribution.  The reason for this change comes 
		 * from “The Art of Scientific Computing” by Press et al…
		 * —–
		 * We assume that you know enough never to evaluate a polynomial this way:
		 * p=c[0]+c[1]*x+c[2]*x*x+c[3]*x*x*x+c[4]*x*x*x*x;
		 * or (even worse!),
		 * p=c[0]+c[1]*x+c[2]*pow(x,2.0)+c[3]*pow(x,3.0)+c[4]*pow(x,4.0);
		 * 
		 * Come the (computer) revolution, all persons found guilty of such criminal behavior will be 
		 * summarily executed, and their programs won’t be! It is a matter of taste, however, whether to write
		 * p=c[0]+x*(c[1]+x*(c[2]+x*(c[3]+x*c[4])));
		 * or
		 * p=(((c[4]*x+c[3])*x+c[2])*x+c[1])*x+c[0];
		 * If the number of coefficients c[0..n-1] is large, one writes
		 * p=c[n-1];
		 * for(j=n-2;j>=0;j–) p=p*x+c[j];
		 * or
		 * p=c[j=n-1];
		 * while (j>0) p=p*x+c[--j];
		 * 
		 * where the original routine was using Math.Pow to evaluate the polinomials so with the Phi function
		 * we should have a little speed increase.  Not verified though so revert this implementaion if need be.
		 * It is just a funtion that I have used before for CDF.
		 * .
		 */
		internal static Blend SigmaBellShape(float focus, float scale)
		{
			Blend blend = new Blend();
			
			float pos = 0.0f;
			int count = 511; /* total no of samples */
			int index;
			float sigma;
			float mean;
			float fall_off_len = 2.0f; /* curve fall off length in terms of SIGMA */
			float delta; /* distance between two samples */
			
			float phi; /* variable to hold the value of Phi - Normal Distribution - CFD etc... */
			
			/* we get a curve not starting from 0 and not ending at 1.
			 * so we subtract the starting value and divide by the curve
			 * height to make it fit in the 0 to scale range
			 */
			float curve_bottom;
			float curve_top;
			float curve_height;
			
			if (focus == 0 || focus == 1) {
				count = 256;
			}
			
			if (blend.Positions.Length != count) {
				blend = new Blend(count);
			}
			
			/* Set the blend colors. We use integral of the Normal Distribution,
			 * i.e. Cumulative Distribution Function (CDF).
			 *
			 * Normal distribution:
			 *
			 * y (x) = (1 / sqrt (2 * PI * sq (sigma))) * exp (-sq (x - mu)/ (2 * sq (sigma)))
			 *
			 * where, y = height of normal curve, 
			 *        sigma = standard deviation
			 *        mu = mean
			 * OR
			 * y (x) = peak * exp ( - z * z / 2)
			 * where, z = (x - mu) / sigma
			 *
			 * In this curve, peak would occur at mean i.e. for x = mu. This results in
			 * a peak value of peak = (1 / sqrt (2 * PI * sq (sigma))).
			 *
			 * Cumulative distribution function:
			 * Ref: http://mathworld.wolfram.com/NormalDistribution.html
			 * see function Phi(x) below - Φ(x)
			 *
			 * D (x) = Phi(z)
			 * where, z = (x - mu) / sigma
			 *
			 */
			if (focus == 0) {
				/* right part of the curve with a complete fall in fall_off_len * SIGMAs */
				sigma = 1.0f / fall_off_len;
				mean = 0.5f;
				delta = 1.0f / 255.0f;
				
				curve_bottom = (float)Phi((1.0f - mean) / sigma);
				curve_top = (float)Phi((focus - mean) / sigma); 
				curve_height = curve_top - curve_bottom;
				
				/* set the start */
				blend.Positions[0] = focus;
				blend.Factors[0] = scale;
				
				for (index = 1, pos = delta; index < 255; index++, pos += delta)
				{
					blend.Positions[index] = pos;
					phi = (float)Phi((pos - mean) / sigma);
					blend.Factors[index] = (scale / curve_height) *
						(phi - curve_bottom);
				}
				
				/* set the end */
				blend.Positions [count - 1] = 1.0f;
				blend.Factors [count - 1] = 0.0f;
			}
			
			else if (focus == 1) {
				/* left part of the curve with a complete rise in fall_off_len * SIGMAs */
				sigma = 1.0f / fall_off_len;
				mean = 0.5f;
				delta = 1.0f / 255.0f;
				
				curve_bottom = (float)Phi((0.0f - mean) / sigma); 
				curve_top = (float)Phi((focus - mean) / sigma); 
				curve_height = curve_top - curve_bottom;
				
				/* set the start */
				blend.Positions[0] = 0.0f;
				blend.Factors[0] = 0.0f;
				
				for (index = 1, pos = delta; index < 255; index++, pos += delta)
				{
					blend.Positions[index] = pos;
					phi = (float)Phi((pos - mean) / sigma);
					blend.Factors[index] = (scale / curve_height) *
						(pos - curve_bottom);
				}
				
				/* set the end */
				blend.Positions [count - 1] = focus;
				blend.Factors [count - 1] = scale;
			}
			
			else {
				/* left part of the curve with a complete fall in fall_off_len * SIGMAs */
				sigma = focus / (2 * fall_off_len);
				mean = focus / 2.0f;
				delta = focus / 255.0f;
				
				/* set the start */
				blend.Positions [0] = 0.0f;
				blend.Factors [0] = 0.0f;
				
				curve_bottom = (float)Phi((0.0f - mean) / sigma);
				curve_top = (float)Phi((focus - mean) / sigma);
				curve_height = curve_top - curve_bottom;
				
				
				for (index = 1, pos = delta; index < 255; index++, pos += delta) {
					blend.Positions [index] = pos;
					phi = (float)Phi((pos - mean) / sigma);
					blend.Factors [index] = (scale / curve_height) * 
						(phi - curve_bottom);
				}
				
				blend.Positions [index] = focus;
				blend.Factors [index] = scale;
				
				/* right part of the curve with a complete fall in fall_off_len * SIGMAs */
				sigma = (1.0f - focus) / (2 * fall_off_len);
				mean = (1.0f + focus) / 2.0f;
				delta = (1.0f - focus) / 255.0f;
				
				curve_bottom = (float)Phi((1.0f - mean) / sigma);
				curve_top = (float)Phi((focus - mean) / sigma);
				
				curve_height = curve_top - curve_bottom;
				
				index ++;
				pos = focus + delta;
				
				for (; index < 510; index++, pos += delta) {
					blend.Positions [index] = pos;
					phi = (float)Phi((pos - mean) / sigma);
					blend.Factors [index] = (scale / curve_height) * 
						(phi - curve_bottom);
				}
				
				/* set the end */
				blend.Positions [count - 1] = 1.0f;
				blend.Factors [count - 1] = 0.0f;
			}
			
			return blend;
		}
		
		/*
		 * The function Φ(x) is the cumulative density function (CDF) of a standard normal 
		 * (Gaussian) random variable. It is closely related to the error function erf(x).
		 * 
		 * http://www.johndcook.com/csharp_phi.html
		 * 
		 * http://www.johndcook.com/erf_and_normal_cdf.pdf
		 * 
		 * This function is used by gradient brushes
		 * 
		 * This routine is self contained with the Erf code included in the funtion.
		 * This could also be implemented by 0.5 * (1.0 + Erf(x))
		 * 
		 * I left this double instead of implementing a float version as the Math.XXX api's are also double
		 */
		static double Phi(double x)
		{

			// constants
			double a1 = 0.254829592;
			double a2 = -0.284496736;
			double a3 = 1.421413741;
			double a4 = -1.453152027;
			double a5 = 1.061405429;
			double p = 0.3275911;
			
			// Save the sign of x
			int sign = 1;
			if (x < 0)
				sign = -1;
			x = Math.Abs(x) / Math.Sqrt(2.0);
			
			// A&S refers to Handbook of Mathematical Functions by Abramowitz and Stegun. 
			// See Stand-alone error function for details of the algorithm.
			// http://www.johndcook.com/blog/2009/01/19/stand-alone-error-function-erf/
			// A&S formula 7.1.26
			double t = 1.0 / (1.0 + p * x);
			double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

			return 0.5 * (1.0 + sign * y);
		}
		
		/*
		 * Not used right now but left here just in case so it does not have to be looked up again
		 * 
		 * http://www.johndcook.com/csharp_erf.html
		 * http://www.johndcook.com/blog/2009/01/19/stand-alone-error-function-erf/
		 * http://www.johndcook.com/erf_and_normal_cdf.pdf
		 */
		static double Erf(double x)
		{
			// constants
			double a1 = 0.254829592;
			double a2 = -0.284496736;
			double a3 = 1.421413741;
			double a4 = -1.453152027;
			double a5 = 1.061405429;
			double p = 0.3275911;
			
			// Save the sign of x
			int sign = 1;
			if (x < 0)
				sign = -1;
			x = Math.Abs(x);
			
			// A&S refers to Handbook of Mathematical Functions by Abramowitz and Stegun. 
			// http://www.johndcook.com/blog/2009/01/19/stand-alone-error-function-erf/
			//
			// A&S formula 7.1.26
			double t = 1.0 / (1.0 + p * x);
			double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);
			
			return sign * y;
		}

		/// <summary>
		/// This method initializes the new CGAffineTransform such that it represents the geometric transform that maps the rectangle 
		/// specified by the rect parameter to the parallelogram defined by the three points in the plgpts parameter. 
		/// 
		/// The upper-left corner of the rectangle is mapped to the first point in the plgpts array, the upper-right corner 
		/// is mapped to the second point, and the lower-left corner is mapped to the third point. The lower-right point of 
		/// the parallelogram is implied by the first three.
		/// </summary>
		/// <returns>The affine transform.</returns>
		/// <param name="rect">Rectangle.</param>
		/// <param name="points">Points.</param>
		internal static CGAffineTransform CreateGeometricTransform(RectangleF rect, PointF[] points)
		{
			var p0 = points [0];
			var p1 = points [1];
			var p2 = points [2];

			var width = rect.Width;
			var height = rect.Height;

			float m11 = (p1.X - p0.X) / width;
			float m12 = (p1.Y - p0.Y) / width;
			float m21 = (p2.X - p0.X) / height;
			float m22 = (p2.Y - p0.Y) / height;

			return new CGAffineTransform (m11, m12, m21, m22, p0.X, p0.Y);

		}

		/// <summary>
		/// This method initializes the new CGAffineTransform such that it represents the geometric transform that maps the rectangle 
		/// specified by the rect parameter to the parallelogram defined by the three points in the plgpts parameter. 
		/// 
		/// The upper-left corner of the rectangle is mapped to the first point in the plgpts array, the upper-right corner 
		/// is mapped to the second point, and the lower-left corner is mapped to the third point. The lower-right point of 
		/// the parallelogram is implied by the first three.
		/// </summary>
		/// <returns>The affine transform.</returns>
		/// <param name="rect">Rectangle.</param>
		/// <param name="points">Points.</param>
		internal static CGAffineTransform CreateGeometricTransform(RectangleF rect, Point[] points)
		{
			var p0 = points [0];
			var p1 = points [1];
			var p2 = points [2];

			var width = rect.Width;
			var height = rect.Height;

			float m11 = (p1.X - p0.X) / width;
			float m12 = (p1.Y - p0.Y) / width;
			float m21 = (p2.X - p0.X) / height;
			float m22 = (p2.Y - p0.Y) / height;

			return new CGAffineTransform (m11, m12, m21, m22, p0.X, p0.Y);

		}
	}
}

