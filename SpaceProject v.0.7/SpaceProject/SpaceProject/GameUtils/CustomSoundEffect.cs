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

        public void UpdateSoundEffect(SoundEffect soundEffect)
        {
            this.soundEffect = soundEffect;
        }

        public SoundEffectInstance CreateInstance()
        {
            if (maxInstances == 1
                && IsSoundEffectPlaying())
            {
                return null;
            }

            SoundEffectInstance instance = soundEffect.CreateInstance();
            instances.Insert(0, instance);

            return instance;
        }

        public void Dispose()
        {
            soundEffect.Dispose();
        }

        public void Update()
        {
            if (fadeOut)
            {
                foreach (SoundEffectInstance instance in playingInstances)
                {
                    if (!instance.IsDisposed)
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
                }

                if (playingInstances.Count <= 0)
                {
                    fadeOut = false;
                }
            }

            CheckInstanceCount();

            foreach (SoundEffectInstance ins in stoppedInstances)
            {
                instances.Remove(ins);
            }

            stoppedInstances.Clear();
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

        private void CheckInstanceCount()
        {
            playingInstances.Clear();

            for (int i = 0; i < instances.Count; i++)
            {
                if (instances[i].IsDisposed
                    || instances[i].State == SoundState.Stopped)
                {
                    stoppedInstances.Add(instances[i]);
                }

                else if (instances[i].State == SoundState.Playing)
                {
                    playingInstances.Add(instances[i]);
                }
            }

            if (playingInstances.Count > maxInstances)
            {
                instances[playingInstances.Count - 1].Volume = 0f;
                instances[playingInstances.Count - 1].Stop(true);
            }
        }

        private bool IsSoundEffectPlaying()
        {
            foreach (SoundEffectInstance instance in instances)
            {
                if (instance.State == SoundState.Playing)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
