using Bravos.Entities;
using Bravos.Entities.Objects;
using Bravos.Entities.PlayerTypes;
using Bravos.Tiles;
using Bravos.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Bravos.GameStates
{
    class Level1 : GameState
    {

        private Player player;
        private TileManager Tm;
        private Background bg;
        private Background clouds;
        private DumbEnemy dumb;
        private Cyclone cyclone;

        private TrapWheel[] wheels;

        private SoundPlayer sp = new SoundPlayer("Sounds/arcadeMusic.wav");

        public Level1(GameStateManager gsm) : base(gsm)
        {
            player = new Dan(Tm);
            Load();
        }

        public Level1(GameStateManager gsm, String ChosenPlayer) : base(gsm)
        {
            Tm = new TileManager("Maps/LevelTest.xml");

            switch (ChosenPlayer)
            {
                case "Dan":
                    player = new Dan(Tm);
                    break;
                case "Cosmin":
                    player = new Cosmin(Tm);
                    break;
                case "Codrin":
                    player = new Codrin(Tm);
                    break;
                default:                   
                    player = new Dan(Tm);
                 break;
            }                     
            player.SetPosition(50, 750);
            Load();

        }

        public override void KeyPressed(KeyEventArgs e)
        {
            player.KeyPressed(e);
            if (e.KeyCode == Keys.Escape)
            {
               // GamePanel.soundtrack["music"].Stop();
                gsm.CurtainCall(GameStateManager.MENU);
            }
        }

        public override void KeyReleased(KeyEventArgs e)
        {
            player.KeyReleased(e);
        }

        public override void Load()
        {    
            collectables = new List<Gologan>();
            
            bg = new Background("Images/grassbg1.gif", 40);
            bg.setPosition(new Vector(0, 0));

            wheels = new TrapWheel[6];

            int wheelX = 400;

            for(int i = 0; i < wheels.Length; i++)
            {
                wheels[i] = new TrapWheel(Tm, player, wheelX + i * 128, 700, 96, 96, TrapWheel.UpDown);
                wheels[i].Range = 6f;
                wheels[i].Moving = true;
                if (i % 2 == 0) wheels[i].time = -1;
                else wheels[i].time = 1;
            }

                     

           // GamePanel.soundtrack["music"].PlayLooping();

            Image image = Sprite.cutImage(Image.FromFile("Images/grassbg1.gif"), 0, 0, 320, 320 / 3);
            clouds = new Background(image, 40);
            clouds.setPosition(new Vector(0, 0));
            clouds.setVelocity(0.15, 0);

            collectables.Add(new Gologan(Tm, 400, 800, Gologan.Argint));
            collectables.Add(new Gologan(Tm, 430, 800, Gologan.Rubin));
            collectables.Add(new Gologan(Tm, 460, 800, Gologan.Aur));
            collectables.Add(new Gologan(Tm, 490, 800, Gologan.Smarald));
            collectables.Add(new Gologan(Tm, 510, 800, Gologan.Cupru));
            collectables.Add(new Gologan(Tm, 540, 800, Gologan.Argint));
            collectables.Add(new Gologan(Tm, 570, 800, Gologan.Aur));

            cyclone = new Cyclone(200, Tm, new Vector(192, 0), 160, 192);
            cyclone.spawnRate = 2;
        }

        public override void Render(Graphics g)
        {          
            bg.Render(g);
            clouds.Render(g);
            Tm.Render(g);
            for (int i = 0; i < collectables.Count; i++)
            {
                collectables.ElementAt(i).Render(g);                
            }
            player.Render(g);

            foreach(TrapWheel wheel in wheels)
            {
                wheel.Render(g);
            }

            cyclone.Render(g);
        }

        public override void Update()
        {
            clouds.Update();
            player.Update();
            foreach (TrapWheel wheel in wheels)
            {
                wheel.Update();
            }
            for (int i = 0; i < collectables.Count; i++)
            {
                collectables.ElementAt(i).Update();
                if (player.hitCollision(collectables.ElementAt(i).GetBounds()) && !player.Flinching) {
                    player.CoinAmount += collectables.ElementAt(i).Type;
                    collectables.RemoveAt(i);
                    i--;
                }
            }
            cyclone.Update();

            Tm.SetPosition(GamePanel.WIDTH * GamePanel.SCALE / 2 - player.GetPosition().X,
                GamePanel.HEIGHT * GamePanel.SCALE / 2 - player.GetPosition().Y);

        }
    }
}
