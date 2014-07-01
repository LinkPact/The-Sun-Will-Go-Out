using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MidMissionAlliance: Mission
    {
        private AllianceFleet allianceFleet;

        public MidMissionAlliance(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            EventArray = new string[4, 1];

            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText3", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText4", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText5", ""));

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", ""); 
            EventArray[1, 0] = configFile.GetPropertyAsString(section, "EventText2", "");
            EventArray[2, 0] = configFile.GetPropertyAsString(section, "EventText3", "");
            EventArray[3, 0] = configFile.GetPropertyAsString(section, "EventText4", "");
        }

        public override void Initialize()
        {
            base.Initialize();

            allianceFleet = new AllianceFleet(this.Game, spriteSheet);
            allianceFleet.Initialize();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
        }

        public override void OnLoad()
        {
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            //if (ObjectiveIndex == 0 && Game.stateManager.overworldState.GetCurrentSpaceRegion ==
            //    Game.stateManager.overworldState.GetOutpostX)
            //{
            //    ObjectiveIndex = 1;
            //    Game.messageBox.DisplayMessage(EventArray[0, 0]);
            //}
            //
            //else if (ObjectiveIndex == 1 && (GameStateManager.currentState == "StationState" &&
            //    Game.stateManager.stationState.Station.name.Equals("Federation Station")))
            //{
            //    missionHelper.ShowEvent(1);
            //    progress = 2;
            //    Game.stateManager.overworldState.AddOverworldObject(allianceFleet);
            //}
            //
            //else if (ObjectiveIndex == 2 && Game.player.Bounds.Intersects(allianceFleet.Bounds) &&
            //    ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && GameStateManager.currentState == "OverworldState"))
            //{
            //    ObjectiveIndex = 3;
            //    Game.stateManager.shipStationState.TextBuffer.Add(EventArray[2, 0]);
            //    Game.stateManager.shipStationState.StateBuffer.Add("ShooterState");
            //    Game.stateManager.shipStationState.LevelToStart = "DanneLevel";
            //    Game.stateManager.shipStationState.LoadShipData(allianceFleet);
            //    Game.stateManager.ChangeState("ShipStationState");
            //}
            //
            //else if (ObjectiveIndex == 3 &&
            //    Game.stateManager.shooterState.GetLevel("DanneLevel").IsObjectiveCompleted)
            //{
            //    ObjectiveIndex = 4;
            //    Game.stateManager.shipStationState.TextBuffer.Add(EventArray[3, 0]);
            //    Game.stateManager.shipStationState.LoadShipData(allianceFleet);
            //    Game.stateManager.shipStationState.ReturnState = "OverworldState";
            //    Game.stateManager.ChangeState("ShipStationState");
            //}
            //
            //else if (ObjectiveIndex == 4 && (GameStateManager.currentState == "StationState" &&
            //    Game.stateManager.stationState.Station.name.Equals("Federation Station")))
            //{
            //    MissionManager.MarkMissionAsCompleted(this.MissionName);
            //
            //    allianceFleet.IsUsed = false;
            //    Game.stateManager.overworldState.RemoveOverworldObject(allianceFleet);
            //}
            //
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
