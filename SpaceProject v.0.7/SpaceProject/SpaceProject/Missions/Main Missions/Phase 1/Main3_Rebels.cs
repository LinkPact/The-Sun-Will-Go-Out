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
            AlmostThere
        }

        FreighterShip freighter1;
        private List<OverworldShip> enemies;
        private float freighterHP;
        public float GetFreighterHP { get { return freighterHP; } }

        public Main3_Rebels(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();
            Station soelaraStation = Game.stateManager.overworldState.GetStation("Soelara Station");
            RewardItems.Add(new BasicShield(Game, ItemVariety.regular));

            freighterHP = 2000;

            Setup();
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

            objectives.Clear();
            Setup();
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

        private void Setup()
        {
            Station soelaraStation = Game.stateManager.overworldState.GetStation("Soelara Station");

            freighter1 = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);
            freighter1.Initialize(Game.stateManager.overworldState.GetSectorX,
                Game.stateManager.overworldState.GetPlanet("Highfence"),
                soelaraStation);
            freighter1.AIManager = new TravelAction(freighter1, soelaraStation);
            freighter1.collisionEvent = new RemoveOnCollisionEvent(Game, freighter1, soelaraStation);

            enemies = Game.stateManager.overworldState.GetSectorX.shipSpawner.GetOverworldShips(2, "rebel");

            foreach (OverworldShip ship in enemies)
            {
                ship.AIManager = new FollowInViewAction(ship, freighter1);
            }

            objectives.Add(new EscortObjective(Game,
                           this,
                           new List<String> { ObjectiveDescriptions[0], ObjectiveDescriptions[1], ObjectiveDescriptions[2] },
                           Game.stateManager.overworldState.GetStation("Soelara Station"),
                           new EscortDataCapsule(freighter1,
                               GetEvent((int)EventID.CaptainIntro).Text,
                               enemies,
                               new List<String> { GetEvent((int)EventID.RebelMessage1).Text, GetEvent((int)EventID.RebelMessage2).Text },
                               null,
                               Game.stateManager.overworldState.GetPlanet("Highfence").position + new Vector2(-200, 0),
                               new List<String> { GetEvent((int)EventID.RebelsAttack1).Text,
                                                              GetEvent((int)EventID.RebelsAttack2).Text },
                               28500,
                               4000,
                               2000,
                               new List<String> { "FreighterEscort1", "FreighterEscort2" },
                               new List<String> { GetEvent((int)EventID.AfterRebelAttack1).Text, GetEvent((int)EventID.AfterRebelAttack2).Text },
                               new List<String> { GetEvent((int)EventID.CaptainChitChat1).Text, GetEvent((int)EventID.CaptainChitChat2).Text,
                                                              GetEvent((int)EventID.AlmostThere).Text },
                               new List<int> { 4000, 20000, 35000 }, 0.4f),
                           true));
        }
    }
}
