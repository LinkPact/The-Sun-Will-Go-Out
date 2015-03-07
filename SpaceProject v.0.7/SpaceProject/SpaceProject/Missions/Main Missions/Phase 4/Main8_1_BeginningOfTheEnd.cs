using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main8_1_BeginningOfTheEnd : Mission
    {
        private enum EventID
        {
            TravellingToCoordinate,
            AtCoordinate,
            IncomingMessage1,
            AllianceMessage,
            SairCommentingOnMessage1,
            IncomingMessage2,
            RebelMessage,
            SairCommentingOnMessage2
        }

        public Main8_1_BeginningOfTheEnd(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
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

            Planet peye = Game.stateManager.overworldState.GetPlanet("Peye");
            Planet telmun = Game.stateManager.overworldState.GetPlanet("Telmun");

            AddDestination(peye, 2);
            AddDestination(telmun, 6);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                3000, 2000, GetEvent((int)EventID.TravellingToCoordinate).Text));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.AtCoordinate), null, EventTextCanvas.MessageBox)));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                3000, 3000, GetEvent((int)EventID.IncomingMessage1).Text));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.AllianceMessage), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[1],
                3000, 3000, GetEvent((int)EventID.SairCommentingOnMessage1).Text));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[1],
                3000, 2000, GetEvent((int)EventID.IncomingMessage2).Text));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[1],
                new EventTextCapsule(GetEvent((int)EventID.RebelMessage), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[1],
                3000, 2000, GetEvent((int)EventID.SairCommentingOnMessage2).Text));
        }
    }
}
