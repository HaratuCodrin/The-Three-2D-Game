using Bravos.Tiles;
using Bravos.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bravos.Entities
{
    public abstract class Entity
    {
        protected TileManager tm;

        public bool up { get; set; }
        public bool down { get; set; }
        public bool right { get; set; }
        public bool left { get; set; }       
        public bool escape { get; set; }

        public float Radius { get; set; }

        protected Vector position;
        protected Vector MapPosition;
        protected int BlockSize;
        protected Animation ani;
        // viteze
        protected float dx;
        protected float dy;

        // acceleratie si viteza maxima
        protected float maxSpeed;
        protected float acc;
        protected float deacc;

        public int width { get; set; }
        public int height { get; set; }

        protected int cutWidth;
        protected int cutHeight;

        protected CollisionBox Bounds;

        public Entity(TileManager tm)
        {
            this.tm = tm;
            MapPosition = tm.GetPosition();
            BlockSize = tm.BlockSize;
        }

        public abstract void Update();
        public abstract void Load();

        public abstract void Render(Graphics g);

        //AABB collision checker
        public bool collides(CollisionBox bBox)
        {
            float ax = ((float)(Bounds.GetPosition().X + MapPosition.X));
            float ay = ((float)(Bounds.GetPosition().Y + MapPosition.Y));
            float bx = ((float)(bBox.GetPosition().X + MapPosition.X));
            float by = ((float)(bBox.GetPosition().Y + MapPosition.Y));

            if (ax < bx + bBox.Width &&
                    ax + Bounds.Width > bx &&
                    ay < by + bBox.Height &&
                    ay + Bounds.Height > by)
                return true;

            return false;
        }

        public bool collidesCircleBox(CollisionBox bBox)
        {
            float ax = ((float)(Bounds.GetPosition().X + MapPosition.X));
            float ay = ((float)(Bounds.GetPosition().Y + MapPosition.Y));
            float bx = ((float)(bBox.GetPosition().X + MapPosition.X));
            float by = ((float)(bBox.GetPosition().Y + MapPosition.Y));

            float centerX = (float)(ax + Bounds.Radius / 2);
            float centerY = (float)(ay + Bounds.Radius / 2);

           float dx = Math.Min((float)Math.Abs(bx - centerX),(float)Math.Abs(bx + bBox.Width - centerX));
           float dy = Math.Min((float)Math.Abs(by - centerY),(float)Math.Abs(by + bBox.Height - centerY));

            if (Math.Sqrt(dx * dx + dy * dy) < Bounds.Radius / 2)
                return true;

                return false;
        }
       

        public Vector GetPosition()
        {
            return position;
        }

        public void SetMapPosition()
        {
            MapPosition = tm.GetPosition();
        }

        public void SetSpeed(float dx, float dy)
        {
            this.dx = dx;
            this.dy = dy;
        }

        public bool NotOnScreen()
        {
            return position.X + MapPosition.X + width < 0 ||
                position.X + MapPosition.X - width > GamePanel.WIDTH * GamePanel.SCALE ||
                position.Y + MapPosition.Y + height < 0 ||
                position.Y + MapPosition.Y - height > GamePanel.HEIGHT * GamePanel.SCALE;
        }

        public void tileCollisionSetup()
        {
            Bounds.CurrCol = (int)Bounds.GetPosition().X / BlockSize;
            Bounds.CurrRow = (int)Bounds.GetPosition().Y / BlockSize;

            Bounds.Xdest = Bounds.GetPosition().X + dx;
            Bounds.Ydest = Bounds.GetPosition().Y + dy;

            Bounds.Xtemp = Bounds.GetPosition().X;
            Bounds.Ytemp = Bounds.GetPosition().Y;
        }

        public void CalculateCorners(double x, double y)
        {
            int leftTile = (int)(x) / BlockSize;
            int rightTile = (int)(x + Bounds.GetWidth()) / BlockSize;
            int topTile = (int)(y)  / BlockSize;
            int bottomTile = (int)(y + Bounds.GetHeight()) / BlockSize;

            int tL = tm.BlockMap[topTile, leftTile];
            int tR = tm.BlockMap[topTile, rightTile];            
            int bL = tm.BlockMap[bottomTile, leftTile];
            int bR = tm.BlockMap[bottomTile, rightTile];

            Bounds.TopLeft = (tL != 0);
            Bounds.TopRight = (tR != 0);
            Bounds.BottomLeft = (bL != 0);
            Bounds.BottomRight = (bR != 0);
        }

        public virtual void CheckTileMapCollision()
        {
            tileCollisionSetup();

            CalculateCorners(Bounds.Xtemp, Bounds.Ydest);
            if (dy < 0)
            {
                if (Bounds.TopLeft || Bounds.TopRight)
                {
                    dy = 0;
                    Bounds.Ytemp = Bounds.CurrRow * tm.BlockSize;
                }
            }
            else
            {
                Bounds.Ytemp += dy;
            }

            if (dy > 0)
            {
                if (Bounds.BottomLeft || Bounds.BottomRight)
                {
                    dy = 0;                   
                    // Bounds.Ytemp = Bounds.CurrRow * tm.BlockSize;
                    Bounds.Ytemp = position.Y;
                }
            }
            else
            {
                Bounds.Ytemp += dy;
            }
            
            CalculateCorners(Bounds.Xdest, Bounds.Ytemp);
            if (dx < 0)
            {
                if (Bounds.TopLeft || Bounds.BottomLeft)
                {
                    dx = 0;
                    Bounds.Xtemp = (Bounds.CurrCol)*tm.BlockSize;
                }
            }
            else
            {
                Bounds.Xtemp += dx;
            }

            if (dx > 0)
            {
                if (Bounds.BottomRight || Bounds.TopRight)
                {
                    dx = 0;
                   // Bounds.Xtemp = Bounds.CurrCol * tm.BlockSize;
                    Bounds.Xtemp = position.X;
                }
            }
            else
            {
                Bounds.Xtemp += dx;
            }
        }

        public CollisionBox GetBounds()
        {
            return Bounds;
        }

        public void SetPosition(double x, double y)
        {
            this.position.X = x;
            this.position.Y = y;
        }


    }


}
