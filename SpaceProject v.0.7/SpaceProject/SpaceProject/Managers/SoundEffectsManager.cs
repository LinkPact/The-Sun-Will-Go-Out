using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public enum SoundEffects
    {
        // Weapons
        BasicLaser,
        Test1,
        Test2,
        Test3,
        Test4,
        Test5,

        // Explosions
        MuffledExplosion,
        SmallExplosion,

        // Engine
        OverworldEngine,

        // Ambience
        Crowd
    }

    public class SoundEffectsManager
    {
        public static bool LoadSoundEffects;

        private Game1 game;

        private List<CustomSoundEffect> soundEffects;
        private List<SoundEffectInstance> soundEffectBuffer;

        private List<SoundEffectInstance> stoppedSoundEffects;

        private bool muted;
        private float volume;

        private CustomSoundEffect test1;
        private CustomSoundEffect test2;
        private CustomSoundEffect test3;
        private CustomSoundEffect test4;
        private CustomSoundEffect test5;
        
        private CustomSoundEffect basicLaser;
        private CustomSoundEffect muffledExplosion;
        private CustomSoundEffect smallExplosion;

        private CustomSoundEffect overworldEngine;

        private CustomSoundEffect crowd;

        public SoundEffectsManager(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            // Load settings
            muted = game.settingsFile.GetPropertyAsBool("sound", "mutesound", true);
            volume = game.settingsFile.GetPropertyAsFloat("sound", "soundvolume", 1);
            LoadSoundEffects = game.settingsFile.GetPropertyAsBool("sound", "loadsound", true);

            soundEffects = new List<CustomSoundEffect>();
            soundEffectBuffer = new List<SoundEffectInstance>();
            stoppedSoundEffects = new List<SoundEffectInstance>();

            if (LoadSoundEffects)
            {
                test1 = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/laser3"), 10);
                test2 = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/laser4_0"), 10);
                test3 = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/laser6"), 10);
                test4 = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/laser7"), 10);
                test5 = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/laser8"), 10);

                soundEffects.Add(test1);
                soundEffects.Add(test2);
                soundEffects.Add(test3);
                soundEffects.Add(test4);
                soundEffects.Add(test5);

                basicLaser = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/laser1"), 10);
                muffledExplosion = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/boom6"), 10);
                smallExplosion = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/boom9"), 10);

                soundEffects.Add(basicLaser);
                soundEffects.Add(muffledExplosion);
                soundEffects.Add(smallExplosion);

                overworldEngine = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/engine1"), 1);

                soundEffects.Add(overworldEngine);

                crowd = new CustomSoundEffect(game.Content.Load<SoundEffect>("SoundEffects/crowd1"), 1);

                soundEffects.Add(crowd);
            }
        }

        // Plays specified sound effect with random pitch
        public void PlaySoundEffect(SoundEffects identifier, float pan)
        {
            if (!muted && LoadSoundEffects && soundEffectBuffer.Count < 32)
            {
                int i = (int)identifier;
            
                SoundEffectInstance instance = soundEffects[i].CreateInstance();

                if (instance != null)
                {
                    instance.Volume = volume;

                    if (pan > 1)
                    {
                        pan = 1;
                    }
                    else if (pan < -1)
                    {
                        pan = -1;
                    }

                    instance.Pan = pan;
                    instance.Pitch = (float)StaticFunctions.GetRandomValue() * 0.5f;
                    instance.Play();

                    soundEffectBuffer.Add(instance);
                }
            }
        }

        public void PlaySoundEffect(SoundEffects identifier, float pan, float pitch)
        {
            if (!muted && soundEffectBuffer.Count < 32)
            {
                int i = (int)identifier;

                SoundEffectInstance instance = soundEffects[i].CreateInstance();

                if (instance != null)
                {
                    instance.Volume = volume;

                    if (pan > 1)
                    {
                        pan = 1;
                    }
                    else if (pan < -1)
                    {
                        pan = -1;
                    }

                    instance.Pan = pan;
                    instance.Pitch = pitch;
                    instance.Play();

                    soundEffectBuffer.Add(instance);
                }
            }
        }

        public void LoopSoundEffect(SoundEffects identifier, float pan, float pitch)
        {
            if (!muted && LoadSoundEffects && soundEffectBuffer.Count < 32)
            {
                int i = (int)identifier;

                SoundEffectInstance instance = soundEffects[i].CreateInstance();

                if (instance != null)
                {
                    instance.Volume = volume;
                    instance.Pan = pan;
                    instance.Pitch = pitch;
                    instance.IsLooped = true;
                    instance.Play();

                    soundEffectBuffer.Add(instance);
                }
            }
        }

        // Stops all instances of the specified sound effect
        public void StopSoundEffect(SoundEffects identifier)
        {
            if (!muted && LoadSoundEffects)
            {
                int i = (int)identifier;

                soundEffects[i].DisposeInstances();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (soundEffectBuffer.Count > 0)
            {
                if (soundEffectBuffer[0].IsDisposed)
                {
                    soundEffectBuffer.Remove(soundEffectBuffer[0]);
                }

                else if (soundEffectBuffer[0].State == SoundState.Stopped)
                {
                    stoppedSoundEffects.Add(soundEffectBuffer[0]);
                    soundEffectBuffer.Remove(soundEffectBuffer[0]);
                }
            }
        }

        public void SetSoundVolume(float volume)
        {
            if (volume > 1.0f)
                volume = 1.0f;

            else if (volume < 0.0f)
                volume = 0.0f;

            this.volume = volume;
        }

        public float GetSoundVolume()
        {
            return volume;
        }

        public void SwitchSoundMuted()
        {
            muted = !muted;
        }

        public void SetSoundMuted(bool val)
        {
            muted = val;
        }

        public bool isSoundMuted()
        {
            return muted;
        }

        public void DisposeSoundEffect()
        {
            for (int i = 0; i < soundEffects.Count; i++)
            {
                soundEffects[i].DisposeInstances();
                soundEffects[i].Dispose();
            }

            for (int i = 0; i < stoppedSoundEffects.Count; i++)
            {
                if (!stoppedSoundEffects[i].IsDisposed)
                {
                    stoppedSoundEffects[i].Dispose();
                }
            }

            for (int i = 0; i < soundEffectBuffer.Count; i++)
            {
                SoundEffectInstance instance = soundEffectBuffer[i];
                if (!instance.IsDisposed)
                {
                    instance.Stop();
                    instance.Dispose();
                }
            }

            soundEffects.Clear();
            stoppedSoundEffects.Clear();
        }
    }
}
