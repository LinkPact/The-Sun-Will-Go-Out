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

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], Game.stateManager.overworldState.GetBeacon("Soelara Beacon"),
                GetEvent((int)EventID.Beacon1).Text, 3000, 1000));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], Game.stateManager.overworldState.GetBeacon("Soelara Beacon"),
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

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], Game.stateManager.overworldState.GetBeacon("Soelara Beacon"),
                GetEvent((int)EventID.Beacon2).Text, 3000, 2500));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetPlanet("Highfence"),
                new EventTextCapsule(GetEvent((int)EventID.ArriveAtHighfence), null, EventTextCanvas.BaseState)));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[1], Game.stateManager.overworldState.GetStation("Fortrun Station I"),
                GetEvent((int)EventID.ToFortrun).Text, 3000, 2500));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetStation("Fortrun Station I")));
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
