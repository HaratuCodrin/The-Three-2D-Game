using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bravos
{
    public class Shape
    {
        public Vector pos {  get;  set; }
        public Vector size { get; set; }

        public Color color { get; set; }

        public Shape(Vector pos, Vector size, Color color)
        {
            this.pos = pos;
            this.size = size;
            this.color = color;
        }

        public void render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(color), (int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

    }
}
