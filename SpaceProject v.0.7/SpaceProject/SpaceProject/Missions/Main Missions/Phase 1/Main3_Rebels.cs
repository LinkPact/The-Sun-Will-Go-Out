using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main3_Rebels : Mission
    {
        private enum EventID
        {
            NotAccept,
            CaptainIntro,
            CaptainChitChat1,
            CaptainChitChat2,
            RebelsAttack1,
            RebelMessage1,
            AfterRebelAttack1,
            RebelsAttack2,
            RebelMessage2,
            AfterRebelAttack2,
            AlmostThere,
            ArriveAtSoelara,
            Beacon1,
            Beacon2
        }

        FreighterShip freighter1;
        FreighterShip freighter2;
        private float freighterHP;
        public float GetFreighterHP { get { return freighterHP; } }

        public Main3_Rebels(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            Station soelaraStation = Game.stateManager.overworldState.GetStation("Soelara Station");

            base.Initialize();

            RestartAfterFail();

            freighterHP = 2000;

            freighter1 = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);
            freighter1.Initialize(Game.stateManager.overworldState.GetSectorX,
                Game.stateManager.overworldState.GetPlanet("Highfence"),
                soelaraStation);
            freighter1.AIManager = new TravelAction(freighter1, soelaraStation);
            freighter1.collisionEvent = new RemoveOnCollisionEvent(Game, freighter1, soelaraStation); 

            // Lots-of-paramaters-version of EscortObjective
            objectives.Add(new EscortObjective(Game,
                this,
                ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Soelara Station"),
                new EscortDataCapsule(freighter1,
                    GetEvent((int)EventID.CaptainIntro).Text,
                    Game.stateManager.overworldState.GetSectorX.shipSpawner.GetOverworldShips(2, "rebel"),
                    new List<String> { GetEvent((int)EventID.RebelMessage1).Text, GetEvent((int)EventID.RebelMessage2).Text },
                    null,
                    Game.stateManager.overworldState.GetPlanet("Highfence").position + new Vector2(-200, 0),
                    new List<String> { GetEvent((int)EventID.RebelsAttack1).Text,
                                       GetEvent((int)EventID.RebelsAttack2).Text },
                    38000,
                    8000,
                    2000,
                    new List<String> { "SecondMissionlvl1", "SecondMissionlvl2" },
                    new List<String> { GetEvent((int)EventID.AfterRebelAttack1).Text, GetEvent((int)EventID.AfterRebelAttack2).Text },
                    new List<String> { GetEvent((int)EventID.CaptainChitChat1).Text, GetEvent((int)EventID.CaptainChitChat2).Text,
                                       GetEvent((int)EventID.AlmostThere).Text },
                    new List<int> { 4000, 24000, 56000}),
                new EventTextCapsule(GetEvent((int)EventID.ArriveAtSoelara),
                    null,
                    EventTextCanvas.BaseState)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetPlanet("Highfence")));
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
