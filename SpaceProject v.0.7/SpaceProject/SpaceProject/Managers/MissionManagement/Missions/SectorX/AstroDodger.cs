using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class AstroDodger: Mission
    {
        private DestroyedShip destroyedShip;

        public AstroDodger(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            EventArray = new string[3, 1];

            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");
            EventArray[1, 0] = configFile.GetPropertyAsString(section, "EventText2", "");
            EventArray[2, 0] = configFile.GetPropertyAsString(section, "EventText3", "");
        }

        public override void Initialize()
        {
            base.Initialize();

            destroyedShip = new DestroyedShip(this.Game, spriteSheet);
            destroyedShip.Initialize();
        }

        public override void StartMission()
        {
            progress = 0;
            ObjectiveIndex = 0;

            destroyedShip.IsUsed = true;
            Game.stateManager.overworldState.AddOverworldObject(destroyedShip);
        }

        public override void OnLoad()
        {
            if (!Game.stateManager.overworldState.ContainsOverworldObject(destroyedShip) &&
                (MissionState == StateOfMission.Active || MissionState == StateOfMission.Completed ||
                MissionState == StateOfMission.Failed))
            {
                destroyedShip.IsUsed = true;
                Game.stateManager.overworldState.AddOverworldObject(destroyedShip);
            }
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            if (progress == 0 && 
                Game.player.Bounds.Intersects(destroyedShip.Bounds) &&
                ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) &&
                GameStateManager.currentState == "OverworldState"))
            {
                Game.stateManager.shipStationState.TextBuffer.Add(EventArray[0, 0]);
                Game.stateManager.shipStationState.TextBuffer.Add(EventArray[1, 0]);
                Game.stateManager.shipStationState.StateBuffer.Add("ShooterState");
                //Game.stateManager.shipStationState.LevelToStart = "MeteorLevel";
                Game.stateManager.shipStationState.LevelToStart = "AstroDodger";
                Game.stateManager.ChangeState("ShipStationState");
                Game.stateManager.shipStationState.LoadShipData(destroyedShip);                
            }

            if (progress == 0 &&
                missionHelper.IsLevelCompleted("AstroDodger"))
            {
                progress = 1;
                ObjectiveIndex = 1;
                Game.stateManager.shipStationState.TextBuffer.Add(EventArray[2, 0]);
                Game.stateManager.shipStationState.LoadShipData(destroyedShip);
                Game.stateManager.shipStationState.ReturnState = "OverworldState";
                Game.stateManager.ChangeState("ShipStationState");
            }

            if (progress == 1 &&
                missionHelper.IsPlayerOnStation("Soelara Station"))
            {
                MissionManager.MarkMissionAsCompleted(this.MissionName);

                destroyedShip.IsUsed = false;
                Game.stateManager.overworldState.RemoveOverworldObject(destroyedShip);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (progress == 0 &&
                Game.player.Bounds.Intersects(destroyedShip.Bounds) &&
                GameStateManager.currentState.Equals("OverworldState"))
            {
                CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, destroyedShip);
                Game.helper.DisplayText("Press 'Enter' to investigate ship.");
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
