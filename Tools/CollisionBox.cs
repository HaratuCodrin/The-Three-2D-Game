using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bravos.Tools
{

    public class CollisionBox
    {
        private Vector Pos;
        private Vector MapPosition;

        public float XOffset { get; set; }
        public float YOffset { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public float Radius { get; set; }


        public int CurrRow { get; set; }
        public int CurrCol { get; set; }
        public double Xdest { get; set; }
        public double Ydest { get; set; }
        public double Xtemp { get; set; }
        public double Ytemp { get; set; }

        public bool TopLeft { get; set; }
        public bool TopRight { get; set; }
        public bool BottomLeft { get; set; }
        public bool BottomRight { get; set; }


        private int size;

        // constructor for rectangle box

        public CollisionBox(Vector pos, int width, int height)
        {
            this.Pos = pos;
            this.Width = width;
            this.Height = height;

            size = (int)Math.Max(Width, Height);
        }

        public CollisionBox(Vector pos, int width, int height, int XOffset ,int YOffset)
        {
            this.XOffset = XOffset;
            this.YOffset = YOffset;
            this.Pos = pos;          
            this.Width = width;
            this.Height = height;
            

            size = (int)Math.Max(Width, Height);
        }

        public CollisionBox(Vector pos, float Radius)
        {
            this.Pos = pos;
            this.Radius = Radius;
            size = (int)Radius;
        }

        public CollisionBox(Vector pos, float Radius, Vector MapPosition)
        {
            this.MapPosition = MapPosition;
            this.Pos = pos;
            this.Radius = Radius;
            size = (int)Radius;
        }


        public bool collidesCircleBox(CollisionBox bBox)
        {
            float ax = (float)GetRelativePosition().X;
            float ay = (float)GetRelativePosition().Y;
            float bx = ((float)(bBox.GetRelativePosition().X));
            float by = ((float)(bBox.GetRelativePosition().Y));

            float centerX = (float)(ax + Radius / 2);
            float centerY = (float)(ay + Radius / 2);

            float dx = Math.Min((float)Math.Abs(bx - centerX), (float)Math.Abs(bx + bBox.Width - centerX));
            float dy = Math.Min((float)Math.Abs(by - centerY), (float)Math.Abs(by + bBox.Height - centerY));

            if (Math.Sqrt(dx * dx + dy * dy) < Radius / 2)
                return true;

            return false;
        }


        public Vector GetPosition()
        {
            return new Vector(Pos.X + XOffset, Pos.Y + YOffset);
        }

        public Vector GetRelativePosition()
        {
            return new Vector(Pos.X + XOffset + MapPosition.X, Pos.Y + YOffset + MapPosition.Y);
        }

        public Vector GetInversePosition()
        {
            return new Vector(Pos.X - XOffset, Pos.Y + YOffset);
        }

        public int GetWidth()
        {
            return (int)Width;
        }


        public int GetHeight()
        {
            return (int)Height;
        }
 
    }
}
