using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bravos.Entities;
using Bravos.Tiles;
using Bravos.Tools;

namespace Bravos.Entities.PlayerTypes
{

    public class Dan : Player
    {

        private const int IDLE = 3, DYING = 1, JUMP = 4, WALK = 5, ATTACK = 0;

        private bool attacking;

        private int[] numFrames = {
            8, 13, 1, 2, 2, 6
        };

        public Dan(TileManager tm) : base(tm)
        {
            position = new Vector(100, 400);
            Load();
        }

        public Dan(TileManager tm, int width, int height, Vector spawn) : base(tm)
        {
            position = spawn;
            Load();
        }

        public override void animate()
        {
            if(currentAction == ATTACK)
            {
                if(ani.hasPlayedOnce())
                {
                    attacking = false;
                }
            }
            

            if(attacking)
            {
                if (currentAction != ATTACK)
                {
                    setAnimation(ATTACK, animations.ElementAt(ATTACK), 5);
                    ani.Hold = false;
                    width = 96;
                }
            }
            else if (Falling && !Jumping)
            {
                if (currentAction != JUMP)
                {
                    setAnimation(JUMP, animations.ElementAt(JUMP), -1);
                    width = 48;
                }
            }
            else if (Jumping)
            {
                if (currentAction != JUMP)
                {
                    setAnimation(JUMP, animations.ElementAt(JUMP), 30);
                    width = 48;
                }
            }
            else if (left || right)
            {
                if (currentAction != WALK)
                {
                    setAnimation(WALK, animations.ElementAt(WALK), 5);
                    width = 96;
                }
            }
            else
            {
                if (currentAction != IDLE)
                {
                    setAnimation(IDLE, animations.ElementAt(IDLE), 10);
                    width = 48;
                }
            }

            base.animate();

        }                 
        protected override void InitParameters()
        {
            Health = MaxHealth = 4;           
            width = 48;
            height = 48;
            acc = 0.3f;
            maxSpeed = 2.6f;
            deacc = 1.6f;
            MaxFallSpeed = 6f;
            JumpStart = -6.8f;
            FallSpeed = 0.15f;
            GravAcc = 0.3f;
        }
        
        public override void Load()
        {
            InitParameters();
            base.Load();
            
            sheets = new Image[numFrames.Length];
            abilityImages = new Image[2];
            abilityCooldowns = new int[] { 2, 15 };

            abilityImages[0] = Image.FromFile("HUD/Abilities/Dan_E.png");
            abilityImages[1] = Image.FromFile("HUD/Abilities/Dan_R.png");

            sheets[0] = Image.FromFile("Player/Knight/Hero-attack-Sheet.png");
            sheets[1] = Image.FromFile("Player/Knight/Hero-die-Sheet.png");
            sheets[2] = Image.FromFile("Player/Knight/Hero-hit-Sheet.png");
            sheets[3] = Image.FromFile("Player/Knight/Hero-idle-Sheet.png");
            sheets[4] = Image.FromFile("Player/Knight/Hero-jump-Sheet.png");
            sheets[5] = Image.FromFile("Player/Knight/Hero-walk-Sheet.png");


          
            Bounds = new CollisionBox(position, width, height);

            cutHeight = 24;
            cutWidth = 24;
            for (int i = 0; i < 6; i++)
            {
                Image[] tempArray = new Image[numFrames[i]];
                switch (i)
                {
                    case 0:
                    case 5:
                        cutWidth = 48;
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        cutWidth = 24;
                        break;
                }

                for (int j = 0; j < numFrames[i]; j++)
                {
                    tempArray[j] = Sprite.cutImage(sheets[i], j, 0, cutWidth, cutHeight);
                }
                animations.Add(tempArray);
            }


            setAnimation(IDLE, animations.ElementAt(IDLE), 10);
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

                if (currentAction == WALK || currentAction == ATTACK)
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X - 24), (int)(position.Y + MapPosition.Y), width, height);
                else
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X), (int)(position.Y + MapPosition.Y), width, height);

            }
            else
            {
             //   g.DrawRectangle(new Pen(new SolidBrush(Color.Green)), new Rectangle((int)(AttackBox.GetPosition().X + MapPosition.X - AttackBox.GetWidth()), (int)(AttackBox.GetPosition().Y + MapPosition.Y + AttackBox.YOffset), AttackBox.GetWidth(), AttackBox.GetHeight())) ;
                if (currentAction == WALK || currentAction == ATTACK)
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X + width - 24), (int)(position.Y + MapPosition.Y), -width, height);
                else
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X + width), (int)(position.Y + MapPosition.Y), -width, height);
            }

           // g.DrawRectangle(new Pen(new SolidBrush(Color.Blue)), new Rectangle((int)(HitBox.GetPosition().X + MapPosition.X + HitBox.XOffset), (int)(HitBox.GetPosition().Y + MapPosition.Y + HitBox.YOffset), HitBox.GetWidth(), HitBox.GetHeight()));


        }

        public override void Update()
        { 
            base.Update();       
        }

        public override void Move()
        {
            if (!attacking)
                base.Move();

            if (cds[0] > 0) cds[0]--;
            if (cds[1] > 0) cds[1]--;


            if (cds[0] == 0)
            {
                if (AbilityE)
                {
                    attacking = true;
                    //  startTimer = true;
                    dx = 0;
                    dy = 0;
                    AbilityE = false;
                    cds[0] = abilityCooldowns[0] * 60;
                }
            }

            if (cds[1] == 0)
            {
                if (AbilityR)
                {
                    cds[1] = abilityCooldowns[1] * 60;
                }
            }

        }

    }


}
