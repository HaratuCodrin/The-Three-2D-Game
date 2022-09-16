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
    public class Cosmin : Player
    {
        private const int CROUCH = 0, DEATH = 1, IDLE = 2, JUMP = 3, WALK = 4;

        private int spoonEnergy;
        private int maxSpoonCount;
        private int spoonCost;
        private bool firingSpoons;        

        private float recoil = 5f;


        public List<Spoon> Spoons { get; private set; }
        

        private int[] numFrames = {
            3, 7, 5, 2, 6
        };

        public Cosmin(TileManager tm) : base(tm)
        {
            position = new Vector(100, 500);
            Load();
        }

        public Cosmin(TileManager tm, Vector spawn) : base(tm)
        {
            this.position = spawn;
            Load();
        }

        public override void animate()
        {
            if (Falling && !Jumping)
            {
                if (currentAction != JUMP)
                {
                    setAnimation(JUMP, animations.ElementAt(JUMP), 30);
                    ani.Hold = true;
                    width = 42;
                    height = 51;
                }
            }
            else if (Jumping)
            {
                if (currentAction != JUMP)
                {
                    setAnimation(JUMP, animations.ElementAt(JUMP), 30);
                    ani.Hold = true;
                    width = 42;
                    height = 51;
                }
            }
            else if (left || right)
            {
                if (currentAction != WALK)
                {
                    setAnimation(WALK, animations.ElementAt(WALK), 5);
                    ani.Hold = false;
                    width = 57;
                    height = 54;
                }
            }
            else if(down)
            {               
                if(currentAction != CROUCH)
                {                    
                    setAnimation(CROUCH, animations.ElementAt(CROUCH), 10);
                    ani.Hold = true;
                    width = 42;
                    height = 50;
                }
            }
            else
            {
                if (currentAction != IDLE)
                {
                    setAnimation(IDLE, animations.ElementAt(IDLE), 10);
                    ani.Hold = false;
                    width = 42;
                    height = 50;
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
            Spoons = new List<Spoon>();
            base.Load();
            sheets = new Image[numFrames.Length];
            abilityImages = new Image[2];
            abilityCooldowns = new int[] { 2, 10 };

            abilityImages[0] = Image.FromFile("HUD/Abilities/Cosmin_E.png");
            abilityImages[1] = Image.FromFile("HUD/Abilities/Cosmin_R.png");

            sheets[0] = Image.FromFile("Player/Cosmin/Cosmin_Crouch.png");
            sheets[1] = Image.FromFile("Player/Cosmin/Cosmin_Death.png");
            sheets[2] = Image.FromFile("Player/Cosmin/Cosmin_Idle.png");
            sheets[3] = Image.FromFile("Player/Cosmin/Cosmin_Jump.png");
            sheets[4] = Image.FromFile("Player/Cosmin/Cosmin_Run.png");

            Bounds = new CollisionBox(position, width, height);

            for (int i = 0; i < 5; i++)
            {
                Image[] tempArray = new Image[numFrames[i]];
                switch (i)
                {
                    case CROUCH:
                    case IDLE:
                        cutWidth = 28;
                        cutHeight = 33;
                        break;
                    case DEATH:
                        cutWidth = 35;
                        cutHeight = 33;
                        break;
                    case JUMP:
                        cutWidth = 28;
                        cutHeight = 34;
                        break;
                    case WALK:
                        cutWidth = 38;
                        cutHeight = 36;
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

            // render spoons
            for(int i = 0; i < Spoons.Count; i++)
            {
                Spoons.ElementAt(i).Render(g);
            }

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
                AttackBox.XOffset = Bounds.Width;
                if (currentAction == WALK)
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X - 12), (int)(position.Y + MapPosition.Y), width, height);
                else
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X), (int)(position.Y + MapPosition.Y), width, height);

            }
            else
            {
                AttackBox.XOffset = -AttackBox.Width;
                if (currentAction == WALK)
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X + width), (int)(position.Y + MapPosition.Y), -width, height);
                else
                    g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X + width), (int)(position.Y + MapPosition.Y), -width, height);
            }

            g.DrawRectangle(new Pen(new SolidBrush(Color.Green)), new Rectangle((int)(AttackBox.GetPosition().X + MapPosition.X), (int)(AttackBox.GetPosition().Y + MapPosition.Y), AttackBox.GetWidth(), AttackBox.GetHeight()));


            g.DrawRectangle(new Pen(new SolidBrush(Color.Blue)), new Rectangle((int)(HitBox.GetPosition().X + MapPosition.X), (int)(HitBox.GetPosition().Y + MapPosition.Y), HitBox.GetWidth(), HitBox.GetHeight()));
            // g.DrawString(spoons.Count + "", helpFont, new SolidBrush(Color.Black), 100, 100);
              g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), new Rectangle((int)(Bounds.GetPosition().X + MapPosition.X), (int)(Bounds.GetPosition().Y + MapPosition.Y), Bounds.GetWidth(), Bounds.GetHeight()));

        }

        public override void Update()
        {
            base.Update(); 
            for (int i = 0; i < Spoons.Count; i++)
            {                
                Spoons.ElementAt(i).Update();               
                
                if(Spoons.ElementAt(i).ShouldRemove)
                {
                    Spoons.RemoveAt(i);
                    i--;
                }
            }
        }


        public override void Move()
        {
            if(!firingSpoons)
            base.Move();          

            if (cds[0] > 0) cds[0]--;
            if (cds[1] > 0) cds[1]--;


            spoonEnergy++;
            if (spoonEnergy > maxSpoonCount) spoonEnergy = maxSpoonCount;

            if(cds[0] == 0)
            {
                if(AbilityE)
                {
                    firingSpoons = true;
                  //  startTimer = true;
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

            if (firingSpoons)
            {
                if (spoonEnergy > spoonCost)
                {
                    spoonEnergy -= spoonCost;
                    
                    Spoon tempSpoon;

                    if (facingRight)
                    {
                        GamePanel.sounds["spoonShot"].Play();
                        tempSpoon = new Spoon(tm, AttackBox.GetPosition(), true);
                        dx = -recoil;
                    }
                    else
                    {
                        GamePanel.sounds["spoonShot"].Play();
                        tempSpoon = new Spoon(tm, AttackBox.GetInversePosition(), false);
                        dx = recoil;
                    }

                    
                    Spoons.Add(tempSpoon);
                    firingSpoons = false;
                }
            }

            
        }

        protected override void InitParameters()
        {
            Health = MaxHealth = 4;          
            width = 42;
            height = 50;
            acc = 0.3f;
            maxSpeed = 2.6f;
            deacc = 1.6f;
            MaxFallSpeed = 6f;
            JumpStart = -6.8f;
            FallSpeed = 0.15f;
            GravAcc = 0.3f;
            maxSpoonCount = 100;
            spoonCost = 25;
        }
    }
}
