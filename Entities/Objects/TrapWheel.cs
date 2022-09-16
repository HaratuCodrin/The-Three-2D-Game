using Bravos.Tiles;
using Bravos.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bravos.Entities.Objects
{
    public class TrapWheel : Entity
    {

        private Player player;

        public const int UpDown = 0, LeftRight = 1;
        private int type;

        private Image[] frames;
        private Image[] reversedFrames;

        public float time { get; set; }
        private float tween;
        public float Range { get; set; }

        public bool Moving { get; set; }


        public TrapWheel(TileManager tm, Player player, double x, double y, int width, int height, int type) : base(tm)
        {
            this.width = width;
            this.player = player;
            this.height = height;
            this.position = new Vector(x, y);
            this.type = type;
            Load();
        }

        public override void Load()
        {
            Bounds = new CollisionBox(position, width);



            tween = 0.02f;
           // time = (float) Math.Floor(Animation.random(-1, 1.5f));
            Moving = true;

            cutWidth = 32;
            cutHeight = 32;
            frames = Sprite.GetRowFromSheet(Image.FromFile("Objects/Traps/TrapWheel.png"), 8, cutWidth, cutHeight);
            reversedFrames = new Image[frames.Length];

            int j = 0;
            for (int i = frames.Length - 1; i >= 0; i--)
            {
                reversedFrames[j] = frames[i];
                j++;
            }

            ani = new Animation(reversedFrames);
            ani.SetDelay(1);

        }

        public override void Render(Graphics g)
        {
            if (NotOnScreen()) return;
            g.DrawImage(ani.getImage(), (float)(position.X + MapPosition.X), (float)(position.Y + MapPosition.Y), width, height);
            g.DrawEllipse(new Pen(new SolidBrush(Color.Red)), (float)(Bounds.GetPosition().X + MapPosition.X), (float)(Bounds.GetPosition().Y + MapPosition.Y), Bounds.Radius, Bounds.Radius);
        }

        public override void Update()
        {
            ani.Update();
            if (Moving)
            {
                switch(type)
                {
                    case UpDown:
                        MoveSinusY();
                        break;
                    case LeftRight:
                        MoveSinusX();
                        break;
                }
            }

            if(collidesCircleBox(player.HitBox) && !player.Flinching)
            {
                if(player.Health > 0)
                    player.Health--;

                player.Flinching = true;
                GamePanel.sounds["hit"].Play();

            }


        }

        public void MoveSinusX()
        {
            if (time > 1)
            {
                time = 1;
                tween *= -1;
            }

            if (time < -1)
            {
                time = -1;
                tween *= -1;
            }

            time += tween;
            if (time >= 0)
            position.X += Range * Animation.easeInOutSine(time);
            else
            position.X -= Range *  Animation.easeInOutSine(time);

        }

        public void MoveSinusY()
        {
            if (time > 1)
            {
                time = 1;
                tween *= -1;
            }

            if (time < -1)
            {
                time = -1;
                tween *= -1;
            }

            time += tween;
            if (time >= 0)
                position.Y += Range * Animation.easeInOutSine(time);
            else
                position.Y -= Range * Animation.easeInOutSine(time);

        }

    }
}
