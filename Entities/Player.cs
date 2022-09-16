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
    public abstract class Player : PlatformerEntity
    {
     
        protected bool AbilityE { get; set; }
        protected bool AbilityR { get; set; }

        public int CoinAmount { get; set; }
        
        // player UI 
        protected Animation hudCoin;
        protected readonly Font HudFont = new Font("Impact", 19, FontStyle.Bold);
        protected readonly SolidBrush HudBrush = new SolidBrush(Color.LightSlateGray);
        protected readonly Image heart = Image.FromFile("HUD/heart.png");

        protected Image[] abilityImages;
        protected int[] abilityCooldowns;
        protected int[] cds = new int[] { 0, 0 };

        private Color cooldownColor = Color.FromArgb(120, Color.White);




      
        public bool Flinching { get; set; }
        protected long FlinchTime;

        public Player(TileManager tm) : base(tm)
        {
            Image image = Image.FromFile("Objects/Gologani/Ban.png");
            hudCoin = new Animation(Sprite.GetRowFromSheet(image, image.Width / 8, 8, 8));
            hudCoin.SetDelay(8);
                    
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
                case Keys.Space:
                    Jumping = true;
                    break;
                case Keys.E:
                    AbilityE = true;
                    break;
                case Keys.R:
                    AbilityR = true;
                    break;
            }
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
                case Keys.Space:
                    Jumping = false;
                    break;
                case Keys.E:
                    AbilityE = false;
                    break;
                case Keys.R:
                    AbilityR = false;
                    break;

            }
        }

        public override void Load()
        {
            base.Load();
            HitBox = new CollisionBox(position, width / 2, height * 3 / 4, width/4, height / 8);
            AttackBox = new CollisionBox(position, width / 3, height / 3, width, height / 3);
            // Flinching = true;
        }

        public override void Render(Graphics g)
        {
            SetMapPosition();

            // drawing the HUD
            for (int i = 0; i < Health; i++)
            {
                g.DrawImage(heart, i * 40 + 32, 32, 32, 32);
            }

            for (int i = 0; i <= 1; i++)
            { 
                if(abilityImages != null)
                {
                    g.DrawImage(abilityImages[i], i * 64 + 32, (int)GamePanel.GetSize().Y - 80, 64, 64);
                    if(cds[i] > 0)
                    {
                        g.FillRectangle(new SolidBrush(cooldownColor), i * 64 + 32 + 2, (int)GamePanel.GetSize().Y - 80 + 2, 60, 60);
                        g.DrawString(""+ cds[i] / 60, HudFont, new SolidBrush(Color.Black), i * 64 + 32 + 20, (int)GamePanel.GetSize().Y - 80 + 15);
                    }
                }
            }



            // Coin animation
            g.DrawImage(hudCoin.getImage(), (int)GamePanel.GetSize().X - 128, 32, 32, 32);
            
            // Coin amount inside current level
            g.DrawString(" : " + CoinAmount + ".", HudFont, HudBrush, (int)GamePanel.GetSize().X - 96, 32);


            if (Flinching)
            {
                FlinchTime++;
                if (FlinchTime >= 120)
                {
                    Flinching = false;
                    FlinchTime = 0;
                }
            }

            // g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)(Bounds.GetPosition().X + MapPosition.X), (int)(Bounds.GetPosition().Y + MapPosition.Y), Bounds.GetWidth(), Bounds.GetHeight()));

            
            /* g.DrawString("Falling = "+Falling, helpFont, new SolidBrush(Color.Black), 100, 130);
             g.DrawString(dy + "", helpFont, new SolidBrush(Color.Black), 100, 150);
             g.DrawString(Bounds.BottomLeft + "", helpFont, new SolidBrush(Color.Black), 100, 170);*/

        }


        public override void Update()
        { 
            ani.Update();
            hudCoin.Update();
            animate();
            Move();           
            CheckTileMapCollision();  
            SetPosition(Bounds.Xtemp, Bounds.Ytemp);
           

        }

        public override void animate()
        {          
            if (right) facingRight = true;
            if (left) facingRight = false;
        }



        public virtual void Move()
        {
            if(left)
            {
                dx -= acc;
                if(dx < -maxSpeed)
                {
                    dx = -maxSpeed;
                }
            } else if(right)
            {
                dx += acc;
                if(dx > maxSpeed)
                {
                    dx = maxSpeed;
                }
            } else
            {
                if(dx > 0)
                {
                    dx -= deacc;
                    if(dx < 0)
                    {
                        dx = 0;
                    }
                }
                else if(dx < 0)
                {
                    dx += deacc;
                    if(dx > 0)
                    {
                        dx = 0;
                    }
                }
            }

            
         
            //jumping and falling
            if(Jumping && !Falling)
            {
                dy = JumpStart;
                Falling = true;
                IsGrounded = false;
            }

            if(Falling)
            {
                dy += FallSpeed;
                if (dy > 0) Jumping = false;
                if (dy < 0 && !Jumping) dy += GravAcc;
                if (dy > MaxFallSpeed) dy = MaxFallSpeed;
            }
            

        }

        protected abstract void InitParameters();

    }
}
