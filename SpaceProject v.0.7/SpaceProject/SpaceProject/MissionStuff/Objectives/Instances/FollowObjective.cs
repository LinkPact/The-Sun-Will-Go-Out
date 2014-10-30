using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

/* 
 * An objective where the player follows a variable number of ships to a location.
 * 
 * If deviation from the ship is not allowed, the first ship in the 'shipsToFollow'-list is considered
 * the center of the deviation-radius
 */

namespace SpaceProject
{
    public class FollowObjective : Objective
    {
        private List<OverworldShip> shipsToFollow;
        private Vector2 startingPosition;
        private Vector2 spacing;
        private String startMessage;
        private bool deviationAllowed = true;
        private int deviationRadius;
        private String deviationMessage;

        private bool startMessageDisplayed;
        private int outOfRangeTimer;

        public FollowObjective(Game1 game, Mission mission, String description,
                GameObjectOverworld destination, EventTextCapsule eventTextCapsule, String startMessage,
                Vector2 startingPosition, Vector2 shipSpacing, params OverworldShip[] ships) :
            base(game, mission, description, destination)
        {
            Setup(eventTextCapsule, startMessage, startingPosition, shipSpacing, ships);
        }

        public FollowObjective(Game1 game, Mission mission, String description,
                GameObjectOverworld destination, EventTextCapsule eventTextCapsule, String startMessage,
                Vector2 startingPosition, Vector2 shipSpacing, int deviationRadius, String deviationMessage,
                params OverworldShip[] ships) :
            base(game, mission, description, destination)
        {
            Setup(eventTextCapsule, startMessage, startingPosition, shipSpacing, ships);

            this.deviationRadius = deviationRadius;
            this.deviationMessage = deviationMessage;

            if (deviationRadius > 0)
            {
                deviationAllowed = false;
            }
            else
            {
                deviationAllowed = true;
            }
        }

        private void Setup(EventTextCapsule eventTextCapsule, String startMessage,
            Vector2 startingPosition, Vector2 spacing, params OverworldShip[] ships)
        {
            if (eventTextCapsule.CompletedText != null)
            {
                objectiveCompletedEventText = eventTextCapsule.CompletedText;
            }

            if (eventTextCapsule.FailedText != null)
            {
                objectiveFailedEventText = eventTextCapsule.FailedText;
            }

            eventTextCanvas = eventTextCapsule.EventTextCanvas;

            this.startMessage = startMessage;
            this.startingPosition = startingPosition;
            this.spacing = spacing;

            if (ships.Length < 1)
            {
                throw new ArgumentException("At least on ship is needed for this objective.");
            }

            else if (ships.Length == 1)
            {
                spacing = Vector2.Zero;
            }

            shipsToFollow = new List<OverworldShip>();

            foreach (OverworldShip ship in ships)
            {
                shipsToFollow.Add(ship);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnActivate()
        {
            base.OnActivate();

            for (int i = 0; i < shipsToFollow.Count; i++)
            {
                game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                    shipsToFollow[i], startingPosition + (spacing * i), "", null);
            }
        }

        public override void Update(PlayTime playTime)
        {
            base.Update(playTime);

            if (GameStateManager.currentState == "OverworldState"
                && !startMessageDisplayed)
            {
                startMessageDisplayed = true;
                game.messageBox.DisplayMessage(startMessage, false, 100);
            }

            if (!deviationAllowed
                && !CollisionDetection.IsPointInsideCircle(game.player.position, shipsToFollow[0].position, deviationRadius)
                && outOfRangeTimer <= 0)
            {
                outOfRangeTimer = 150;
                game.messageBox.DisplayMessage(deviationMessage, false);

                foreach (OverworldShip ship in shipsToFollow)
                {
                    ship.Wait();
                }
            }

            if (!deviationAllowed
                && outOfRangeTimer > 0)
            {
                outOfRangeTimer--;

                if (outOfRangeTimer == 149)
                {
                    game.player.InitializeHyperSpeedJump(shipsToFollow[0].position, false);
                }

                if (outOfRangeTimer < 1)
                {
                    outOfRangeTimer = -100;

                    foreach (OverworldShip ship in shipsToFollow)
                    {
                        ship.Start();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
        }

        public override void OnFailed()
        {
            base.OnFailed();
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override bool Completed()
        {
            return shipsToFollow[0].AIManager.Finished || shipsToFollow[0].IsDead;
        }

        public override bool Failed()
        {
            return false;
        }
    }
}
