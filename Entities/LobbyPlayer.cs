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
    public class LobbyPlayer : LobbyEntity
    {
        private Vector spawn;
        private Font helpFont;
        public int CoinAmount { get; set; }
        public int Progress { get; private set; }

        public LobbyPlayer(TileManager tm) : base(tm)
        {
            maxSpeed = 2.3f;
            acc = 1f;
            deacc = 0.8f;
        }

        public LobbyPlayer(TileManager tm, Sprite sprite, int width, int height) : base(tm, sprite, width, height)
        {
            maxSpeed = 2.3f;
            acc = 1f;
            deacc = 0.8f;
            spawn = new Vector(300, 100);
            helpFont = new Font("Arial", 12);
            position = spawn;

            Bounds = new CollisionBox(position, width, height);
        }


        public LobbyPlayer(TileManager tm, Sprite sprite, int width, int height, Vector spawn) : base(tm, sprite, width, height)
        {
            maxSpeed = 2.3f;
            acc = 1f;
            deacc = 0.8f;
            this.spawn = spawn;
            position = spawn;
        }

        public virtual void KeyReleased(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    up = false;
                    break;
                case Keys.S:
                case Keys.Down:
                    down = false;
                    break;
                case Keys.A:
                case Keys.Left:
                    left = false;
                    break;
                case Keys.D:
                case Keys.Right:
                    right = false;
                    break;
            }
        }


        public virtual void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    up = true;
                    break;
                case Keys.S:
                case Keys.Down:
                    down = true;
                    break;
                case Keys.A:
                case Keys.Left:
                    left = true;
                    break;
                case Keys.D:
                case Keys.Right:
                    right = true;
                    break;
            }
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public override void Render(Graphics g)
        {
            SetMapPosition();
            g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X), (int)(position.Y + MapPosition.Y), width, height);
            g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)(Bounds.GetPosition().X + MapPosition.X), (int)(Bounds.GetPosition().Y + MapPosition.Y), Bounds.GetWidth(), Bounds.GetHeight()));
            g.DrawString(Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)) + "", helpFont, new SolidBrush(Color.Black), 100, 100);
        }

        public override void Update()
        {
             base.Update();
             MoveOptimal();
             CheckTileMapCollision();
             SetPosition(Bounds.Xtemp, Bounds.Ytemp);            
        }


        // miscarea personajuli
        public void Move()
        {
            if (up)
            {
                dy -= acc;
                if (dy < -maxSpeed)
                {
                    dy = -maxSpeed;
                }
                down = false;
            }
            else
            {
                if (dy < 0)
                {
                    dy += deacc;
                    if (dy > 0)
                    {
                        dy = 0;
                    }
                }
            }
            if (down)
            {
                dy += acc;
                if (dy > maxSpeed)
                {
                    dy = maxSpeed;
                }
                up = false;
            }
            else
            {
                if (dy > 0)
                {
                    dy -= deacc;
                    if (dy < 0)
                    {
                        dy = 0;
                    }
                }
            }
            if (left)
            {
                dx -= acc;
                if (dx < -maxSpeed)
                {
                    dx = -maxSpeed;
                }
                right = false;
            }
            else
            {
                if (dx < 0)
                {
                    dx += deacc;
                    if (dx > 0)
                    {
                        dx = 0;
                    }
                }
            }
            if (right)
            {
                dx += acc;
                if (dx > maxSpeed)
                {
                    dx = maxSpeed;
                }
                left = false;
            }
            else
            {
                if (dx > 0)
                {
                    dx -= deacc;
                    if (dx < 0)
                    {
                        dx = 0;
                    }
                }
            }




        }


        public void MoveOptimal()
        {
            if (up)
            {
                dy -= acc;
                if(left || right)
                {
                    normalizeVector();
                }


                if (dy < -maxSpeed)
                {
                    dy = -maxSpeed;
                }
                down = false;
            }
            else
            {
                if (dy < 0)
                {
                    dy += deacc;
                    if (dy > 0)
                    {
                        dy = 0;
                    }
                }
            }
            if (down)
            {
                dy += acc;
                if (left || right)
                {
                    normalizeVector();
                }
                if (dy > maxSpeed)
                {
                    dy = maxSpeed;
                }
                up = false;
            }
            else
            {
                if (dy > 0)
                {
                    dy -= deacc;
                    if (dy < 0)
                    {
                        dy = 0;
                    }
                }
            }
            if (left)
            {
                dx -= acc;
                if (up || down)
                {
                    normalizeVector();
                }
                if (dx < -maxSpeed)
                {
                    dx = -maxSpeed;
                }
                right = false;
            }
            else
            {
                if (dx < 0)
                {
                    dx += deacc;
                    if (dx > 0)
                    {
                        dx = 0;
                    }
                }
            }
            if (right)
            {
                dx += acc;
                if (up || down)
                {
                    normalizeVector();
                }
                if (dx > maxSpeed)
                {
                    dx = maxSpeed;
                }
                left = false;
            }
            else
            {
                if (dx > 0)
                {
                    dx -= deacc;
                    if (dx < 0)
                    {
                        dx = 0;
                    }
                }
            }


        }      

    }
}
