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

            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));

            RestartAfterFail();
        }

        public override void Initialize()
        {
            base.Initialize();
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

            if (EventBuffer.Count <= 0 && progress == 0 &&
                GameStateManager.currentState == "PlanetState" &&
                Game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            {
                progress = 1;
                Game.stateManager.shooterState.BeginLevel("AstroScan");
            }

            if (progress == 1 && GameStateManager.currentState != "ShooterState")
            {
                MissionManager.MarkMissionAsFailed(this.MissionName);
            }

            if (progress == 1 &&
                Game.stateManager.shooterState.GetLevel("AstroScan").IsObjectiveCompleted)
            {
                updateLogic = true;
                Game.messageBox.DisplayMessage(EventArray[0, 0]);
                MissionManager.MarkMissionAsCompleted(this.MissionName);
            }
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
