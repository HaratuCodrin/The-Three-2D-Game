using Bravos.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;

namespace Bravos
{
    public class GamePanel
    {

        public const int WIDTH = 320;
        public const int HEIGHT = 240;
        public const int SCALE = 2;
        private GameStateManager gsm;

        public static List<Shape> shapes = new List<Shape>();
        public Vector ScreenSize = new Vector(500, 500);
        public static String title = "The Three";
        public static Canvas Window = null;
        private Thread GameLoopThread = null;
        private Boolean running = true;
        public static int fps = 60;
        public static int ticks;

        public static Dictionary<string, SoundPlayer> soundtrack;
        public static Dictionary<string, Sound> sounds;


        public GamePanel(Vector ScreenSize, string title)
        {
            this.ScreenSize = ScreenSize;
            GamePanel.title = title;
            Window = new Canvas();
            Window.FormBorderStyle = FormBorderStyle.FixedSingle;
            Window.MaximizeBox = false;
            Window.MinimizeBox = false;
            Window.ClientSize = new Size((int)ScreenSize.X, (int)ScreenSize.Y);
            Window.Text = title;
            Window.Paint += Renderer; // suprascrie metodele din Form
            Window.KeyDown += KeyPressed;
            Window.KeyUp += KeyReleased;
            Load();


            GameLoopThread = new Thread(Run);
            GameLoopThread.SetApartmentState(ApartmentState.STA);
            GameLoopThread.Start();
            Application.Run(Window);
        }

        public static void RegisterShape(Shape s)
        {
            if (s != null)
            {
                shapes.Add(s);
            }
        }

        private void Run()
        {
           // Load();
           // frame rate
            double TPT = 1000000000 / fps;
            double delta = 0;
            long now;
            long lastTime = nanoTime();
            long timer = 0;
            ticks = 0;

            while (running)
            { // the game loop
                now = nanoTime();
                delta += (now - lastTime) / TPT;
                timer += now - lastTime;
                lastTime = now;

                if (delta >= 1)
                {
                    Update();
                    Window.BeginInvoke((MethodInvoker)delegate
                    { Window.Refresh(); });
                    ticks++;
                    delta--;
                }

                if (timer >= 1000000000)
                {
                    ticks = 0;
                    timer = 0;
                }
            }

        }

        public static long nanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }


        private void Renderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
          //  g.Clear(Color.White);
             gsm.Render(g);
        }

        public void Load()
        {

            soundtrack = new Dictionary<string, SoundPlayer>();
            sounds = new Dictionary<string, Sound>();

            sounds.Add("menuHover", new Sound("Sounds/retroClick.wav", 0.1f));
            sounds.Add("menuSelect", new Sound("Sounds/menu.wav"));
            sounds.Add("hit", new Sound("Sounds/smallHit.wav"));
            sounds.Add("spoonShot", new Sound("Sounds/spoon.wav"));

            soundtrack.Add("music", new SoundPlayer("Sounds/arcadeMusic.wav"));

           // Sound.Disabled = true;           
            gsm = GameStateManager.GetInstance();
        }
        public void Update()
        {
            gsm.Update();
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            gsm.KeyPressed(e);
        }

        private void KeyReleased(object sender, KeyEventArgs e)
        {
            gsm.KeyReleased(e);
        }

        public static Vector GetCenter()
        {
            return new Vector(WIDTH * SCALE / 2, HEIGHT * SCALE / 2);
        }

        public static Vector GetSize()
        {
            return new Vector(WIDTH * SCALE, HEIGHT * SCALE);
        }

        public static void PlaySound(String path)
        {
            SoundPlayer sound = new SoundPlayer(path);
            sound.Play();
        }
    }
}
