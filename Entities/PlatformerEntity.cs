using Bravos.Tiles;
using Bravos.Tools;
using System.Collections.Generic;
using System.Drawing;

namespace Bravos.Entities
{
    public abstract class PlatformerEntity : Entity
    {            

        protected Font helpFont;

        protected bool facingRight { get; set; }
        protected bool Jumping { get; set; }

        public CollisionBox HitBox { get; set; }
        public CollisionBox AttackBox { get; set; }

        protected bool Falling { get; set; }
               
        protected int currentAction;

        protected List<Image[]> animations { get; set; }

        protected Image[] sheets { get; set; }

        public bool IsGrounded { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        
        protected bool Dead { get; set; }

        protected float MaxFallSpeed { get; set; }

        protected float JumpStart { get; set; }

        protected float GravAcc { get; set; }

        protected float FallSpeed { get; set; }


        public PlatformerEntity(TileManager tm) : base(tm)
        {

        }

        public void setAnimation(int i, Image[] frames, int delay)
        {
            currentAction = i;
            ani.SetFrames(frames);
            ani.SetDelay(delay);
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
                    // Bounds.Xtemp = Bounds.CurrCol * tm.BlockSize;
                    Bounds.Xtemp = position.X;
                }
            }
            else
            {
                Bounds.Xtemp += dx;
            }


            if(!Falling)
            {
                CalculateCorners(Bounds.GetPosition().X, Bounds.Ydest + 1);
                if(!Bounds.BottomLeft && !Bounds.BottomRight)
                {
                    Falling = true;
                    IsGrounded = false;
                }
            }
        }

        public bool hitCollision(CollisionBox bBox)
        {
            float ax = ((float)(HitBox.GetPosition().X + MapPosition.X));
            float ay = ((float)(HitBox.GetPosition().Y + MapPosition.Y));
            float bx = ((float)(bBox.GetPosition().X + MapPosition.X));
            float by = ((float)(bBox.GetPosition().Y + MapPosition.Y));

            if (ax < bx + bBox.Width &&
                    ax + HitBox.Width > bx &&
                    ay < by + bBox.Height &&
                    ay + HitBox.Height > by)
                return true;

            return false;
        }

        public bool attackCollision(CollisionBox bBox)
        {
            float ax = ((float)(AttackBox.GetPosition().X + MapPosition.X));
            float ay = ((float)(AttackBox.GetPosition().Y + MapPosition.Y));
            float bx = ((float)(bBox.GetPosition().X + MapPosition.X));
            float by = ((float)(bBox.GetPosition().Y + MapPosition.Y));

            if (ax < bx + bBox.Width &&
                    ax + AttackBox.Width > bx &&
                    ay < by + bBox.Height &&
                    ay + AttackBox.Height > by)
                return true;

            return false;
        }

        public override void Load()
        {
            animations = new List<Image[]>();
            ani = new Animation();
            facingRight = true;
            helpFont = new Font("Arial", 12);
        }

        public override void Update()
        {
            ani.Update();
        }

        public abstract void animate();

    }


}
