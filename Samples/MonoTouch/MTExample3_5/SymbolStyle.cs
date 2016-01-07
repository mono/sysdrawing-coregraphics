using System.Drawing;
using CoreGraphics;

namespace MTExample3_5 {
	public class SymbolStyle {
		SymbolTypeEnum symbolType;
		float symbolSize;
		Color borderColor;
		Color fillColor;
		float borderThickness;

		public SymbolStyle ()
		{
			symbolType = SymbolTypeEnum.None;
			symbolSize = 8f;
			borderColor = Color.Black;
			fillColor = Color.White;
			borderThickness = 1f;
		}

		public float BorderThickness {
			get {
				return borderThickness;
			}
			set {
				borderThickness = value;
			}
		}

		public Color BorderColor {
			get {
				return borderColor;
			}
			set {
				borderColor = value;
			}
		}

		public Color FillColor {
			get {
				return fillColor;
			}
			set {
				fillColor = value;
			}
		}

		public float SymbolSize {
			get {
				return symbolSize;
			}
			set {
				symbolSize = value;
			}
		}

		public SymbolTypeEnum SymbolType {
			get {
				return symbolType;
			}
			set {
				symbolType = value;
			}
		}

		public enum SymbolTypeEnum {
			Box = 0,
			Circle = 1,
			Cross = 2,
			Diamond = 3,
			Dot = 4,
			InvertedTriangle = 5,
			None = 6,
			OpenDiamond = 7,
			OpenInvertedTriangle = 8,
			OpenTriangle = 9,
			Square = 10,
			Star = 11,
			Triangle = 12,
			Plus = 13
		}

		public void DrawSymbol (Graphics g, CGPoint pt)
		{
			var aPen = new Pen (BorderColor, BorderThickness);
			var aBrush = new SolidBrush (FillColor);
			var x = (float)pt.X;
			var y = (float)pt.Y;
			float size = SymbolSize;
			float halfSize = size / 2f;
			var aRectangle = new RectangleF (x - halfSize, y - halfSize, size, size);

			switch (SymbolType) {
				case SymbolTypeEnum.Square:
					g.DrawLine (aPen, x - halfSize, y - halfSize, x + halfSize, y - halfSize);
					g.DrawLine (aPen, x + halfSize, y - halfSize, x + halfSize, y + halfSize);
					g.DrawLine (aPen, x + halfSize, y + halfSize, x - halfSize, y + halfSize);
					g.DrawLine (aPen, x - halfSize, y + halfSize, x - halfSize, y - halfSize);
					break;
				case SymbolTypeEnum.OpenDiamond:
					g.DrawLine (aPen, x, y - halfSize, x + halfSize, y);
					g.DrawLine (aPen, x + halfSize, y, x, y + halfSize);
					g.DrawLine (aPen, x, y + halfSize, x - halfSize, y);
					g.DrawLine (aPen, x - halfSize, y, x, y - halfSize);
					break;
				case SymbolTypeEnum.Circle:
					g.DrawEllipse (aPen, x - halfSize, y - halfSize, size, size);
					break;
				case SymbolTypeEnum.OpenTriangle:
					g.DrawLine (aPen, x, y - halfSize, x + halfSize, y + halfSize);
					g.DrawLine (aPen, x + halfSize, y + halfSize, x - halfSize, y + halfSize);
					g.DrawLine (aPen, x - halfSize, y + halfSize, x, y - halfSize);
					break;
				case SymbolTypeEnum.None:
					break;
				case SymbolTypeEnum.Cross:
					g.DrawLine (aPen, x - halfSize, y - halfSize, x + halfSize, y + halfSize);
					g.DrawLine (aPen, x + halfSize, y - halfSize, x - halfSize, y + halfSize);
					break;
				case SymbolTypeEnum.Star:
					g.DrawLine (aPen, x, y - halfSize, x, y + halfSize);
					g.DrawLine (aPen, x - halfSize, y, x + halfSize, y);
					g.DrawLine (aPen, x - halfSize, y - halfSize, x + halfSize, y + halfSize);
					g.DrawLine (aPen, x + halfSize, y - halfSize, x - halfSize, y + halfSize);
					break;
				case SymbolTypeEnum.OpenInvertedTriangle:
					g.DrawLine (aPen, x - halfSize, y - halfSize, x + halfSize, y - halfSize);
					g.DrawLine (aPen, x + halfSize, y - halfSize, x, y + halfSize);
					g.DrawLine (aPen, x, y + halfSize, x - halfSize, y - halfSize);
					break;
				case SymbolTypeEnum.Plus:
					g.DrawLine(aPen, x, y - halfSize, x, y + halfSize);
					g.DrawLine(aPen, x - halfSize, y, x + halfSize, y);
					break;
				case SymbolTypeEnum.Dot:
					g.FillEllipse (aBrush, aRectangle);
					g.DrawEllipse (aPen, aRectangle);
					break;
				case SymbolTypeEnum.Box:
					g.FillRectangle (aBrush, aRectangle);
					g.DrawLine (aPen, x - halfSize, y - halfSize, x + halfSize, y - halfSize);
					g.DrawLine (aPen, x + halfSize, y - halfSize, x + halfSize, y + halfSize);
					g.DrawLine (aPen, x + halfSize, y + halfSize, x - halfSize, y + halfSize);
					g.DrawLine (aPen, x - halfSize, y + halfSize, x - halfSize, y - halfSize);
					break;
				case SymbolTypeEnum.Diamond:
					PointF[] pta = new PointF[4];
					pta[0].X = x;
					pta[0].Y = y - halfSize;
					pta[1].X = x + halfSize;
					pta[1].Y = y;
					pta[2].X = x;
					pta[2].Y = y + halfSize;
					pta[3].X = x - halfSize;
					pta[3].Y = y;
					g.FillPolygon (aBrush, pta);
					g.DrawPolygon (aPen, pta);
					break;
				case SymbolTypeEnum.InvertedTriangle:
					PointF[] ptb = new PointF[3];
					ptb[0].X = x - halfSize;
					ptb[0].Y = y - halfSize;
					ptb[1].X = x + halfSize;
					ptb[1].Y = y - halfSize;
					ptb[2].X = x;
					ptb[2].Y = y + halfSize;
					g.FillPolygon (aBrush, ptb);
					g.DrawPolygon (aPen, ptb);
					break;
				case SymbolTypeEnum.Triangle:
					PointF[] ptc = new PointF[3];
					ptc[0].X = x;
					ptc[0].Y = y - halfSize;
					ptc[1].X = x + halfSize;
					ptc[1].Y = y + halfSize;
					ptc[2].X = x - halfSize;
					ptc[2].Y = y + halfSize;
					g.FillPolygon (aBrush, ptc);
					g.DrawPolygon (aPen, ptc);
					break;
			}
		}
	}
}
