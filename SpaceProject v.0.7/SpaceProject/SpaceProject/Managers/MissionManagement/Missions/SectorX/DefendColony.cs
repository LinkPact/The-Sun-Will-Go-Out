using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class DefendColony : Mission
    {

        public DefendColony(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            EventArray = new string[1, 1];

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");   //index 0,0

            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
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
                Game.stateManager.shooterState.BeginLevel("DefendColony");
            }

            if (progress == 0 &&
                Game.stateManager.shooterState.GetLevel("DefendColony").IsObjectiveCompleted)
            {
                updateLogic = true;
                ShowEventAndUpdateProgress(0, 1);
                MissionManager.MarkMissionAsCompleted(this.MissionName);

                Game.stateManager.GotoPlanetSubScreen("New Norrland", "Colony");
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
