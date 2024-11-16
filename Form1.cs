using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brezenghem
{
    public partial class Form1 : Form
    {
        private Bitmap canvas;
        private Color[] colors = { Color.Red, Color.Blue, Color.Coral, Color.DeepPink };

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1000, 800);
            this.Paint += new PaintEventHandler(this.Form1_Paint);
            this.canvas = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(canvas);

            float width = this.ClientSize.Width;
            float height = this.ClientSize.Height;
            float centerX = width / 2;
            float centerY = height / 2;
            float radius = 150;
            float distance = 150;
            float x, y;

            int count = 12;

            BresenhamCircle.DrawCircle(canvas, (int)centerX, (int)centerY, (int)radius);

            for (int i = 0; i < count; i++)
            {
                double angle = i * Math.PI / 6;
                x = centerX + (float)(distance * Math.Cos(angle));
                y = centerY + (float)(distance * Math.Sin(angle));
                BresenhamCircle.DrawCircle(canvas, (int)x, (int)y, (int)radius);
            }

            BresenhamCircle.FillSector(canvas, (int)centerX, (int)centerY, 260, colors[1], 2.4f, 14);
            BresenhamCircle.FillSector(canvas, (int)centerX, (int)centerY, 280, colors[2]);
            BresenhamCircle.FillSector(canvas, (int)centerX, (int)centerY, 148, colors[3], 2.2f, 14);
            BresenhamCircle.FillSector(canvas, (int)centerX, (int)centerY, 90, colors[0]);
            BresenhamCircle.FillSector(canvas, (int)centerX, (int)centerY, 152, colors[1], 2.4f, 14);
            BresenhamCircle.FillSector(canvas, (int)centerX, (int)centerY, 250, colors[0]);
            BresenhamCircle.FillSector(canvas, (int)centerX, (int)centerY, 12, colors[2], 2.4f, 16);



            e.Graphics.DrawImage(canvas, 0, 0);
        }
    }

    public class BresenhamCircle
    {
        public static void DrawCircle(Bitmap bmp, int centerX, int centerY, int radius)
        {
            int x = 0;
            int y = radius;
            int d = 3 - 2 * radius;

            while (x <= y)
            {
                PutPixel(bmp, centerX + x, centerY + y, Color.Black);
                PutPixel(bmp, centerX - x, centerY + y, Color.Black);
                PutPixel(bmp, centerX + x, centerY - y, Color.Black);
                PutPixel(bmp, centerX - x, centerY - y, Color.Black);
                PutPixel(bmp, centerX + y, centerY + x, Color.Black);
                PutPixel(bmp, centerX - y, centerY + x, Color.Black);
                PutPixel(bmp, centerX + y, centerY - x, Color.Black);
                PutPixel(bmp, centerX - y, centerY - x, Color.Black);

                if (d < 0)
                {
                    d += 4 * x + 6;
                }
                else
                {
                    d += 4 * (x - y) + 10;
                    y--;
                }
                x++;
            }
        }

        private static void PutPixel(Bitmap bmp, int x, int y, Color color)
        {
            if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
            {
                bmp.SetPixel(x, y, color);
            }
        }

        public static void FloodFillUntilBlack(Bitmap bmp, int x, int y, Color fillColor)
        {
            Color targetColor = bmp.GetPixel(x, y);
            if (targetColor.ToArgb() == fillColor.ToArgb() || targetColor.ToArgb() == Color.Black.ToArgb())
                return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(new Point(x, y));

            while (pixels.Count > 0)
            {
                Point pt = pixels.Pop();
                if (pt.X < 0 || pt.Y < 0 || pt.X >= bmp.Width || pt.Y >= bmp.Height)
                    continue;

                Color currentColor = bmp.GetPixel(pt.X, pt.Y);
                if (currentColor.ToArgb() != targetColor.ToArgb() || currentColor.ToArgb() == Color.Black.ToArgb())
                    continue;

                bmp.SetPixel(pt.X, pt.Y, fillColor);

                if (pt.X + 1 < bmp.Width && bmp.GetPixel(pt.X + 1, pt.Y).ToArgb() != Color.Black.ToArgb())
                    pixels.Push(new Point(pt.X + 1, pt.Y));

                if (pt.X - 1 >= 0 && bmp.GetPixel(pt.X - 1, pt.Y).ToArgb() != Color.Black.ToArgb())
                    pixels.Push(new Point(pt.X - 1, pt.Y));

                if (pt.Y + 1 < bmp.Height && bmp.GetPixel(pt.X, pt.Y + 1).ToArgb() != Color.Black.ToArgb())
                    pixels.Push(new Point(pt.X, pt.Y + 1));

                if (pt.Y - 1 >= 0 && bmp.GetPixel(pt.X, pt.Y - 1).ToArgb() != Color.Black.ToArgb())
                    pixels.Push(new Point(pt.X, pt.Y - 1));
            }
        }

        public static void FillSector(Bitmap bmp, int x, int y, int targetPoint, Color fillColor, float drift = 2,
            int count = 12)
        {
            float pi = (float)Math.PI;
            float posX, posY;

            for (int i = 0; i < count; i++)
            {
                float angle = (drift * pi * i) / count;
                posX = (float)(x + targetPoint * Math.Cos(angle));
                posY = (float)(y + targetPoint * Math.Sin(angle));
                BresenhamCircle.FloodFillUntilBlack(bmp, (int)posX, (int)posY, fillColor);
            }
        }
    }
}
