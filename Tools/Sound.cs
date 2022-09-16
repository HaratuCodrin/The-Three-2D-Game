using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Bravos.Tools
{
    public class Sound
    {
        public WaveStream stream { get; private set; }

        public WaveOut soundPlayer { get; private set; }

        public float Volume { set; get; }

        public static bool Disabled;

        public Sound(string path)
        {
            this.stream = new AudioFileReader(path);
            this.soundPlayer = new WaveOut();
            soundPlayer.Init(stream);
            this.Volume = soundPlayer.Volume;
        }

        public Sound(string path, float volume)
        {
            this.Volume = volume;
            this.stream = new AudioFileReader(path);
            this.soundPlayer = new WaveOut();
            soundPlayer.Init(stream);

            SetVolume(volume);
        }

        public void Play()
        {
            if (Disabled) return;
            if (soundPlayer.PlaybackState is PlaybackState.Playing) soundPlayer.Stop();
            stream.CurrentTime = new TimeSpan(0L);
            soundPlayer.Play();
        }


        public void SetVolume(float volume)
        {
            this.Volume = volume;
            this.soundPlayer.Volume = volume;
        }

    }
}
