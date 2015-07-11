using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main8_2_TheEnd : Mission
    {
        private enum EventID
        {
            ToAsteroidBelt1,
            ToAsteroidBelt2,
            ToAsteroidBelt3,
            ToAsteroidBelt4,
            AtAsteroidBelt
        }

        private readonly int Delay1 = 2000;
        private readonly int Delay2 = 1500;

        private float completeObjectiveTime;

        public Main8_2_TheEnd(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
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

            Planet telmun = Game.stateManager.overworldState.GetPlanet("Murt Asteroid");

            AddDestination(telmun, 5);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                3000, 1000, PortraitID.Sair, GetEvent((int)EventID.ToAsteroidBelt1).Text));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.ToAsteroidBelt2), null, EventTextCanvas.MessageBox, PortraitID.Sair),
                delegate
                {
                    completeObjectiveTime = StatsManager.PlayTime.GetFutureOverworldTime(Delay1);
                },
                delegate { },
                delegate
                {
                    return StatsManager.PlayTime.HasOverworldTimePassed(completeObjectiveTime);
                },
                delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.ToAsteroidBelt3), null, EventTextCanvas.MessageBox, PortraitID.Sair),
                delegate
                {
                    completeObjectiveTime = StatsManager.PlayTime.GetFutureOverworldTime(Delay2);
                },
                delegate { },
                delegate
                {
                    return StatsManager.PlayTime.HasOverworldTimePassed(completeObjectiveTime);
                },
                delegate { return false; }));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                2000, 3000, PortraitID.Sair, GetEvent((int)EventID.ToAsteroidBelt4).Text));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], 1200,
                new EventTextCapsule(GetEvent((int)EventID.AtAsteroidBelt), null, EventTextCanvas.MessageBox, PortraitID.Sair)));
        }
    }
}
