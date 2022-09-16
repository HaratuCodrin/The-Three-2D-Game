using Bravos.Tiles;
using Bravos.Tools;
using System;
using System.Drawing;
using System.Linq;

namespace Bravos.Entities
{
    public class DumbEnemy : Enemy
    {
      
        private readonly String[] fileNames = { "Attack.png", "Death.png", "Hurt.png", "Idle.png", "Walk.png"};

        private int[] numFrames;
        

        private const int Attack = 0, Death = 1, Hit = 2, Idle = 3, Walk = 4;
        private int travelDistance;

        private String name;

        public DumbEnemy(TileManager tm, Player player, ID id, int type) : base(tm, player)
        {
            position = new Vector(200,500);
            Load();
            switch (id)
            {
                case ID.Dog:               
                    cutWidth = 48;
                    cutHeight = 48;
                    width = 48;
                    height = 48;
                    numFrames = new int[] { 4, 4, 2, 4, 6 };
                    name = "Dog";
                    break;
                case ID.Cat:
                    cutWidth = 48;
                    cutHeight = 48;
                    width = 48;
                    height = 48;
                    numFrames = new int[] { 4, 4, 2, 4, 6 };
                    name = "Cat";
                    break;
            }
            sheets = new Image[numFrames.Length];
            Bounds = new CollisionBox(position, width, height);
            for(int i = 0; i < numFrames.Length; i++)
            {
                sheets[i] = Image.FromFile("Enemies/" + name + type +"/"+ fileNames[i]);
                animations.Add(Sprite.GetRowFromSheet(sheets[i], sheets[i].Width / cutWidth, cutWidth, cutHeight));
            }
            
          
        }

        public override void animate()
        {

            if (right) facingRight = true;
            if (left) facingRight = false;

            if (left || right)
            {
                if (currentAction != Walk)
                {
                    setAnimation(Walk, animations.ElementAt(Walk), 8);
                    ani.Hold = false;                   
                }
            }
            else
            {
                if (currentAction != Idle)
                {
                    setAnimation(Idle, animations.ElementAt(Idle), 15);
                    ani.Hold = false;                   
                }
            }
        }

        public override void CheckTileMapCollision()
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
                    Falling = false;
                    IsGrounded = true;
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
                    right = true;
                    left = false;
                    Bounds.Xtemp = (Bounds.CurrCol) * tm.BlockSize;
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
                    left = true;
                    right = false;
                    // Bounds.Xtemp = Bounds.CurrCol * tm.BlockSize;
                    Bounds.Xtemp = position.X;
                }
            }
            else
            {
                Bounds.Xtemp += dx;
            }


            if (!Falling)
            {
                CalculateCorners(Bounds.GetPosition().X, Bounds.Ydest + 1);
                if (!Bounds.BottomLeft && !Bounds.BottomRight)
                {
                    Falling = true;
                    IsGrounded = false;
                }
            }
        }



        public override void Render(Graphics g)
        {
            SetMapPosition();
            if (NotOnScreen()) return;

            if (facingRight)
            {
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X), (int)(position.Y + MapPosition.Y), width, height);               
            }
            else
            {
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X + width), (int)(position.Y + MapPosition.Y), -width, height);               
            }

             g.DrawRectangle(new Pen(new SolidBrush(Color.Blue)), new Rectangle((int)(Bounds.GetPosition().X + MapPosition.X + Bounds.XOffset), (int)(Bounds.GetPosition().Y + MapPosition.Y + Bounds.YOffset), Bounds.GetWidth(), Bounds.GetHeight()));
            g.DrawString(Falling + "", helpFont, new SolidBrush(Color.Black), 100, 100);
            g.DrawString(dx + "," + dy, helpFont, new SolidBrush(Color.Black), 200, 100);

        }

        public override void Load()
        {
            InitParameters();
            base.Load();
            right = true;

        }

        protected override void Move()
        {

            if (left)
            {
                dx -= acc;
                if (dx < -maxSpeed)
                {
                    dx = -maxSpeed;
                }
            }
            else if (right)
            {
                dx += acc;
                if (dx > maxSpeed)
                {
                    dx = maxSpeed;
                }
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
                else if (dx < 0)
                {
                    dx += deacc;
                    if (dx > 0)
                    {
                        dx = 0;
                    }
                }
            }

            if (Falling)
            {
                dy += FallSpeed;
                /*if (dy > 0) Jumping = false;
                if (dy < 0 && !Jumping) dy += GravAcc;*/
                if (dy > MaxFallSpeed) dy = MaxFallSpeed;
            }

        }

        protected override void InitParameters()
        {
            Health = MaxHealth = 2;           
            acc = 0.4f;
            maxSpeed = 2.6f;
            deacc = 1.6f;
            MaxFallSpeed = 6f;
            FallSpeed = 0.15f;
            GravAcc = 0.3f;
        }
    }




}
