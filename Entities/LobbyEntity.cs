using Bravos.Tiles;
using Bravos.Tools;
using System;
using System.Drawing;

namespace Bravos.Entities
{
    public abstract class LobbyEntity : Entity
    {
        protected const int UP = 3;
        protected const int DOWN = 0;
        protected const int RIGHT = 2;
        protected const int LEFT = 1;
        protected int currentAnimation;       
        protected Sprite sprite;
        

        public LobbyEntity(TileManager tm) : base(tm)
        {
            
        }


        public LobbyEntity(TileManager tm, Sprite sprite, int width, int height) : base(tm)
        {
            this.width = width;
            this.height = height;
            this.sprite = sprite;
            ani = new Animation();
            setAnimation(LEFT, sprite.GetRow(LEFT), 10);
        }

        public void animate()
        {
            if (up)
            {
                if (currentAnimation != UP || ani.GetDelay() == -1)
                {
                    setAnimation(UP, sprite.GetRow(UP), 5);
                }
            }
            else if (down)
            {
                if (currentAnimation != DOWN || ani.GetDelay() == -1)
                {
                    setAnimation(DOWN, sprite.GetRow(DOWN), 5);
                }
            }
            else if (left)
            {
                if (currentAnimation != LEFT || ani.GetDelay() == -1)
                {
                    setAnimation(LEFT, sprite.GetRow(LEFT), 5);
                }
            }
            else if (right)
            {
                if (currentAnimation != RIGHT || ani.GetDelay() == -1)
                {
                    setAnimation(RIGHT, sprite.GetRow(RIGHT), 5);
                }
            }
            else
            {
                setAnimation(currentAnimation, sprite.GetRow(currentAnimation), -1);
            }
        }

        public void setAnimation(int i, Image[] frames, int delay)
        {
            currentAnimation = i;
            ani.SetFrames(frames);
            ani.SetDelay(delay);
        }

        public override void Update()
        {
            ani.Update();
            animate();
        }

        public void normalizeVector()
        {
            float magnitude = (float)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

            dx = (dx / magnitude)* maxSpeed;
            dy = (dy / magnitude)* maxSpeed;
        }      
            
        

    }


}
