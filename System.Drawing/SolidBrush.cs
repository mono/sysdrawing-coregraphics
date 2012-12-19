//
// SolidBrush.cs: SolidBrush implementation for MonoTouch
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2011 Xamarin Inc
//
using System;
using System.Drawing.Drawing2D;
#if MONOMAC
using MonoMac.CoreGraphics;
#else
using MonoTouch.CoreGraphics;
#endif

namespace System.Drawing {

	public partial class SolidBrush : Brush {
		Color color;
		bool isModifiable;
		
		public SolidBrush (Color color)
		{
			this.color = color;
			isModifiable = true;
		}

		internal SolidBrush (Color color, bool isModifiable)
		{
			this.color = color;
			this.isModifiable = isModifiable;
		}
		
		public Color Color {
			get {
				return color;
			}
			set {
				color = value;
			}
		}
		
		public override void Dispose (bool disposing)
		{
			if (disposing){
			}
		}

		public override object Clone ()
		{
			return new SolidBrush (color);
		}

#if MONOMAC
		internal override void Setup (Graphics graphics, bool fill)
		{
			bool sourceCopy = graphics.CompositingMode == CompositingMode.SourceCopy;
			var scolor = new CGColor(color.R / 255f, color.G/255f, color.B/255f, sourceCopy ? 1f : color.A/255f );
			if (fill){
				graphics.context.SetFillColorWithColor (scolor);

			} else {
				graphics.context.SetStrokeColorWithColor (scolor);
			}
		}
#else
		internal override void Setup (Graphics graphics, bool fill)
		{
			bool sourceCopy = graphics.CompositingMode == CompositingMode.SourceCopy;
			if (fill){
				graphics.context.SetFillColor(color.R / 255f, color.G/255f, color.B/255f, sourceCopy ? 1f : color.A/255f );
				
			} else {
				graphics.context.SetStrokeColor(color.R / 255f, color.G/255f, color.B/255f, sourceCopy ? 1f : color.A/255f );
			}
		}
#endif
	}
}