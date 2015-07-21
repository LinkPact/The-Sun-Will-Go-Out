using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public abstract class BossLevelEvent : LevelEvent
    {
        //private float finishDelay;
        private Boolean isInitialized;

        // Time passed since event start
        protected float timePassed;

        protected List<LevelEvent> waveEvents = new List<LevelEvent>();
        protected List<CreaturePackage> bossCreatures = new List<CreaturePackage>();
        protected List<CreaturePackage> untriggeredCreatures = new List<CreaturePackage>();

        public BossLevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, float time)
            : base(Game, player, spriteSheet, level, time)
        {
            //finishDelay = 3000;
            isInitialized = false;
        }

        public override void Run(GameTime gameTime)
        {
            if (!isInitialized)
            {
                PrivateInitialize();
            }

            if (TriggerStatus == Trigger.Running)
            {
                CreateCreatures();

                Boolean creaturesStillAlive = AreCreaturesAlive();

                if (!creaturesStillAlive && untriggeredCreatures.Count == 0)
                {
                    TriggerStatus = Trigger.Completed;
                }
            }
        }

        private void PrivateInitialize()
        {
            isInitialized = true;
            TriggerStatus = Trigger.Running;

            foreach (CreaturePackage package in bossCreatures)
            {
                untriggeredCreatures.Add(package);
            }
        }

        private void CreateCreatures()
        {
            List<CreaturePackage> removeList = new List<CreaturePackage>();
            foreach (CreaturePackage package in untriggeredCreatures)
            {
                if (package.StartPassed(timePassed))
                {
                    package.CreateCreature(level);
                    removeList.Add(package);
                }
            }
            foreach (CreaturePackage package in removeList)
            {
                untriggeredCreatures.Remove(package);
            }
            removeList.Clear();
        }

        private Boolean AreCreaturesAlive()
        {
            Boolean creaturesStillAlive = false;

            foreach (CreaturePackage creature in bossCreatures)
            {
                if (Game.stateManager.shooterState.gameObjects.Contains((GameObjectVertical)creature.RetrieveCreature()))
                {
                    creaturesStillAlive = true;
                    break;
                }
            }

            return creaturesStillAlive;
        }

        protected void CompileEvents()
        {
            foreach (LevelEvent event_ in waveEvents)
            {
                List<CreaturePackage> temp = new List<CreaturePackage>();

                if (event_.RetrieveCreatures() != null)
                    temp = event_.RetrieveCreatures();

                foreach (CreaturePackage package in temp)
                {
                    bossCreatures.Add(package);
                }
            }
        }
    }
}
