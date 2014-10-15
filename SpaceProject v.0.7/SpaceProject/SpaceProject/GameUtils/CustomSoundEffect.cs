using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

// Wrapper class for XNA 'SoundEffect'

namespace SpaceProject
{
    public class CustomSoundEffect
    {
        #region variables
        private int maxInstances;

        private SoundEffect soundEffect;
        private List<SoundEffectInstance> instances;
        private List<SoundEffectInstance> playingInstances;
        private List<SoundEffectInstance> stoppedInstances;
        private bool fadeOut;

        #endregion

        #region properties
        public TimeSpan Duration 
        { 
            get
            { 
                {
                    if (soundEffect != null)
                    {
                        return soundEffect.Duration;
                    }
                    else
                    {
                        throw new Exception("Sound effect has not been created.");
                    }
                } 
            }
            private set { ;}
        }

        public bool IsDisposed
        {
            get
            {
                {
                    if (soundEffect != null)
                    {
                        return soundEffect.IsDisposed;
                    }
                    else
                    {
                        throw new Exception("Sound effect has not been created.");
                    }
                }

            }
            private set { ;}
        }

        public bool FadeOut
        {
            get { return fadeOut; }
            set 
            {
                if (playingInstances.Count > 0)
                {
                    fadeOut = value;
                }

                else
                {
                    fadeOut = false;
                }
            }
        }

        #endregion

        public CustomSoundEffect(SoundEffect soundEffect, int maxInstances)
        {
            this.soundEffect = soundEffect;
            this.maxInstances = maxInstances;
            instances = new List<SoundEffectInstance>();
            playingInstances = new List<SoundEffectInstance>();
            stoppedInstances = new List<SoundEffectInstance>();
        }

        public SoundEffectInstance CreateInstance()
        {
            if (OkayToCreateNewInstance())
            {
                SoundEffectInstance instance = soundEffect.CreateInstance();
                instances.Add(instance);

                return instance;
            }

            return null;
        }

        public void Dispose()
        {
            soundEffect.Dispose();
        }

        public void Play()
        {
            soundEffect.Play();
        }

        public void Play(float volume, float pitch, float pan)
        {
            soundEffect.Play(volume, pitch, pan);
        }

        public void Update()
        {
            if (fadeOut)
            {
                foreach (SoundEffectInstance instance in playingInstances)
                {
                    if (instance.Volume > 0.05f)
                    {
                        instance.Volume -= 0.04f;
                    }

                    if (instance.Volume <= 0.05f)
                    {
                        instance.Stop();
                        instance.Dispose();
                        stoppedInstances.Add(instance);
                    }
                }

                foreach (SoundEffectInstance instance in stoppedInstances)
                {
                    instances.Remove(instance);
                }

                stoppedInstances.Clear();

                if (playingInstances.Count <= 0)
                {
                    fadeOut = false;
                }
            }
        }

        // Disposes all current instances of this sound effect
        public void DisposeInstances()
        {
            for (int i = 0; i < instances.Count; i++)
            {
                instances[i].Stop();
                instances[i].Dispose();
                stoppedInstances.Add(instances[i]);
            }

            instances.Clear();
            stoppedInstances.Clear();
        }

        // Checks if number of playing instances does not exceed maximum allowed instances.
        private bool OkayToCreateNewInstance()
        {
            playingInstances.Clear();

            for (int i = 0; i < instances.Count; i++)
            {
                if (instances[i].State == SoundState.Playing)
                {
                    playingInstances.Add(instances[i]);
                }

                else if (instances[i].State == SoundState.Stopped)
                {
                    stoppedInstances.Add(instances[i]);
                }
            }

            for (int i = 0; i < stoppedInstances.Count; i++)
            {
                instances.Remove(stoppedInstances[i]);
                stoppedInstances[i].Dispose();
            }

            stoppedInstances.Clear();

            if (playingInstances.Count >= maxInstances)
            {
                return false;
            }

            return true;
        }

    }
}
