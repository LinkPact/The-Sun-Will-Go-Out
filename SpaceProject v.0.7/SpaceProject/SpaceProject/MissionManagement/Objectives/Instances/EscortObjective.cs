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

        private int numberOfEnemyShips;
        private int enemyShipSpawnDelay;

        private List<String> levels;

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
            enemyShipSpawnDelay = escortDataCapsule.EnemyAttackFrequency;
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
            escortDataCapsule.EnemyAttackStartTime--;

            // Player talks to freighter to begin escort
            if (GameStateManager.currentState.Equals("OverworldState") &&
                CollisionDetection.IsPointInsideRectangle(game.player.position, escortDataCapsule.ShipToDefend.Bounds) &&
                ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
            {
                game.messageBox.DisplayMessage(escortDataCapsule.ShipToDefendText);

                ((FreighterShip)escortDataCapsule.ShipToDefend).Start();

                PirateShip.FollowPlayer = false;
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
                    game.messageBox.DisplayMessage(escortDataCapsule.EnemyMessages[0]);
                    escortDataCapsule.EnemyMessages.RemoveAt(0);

                    game.stateManager.overworldState.GetSectorX.shipSpawner.AddRebelShip(
                        escortDataCapsule.ShipToDefend.position +
                        (650 * escortDataCapsule.ShipToDefend.Direction.GetDirectionAsVector()),
                        levels[0], escortDataCapsule.ShipToDefend);

                    levels.RemoveAt(0);

                    numberOfEnemyShips--;

                    if (numberOfEnemyShips > 0)
                    {
                        enemyShipSpawnDelay = escortDataCapsule.EnemyAttackFrequency;
                    }
                }
            }

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool Completed()
        {
            return false;
        }

        public override void OnCompleted()
        {
            PirateShip.FollowPlayer = true;

            base.OnCompleted();
        }

        public override bool Failed()
        {
            return false;
        }

        public override void OnFailed()
        {
            base.OnFailed();
        }
    }
}
