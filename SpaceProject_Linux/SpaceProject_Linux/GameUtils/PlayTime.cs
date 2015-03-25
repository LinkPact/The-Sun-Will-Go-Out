using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    public class PlayTime
    {
        private float overallPlayTime;
        public float OverallPlayTime { get { return overallPlayTime; } set { overallPlayTime = value; } }

        private float overworldTime;
        public float OverworldTime { get { return overworldTime; } set { overworldTime = value; } }

        private float shooterPartTime;
        public float ShooterPartTime { get { return shooterPartTime; } set { shooterPartTime = value; } } 

        public void Update(GameTime gameTime)
        {
            overallPlayTime += gameTime.ElapsedGameTime.Milliseconds;

            if (GameStateManager.currentState.Equals("OverworldState"))
            {
                overworldTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            else if (GameStateManager.currentState.Equals("ShooterState"))
            {
                shooterPartTime += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        public float GetFuturePlayTime(float milliseconds)
        {
            return overallPlayTime + milliseconds;
        }

        public float GetFutureOverworldTime(float milliseconds)
        {
            return overworldTime + milliseconds;
        }

        public float GetFutureShooterPartTime(float milliseconds)
        {
            return shooterPartTime + milliseconds;
        }

        public bool HasPlayTimePassed(float time)
        {
            return overallPlayTime >= time;
        }

        public bool HasShooterPartTimePassed(float time)
        {
            return shooterPartTime >= time;
        }

        public bool HasOverworldTimePassed(float time)
        {
            return overworldTime >= time;
        }
    }
}
