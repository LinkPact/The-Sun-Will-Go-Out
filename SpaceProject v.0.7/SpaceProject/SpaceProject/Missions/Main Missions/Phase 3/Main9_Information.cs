using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main9_Information : Mission
    {
        private enum EventID
        {
            Introduction,
            Followed,
            Followed2,
            DispatchRebels,
            AfterBattle,
            HubFound,
            AtHub
        }

        public Main9_Information(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();
            SetDestinations();
            SetupObjectives();
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

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            GameObjectOverworld highfence = Game.stateManager.overworldState.GetPlanet("Highfence");

            destinations.Add(highfence);
            destinations.Add(highfence);
            destinations.Add(highfence);
            destinations.Add(highfence);
            destinations.Add(highfence);
            destinations.Add(highfence);
        }

        protected override void SetupObjectives()
        {
            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[0],
                new EventTextCapsule(GetEvent((int)EventID.Introduction), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return (GameStateManager.currentState.ToLower().Equals("overworldstate")); },
                delegate { return false; }));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], destinations[1],
                GetEvent((int)EventID.Followed).Text,
                4000, 5000));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], destinations[2], 3000,
                new EventTextCapsule(GetEvent((int)EventID.Followed2), null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], destinations[3],
                "Information",
                LevelStartCondition.TextCleared,
                new EventTextCapsule(
                    GetEvent((int)EventID.AfterBattle), null, EventTextCanvas.MessageBox)));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], destinations[4], 600,
                new EventTextCapsule(GetEvent((int)EventID.HubFound), null, EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], destinations[5],
                new EventTextCapsule(GetEvent((int)EventID.AtHub), null, EventTextCanvas.BaseState)));
        }
    }
}
