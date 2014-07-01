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
            EventArray = new string[1, 1];

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");   //index 0,0

            RewardItems.Add(new DrillBeamWeapon(Game));

            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));

            RestartAfterFail();
        }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.getPlanet("Lavis")));
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.getPlanet("Lavis"), "AstroScan", LevelStartCondition.Immediately,
                new List<int> { 0 }, EventTextFormat.MessageBox));
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
