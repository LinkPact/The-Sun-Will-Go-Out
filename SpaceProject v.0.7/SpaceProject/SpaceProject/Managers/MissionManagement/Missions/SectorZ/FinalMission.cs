using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class FinalMission : Mission
    {
        public FinalMission(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
        }

        public override void Initialize()
        {
            base.Initialize();
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

            if (GameStateManager.currentState.Equals("PlanetState") &&
                Game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            {
                Game.stateManager.shooterState.BeginLevel("mapCreator2");
            }

            if (Game.stateManager.shooterState.GetLevel("mapCreator2").IsObjectiveCompleted)
            {
                Game.stateManager.ChangeState("OutroState");
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
