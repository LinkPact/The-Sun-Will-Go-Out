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

            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
        }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.getPlanet("New Norrland"), "DefendColony", LevelStartCondition.OnStartMission,
                new EventTextCapsule(new List<String>{EventArray[0, 0]}, null, EventTextCanvas.BaseState)));
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
