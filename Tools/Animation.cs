using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bravos.Tools
{
    public class Animation
    {
        private Image[] Frames;

        private int CurrentFrame;
        private int NumFrames;

        private int Count;
        private int Delay;

        public bool Hold { get; set; }

        public int TimesPlayed { get; set; }

        public Animation(Image[] frames)
        {
            TimesPlayed = 0;
            SetFrames(frames);
        }

        public Animation()
        {
            TimesPlayed = 0;
        }
 
        public void SetFrames(Image[] frames)
        {
            this.Frames = frames;
            CurrentFrame = 0;
            Count = 0;
            TimesPlayed = 0;
            Delay = 0;
            NumFrames = frames.Length;
        }


        public void SetDelay(int Delay)
        {
            this.Delay = Delay;
        }

        public void Update()
        {
            if (Delay == -1) return;
            Count++;

            if (Hold)
            {
                if(!hasPlayedOnce())
                if (Count == Delay)
                {
                    CurrentFrame++;
                    Count = 0;
                }


                if (CurrentFrame == NumFrames)
                {
                    CurrentFrame = NumFrames-1;
                    //   if (TimesPlayed > 100)
                    TimesPlayed++;
                }
            }
            else
            {
                if (Count == Delay)
                {
                    CurrentFrame++;
                    Count = 0;
                }


                if (CurrentFrame == NumFrames)
                {
                    CurrentFrame = 0;
                    //   if (TimesPlayed > 100)
                    TimesPlayed++;
                }
            }
        }

        public static float easeInOutSine(float t)
        {
            return (float)(-(Math.Cos(Math.PI * t) - 1) / 2);
        }


        public static float random(float a, float b)
        {
            Random random = new Random();
            float number = (float)(random.NextDouble() * (b - a) + a);
            return number;
        }

        public static Vector rotatedVector(Vector v, double rotAngle)
        {
            float xRot = (float)(v.X * Math.Cos(rotAngle)) - (float)(v.Y * Math.Sin(rotAngle));
            float yRot = (float)(v.X * Math.Sin(rotAngle)) + (float)(v.Y * Math.Cos(rotAngle));
            return new Vector(xRot, yRot);
        }
       

        public int GetDelay()
        {
            return Delay;
        }

        public int getCurrentFrame()
        {
            return CurrentFrame;
        }

        public int GetCount()
        {
            return Count;
        }

        public Image getImage() { return Frames[CurrentFrame]; }

        public bool hasPlayedOnce()
        {
            return TimesPlayed > 0;
        }

        public bool hasPlayed(int i) { return TimesPlayed == i; }

    }
}
