using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class PlayTime
    {
        private float playTime;
        public float PlayTime { get { return playTime; } set { playTime = value; } }

        public void Update(GameTime gameTime)
        {
            playTime += gameTime.ElapsedGameTime.Milliseconds;
        }

        public float GetPlayTime()
        {
            return playTime;
        }

        public void SetPlayTime(float playTime)
        {
            this.playTime = playTime;
        }

        public float GetFutureTime(float milliSeconds)
        {
            return playTime + milliSeconds;
        }

        public bool HasTimePassed(float time)
        {
            return playTime >= time;
        }
    }
}
