using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class DeathByMeteorMission : Mission
    {

        public DeathByMeteorMission(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            EventArray = new string[2, 1];

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");
            EventArray[1, 0] = configFile.GetPropertyAsString(section, "EventText1", "");

            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));

            FlameShotWeapon flameShot = new FlameShotWeapon(Game, ItemVariety.high);
            RewardItems.Add(flameShot);

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

            String missionLevel = "DeathByMeteor";

            if (progress == 0
                && missionHelper.IsPlayerOnPlanet("Peye"))
            {
                ObjectiveIndex = 1;
                progress = 1;
                missionHelper.StartLevelAfterCondition(missionLevel, LevelStartCondition.TextCleared);
            }

            if (progress == 1 && GameStateManager.currentState == "OverworldState")
            {
                MissionManager.MarkMissionAsFailed(this.MissionName);
            }

            if (progress == 1 &&
                Game.stateManager.shooterState.GetLevel(missionLevel).IsObjectiveCompleted)
            {
                updateLogic = true;
                missionHelper.ShowEvent(new List<int> { 0, 1 });
                MissionManager.MarkMissionAsCompleted(this.MissionName);
                    
                Game.stateManager.ChangeState("OverwordState");
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
