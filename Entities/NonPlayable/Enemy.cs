using Bravos.Tiles;
using Bravos.Tools;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Bravos.Entities
{
    public enum ID
    {
        Cat,
        Bird,
        Dog
    }
    public abstract class Enemy : PlatformerEntity
    {

        protected Player player;
        protected Sprite sprite;        

        public bool Hurt { get; set; }

        public Enemy(TileManager tm, Player player) : base(tm)
        {
            this.player = player;


        }



        public override void Load()
        {
            base.Load();
            HitBox = new CollisionBox(position, width / 2, height / 2, width / 4, height / 4);
            AttackBox = new CollisionBox(position, width / 3, height / 3, width, height / 3);
        }

        public override void Update()
        {
            base.Update();
            animate();
            Move();
            CheckTileMapCollision();           
            SetPosition(Bounds.Xtemp, Bounds.Ytemp);

            if (collides(player.HitBox) && !player.Flinching)
            {
                GamePanel.sounds["hit"].Play();
                player.Flinching = true;
                if(player.Health > 0)
                    player.Health--;
            }
        }

        protected abstract void Move();
        protected abstract void InitParameters();

    }




}
