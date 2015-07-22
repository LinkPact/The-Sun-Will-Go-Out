using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    public class Main1_2_ToHighfence : Mission
    {
        private enum EventID
        {
            Introduction,
            TrainingArea
        }

        private readonly string ActionKeyID = "[ACTIONKEY2]";
        private readonly int MessageDelay = 1500;

        private float messageTime;

        public Main1_2_ToHighfence(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
            isMainMission = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            ReplaceObjectiveText(TextType.Event, ActionKeyID,
                "'" + ControlManager.GetKeyName(RebindableKeys.Action2) + "'");
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));
        }

        public override void OnLoad()
        {
         
        }

        public override void OnReset()
        {
            base.OnReset();
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

            AddDestination(highfence, 2);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.TrainingArea), null, EventTextCanvas.MessageBox),
                delegate 
                {
                    messageTime = StatsManager.PlayTime.GetFutureOverworldTime(MessageDelay);
                },
                delegate {},
                delegate { return StatsManager.PlayTime.HasOverworldTimePassed(messageTime); },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0]));
        }
    }
}
