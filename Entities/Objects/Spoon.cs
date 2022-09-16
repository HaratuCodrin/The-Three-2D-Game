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
     public class Spoon : Entity
    {

        private readonly Image sprite = Image.FromFile("Objects/Spoon.png");

        private bool hit;
        public Vector origin;
        public bool ShouldRemove { get; set; }

        private int distanceToTravel;
        
        public Spoon(TileManager tm, bool right) : base(tm)
        {
            acc = 3.8f;
            this.right = right;
            if (right) dx = acc;
            else dx = -acc;
            Load();
        }
        public Spoon(TileManager tm, Vector position, bool right) : base(tm)
        {
            acc = 6.8f;
            this.right = right;
            this.position = position;
            this.origin = new Vector(position.X, position.Y);
            if (right) dx = acc;
            else dx = -acc;
            Load();
        }

        public void SetHit()
        {
            if (hit) return;
            hit = true;
            dx = 0;
        }
      
        public override void Load()
        {
            width = 56;
            height = 14;
            Bounds = new CollisionBox(position, width, height);
            cutWidth = 560;
            cutHeight = 140;
            distanceToTravel = (int)GamePanel.GetSize().X / 2;
        }

        public override void Render(Graphics g)
        {
            SetMapPosition();
            if (NotOnScreen()) return;
            if (right)
            {
                    g.DrawImage(sprite, (int)(position.X + MapPosition.X), (int)(position.Y + MapPosition.Y), width, height);                
            }
            else
            {               
                    g.DrawImage(sprite, (int)(position.X + MapPosition.X + width), (int)(position.Y + MapPosition.Y), -width, height);            
            }

        }

        

        public override void Update()
        {
            CheckTileMapCollision();
            SetPosition(Bounds.Xtemp, Bounds.Ytemp);

            if(dx == 0 && !hit) { SetHit(); }

            if(right)
            {
                if(position.X - origin.X > distanceToTravel && !hit)
                {
                    SetHit();
                }
            } else
            {
                if (origin.X - position.X > distanceToTravel && !hit)
                {
                    SetHit();
                }
            }

            if(hit)
            {
                ShouldRemove = true;
            }

        }
    }
}
