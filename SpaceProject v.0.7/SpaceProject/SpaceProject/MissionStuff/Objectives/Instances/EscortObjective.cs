﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class EscortObjective : Objective
    {
        private EscortDataCapsule escortDataCapsule;

        private List<OverworldShip> enemies;

        private int startingNumberOfEnemyShips;
        private int numberOfEnemyShips;
        private int enemyShipSpawnDelay;
        private float enemyAttackStartTime;
        private float enemyNextWaveTime;

        private List<String> levels;
        private List<String> enemyMessages;

        private float shipToDefendHP;
        private float shipToDefendMaxHP;

        private int messageTimer;
        private bool started;
        private List<float> timedMessageTimes;
        private int timedMessageCount;

        public EscortObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, EscortDataCapsule escortDataCapsule) :
            base(game, mission, description, destination)
        {
            Setup(escortDataCapsule);
        }

        public EscortObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, EscortDataCapsule escortDataCapsule,
            EventTextCapsule eventTextCapsule) :
            base(game, mission, description, destination)
        {
            Setup(escortDataCapsule);

            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;
        }

        private void Setup(EscortDataCapsule escortDataCapsule)
        {
            this.escortDataCapsule = escortDataCapsule;
            startingNumberOfEnemyShips = escortDataCapsule.EnemyShips.Count;
            numberOfEnemyShips = escortDataCapsule.EnemyShips.Count;
            enemyShipSpawnDelay = escortDataCapsule.EnemyAttackFrequency;
            levels = escortDataCapsule.Levels;
            shipToDefendMaxHP = escortDataCapsule.ShipToDefendHP;
            shipToDefendHP = escortDataCapsule.ShipToDefendHP;
            enemyMessages = escortDataCapsule.EnemyMessages;
            enemies = new List<OverworldShip>();
            timedMessageCount = -1;
            timedMessageTimes = new List<float>();

            for (int i = 0; i < escortDataCapsule.EnemyShips.Count; i++)
            {
                enemies.Add(escortDataCapsule.EnemyShips[i]);
            }

            if (enemyMessages.Count < escortDataCapsule.EnemyShips.Count)
            {
                for (int i = enemyMessages.Count; i < escortDataCapsule.EnemyShips.Count; i++)
                {
                    if (enemyMessages.Count > 0)
                    {
                        enemyMessages.Add(enemyMessages[0]);
                    }
                }
            }
        }

        public override void OnActivate()
        {
            if (escortDataCapsule.ShipToDefend is FreighterShip)
            {
                game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                    (FreighterShip)escortDataCapsule.ShipToDefend, escortDataCapsule.StartingPoint);

                ((FreighterShip)escortDataCapsule.ShipToDefend).Wait();
            }

            OverworldShip.FollowPlayer = false;

            base.OnActivate();
        }

        public override void Reset()
        {
            base.Reset();

            started = false;
            startingNumberOfEnemyShips = escortDataCapsule.EnemyShips.Count;
            numberOfEnemyShips = escortDataCapsule.EnemyShips.Count;
            enemies = new List<OverworldShip>();

            for (int i = 0; i < levels.Count; i++)
            {
                game.stateManager.shooterState.GetLevel(levels[i]).Initialize();
            }

            for (int i = 0; i < escortDataCapsule.EnemyShips.Count; i++)
            {
                escortDataCapsule.EnemyShips[i].Initialize();
                enemies.Add(escortDataCapsule.EnemyShips[i]);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(PlayTime playTime)
        {
            base.Update(playTime);

            if (timedMessageCount >= 0
                && timedMessageCount < escortDataCapsule.TimedMessages.Count
                && StatsManager.PlayTime.HasOverworldTimePassed(timedMessageTimes[timedMessageCount]))
            {
                game.messageBox.DisplayRealtimeMessage(4000, escortDataCapsule.TimedMessages[timedMessageCount]);
                timedMessageCount++;
            }

            // Player talks to freighter to begin escort
            if (!started
                && GameStateManager.currentState.Equals("OverworldState")
                && CollisionDetection.IsPointInsideRectangle(game.player.position, escortDataCapsule.ShipToDefend.Bounds)
                && ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
            {
                game.messageBox.DisplayMessage(0, escortDataCapsule.ShipIntroductionText);

                ((FreighterShip)escortDataCapsule.ShipToDefend).Start();

                started = true;

                enemyAttackStartTime = StatsManager.PlayTime.GetFutureOverworldTime(escortDataCapsule.EnemyAttackStartTime);

                for (int i = 0; i < escortDataCapsule.TimedMessages.Count; i++)
                {
                    timedMessageTimes.Add(StatsManager.PlayTime.GetFutureOverworldTime(escortDataCapsule.TimedMessageTriggers[i]));
                }
                timedMessageCount = 0;
            }

            // Escort mission begins
            if (started 
                && GameStateManager.currentState.Equals("OverworldState") &&
                numberOfEnemyShips > 0 &&
                StatsManager.PlayTime.HasOverworldTimePassed(enemyAttackStartTime))
            {
                // Ready to spawn a new enemy ship
                if (StatsManager.PlayTime.HasOverworldTimePassed(enemyNextWaveTime))
                {
                    int i = startingNumberOfEnemyShips - numberOfEnemyShips;

                    game.messageBox.DisplayMessage(0, escortDataCapsule.AttackStartText[i]);

                    game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                        enemies[0],
                        escortDataCapsule.ShipToDefend.position +
                        (650 * escortDataCapsule.ShipToDefend.Direction.GetDirectionAsVector()),
                        levels[startingNumberOfEnemyShips - numberOfEnemyShips], escortDataCapsule.ShipToDefend);

                    numberOfEnemyShips--;

                    if (numberOfEnemyShips > 0)
                    {
                        enemyNextWaveTime = StatsManager.PlayTime.GetFutureOverworldTime(escortDataCapsule.EnemyAttackFrequency);
                    }
                }
            }
            
            // Transfers freigter hp between levels
            for (int i = 0; i < levels.Count; i++)
            {
                if (GameStateManager.currentState.Equals("ShooterState")
                    && game.stateManager.shooterState.CurrentLevel.Identifier.Equals(levels[i])
                    && game.stateManager.shooterState.GetLevel(levels[i]).IsObjectiveCompleted)
                {
                    shipToDefendHP = game.stateManager.shooterState.CurrentLevel.GetFreighterHP();
                    game.messageBox.DisplayMessage(200, escortDataCapsule.AfterAttackMessages[i]);
                }
            }

            Collision();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (!started 
                && GameStateManager.currentState.Equals("OverworldState") 
                && CollisionDetection.IsRectInRect(game.player.Bounds, escortDataCapsule.ShipToDefend.Bounds))
            {
                CollisionHandlingOverWorld.DrawRectAroundObject(game, spriteBatch, escortDataCapsule.ShipToDefend, new Rectangle(2, 374, 0, 0));
                game.helper.DisplayText("Press 'Enter' to talk to freighter captain..");
            }
        }

        public override bool Completed()
        {
            return (escortDataCapsule.ShipToDefend.AIManager.Finished 
                || escortDataCapsule.ShipToDefend.IsDead
                || ControlManager.CheckKeypress(Keys.V));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
            OverworldShip.FollowPlayer = true;

            if (Destination is Station)
            {
                game.stateManager.GotoStationSubScreen(Destination.name, "Overview");
            }
            else if (Destination is Planet)
            {
                game.stateManager.GotoPlanetSubScreen(Destination.name, "Overview");
            }
        }

        public override bool Failed()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (GameStateManager.currentState.Equals("ShooterState")
                    && game.stateManager.shooterState.CurrentLevel.Identifier.Equals(levels[i])
                    && game.stateManager.shooterState.GetLevel(levels[i]).GetFreighterHP() <= 0)
                {
                    return true;
                }

                else if (GameStateManager.currentState.Equals("OverworldState")
                    && game.stateManager.shooterState.CurrentLevel != null 
                    && game.stateManager.shooterState.CurrentLevel.Identifier.Equals(levels[i])
                    && game.stateManager.shooterState.CurrentLevel.IsObjectiveFailed)
                {
                    return true;
                }
            }

            return false;
        }

        public override void OnFailed()
        {
            base.OnFailed();

            mission.OnReset();
            mission.MissionState = StateOfMission.Failed;
            game.stateManager.overworldState.RemoveOverworldObject(escortDataCapsule.ShipToDefend);
            game.messageBox.DisplayMessage(0, "Noooo! The freighter was destroyed. We failed.");
            game.stateManager.ChangeState("OverworldState");
        }

        private void Collision()
        {
            messageTimer--;

            if (numberOfEnemyShips < startingNumberOfEnemyShips)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (CollisionDetection.IsRectInRect(escortDataCapsule.ShipToDefend.Bounds, enemies[i].Bounds))
                    {
                        if (enemyMessages.Count > 0)
                        {
                            game.messageBox.DisplayMessage(0, enemyMessages[0]);
                            enemyMessages.RemoveAt(0);
                        }

                        game.stateManager.overworldState.RemoveOverworldObject(enemies[i]);
                        game.stateManager.shooterState.BeginLevel(enemies[i].Level);
                        game.stateManager.shooterState.CurrentLevel.SetFreighterMaxHP(shipToDefendMaxHP);
                        game.stateManager.shooterState.CurrentLevel.SetFreighterHP(shipToDefendHP);
                        enemies.RemoveAt(i);
                    }
                }
            }

            if (started 
                && !CollisionDetection.IsPointInsideCircle(game.player.position, escortDataCapsule.ShipToDefend.position, 600) &&
                messageTimer <= 0)
            {
                game.messageBox.DisplayMessage(0, "\"Don't stray too far from the freighter, get back here!\"");
                messageTimer = 200;
            }

            if (started
                && CollisionDetection.IsPointInsideCircle(game.player.position, escortDataCapsule.ShipToDefend.position, 600) &&
                messageTimer > 0)
            {
                game.messageBox.DisplayMessage(0, "\"Good! Now keep the freighter in sight at all times!\"");
                messageTimer = 0;
            }

            if (started
                && messageTimer == 1)
            {
                mission.OnReset();
                game.stateManager.overworldState.RemoveOverworldObject(escortDataCapsule.ShipToDefend);
                mission.MissionState = StateOfMission.Failed;
                game.messageBox.DisplayMessage(0, "\"What are you doing?! You compromised our entire mission. Get out of my sight, you moron!\"");
            }
        }
    }
}
