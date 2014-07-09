using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class FlightTraining : Mission
    {
        private enum EventID
        {
            FirstCleared = 0,
            SecondCleared = 1
        }

        public FlightTraining(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            SideMissilesWeapon sideMissiles = new SideMissilesWeapon(Game, ItemVariety.regular);
            RewardItems.Add(sideMissiles);

            Station borderStation = Game.stateManager.overworldState.getStation("Border Station");

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                "flightTraining_1", LevelStartCondition.EnteringOverworld));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                new EventTextCapsule(GetEvent((int)EventID.FirstCleared), null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                "flightTraining_2", LevelStartCondition.EnteringOverworld));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                new EventTextCapsule(GetEvent((int)EventID.SecondCleared), null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                "flightTraining_3", LevelStartCondition.EnteringOverworld));

            RestartAfterFail();
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
    }
}
