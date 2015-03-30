using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    public class Main7_Information : Mission
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

        public Main7_Information(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
            isMainMission = true;
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
            SetDestinations();
            SetupObjectives();

            base.OnReset();
        }

        public override void OnFailed()
        {
            base.OnFailed();

            PopupHandler.DisplayMessage("You did not manage to shake off the suspicious rebels. Go back to the rebel base to try again.");
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

            GameObjectOverworld lavis = Game.stateManager.overworldState.GetPlanet("Lavis");

            AddDestination(lavis, 6);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.Introduction), null, EventTextCanvas.MessageBox, PortraitID.Sair),
                delegate { },
                delegate { },
                delegate { return (GameStateManager.currentState.ToLower().Equals("overworldstate")); },
                delegate { return false; }));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                4000, 5000, PortraitID.Sair, GetEvent((int)EventID.Followed).Text));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], 3000,
                new EventTextCapsule(GetEvent((int)EventID.Followed2), null, EventTextCanvas.MessageBox, PortraitID.Sair)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "Information",
                LevelStartCondition.TextCleared,
                new EventTextCapsule(
                    GetEvent((int)EventID.AfterBattle), null, EventTextCanvas.MessageBox, PortraitID.Sair)));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], 600,
                new EventTextCapsule(GetEvent((int)EventID.HubFound), null, EventTextCanvas.MessageBox, PortraitID.Sair)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.AtHub), null, EventTextCanvas.MessageBox, PortraitID.Sair)));
        }
    }
}
