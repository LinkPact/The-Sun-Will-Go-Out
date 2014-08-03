using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main10_InTheNameOfScience : Mission
    {
        private enum EventID
        {
            CloseInOnStation,
            FirstLevelCompleted,
            FirstLevelFailed,
            ArriveAtStation,
            ReturnToOverworld
        }

        public Main10_InTheNameOfScience(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Peye Science Station"), 1000,
                new EventTextCapsule(new EventText("1 - CloseInOnStation"), null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Peye Science Station"),
                "Main10_AllianceDefence", LevelStartCondition.TextCleared,
                new EventTextCapsule(new EventText("2 - FirstLevelCompleted"),
                    new EventText("3 - FirstLevelFailed"), EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Peye Science Station"),
                new EventTextCapsule(new EventText("4 - ArriveAtStation"), null, EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Rebel Station 1"),
                new EventTextCapsule(new EventText("5 - ReturnToOverworld"), null, EventTextCanvas.MessageBox),
                delegate { }, delegate { },
                delegate 
                {
                    return GameStateManager.currentState == "OverworldState";
                },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], 
                Game.stateManager.overworldState.GetStation("Rebel Station 1")));
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
        }

        public override void OnLoad()
        {
         
        }

        public override void OnReset()
        {
            base.OnReset();

            ObjectiveIndex = 0;

            for (int i = 0; i < objectives.Count; i++)
            {
                objectives[i].Reset();
            }
        }

        public override void MissionLogic()
        {
            base.MissionLogic();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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
