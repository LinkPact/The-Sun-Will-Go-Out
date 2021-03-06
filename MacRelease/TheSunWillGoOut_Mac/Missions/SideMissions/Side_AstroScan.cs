﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class Side_AstroScan : Mission
    {
        private enum EventID
        {
            FlyBack = 0
        }

        public Side_AstroScan(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
            RestartAfterFail();
        }

        public override void Initialize()
        {
            base.Initialize();

            RewardItems.Add(new RegularShield(Game, ItemVariety.High));

            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;

            SetDestinations();
            SetupObjectives();
        }

        public override void OnLoad()
        { }

        public override void OnFailed()
        {
            base.OnFailed();

            PopupHandler.DisplayPortraitMessage(PortraitID.RebelTroopLeader, "[Mineral researcher] Too bad. Fly back to Lavis Station and talk to me to try again.");
        }

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

            GameObjectOverworld lavis = Game.stateManager.overworldState.GetSectorX.GetGameObject("Lavis");

            AddDestination(lavis, 3);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate 
                {
                    ((SubInteractiveObject)Game.stateManager.overworldState.GetSectorX.GetGameObject("Lavis")).OverrideEvent(
                        new DisplayTextOE("Fly through the asteroid field to scan it."));
                },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0]));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1], "AstroScan", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.FlyBack), null, EventTextCanvas.MessageBox)));
        }
    }
}
