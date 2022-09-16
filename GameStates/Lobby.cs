using Bravos.Entities;
using Bravos.Entities.Objects;
using Bravos.Tiles;
using Bravos.Tools;
using System.Drawing;
using System.Windows.Forms;

namespace Bravos
{
    public class Lobby : GameState
    {
       
        private TileManager Tm;
        private LobbyPlayer player;
        private string playerName; 

        private Cyclone cyclone;



        public Lobby(GameStateManager gsm) : base(gsm)
        {
            Load();
        }

        public Lobby(GameStateManager gsm, double x, double y, int cointAmount, string Name, string Character, int Progress) : base(gsm)
        {
            Load();
        }

        public override void KeyPressed(KeyEventArgs e)
        {
            player.KeyPressed(e);
        }

        public override void KeyReleased(KeyEventArgs e)
        {
            player.KeyReleased(e);
            if (e.KeyCode == Keys.Escape) gsm.SetState(GameStateManager.MENU);
        }

        public override void Load()
        {
            Tm = new TileManager("Maps/BaseTileMapLobby.xml");
            player = new LobbyPlayer(Tm, new Sprite(Image.FromFile("Player/LobbyCodrin.png"), 32, 48, 1), 32, 48);
            cyclone = new Cyclone(200, Tm, new Vector(500, 500), 150, 300);
            cyclone.spawnRate = 2;
        }

        public override void Render(Graphics g)
        {       
            Tm.Render(g);
            player.Render(g);
            cyclone.Render(g);
        }

        public override void Update()
        {
            player.Update();
            Tm.SetPosition(GamePanel.WIDTH*GamePanel.SCALE/2 - player.GetPosition().X,
                GamePanel.HEIGHT*GamePanel.SCALE/2 - player.GetPosition().Y);
            cyclone.Update();
        }
    }

}