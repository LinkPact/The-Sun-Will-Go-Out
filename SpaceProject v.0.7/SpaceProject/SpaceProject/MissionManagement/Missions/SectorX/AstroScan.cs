using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class AstroScan : Mission
    {
        public AstroScan(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            RestartAfterFail();
        }

        public override void Initialize()
        {
            base.Initialize();

            RewardItems.Add(new DrillBeamWeapon(Game));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.getPlanet("Lavis")));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.getPlanet("Lavis"), "AstroScan", LevelStartCondition.Immediately,
                new EventTextCapsule(new List<String>{EventArray[0, 0]}, null, EventTextCanvas.MessageBox)));
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
