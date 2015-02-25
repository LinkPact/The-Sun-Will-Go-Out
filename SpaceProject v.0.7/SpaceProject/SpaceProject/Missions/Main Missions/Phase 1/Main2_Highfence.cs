using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main2_Highfence : Mission
    {
        private enum EventID
        {
            Introduction
        }

        private readonly string ActionKeyID = "[ACTIONKEY2]";

        public Main2_Highfence(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            SetDestinations();
            SetupObjectives();

            ReplaceObjectiveText(TextType.Event, ActionKeyID, ControlManager.GetKeyName(RebindableKeys.Action2), 0);
        }

        public override void StartMission()
        {
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));
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
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], destinations[0]));
        }
    }
}
