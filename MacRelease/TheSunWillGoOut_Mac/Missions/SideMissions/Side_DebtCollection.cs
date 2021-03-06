﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
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
                Game.stateManager.overworldState.GetStation("Fortrun Station");

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
                            if (StatsManager.Crebits >= 1000)
                            {
                                missionHelper.ShowEvent(GetEvent(2), true);
                                StatsManager.Crebits -= 1000;
                            }
            
                            else
                            {
                                missionHelper.ShowEvent(GetEvent(6), false);
                            }
                        },
            
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent(3), true);
                            MissionManager.MarkMissionAsFailed(this.MissionID);
                        },
            
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent(5), true);
                            missionHelper.ShowEvent(GetEvent(4), true);
                        },
            
                        delegate
                        {
                            missionHelper.ShowEvent(GetEvent(5), true);
                            missionHelper.ShowEvent(GetEvent(4), true);
                        }
                    }, EventTextCanvas.BaseState)));
        }
    }
}
