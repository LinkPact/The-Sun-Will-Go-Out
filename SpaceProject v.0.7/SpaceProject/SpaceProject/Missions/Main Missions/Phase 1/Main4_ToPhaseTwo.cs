using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main4_ToPhaseTwo : Mission
    {
        private enum EventID
        {
            Beacon1,
            Beacon2,
            ArriveAtHighfence,
            ToFortrun
        }

        public Main4_ToPhaseTwo(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
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

            GameObjectOverworld soelaraBeacon = Game.stateManager.overworldState.GetBeacon("Soelara Beacon");
            GameObjectOverworld highfence = Game.stateManager.overworldState.GetPlanet("Highfence");
            GameObjectOverworld fortrunStation1 = Game.stateManager.overworldState.GetStation("Fortrun Station I");

            destinations.Add(soelaraBeacon);
            destinations.Add(soelaraBeacon);
            destinations.Add(soelaraBeacon);
            destinations.Add(highfence);
            destinations.Add(fortrunStation1);
            destinations.Add(fortrunStation1);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], destinations[0],
                GetEvent((int)EventID.Beacon1).Text, 3000, 1000));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[1],
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

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], destinations[2],
                GetEvent((int)EventID.Beacon2).Text, 3000, 2500));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], destinations[3],
                new EventTextCapsule(GetEvent((int)EventID.ArriveAtHighfence), null, EventTextCanvas.BaseState)));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[1], destinations[4],
                GetEvent((int)EventID.ToFortrun).Text, 3000, 2500));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1], destinations[5]));
        }
    }
}
