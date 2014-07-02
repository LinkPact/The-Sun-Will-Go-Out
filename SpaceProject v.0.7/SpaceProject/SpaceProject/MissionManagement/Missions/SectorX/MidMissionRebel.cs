using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MidMissionRebel: Mission
    {

        public MidMissionRebel(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            Game.stateManager.overworldState.getStation("Abandoned Station").Abandoned = false;
        }

        public override void OnLoad()
        {
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            //if (ObjectiveIndex == 0 &&
            //    Game.stateManager.currentGameState.Name == "StationState" &&
            //    Game.stateManager.stationState.Station.name == "Abandoned Station")
            //{
            //    missionHelper.ShowEvent(0);
            //    progress = 1;
            //}
            //
            //else if (ObjectiveIndex == 1 &&
            //    Game.stateManager.currentGameState.Name == "StationState" &&
            //    Game.stateManager.stationState.Station.name == "Blue Planet Station")
            //{
            //    missionHelper.ShowEvent(1);
            //    progress = 2;
            //}
            //
            //else if (ObjectiveIndex == 2 &&
            //    Game.stateManager.currentGameState.Name == "StationState" &&
            //    Game.stateManager.stationState.Station.name == "Abandoned Station")
            //{
            //    missionHelper.ShowEvent(2);
            //    progress = 3;
            //}
            //
            //else if (ObjectiveIndex == 3 &&
            //    Game.stateManager.currentGameState.Name == "OverworldState")
            //{
            //    ObjectiveIndex = 4;
            //}
            //
            //else if (ObjectiveIndex == 4 &&
            //    Game.stateManager.currentGameState.Name == "StationState" &&
            //    Game.stateManager.stationState.Station != null &&
            //    Game.stateManager.stationState.Station.name == "Abandoned Station")
            //{
            //    missionHelper.ShowEvent(3);
            //    progress = 5;
            //}
            //
            //else if (ObjectiveIndex == 5 &&
            //    Game.stateManager.stationState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            //{
            //    ObjectiveIndex = 6;
            //    Game.stateManager.shooterState.BeginLevel("mapCreator2");
            //}
            //
            //else if (ObjectiveIndex == 6 &&
            //    Game.stateManager.currentGameState.Name == "ShooterState" &&
            //    Game.stateManager.shooterState.GetLevel("mapCreator2").IsObjectiveCompleted)
            //{
            //    missionHelper.ShowEvent(4);
            //    Game.stateManager.GotoStationSubScreen("Abandoned Station", "Overview");
            //}
            //
            //else if (ObjectiveIndex == 6 &&
            //    Game.stateManager.currentGameState.Name == "StationState")
            //{
            //    MissionManager.MarkMissionAsCompleted(this.MissionName);
            //}
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
