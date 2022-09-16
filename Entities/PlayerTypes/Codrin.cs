using Bravos.Tiles;
using Bravos.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bravos.Entities.PlayerTypes
{
    public class Codrin : Player
    {
        private const int DASH = 0, IDLE = 1, JUMP = 2, WALK = 3;
        
        private bool dashing;
        private float dashStartSpeed;        
        private int currentDashTimer;      

      
        private int[] numFrames = {
            5, 4, 10, 6
        };
        

        public Codrin(TileManager tm) : base(tm)
        {
            position = new Vector(100, 500);
            Load();
        }

        public Codrin(TileManager tm, Vector spawn) : base(tm)
        {
            this.position = spawn;
            Load();
        }

        public override void animate()
        {

            if(currentAction == DASH)
            {
                if(ani.hasPlayedOnce())
                {
                    dashing = false;
                }
            }

           if (dashing)
            {
                if (currentAction != DASH)
                {
                    setAnimation(DASH, animations.ElementAt(DASH), 5);
                    ani.Hold = false;
                    width = 54;
                    height = 56;
                }
            }
            else if (Falling && !Jumping)
            {
                if (currentAction != JUMP)
                {
                    setAnimation(JUMP, animations.ElementAt(JUMP), 5);
                    width = 53;
                    height = 63;
                }
            }
            else if (Jumping)
            {
                if (currentAction != JUMP)
                {
                    setAnimation(JUMP, animations.ElementAt(JUMP), 8);
                    ani.Hold = true;
                    width = 53;
                    height = 63;
                }
            }
           
            else if (left || right)
            {
                if (currentAction != WALK)
                {
                    setAnimation(WALK, animations.ElementAt(WALK), 8);
                    ani.Hold = false ;
                    width = 52;
                    height = 56;
                }                
            }          
            else
            {
                if (currentAction != IDLE)
                {
                    setAnimation(IDLE, animations.ElementAt(IDLE), 15);
                    ani.Hold = false;
                    width = 35;
                    height = 56;
                }
            }

            base.animate();
        }

        public override void KeyPressed(KeyEventArgs e)
        {
            base.KeyPressed(e);          
        }

        public override void KeyReleased(KeyEventArgs e)
        {
            base.KeyReleased(e);           
        }

        public override void Load()
        {
            InitParameters();
            base.Load();
            sheets = new Image[numFrames.Length];
            abilityImages = new Image[2];
            abilityCooldowns = new int[] { 2, 10 };

            abilityImages[0] = Image.FromFile("HUD/Abilities/Codrin_E.png");
            abilityImages[1] = Image.FromFile("HUD/Abilities/Codrin_R.png");

            sheets[0] = Image.FromFile("Player/Codrin/Codrin_Dash.png");
            sheets[1] = Image.FromFile("Player/Codrin/Codrin_Idle.png");
            sheets[2] = Image.FromFile("Player/Codrin/Codrin_Jump.png");
            sheets[3] = Image.FromFile("Player/Codrin/Codrin_Walk.png");
            


            Bounds = new CollisionBox(position, width, height);

            for (int i = 0; i < numFrames.Length; i++)
            {
                Image[] tempArray = new Image[numFrames[i]];
                switch (i)
                {                   
                    case IDLE:
                        cutWidth = 17;
                        cutHeight = 30;
                        break;                    
                    case JUMP:
                        cutWidth = 27;
                        cutHeight = 35;
                        break;
                    case WALK:
                        cutWidth = 26;
                        cutHeight = 28;
                        break;
                    case DASH:
                        cutWidth = 36;
                        cutHeight = 28;
                        break;

                }

                for (int j = 0; j < numFrames[i]; j++)
                {
                    tempArray[j] = Sprite.cutImage(sheets[i], j, 0, cutWidth, cutHeight);
                }

                animations.Add(tempArray);
            }

            setAnimation(IDLE, animations.ElementAt(IDLE), 20);

        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            if (Flinching)
            {
                long elapsed = (GamePanel.nanoTime()) / 1000000 / 2;
                if (elapsed / 100 % 2 == 0)
                {
                    return;
                }
            }

            if (facingRight)
            {
             //   g.DrawRectangle(new Pen(new SolidBrush(Color.Green)), new Rectangle((int)(AttackBox.GetPosition().X + MapPosition.X + AttackBox.XOffset), (int)(AttackBox.GetPosition().Y + MapPosition.Y + AttackBox.YOffset), AttackBox.GetWidth(), AttackBox.GetHeight()));               
                if (currentAction == WALK)
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X), (int)(position.Y + MapPosition.Y), width, height);
                else
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X), (int)(position.Y + MapPosition.Y), width, height);
            }
            else
            {
                //  g.DrawRectangle(new Pen(new SolidBrush(Color.Green)), new Rectangle((int)(AttackBox.GetPosition().X + MapPosition.X - AttackBox.GetWidth()), (int)(AttackBox.GetPosition().Y + MapPosition.Y + AttackBox.YOffset), AttackBox.GetWidth(), AttackBox.GetHeight()));
                
                if (currentAction == WALK)
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X + width), (int)(position.Y + MapPosition.Y), -width, height);
                else
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X + width + 12), (int)(position.Y + MapPosition.Y), -width, height);
            }

          //  g.DrawRectangle(new Pen(new SolidBrush(Color.Blue)), new Rectangle((int)(HitBox.GetPosition().X + MapPosition.X + HitBox.XOffset), (int)(HitBox.GetPosition().Y + MapPosition.Y + HitBox.YOffset), HitBox.GetWidth(), HitBox.GetHeight()));

        }

        public override void Move()
        {
            if(!dashing)
            base.Move();

            // dashing
            if (currentDashTimer >= 50) currentDashTimer = 50;
            currentDashTimer++;                    

            if (cds[0] > 0) cds[0]--;
            if (cds[1] > 0) cds[1]--;         

            if(cds[0] == 0)
            {
                if (AbilityE)
                {
                    dashing = true;                   
                    currentDashTimer = 0;
                    dx = 0;
                    dy = 0;
                    AbilityE = false;
                    cds[0] = abilityCooldowns[0] * 60;
                }
            }

            if(cds[1] == 0)
            {
                if(AbilityR)
                {
                    cds[1] = abilityCooldowns[1] * 60;
                }
            }


            if (dashing)
            {
                if (facingRight)
                    dx = dashStartSpeed;
                else
                    dx = -dashStartSpeed;

                if(currentDashTimer >= 50)
                {
                    dashing = false;
                }             
            }

        }

        public override void Update()
        {
            base.Update();
        }

        protected override void InitParameters()
        {
            Health = MaxHealth = 4;           
            width = 52;
            height = 56;
            acc = 0.3f;
            maxSpeed = 2.6f;
            deacc = 1.6f;
            MaxFallSpeed = 6f;
            JumpStart = -6.8f;
            FallSpeed = 0.15f;
            GravAcc = 0.3f;
            dashStartSpeed = 7.2f;                     
        }
    }


}

