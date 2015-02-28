using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
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

            RewardItems.Add(new BasicLaserWeapon(Game, ItemVariety.high));
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

            GameObjectOverworld lavis = Game.stateManager.overworldState.GetPlanet("Lavis");

            destinations.Add(lavis);
            destinations.Add(lavis);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                destinations[0]));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                destinations[1], "AstroScan", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.FlyBack), null, EventTextCanvas.MessageBox)));
        }
    }
}
