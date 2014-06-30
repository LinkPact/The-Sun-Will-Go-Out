using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main1_AColdWelcome: Mission
    {
        private Battlefield battlefield;
        private bool died;
        private int tempTimer;

        public Main1_AColdWelcome(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            EventArray = new string[5, 1];

            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText3", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText4", ""));

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");
            EventArray[1, 0] = configFile.GetPropertyAsString(section, "EventText2", "");
            EventArray[2, 0] = configFile.GetPropertyAsString(section, "EventText3", "");
            EventArray[3, 0] = configFile.GetPropertyAsString(section, "EventText4", "");
            EventArray[4, 0] = configFile.GetPropertyAsString(section, "EventText5", "");
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            battlefield = new Battlefield(Game, spriteSheet);
            battlefield.Initialize();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;

            missionHelper.ShowEvent(new List<int>{0, 1, 2});
            Game.stateManager.GotoStationSubScreen("Border Station", "Overview");

            Game.stateManager.overworldState.AddOverworldObject(battlefield);
        }

        public override void OnLoad()
        {
            Game.stateManager.overworldState.AddOverworldObject(battlefield);

            if (progress == 2)
            {
                objectiveDestination = battlefield;
            }

            else if (progress == 3)
            {
                objectiveDestination = Game.stateManager.overworldState.getStation("Border Station");
            }
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            if (progress == 0 && GameStateManager.currentState.Equals("StationState") &&
                missionHelper.IsTextCleared())
            {
                ObjectiveIndex = 1;
                progress = 1;
            }

            if (progress == 1 && GameStateManager.currentState.Equals("OverworldState"))
            {
                ObjectiveIndex = 2;
                progress = 2;
                objectiveDestination = battlefield;
            }


            if (progress == 2 && GameStateManager.currentState.Equals("OverworldState") &&
                CollisionDetection.IsPointInsideRectangle(Game.player.position, battlefield.Bounds) &&
                ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
            {
                Game.stateManager.shooterState.BeginLevel("FirstMissionLevel");
                Game.messageBox.DisplayMessage("Avoid the astroids and collect resources.");
            }

            if (GameStateManager.currentState.Equals("ShooterState") &&
                Game.stateManager.shooterState.CurrentLevel.Name.Equals("FirstMissionLevel"))
            {
                if (Game.stateManager.shooterState.CurrentLevel.IsGameOver)
                {
                    died = true;
                    tempTimer = 5;
                    ((FirstMissionLevel)Game.stateManager.shooterState.GetLevel("FirstMissionLevel")).SuppliesCount = 0;
                }
            }

            if (died && GameStateManager.currentState.Equals("OverworldState"))
            {
                tempTimer--;

                if (tempTimer < 0)
                {
                    Game.messageBox.DisplayMessage("Fei Yan: Too bad. Try again! Return to Border Station if you need to repair your ship.");
                    died = false;
                    ObjectiveIndex = 2;
                    progress = 2;
                }
            }

            if (progress == 2 &&
                missionHelper.IsLevelCompleted("FirstMissionLevel"))
            {
                died = false;

                ObjectiveIndex = 3;
                progress = 3;
                Game.messageBox.DisplayMessage(EventArray[3, 0]);
                objectiveDestination = Game.stateManager.overworldState.getStation("Border Station");
            }

            if (progress == 3 
                && missionHelper.IsPlayerOnStation("Border Station"))
            {
                if (((FirstMissionLevel)Game.stateManager.shooterState.GetLevel("FirstMissionLevel")).SuppliesCount >= 6)
                {
                    TitaniumResource titanium = new TitaniumResource(Game, 100);
                    RewardItems.Add(titanium);

                    missionHelper.ShowEvent(4);
                }

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
