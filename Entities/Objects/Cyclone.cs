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

    internal class Particle : Entity
    {
        private float tween;
        private float time;
        public SolidBrush Brush { get; set; }

        public int Zone { get; set; }
        public bool IsSimple { get; set; }

        private Cyclone cyclone;

        public Particle(float x, float y, TileManager tm, SolidBrush brush, bool simple) : base(tm)
        {
            IsSimple = simple;
            Load();
            this.position = new Vector(x, y);
            this.Brush = brush;
            this.dy = -10;
        }

        public Particle(Cyclone cyclone, float x, float y, TileManager tm, SolidBrush brush, bool simple) : base(tm)
        {
            IsSimple = simple;
            this.cyclone = cyclone;
            Load();
            this.position = new Vector(x, y);
            this.Brush = brush;
            if (!IsSimple)
                this.dy = -1;
            else
                this.dy = -5;
        }

        public override void Load()
        {
            width = 7;
            height = 7;
            
            if (!IsSimple)
            {
                Random random = new Random();
                time = (float)Math.Floor(0.5f + (float)random.NextDouble());
                tween = (float)((random.NextDouble() * 0.04) + 0.02);
            }
        }

        public override void Render(Graphics g)
        {
            SetMapPosition();
            if (NotOnScreen()) return;
            g.FillEllipse(Brush, (float)(position.X + MapPosition.X), (float)(position.Y + MapPosition.Y), width, height);
        }

        public override void Update()
        {
            if (IsSimple) MoveSimple();
            else MoveSinus();
        }

        public void MoveSimple()
        {
            position.X += dx;
            position.Y += dy;
        }

        public void MoveSinus()
        {
            position.Y += dy;
            if(time > 1)
            {
                time = 1;
                tween *= -1;
            }

            if(time < -1)
            {
                time = -1;
                tween *= -1;
            }

            time += tween;
            position.X = (Zone - cyclone.Width) * Animation.easeInOutSine(time) + cyclone.Position.X + cyclone.Width;

        }
       
    }

    public class Cyclone
    {
        private List<Particle> verticalParticles;
        private List<Particle> sinusParticles;
        private TileManager Tm;

        public int spawnRate { get; set; }
        public int timer { get; set; }

        const int Psize = 5;

        public Vector Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Alpha { set; get; }

        public SolidBrush Brush { get; set; }

        public Cyclone(int alpha, TileManager Tm, Vector position, int width, int height)
        {
            this.Alpha = alpha;
            this.Tm = Tm;
            this.Width = width;
            this.Height = height;
            this.Position = position;
            Brush = new SolidBrush(Color.FromArgb(Alpha, Color.White));
            Load();
        }

        public Cyclone(int alpha, TileManager Tm, Vector pos)
        {
            this.Alpha = alpha;
            Position = pos;
            this.Tm = Tm;
            Brush = new SolidBrush(Color.FromArgb(Alpha, Color.White));
            Load();
        }

        public void Load()
        {            
            verticalParticles = new List<Particle>();
            sinusParticles = new List<Particle>();
            spawnParticles();
            spawnRate = 20;

        }

        public void Render(Graphics g)
        {
            for (int i = 0; i < verticalParticles.Count; i++)
            {
                Particle particle = verticalParticles[i];
                particle.Render(g);
            }

            for (int i = 0; i < sinusParticles.Count; i++)
            {
                Particle particle = sinusParticles[i];
                particle.Render(g);
            }

        }


        public void Update()
        {
            timer++;
            if(timer == spawnRate)
            {
                spawnParticles();
                timer = 0;
            }


            for (int i = 0; i < verticalParticles.Count; i++)
            {
                Particle particle = verticalParticles[i];
                particle.Update();
                if (particle.GetPosition().Y < Position.Y)
                {
                    verticalParticles.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < sinusParticles.Count; i++)
            {
                Particle particle = sinusParticles[i];
                particle.Update();
                if (particle.GetPosition().Y < Position.Y)
                {
                    sinusParticles.RemoveAt(i);
                    i--;
                }
            }


        }

        private void spawnParticles()
        {
            Random random = new Random();
            verticalParticles.Add(new Particle(this,
                                               Animation.random((float)Position.X, (float)Position.X + Width - Psize),
                                               (float)(Position.Y + Height - Psize),
                                               Tm,
                                               Brush,
                                               true)
                                  );

            sinusParticles.Add(new Particle(this,
                                               Animation.random((float)Position.X, (float)Position.X + Width - Psize),
                                               (float)(Position.Y + Height - Psize),
                                               Tm,
                                               Brush,
                                               false)
                                  );
        }


    }


}
