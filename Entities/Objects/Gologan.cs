using Bravos.Tiles;
using Bravos.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bravos.Entities
{

    public class Gologan : Entity
    {
        public const int Argint = 3, Cupru = 1, Aur = 5, Rubin = 25, Smarald = 50;

        public readonly Dictionary<int, String> typesOfGologani = new Dictionary<int, string>() {
            {Argint, "Argint.png"},
            {Cupru, "Cupru.png"},
            {Aur, "Aur.png"},
            {Rubin, "Rubin.png"},
            {Smarald, "Smarald.png"}
        };
       
        public int Type { get; set; }

        public Gologan(TileManager tm, Vector position, int type) : base(tm)
        {
            this.position = position;
            this.Type = type;
            Load();
        }

        public Gologan(TileManager tm, double x, double y, int type) : base(tm)
        {
            this.position = new Vector(x, y);
            this.Type = type;
            Load();
        }

        public override void Load()
        {
            Image animationSheet = Image.FromFile("Objects/Gologani/"+typesOfGologani[Type]);
             
                cutWidth = cutHeight = 16;
                width = height = 24;
                Bounds = new CollisionBox(position, width, height);
            
           
            int numCols = animationSheet.Width / cutWidth;
            Image[] frames = new Image[numCols];
            for(int i = 0; i < numCols; i++)
            {
                frames[i] = Sprite.cutImage(animationSheet, i, 0, cutWidth, cutHeight);
            }

            ani = new Animation(frames);
            ani.SetDelay(5);
           
        }

        public override void Render(Graphics g)
        {
            SetMapPosition();
            if (NotOnScreen()) return;

           
            g.DrawImage(ani.getImage(), (int)(position.X + MapPosition.X), (int)(position.Y + MapPosition.Y), width, height);
            



        }

        public override void Update()
        {
            ani.Update();
        }

    }
}
