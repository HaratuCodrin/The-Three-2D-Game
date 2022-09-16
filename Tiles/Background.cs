using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bravos
{
    public class Background
    {
        private Image image;

        private Vector pos;
        private double dx;
        private double dy;

        private double moveScale;

        private int width;
        private int height;

        public Background(string s, double ms)
        {
            image = Image.FromFile(s);
            moveScale = ms;
            width = GamePanel.WIDTH*GamePanel.SCALE;
            height = GamePanel.HEIGHT*GamePanel.SCALE;


        }

        public Background(Image image, double ms)
        {
            this.image = image;
            moveScale = ms;
            width = image.Width * GamePanel.SCALE;
            height = image.Height * GamePanel.SCALE;


        }

        public void setPosition(Vector position)
        {
            this.pos = position;
        }

        public void setPosition(double x, double y)
        {
            this.pos.X = (x*moveScale) % width*GamePanel.SCALE;
            this.pos.Y = (y*moveScale) % height*GamePanel.SCALE;
        }

        public void setVelocity(double dx, double dy)
        {
            this.dx = dx;
            this.dy = dy;
        }

        public void Update()
        {
            pos.X += dx;
            if (pos.X <= -width) pos.X += width;
            if (pos.X >= width) pos.X -= width;
            pos.Y += dy;
            if (pos.Y <= -height) pos.Y += height;
            if (pos.Y >= height) pos.Y -= height;
        }

        public void Render(Graphics g)
        {
            g.DrawImage(image, (int)pos.X, (int)pos.Y, width, height);
            if (pos.X < 0)
            {
                g.DrawImage(image, (int)pos.X + width -1, (int)pos.Y, width, height);
            }
            if (pos.X > 0)
            {
                g.DrawImage(image, (int)pos.X - width +1, (int)pos.Y, width, height);
            }


        }

        

    }
}
