﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class CreditState : GameState
    {
        private TextBox textBox;
        private SpriteFont spriteFont;

        private float txtSpeed;
        private const float txtMaxSpeed = 0.0f;
        private const float txtMinSpeed = 0.0f;
        private Texture2D backdrop;  

        public CreditState(Game1 Game, string name) :
            base(Game, name)
        {
            spriteFont = Game.fontManager.GetFont(14);
            textBox = TextUtils.CreateTextBox(spriteFont, new Rectangle(10, 300, Game.Window.ClientBounds.Width - 20,
                                        Game.Window.ClientBounds.Height - 20), false, 
                                        "This game was developed by:\n\nDaniel Alm Grundstrom\n\nJakob Willforss\n\nJohan Philipsson");
        }

        public override void Initialize()
        {
            txtSpeed = 0.15f;
            base.Initialize();
            backdrop = Game.Content.Load<Texture2D>("Overworld-Sprites/introBackdrop");
        }

        public override void OnEnter()
        {
        }

        public override void OnLeave()
        {          
        }

        public override void Update(GameTime gameTime)
        {
            //Game.Window.Title = ("SpaceExplorationGame - " + "Outro");

            #region Input
            
            //if (ControlManager.CheckHold(RebindableKeys.Up))
            //{
            //    txtSpeed += 0.01f;
            //}
            //
            //else
            //    txtSpeed -= 0.01f;
            //
            if (ControlManager.IsGamepadConnected == false)
            {
            
                if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)))
                {
                    Game.stateManager.ChangeState("MainMenuState");
                }
            }
            
            if (ControlManager.IsGamepadConnected == true)
            {
            
                if (ControlManager.CurrentGamepadState.IsButtonDown(ControlManager.GamepadAction))
                {
                    Game.stateManager.ChangeState("MainMenuState");
                }
            }

            if (ControlManager.IsLeftMouseButtonClicked())
            {
                Game.stateManager.ChangeState("MainMenuState");
            }
            
            #endregion

            textBox.TextBoxPosY -= txtSpeed;

            if (txtSpeed > txtMaxSpeed)
                txtSpeed = txtMaxSpeed;

            else if (txtSpeed < txtMinSpeed)
                txtSpeed = txtMinSpeed;

            if (textBox.TextBoxPosY < -20 || ControlManager.CheckKeypress(Keys.Enter))
            {
                Game.Restart();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backdrop, Vector2.Zero, Color.White);
            textBox.Draw(spriteBatch, Game.fontManager.FontColor, Game.fontManager.FontOffset);
        }

    }
}