using System;
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

        private int startingNumberOfEnemyShips;
        private int numberOfEnemyShips;
        private int enemyShipSpawnDelay;

        private List<String> levels;

        private float shipToDefendHP;
        private float shipToDefendMaxHP;

        private int messageTimer;

        private bool started;

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
        }

        public override void OnActivate()
        {
            if (escortDataCapsule.ShipToDefend is FreighterShip)
            {
                game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                    (FreighterShip)escortDataCapsule.ShipToDefend, escortDataCapsule.StartingPoint);

                ((FreighterShip)escortDataCapsule.ShipToDefend).Wait();
            }

            base.OnActivate();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            if (started)
            {
                escortDataCapsule.EnemyAttackStartTime--;
            }

            // Player talks to freighter to begin escort
            if (!started
                && GameStateManager.currentState.Equals("OverworldState")
                && CollisionDetection.IsPointInsideRectangle(game.player.position, escortDataCapsule.ShipToDefend.Bounds)
                && ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
            {
                game.messageBox.DisplayMessage(escortDataCapsule.ShipToDefendText);

                ((FreighterShip)escortDataCapsule.ShipToDefend).Start();

                PirateShip.FollowPlayer = false;

                started = true;
            }

            // Escort mission begins
            if (GameStateManager.currentState.Equals("OverworldState") &&
                numberOfEnemyShips > 0 &&
                escortDataCapsule.EnemyAttackStartTime < 0)
            {
                enemyShipSpawnDelay--;

                // Ready to spawn a new enemy ship
                if (enemyShipSpawnDelay < 0)
                {
                    //game.messageBox.DisplayMessage(escortDataCapsule.EnemyMessages[0]);
                    //escortDataCapsule.EnemyMessages.RemoveAt(0);

                    game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                        escortDataCapsule.EnemyShips[0],
                        escortDataCapsule.ShipToDefend.position +
                        (650 * escortDataCapsule.ShipToDefend.Direction.GetDirectionAsVector()),
                        levels[startingNumberOfEnemyShips - numberOfEnemyShips], escortDataCapsule.ShipToDefend);

                    numberOfEnemyShips--;

                    if (numberOfEnemyShips > 0)
                    {
                        enemyShipSpawnDelay = escortDataCapsule.EnemyAttackFrequency;
                    }
                }
            }
            
            // Transfers freigter hp between levels
            for (int i = 0; i < levels.Count; i++)
            {
                if (GameStateManager.currentState.Equals("ShooterState")
                    && game.stateManager.shooterState.CurrentLevel.Name.Equals(levels[i])
                    && game.stateManager.shooterState.GetLevel(levels[i]).IsObjectiveCompleted)
                {
                    shipToDefendHP = game.stateManager.shooterState.CurrentLevel.GetFreighterHP();
                }
            }

            Collision();

            base.Update();
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
            return (((FreighterShip)escortDataCapsule.ShipToDefend).HasArrived || ControlManager.CheckKeypress(Keys.V));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
            PirateShip.FollowPlayer = true;

            game.stateManager.GotoStationSubScreen(Destination.name, "Overview");
        }

        public override bool Failed()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (GameStateManager.currentState.Equals("ShooterState")
                    && game.stateManager.shooterState.CurrentLevel.Name.Equals(levels[i])
                    && game.stateManager.shooterState.GetLevel(levels[i]).GetFreighterHP() <= 0)
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
            game.messageBox.DisplayMessage("Noooo! The freighter was destroyed. We failed.");
            game.stateManager.ChangeState("OverworldState");
        }

        private void Collision()
        {
            messageTimer--;

            if (numberOfEnemyShips < startingNumberOfEnemyShips)
            {
                for (int i = 0; i < escortDataCapsule.EnemyShips.Count; i++)
                {
                    if (CollisionDetection.IsRectInRect(escortDataCapsule.ShipToDefend.Bounds, escortDataCapsule.EnemyShips[i].Bounds))
                    {
                        game.stateManager.overworldState.RemoveOverworldObject(escortDataCapsule.EnemyShips[i]);
                        game.messageBox.DisplayMessage("Death to the Alliance!");
                        game.stateManager.shooterState.BeginLevel(escortDataCapsule.EnemyShips[i].Level);
                        game.stateManager.shooterState.CurrentLevel.SetFreighterMaxHP(shipToDefendMaxHP);
                        game.stateManager.shooterState.CurrentLevel.SetFreighterHP(shipToDefendHP);
                        escortDataCapsule.EnemyShips.RemoveAt(i);
                    }
                }
            }

            if (!CollisionDetection.IsPointInsideCircle(game.player.position, escortDataCapsule.ShipToDefend.position, 600) &&
                messageTimer <= 0)
            {
                game.messageBox.DisplayMessage("\"Don't stray too far from the freighter, get back here!\"");
                messageTimer = 200;
            }

            if (CollisionDetection.IsPointInsideCircle(game.player.position, escortDataCapsule.ShipToDefend.position, 600) &&
                messageTimer > 0)
            {
                game.messageBox.DisplayMessage("\"Good! Now keep the freighter in sight at all times!\"");
                messageTimer = 0;
            }

            if (messageTimer == 1)
            {
                mission.OnReset();
                game.messageBox.DisplayMessage("\"What are you doing?! You compromised our entire mission. Get out of my sight, you moron!\"");
            }
        }
    }
}
