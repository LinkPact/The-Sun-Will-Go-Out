using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class Side_FlightTraining : Mission
    {
        private enum EventID
        {
            FirstCleared = 0,
            StartSecond = 1,
            SecondCleared = 2,
            StartThird = 3,
            LevelFailed = 4
        }

        public Side_FlightTraining(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            DualLaserWeapon weapon = new DualLaserWeapon(Game, ItemVariety.regular);
            RewardItems.Add(weapon);

            RestartAfterFail();

            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
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

            Station trainingArea = Game.stateManager.overworldState.GetStation("Training Area");

            AddDestination(trainingArea, 7);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "flightTraining_1", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.FirstCleared), GetEvent((int)EventID.LevelFailed), EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate { },
                delegate { },
                delegate { return GameStateManager.currentState.Equals("OverworldState"); },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.StartSecond), null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "flightTraining_2", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.SecondCleared), GetEvent((int)EventID.LevelFailed), EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate { },
                delegate { },
                delegate { return GameStateManager.currentState.Equals("OverworldState"); },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.StartThird), GetEvent((int)EventID.LevelFailed), EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "flightTraining_3", LevelStartCondition.TextCleared));
        }
    }
}
