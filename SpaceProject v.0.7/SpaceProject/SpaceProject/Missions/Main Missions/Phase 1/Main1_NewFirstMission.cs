﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main1_NewFirstMission: Mission
    {
        private enum EventID
        {
            Introduction,
            TravelingToAsteroids,
            TalkWithCaptain1,
            TalkWithCaptain2,
            InLevel1,
            InLevel2,
            AfterCombat,
            TravelingBack
        }

        private readonly string MoneyID = "[MONEY]";
        private readonly string BonusID = "[BONUS]";

        private readonly int DownedShipsMultiplier = 10;

        private int downedShips = -1;
        private AllyShip ally1;

        public Main1_NewFirstMission(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RewardItems.Add(new SpreadBulletWeapon(Game, ItemVariety.regular));

            CreateAllyShip();

            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;

            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));

            Game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(ally1, ally1.position, "", null);
        }

        public override void OnLoad()
        {
            Game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(ally1, ally1.position, "", null);
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            if (downedShips == -1 
                && Game.stateManager.shooterState.GetLevel("RebelsInTheMeteors").IsMapCompleted)
            {
                downedShips = Game.stateManager.shooterState.GetLevel("RebelsInTheMeteors").enemiesKilledByPlayer;
                int bonus = downedShips * DownedShipsMultiplier;
                ReplaceObjectiveText(TextType.Completed, MoneyID, MoneyReward);
                ReplaceObjectiveText(TextType.Completed, BonusID, bonus);
                moneyReward += bonus;
            }
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

        public override void OnReset() { }

        protected override void SetDestinations()
        {
            GameObjectOverworld miningAsteroids =
                Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids");
            GameObjectOverworld borderStation =
                Game.stateManager.overworldState.GetBorderXOutpost.GetGameObject("Border Station");

            destinations = new List<GameObjectOverworld>();

            destinations.Add(ally1);
            destinations.Add(ally1);
            destinations.Add(ally1);
            destinations.Add(ally1);
            destinations.Add(miningAsteroids);
            destinations.Add(borderStation);
            destinations.Add(borderStation);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], destinations[0],
                GetEvent((int)EventID.TravelingToAsteroids).Text, 3000, 3000));

            Objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0],
                destinations[1], 500));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], destinations[2],
                GetEvent((int)EventID.TalkWithCaptain1).Text,
                3000, 0));

            ArriveAtLocationObjective talkToCaptainObjective = new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                destinations[3], new EventTextCapsule(GetEvent((int)EventID.TalkWithCaptain2), null, EventTextCanvas.MessageBox));

            Objectives.Add(talkToCaptainObjective);

            ShootingLevelObjective shootingLevelObjective = new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                destinations[4], "RebelsInTheMeteors", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.AfterCombat),
                    null,
                    EventTextCanvas.MessageBox));

            shootingLevelObjective.SetFailLogic(
                delegate
                {
                    Game.player.position = Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("mining asteroids").position;
                    Game.messageBox.DisplayMessage("Too bad. Talk to me to try again.", false);
                    talkToCaptainObjective.Reset();
                    shootingLevelObjective.Reset();
                    ObjectiveIndex = 3;
                }
            );

            Objectives.Add(shootingLevelObjective);

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[1], destinations[5],
                GetEvent((int)EventID.TravelingBack).Text, 3000, 3000));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1], destinations[6]));
        }

        private void CreateAllyShip()
        {
            ally1 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Alliance);
            ally1.Initialize(Game.stateManager.overworldState.GetSectorX,
                new Vector2(Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids").position.X - 200,
                    Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids").position.Y + 200),
                    Vector2.Zero);
            ally1.AIManager = new WaitAction(ally1, delegate { return false; });
        }
    }
}
