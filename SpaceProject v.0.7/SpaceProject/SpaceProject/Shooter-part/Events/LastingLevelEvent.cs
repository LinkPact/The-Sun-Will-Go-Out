using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceProject.MapCreator;

namespace SpaceProject
{
    public abstract class LastingLevelEvent : LevelEvent
    {
        protected float timePassed;
        
        protected bool isTriggered;
        public bool IsTriggered { get { return isTriggered; } }

        private float duration;
        public float Duration { get { return duration; } }

        protected LastingLevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, EnemyType identifier,
            float startTime, float duration)
            : base(Game, player, spriteSheet, level, identifier, startTime)
        {
            this.duration = duration;

            hasDuration = true;
            timePassed = 0;
        }

        protected LastingLevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, PointLevelEvent pointLevelEvent,
            float startTime, float duration)
            : base(Game, player, spriteSheet, level, pointLevelEvent.EnemyType, startTime)
        {
            this.duration = duration;

            hasDuration = true;
            timePassed = 0;
        }

        public override void Run(GameTime gameTime)
        {
            if (timePassed >= duration) TriggerStatus = Trigger.Completed;
        }

        public Boolean IsTimeDuringEvent(float time)
        { 
            if (time > startTime && time < startTime + duration)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CutDown(float newStartTime)
        {
            timePassed = newStartTime - startTime;
        }
    }
}
