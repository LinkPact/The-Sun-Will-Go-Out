using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class Side_FlightTraining2 : Mission
    {
        private enum EventID
        {
            FirstCleared = 0,
            StartSecond = 1,
            SecondCleared = 2,
            StartThird = 3,
            ThirdCleared = 4,
            StartFourth = 5,
            FourthCleared = 6,
            StartFifth = 7,
            LevelFailed = 8,
            Question = 9,
            DeclineResponse = 10
        }

        public Side_FlightTraining2(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            AdvancedEnergyCell cell = new AdvancedEnergyCell(Game, ItemVariety.high);
            RewardItems.Add(cell);

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

            Station trainingArea2 = Game.stateManager.overworldState.GetStation("Training Area 2");

            int missionDataSlots = 9;
            AddDestination(trainingArea2, missionDataSlots);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            // ft2_1, ft2_2 ...
            // Old: flightTraining_1, flightTraining_2 ..

            // First level
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "ft2_1", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.FirstCleared), GetEvent((int)EventID.LevelFailed), EventTextCanvas.BaseState)));

            objectives.Add(new AskToProgressObjective(Game, this, ObjectiveDescriptions[0],
                GetEvent((int)EventID.Question), GetEvent((int)EventID.StartSecond),
                GetEvent((int)EventID.DeclineResponse), LocationName));

            // Second level
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "ft2_2", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.SecondCleared), GetEvent((int)EventID.LevelFailed), EventTextCanvas.BaseState)));

            objectives.Add(new AskToProgressObjective(Game, this, ObjectiveDescriptions[0],
                GetEvent((int)EventID.Question), GetEvent((int)EventID.StartThird),
                GetEvent((int)EventID.DeclineResponse), LocationName));

            // Third level
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "ft2_3", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.ThirdCleared), GetEvent((int)EventID.LevelFailed), EventTextCanvas.BaseState)));

            objectives.Add(new AskToProgressObjective(Game, this, ObjectiveDescriptions[0],
                GetEvent((int)EventID.Question), GetEvent((int)EventID.StartFourth),
                GetEvent((int)EventID.DeclineResponse), LocationName));

            // Fourth level
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "ft2_4", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.FourthCleared), GetEvent((int)EventID.LevelFailed), EventTextCanvas.BaseState)));

            objectives.Add(new AskToProgressObjective(Game, this, ObjectiveDescriptions[0],
                GetEvent((int)EventID.Question), GetEvent((int)EventID.StartFifth),
                GetEvent((int)EventID.DeclineResponse), LocationName));

            // Fifth level
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "ft2_5", LevelStartCondition.TextCleared,
                new EventTextCapsule(null, GetEvent((int)EventID.LevelFailed), EventTextCanvas.BaseState)));
        }
    }
}
