using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main2_1_TheConvoy : Mission
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

        public Main2_1_TheConvoy(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
            isMainMission = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();
            Station soelaraStation = Game.stateManager.overworldState.GetStation("Soelara Station");
            moneyReward = 500;

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

        public override void OnFailed()
        {
            base.OnFailed();

            PopupHandler.DisplayMessage("The freighter was destroyed and the mission failed. Return to Highfence to try again.");
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
            SetDestinations();

            freighter1 = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);
            freighter1.Initialize(Game.stateManager.overworldState.GetSectorX,
                Game.stateManager.overworldState.GetPlanet("Highfence"),
                destinations[0]);
            freighter1.AIManager = new TravelAction(freighter1, destinations[0]);
            freighter1.collisionEvent = new RemoveOnCollisionEvent(Game, freighter1, destinations[0]);
            freighter1.SaveShip = false;

            enemies = Game.stateManager.overworldState.GetSectorX.shipSpawner.GetOverworldShips(2, "rebel");

            foreach (OverworldShip ship in enemies)
            {
                ship.AIManager = new FollowInViewAction(ship, freighter1);
            }

            SetupObjectives();
        }

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            Station soelaraStation = Game.stateManager.overworldState.GetStation("Soelara Station");

            AddDestination(soelaraStation);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();
            
            EscortObjective escortObjective = new EscortObjective(Game,
               this,
               new List<String> { ObjectiveDescriptions[0], ObjectiveDescriptions[1], ObjectiveDescriptions[2] },
               new EscortDataCapsule(freighter1, enemies,
                   null,
                   Game.stateManager.overworldState.GetPlanet("Highfence").position + new Vector2(-200, 0),
                   28500,
                   4000,
                   1000,
                   new List<String> { "FreighterEscort1", "FreighterEscort2" },
                   new List<int> { 4000, 18000, 34500 }, 0.4f),
               true);

            escortObjective.SetIntroductionMessage(GetEvent((int)EventID.CaptainIntro).Text, PortraitID.AllianceCaptain);

            escortObjective.SetEnemyMessage(null,
                GetEvent((int)EventID.RebelMessage1).Text, GetEvent((int)EventID.RebelMessage2).Text);

            escortObjective.SetAttackStartText(new List<PortraitID> { PortraitID.None, PortraitID.Sair },
                GetEvent((int)EventID.RebelsAttack1).Text, GetEvent((int)EventID.RebelsAttack2).Text);

            escortObjective.SetAfterAttackMessages(new List<PortraitID>() 
                { PortraitID.AllianceCaptain, PortraitID.AllianceCaptain},
                GetEvent((int)EventID.AfterRebelAttack1).Text, GetEvent((int)EventID.AfterRebelAttack2).Text);

            escortObjective.SetTimedMessages(new List<PortraitID>() 
                { PortraitID.AllianceCaptain, PortraitID.AllianceCaptain, PortraitID.AllianceCaptain },
                GetEvent((int)EventID.CaptainChitChat1).Text, GetEvent((int)EventID.CaptainChitChat2).Text,
                GetEvent((int)EventID.AlmostThere).Text);

            objectives.Add(escortObjective);
        }
    }
}
