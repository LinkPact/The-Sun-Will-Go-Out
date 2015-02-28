using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class Side_FlightTraining : Mission
    {
        private enum EventID
        {
            FirstCleared = 0,
            SecondCleared = 1
        }

        public Side_FlightTraining(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            SideMissilesWeapon sideMissiles = new SideMissilesWeapon(Game, ItemVariety.regular);
            RewardItems.Add(sideMissiles);

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

            Station borderStation = Game.stateManager.overworldState.GetStation("Border Station");

            destinations.Add(borderStation);
            destinations.Add(borderStation);
            destinations.Add(borderStation);
            destinations.Add(borderStation);
            destinations.Add(borderStation);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "flightTraining_1", LevelStartCondition.EnteringOverworld));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.FirstCleared), null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "flightTraining_2", LevelStartCondition.EnteringOverworld));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.SecondCleared), null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "flightTraining_3", LevelStartCondition.EnteringOverworld));
        }
    }
}
