using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceProject.MapCreator;

namespace SpaceProject
{
    public abstract class PointLevelEvent : LevelEvent
    {
        //Regular method for random position.
        protected PointLevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, EnemyType identifier,
            float startTime)
            : base(Game, player, spriteSheet, level, identifier, startTime)
        {
            this.xPos = -1f;
            hasDuration = false;
        }

        //Regular method for determined position.
        protected PointLevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, EnemyType identifier,
            float startTime, float xPos)
            : base(Game, player, spriteSheet, level, identifier, startTime)
        {
            this.xPos = xPos;
            hasDuration = false;
        }

        //Method used when event should be manually triggered, by for example Swarm event.
        protected PointLevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, EnemyType identifier)
            : base(Game, player, spriteSheet, level, identifier)
        {
            this.xPos = -1f;
            hasDuration = false;
        }

        public void SetX(float x)
        {
            xPos = x;
        }

        public override void Run(GameTime gameTime)
        {
            TriggerStatus = Trigger.Completed;
        }

        public override List<CreaturePackage> RetrieveCreatures() 
        { 
            return null; 
        }
    }
}
