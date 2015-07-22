using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    //Event type which creates "waves" of creatures, which will continue
    //passing over the screen until they are shot down.
    public abstract class WavesLevelEvent : LevelEvent
    {
        #region declaration
        
        //Time passed since start of each wave
        protected float timePassed;

        //Uncompiled list with the event contained in the instances
        protected List<LevelEvent> waveEvents = new List<LevelEvent>();

        //Compiled list with the creaturepackets for the event
        protected List<CreaturePackage> waveCreatures = new List<CreaturePackage>();

        //Temporary list used for triggering of each wave
        //Gets smaller when creatures are killed
        protected List<CreaturePackage> untriggeredCreatures = new List<CreaturePackage>();

        protected bool isTriggered;
        public bool IsTriggered { get { return isTriggered; } }

        #endregion

        protected WavesLevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, float startTime)
            : base(Game, player, spriteSheet, level, startTime)
        {
            hasDuration = true;
            timePassed = 0;
        }

        public override void Run(GameTime gameTime)
        {
            //Used to check if there are alive creatures still on the screen
            bool creatureAlive = false;

            timePassed += gameTime.ElapsedGameTime.Milliseconds;

            //Checks for creatures ready to be triggered.
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

            //Check if there are any wavecreatures alive in the shooterstate.
            foreach (CreaturePackage creature in waveCreatures)
            {
                if (Game.stateManager.shooterState.gameObjects.Contains((GameObjectVertical)creature.RetrieveCreature()))
                {
                    creatureAlive = true;
                    break;
                }
            }

            if (!creatureAlive && untriggeredCreatures.Count == 0)
            {
                if (waveCreatures.Count > 0)
                    NewWave();
                else
                    EndEvent();
            }
        }

        //Tries creating a new wave when no creatures are left in shooterstate.
        //Cancels the wave if all creatures are killed.
        public void NewWave()
        {
            timePassed = 0;
            List<CreaturePackage> deadCreatures = new List<CreaturePackage>();

            foreach (CreaturePackage package in waveCreatures)
            {
                if (package.RetrieveCreature().IsKilled)
                    deadCreatures.Add(package);
            }
            foreach (CreaturePackage deadPack in deadCreatures)
                waveCreatures.Remove(deadPack);

            deadCreatures.Clear();

            if (waveCreatures.Count > 0)
            {
                foreach (CreaturePackage package in waveCreatures)
                {
                    untriggeredCreatures.Add(package);
                }
            }
            else
            {
                TriggerStatus = Trigger.Completed;
            }
        }

        //Extracts individual creatures and timings from levelevent.
        //So: Start with list of 
        //pointlevelevents -> compile -> list with creatures, identical to events.
        protected void CompileEvents()
        {
            foreach (LevelEvent event_ in waveEvents)
            {
                List<CreaturePackage> temp = new List<CreaturePackage>();

                if (event_.RetrieveCreatures() != null)
                    temp = event_.RetrieveCreatures();

                foreach (CreaturePackage package in temp)
                {
                    waveCreatures.Add(package);
                }
            }
        }
    }
}
