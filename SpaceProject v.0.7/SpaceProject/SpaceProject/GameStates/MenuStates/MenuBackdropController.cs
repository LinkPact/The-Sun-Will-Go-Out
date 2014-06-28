using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MenuBackdropController
    {
        private Game1 game;

        public Sprite menuSpriteSheet;

        public Vector2 backdropPosition;
        private bool moveBackdrop;
        private String nextGameState = "";
        private Vector2 preferredBackdropPosition;
        private Direction backdropDirection;
        public float backdropSpeed;
        private const float backdropAcc = 0.01f;
        private double totalDistance;
        private double distance;

        private int pressDelay;

        public bool DisplayButtons { get { return !moveBackdrop; } }

        public MenuBackdropController(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            menuSpriteSheet = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites/MenuBackdrop"), null);
            backdropDirection = new Direction();
            backdropPosition = new Vector2(-101, -703);
        }

        public void Update(GameTime gameTime) 
        {
            if (moveBackdrop)
            {
                UpdateBackdropPosition(gameTime);
            }
        }

        public void SetPreferredBackdropPosition(Vector2 pos, String nextGameState)
        {
            moveBackdrop = true;
            preferredBackdropPosition = pos;
            this.nextGameState = nextGameState;
            totalDistance = Math.Abs(Vector2.Distance(preferredBackdropPosition, backdropPosition));
            backdropSpeed = 0;
            pressDelay = 5;
        }

        public void SetBackdropPosition(Vector2 pos)
        {
            backdropPosition = pos;
        }

        public void UpdateBackdropPosition(GameTime gameTime)
        {
            distance = Math.Abs(Vector2.Distance(preferredBackdropPosition, backdropPosition));

            backdropPosition += (backdropSpeed * backdropDirection.GetDirectionAsVector()) * gameTime.ElapsedGameTime.Milliseconds;
            backdropDirection.RotateTowardsPoint(backdropPosition, preferredBackdropPosition, 1f);

            if (distance > totalDistance / 2)
            {
                backdropSpeed += backdropAcc;
            }
            else if (distance <= totalDistance / 2)
            {
                backdropSpeed -= backdropAcc;
            }

            pressDelay--;

            if (pressDelay < 0 &&
                (ControlManager.CheckPress(RebindableKeys.Action1) || 
                ControlManager.CheckPress(RebindableKeys.Action2) ||
                ControlManager.CheckKeypress(Keys.Enter) ||
                (ControlManager.IsLeftMouseButtonClicked() && game.IsActive)))
            {
                backdropSpeed = 0;
                backdropPosition = preferredBackdropPosition;
                game.stateManager.ChangeState(nextGameState);
                moveBackdrop = false;
            }

            if (distance < 5)
            {
                backdropSpeed = 0;
                game.stateManager.ChangeState(nextGameState);
                moveBackdrop = false;
            }
        }
    }
}
