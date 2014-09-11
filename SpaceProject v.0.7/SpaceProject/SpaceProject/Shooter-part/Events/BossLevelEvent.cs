using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class BossLevelEvent : LevelEvent
    {
        private float finishDelay;
        private Boolean isInitialized;

        protected List<CreaturePackage> untriggeredCreatures = new List<CreaturePackage>();



        public BossLevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, float time)
            : base(Game, player, spriteSheet, level, time)
        {
            finishDelay = 3000;
            isInitialized = false;

            
        }

        public override void Run(GameTime gameTime)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                xPos = 100;

                enemyType = EnemyType.turret;
                CreateCreature(xPos);

                TriggerStatus = Trigger.Running;
            }
            else if (finishDelay > 0)
            {
                finishDelay -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                TriggerStatus = Trigger.Completed;
            }
        }

    }
}
