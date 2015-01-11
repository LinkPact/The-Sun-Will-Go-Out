using System;
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
            AfterCombat,
            TravelingBack,

        }

        private readonly string ShipsID = "[SHIPS]";
        private readonly string MoneyID = "[MONEY]";
        private readonly string BonusID = "[BONUS]";

        private readonly int DownedShipsMultiplier = 10;

        bool textChanged = false;

        private AllyShip ally1;

        public Main1_NewFirstMission(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RewardItems.Add(new SpreadBulletWeapon(Game));

            ally1 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Alliance);
            ally1.Initialize(Game.stateManager.overworldState.GetSectorX,
                new Vector2(Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids").position.X - 200,
                    Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids").position.Y + 200),
                    Vector2.Zero);
            ally1.AIManager = new WaitAction(ally1, delegate { return false; });

            // OBJECTIVES

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
                GetEvent((int)EventID.TravelingToAsteroids).Text, 3000, 3000));

            Objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0],
                ally1, 500));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
                GetEvent((int)EventID.TalkWithCaptain1).Text,
                3000, 0));

            Objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], ally1,
                new EventTextCapsule(GetEvent((int)EventID.TalkWithCaptain2), null, EventTextCanvas.MessageBox)));

            Objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
                "RebelsInTheMeteors", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.AfterCombat),
                    null,
                    EventTextCanvas.MessageBox)));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetBorderXOutpost.GetGameObject("Border Station"),
                GetEvent((int)EventID.TravelingBack).Text, 3000, 3000));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Border Station")));
                
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
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            if (!textChanged 
                && Game.stateManager.shooterState.GetLevel("RebelsInTheMeteors").IsMapCompleted)
            {
                int downedShips = Game.stateManager.shooterState.GetLevel("RebelsInTheMeteors").enemiesKilled;
                ReplaceText(4, GetEvent((int)EventID.AfterCombat), ShipsID, downedShips);
                textChanged = true;
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

        private void ReplaceText(int objectiveIndex, EventText e, string key, int val)
        {
            if (objectives[objectiveIndex].ObjectiveCompletedEventText.Text.Contains(key))
            {
                objectives[objectiveIndex].ObjectiveCompletedEventText.Text = 
                    objectives[objectiveIndex].ObjectiveCompletedEventText.Text.Replace(key, val.ToString());
            }

            else
            {
                throw new ArgumentException("Key not found in specified event text.");
            }
        }
    }
}
