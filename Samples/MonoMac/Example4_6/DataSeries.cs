using System;
using System.Collections;
using System.DrawingNative;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;
namespace Example4_6
{
    public class DataSeries
    {
        private ArrayList dataList;
        private ArrayList labelList;
        private ArrayList explodeList;
        private int[,] cmap;
        private Color borderColor = Color.Black;
        private float borderThickness = 1.0f;
        

        public DataSeries()
        {
            dataList = new ArrayList();
            labelList = new ArrayList();
            explodeList = new ArrayList();
        }

        public int[,] CMap
        {
            get { return cmap; }
            set { cmap = value; }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        public float BorderThickness
        {
            get { return borderThickness; }
            set { borderThickness = value; }
        }

        public ArrayList DataList
        {
            get { return dataList; }
            set { dataList = value; }
        }

        public ArrayList LabelList
        {
            get { return labelList; }
            set { labelList = value; }
        }

        public ArrayList ExplodeList
        {
            get { return explodeList; }
            set { explodeList = value; }
        }

        public void AddData(float data)
        {
            dataList.Add(data);
            labelList.Add(data.ToString());
            explodeList.Add(0);
        }

        /*public void AddLabel(string str)
        {
            labelList.Clear();
            labelList.Add(str);
        }

        public void AddExplode(int nOffset)
        {
            explodeList.Clear();
            explodeList.Add(nOffset);
        }*/

        public void AddPie(Graphics g, ChartStyle cs)
        {
            SolidBrush aBrush = new SolidBrush(Color.Black);
            Pen aPen = new Pen(BorderColor);
            int nData = DataList.Count;
            float fSum = 0;
            for (int i = 0; i < nData; i++)
            {
                fSum = fSum + (float)DataList[i];
            }
            float startAngle = 0;
            float sweepAngle = 0;
            Rectangle rect = cs.SetPieArea();

            for (int i = 0; i < nData; i++)
            {
                Color fillColor = Color.FromArgb(CMap[i, 0], CMap[i, 1], 
                    CMap[i, 2], CMap[i, 3]);
                aBrush = new SolidBrush(fillColor);
                int explode = (int)ExplodeList[i];

                if (fSum < 1)
                {
                    startAngle = startAngle + sweepAngle;
                    sweepAngle = 360 * (float)DataList[i];
                }
                else if (fSum >= 1)
                {
                    startAngle = startAngle + sweepAngle;
                    sweepAngle = 360 * (float)DataList[i] / fSum;

                }

                int xshift = (int)(explode * Math.Cos((startAngle +
                    sweepAngle / 2) * Math.PI / 180));
                int yshift = (int)(explode * Math.Sin((startAngle +
                    sweepAngle / 2) * Math.PI / 180));
                Rectangle rect1 = new Rectangle(rect.X + xshift, rect.Y + yshift,
                    rect.Width, rect.Height);
                g.FillPie(aBrush, rect1, startAngle, sweepAngle);
                g.DrawPie(aPen, rect1, startAngle, sweepAngle);
            }
        }
    }
}
