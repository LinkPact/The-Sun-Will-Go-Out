using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class Side_DebtCollection : Mission
    {
        public Side_DebtCollection(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            SetDestinations();
            SetupObjectives();
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

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            GameObjectOverworld fortrunStaion1 =
                Game.stateManager.overworldState.GetStation("Fortrun Station I");

            destinations.Add(fortrunStaion1);
            destinations.Add(fortrunStaion1);

        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent(0), null, EventTextCanvas.BaseState)));

            objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[1],
                new ResponseTextCapsule(GetEvent(1), GetAllResponses(1),
                    new List<System.Action>()
                    {
                        delegate 
                        {
                            if (StatsManager.Rupees >= 1000)
                            {
                                missionHelper.ShowEvent(GetEvent(2));
                                StatsManager.Rupees -= 1000;
                                //success = true;
                            }
            
                            else
                            {
                                missionHelper.ShowEvent(GetEvent(6), false);
                            }
                        },
            
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent(3));
                            //failed = true;
                        },
            
                        delegate
                        {
                            missionHelper.ShowEvent(new List<EventText> { GetEvent(4), GetEvent(5) } );
                            //success = true;
                        },
            
                        delegate
                        {
                            missionHelper.ShowEvent(new List<EventText> { GetEvent(4), GetEvent(5) } );
                            //success = true;
                        }
                    }, EventTextCanvas.BaseState)));

            // SUBINTERACTIVE OBJECT MISSION TEST
            //objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
            //    Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
            //    new EventTextCapsule(GetEvent(0), null, EventTextCanvas.MessageBox)));

            //objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[1],
            //    Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
            //    new ResponseTextCapsule(GetEvent(1), GetAllResponses(1),
            //    new List<System.Action>()
            //        {
            //            delegate 
            //            {
            //                if (StatsManager.Rupees >= 1000)
            //                {
            //                    Game.messageBox.DisplayMessage(GetEvent(2).Text, 20);
            //                    StatsManager.Rupees -= 1000;
            //                    success = true;
            //                }
            //
            //                else
            //                {
            //                    Game.messageBox.DisplayMessage(GetEvent(6).Text, 20);
            //                }
            //            },
            //
            //            delegate
            //            {
            //                Game.messageBox.DisplayMessage(GetEvent(3).Text, 20);
            //                failed = true;
            //            },
            //
            //            delegate
            //            {
            //                Game.messageBox.DisplayMessage(new List<String> { GetEvent(4).Text, GetEvent(5).Text }, 20);
            //                success = true;
            //            },
            //
            //            delegate
            //            {
            //                Game.messageBox.DisplayMessage(new List<String> { GetEvent(4).Text, GetEvent(5).Text }, 20);
            //                success = true;
            //            }
            //        }, EventTextCanvas.MessageBox)));
            //
            //objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
            //    Game.stateManager.overworldState.GetPlanet("Highfence"), null, null,
            //    delegate { return (missionHelper.IsPlayerOnPlanet("Highfence") && success); },
            //    delegate { return (missionHelper.IsPlayerOnPlanet("Highfence") && failed); }));
        }
    }
}
