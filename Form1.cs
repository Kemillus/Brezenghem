using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brezenghem
{
    public class BresenhamCircle
    {
        public static void DrawCircle(Graphics g, int centerX, int centerY, int radius)
        {
            int x = 0;
            int y = radius;
            int d = 3 - 2 * radius;

            while (x <= y)
            {
                PutPixel(g, centerX + x, centerY + y, Color.Black);
                PutPixel(g, centerX - x, centerY + y, Color.Black);
                PutPixel(g, centerX + x, centerY - y, Color.Black);
                PutPixel(g, centerX - x, centerY - y, Color.Black);
                PutPixel(g, centerX + y, centerY + x, Color.Black);
                PutPixel(g, centerX - y, centerY + x, Color.Black);
                PutPixel(g, centerX + y, centerY - x, Color.Black);
                PutPixel(g, centerX - y, centerY - x, Color.Black);

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

        private static void PutPixel(Graphics g, int x, int y, Color color)
        {
            g.FillRectangle(new SolidBrush(color), x, y, 1, 1);
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(this.Form1_Paint);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Размеры и центр формы
            float width = this.ClientSize.Width;
            float height = this.ClientSize.Height;
            float centerX = width / 2;
            float centerY = height / 2;
            float radius = 100; // Радиус кругов
            float distance = 100; // Расстояние от центра до центров внешних кругов

            // Рисуем и заполняем центральный круг
            BresenhamCircle.DrawCircle(g, (int)centerX, (int)centerY, (int)radius);

            // Рисуем и заполняем 12 кругов вокруг центрального
            for (int i = 0; i < 12; i++)
            {
                double angle = i * Math.PI / 6; // Углы для кругов (разделение по 30 градусов)
                float x = centerX + (float)(distance * Math.Cos(angle));
                float y = centerY + (float)(distance * Math.Sin(angle));
                BresenhamCircle.DrawCircle(g, (int)x, (int)y, (int)radius);
            }
        }
    }

}
