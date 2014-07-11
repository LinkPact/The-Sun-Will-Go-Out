using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    //Change class-name to the name of the mission 
    class DebtCollection : Mission
    {
        private bool failed;
        private bool success;

        public DebtCollection(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Fotrun Station I"),
                new EventTextCapsule(GetEvent(0), null, EventTextCanvas.BaseState)));

            objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetStation("Fotrun Station I"),
                new ResponseTextCapsule(GetEvent(1), GetAllResponses(1),
                    new List<System.Action>()
                    {
                        delegate 
                        {
                            if (StatsManager.Rupees >= 1000)
                            {
                                missionHelper.ShowEvent(GetEvent(2));
                                StatsManager.Rupees -= 1000;
                                success = true;
                            }

                            else
                            {
                                missionHelper.ShowEvent(GetEvent(6), false);
                            }
                        },

                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent(3));
                            failed = true;
                        },

                        delegate
                        {
                            missionHelper.ShowEvent(new List<EventText> { GetEvent(4), GetEvent(5) } );
                            success = true;
                        },

                        delegate
                        {
                            missionHelper.ShowEvent(new List<EventText> { GetEvent(4), GetEvent(5) } );
                            success = true;
                        }
                    })));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetPlanet("Highfence"), null, null,
                delegate { return (missionHelper.IsPlayerOnPlanet("Highfence") && success); },
                delegate { return (missionHelper.IsPlayerOnPlanet("Highfence") && failed); }));
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
        }

        public override void OnLoad()
        { }

        public override void MissionLogic()
        {
            base.MissionLogic();                                                                          
        }

        public override int GetProgress()
        {
            return progress;
        }

        public override void SetProgress(int progress)
        {
            this.progress = progress;
        }
    }
}
