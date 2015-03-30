using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public abstract class Swarm : LastingLevelEvent
    {
        #region declaration

        private float spawnDelay;
        protected float SpawnDelay { get { return spawnDelay; } set { spawnDelay = value; } }
        private float spawnTime;        
        private PointLevelEvent singleEvent;

        #endregion

        //Test-constructor meant for Swarm-superclass
        protected Swarm(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, float duration) :
            base (Game, player, spriteSheet, level, identifier, startTime, duration)
        { 
            
        }

        protected Swarm(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            PointLevelEvent singleEvent, float startTime, float duration) :
            base(Game, player, spriteSheet, level, singleEvent, startTime, duration)
        {
            this.singleEvent = singleEvent;
        }

        public override void Run(GameTime gameTime)
        {
            base.Run(gameTime);

            timePassed += gameTime.ElapsedGameTime.Milliseconds;
            spawnTime += gameTime.ElapsedGameTime.Milliseconds;

            if (TriggerStatus == Trigger.Running)
            {
                if (spawnTime >= spawnDelay)
                {
                    float testWidth = level.LevelWidth - 2 * xPadding;

                    float xPos = xPadding + (float)(random.NextDouble() * (level.LevelWidth - 2 * xPadding));

                    if (singleEvent == null)
                    {
                        CreateCreature();
                        spawnTime = 0;
                    }
                    else
                    {
                        PointLevelEvent event_ = singleEvent;
                        event_.SetX(random.Next(level.LevelWidth));
                        event_.Run(gameTime);

                        spawnTime = 0;
                    }
                }
            }
        }
    }
}
