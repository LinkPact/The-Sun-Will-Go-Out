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
        private enum EventID
        {
            LevelCleared = 0,
        }

        public DefendColony(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Rebel Station 1"),
                new EventTextCapsule(GetEvent(0), null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetStation("Rebel Station 1"), "DefendColony", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent(1), null, EventTextCanvas.BaseState)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[2],
                Game.stateManager.overworldState.GetStation("Fotrun Station I")));

            objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[3],
                Game.stateManager.overworldState.GetStation("Fotrun Station I"),
                new ResponseTextCapsule(GetEvent(2), GetAllResponses(2),
                    new List<System.Action> 
                                {
                                    delegate 
                                    {
                                        missionHelper.ShowEvent(GetEvent(3));
                                    },
                                    delegate 
                                    {
                                        missionHelper.ShowEvent(GetEvent(4));
                                    }
                                })));

            RestartAfterFail();
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
