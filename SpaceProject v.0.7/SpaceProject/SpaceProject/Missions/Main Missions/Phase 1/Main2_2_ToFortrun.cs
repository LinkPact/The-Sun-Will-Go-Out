using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main2_2_ToFortrun : Mission
    {
        private enum EventID
        {
            Beacon1,
            Beacon2,
            ArriveAtHighfence,
            ToFortrun
        }

        private float time;

        public Main2_2_ToFortrun(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
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
            ObjectiveIndex = 0;
            progress = 0;
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

            GameObjectOverworld soelaraBeacon = Game.stateManager.overworldState.GetBeacon("Soelara Beacon");
            GameObjectOverworld highfence = Game.stateManager.overworldState.GetPlanet("Highfence");
            GameObjectOverworld fortrunStation1 = Game.stateManager.overworldState.GetStation("Fortrun Station I");

            AddDestination(soelaraBeacon, 3);
            AddDestination(highfence);
            AddDestination(fortrunStation1, 2);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.Beacon1), null, EventTextCanvas.MessageBox),
                delegate 
                {
                    time = StatsManager.PlayTime.GetFutureOverworldTime(500);
                },
                delegate { },
                delegate { return StatsManager.PlayTime.HasOverworldTimePassed(time); },
                delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate
                {
                    if (!Game.stateManager.overworldState.GetBeacon("Highfence Beacon").IsActivated)
                    {
                        Game.stateManager.overworldState.GetBeacon("Highfence Beacon").Activate();
                    }
                },
                delegate { },
                delegate
                {
                    return (Game.player.HyperspeedOn
                            && Game.stateManager.overworldState.GetBeacon("Soelara Beacon").GetFinalDestination.name.ToLower() ==
                                Game.stateManager.overworldState.GetBeacon("Highfence Beacon").name.ToLower());
                },
                delegate { return false; }
                ));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                3000, 2500, GetEvent((int)EventID.Beacon2).Text));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.ArriveAtHighfence), null, EventTextCanvas.BaseState)));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[1],
                3000, 2500, GetEvent((int)EventID.ToFortrun).Text));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1]));
        }
    }
}
