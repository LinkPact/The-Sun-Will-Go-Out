using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class OutroState : GameState
    {
        private TextBox textBox;
        private SpriteFont spriteFont;

        private float txtSpeed;
        private const float txtMaxSpeed = 0.75f;
        private const float txtMinSpeed = 0.15f;
        private Texture2D backdrop;  

        public OutroState(Game1 Game, string name) :
            base(Game, name)
        {
            spriteFont = Game.fontManager.GetFont(14);
            textBox = TextUtils.CreateTextBoxConfig(spriteFont,
                                                    new Rectangle(10,
                                                                  Game.Window.ClientBounds.Height,
                                                                  Game.Window.ClientBounds.Width - 20,
                                                                  Game.Window.ClientBounds.Height - 20),
                                                    "Data/storydata.dat",
                                                    false);
        }

        public override void Initialize()
        {
            txtSpeed = 0.15f;
            base.Initialize();
            backdrop = Game.Content.Load<Texture2D>("Overworld-Sprites/introBackdrop");

            List<string> tempList = new List<string>();

            tempList.Add("Outro1");

            textBox.LoadFormattedText("Outro",
                                     tempList,
                                     null);
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

            if (ControlManager.CheckHold(RebindableKeys.Up))
            {
                txtSpeed += 0.01f;
            }

            else
                txtSpeed -= 0.01f;

            if (ControlManager.GamepadReady == false)
            {

                if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter)))
                {
                    Game.Restart();
                }
            }

            if (ControlManager.GamepadReady == true)
            {

                if (ControlManager.CurrentGamepadState.IsButtonDown(ControlManager.GamepadAction))
                {
                    Game.Restart();
                }
            }

            #endregion

            textBox.TextBoxPosY -= txtSpeed;

            if (txtSpeed > txtMaxSpeed)
                txtSpeed = txtMaxSpeed;

            else if (txtSpeed < txtMinSpeed)
                txtSpeed = txtMinSpeed;

            if (textBox.TextBoxPosY < -20 || ControlManager.CheckKeyPress(Keys.Enter))
            {
                Game.Restart();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backdrop,
                             Vector2.Zero,
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width / Game.DefaultResolution.X,
                                         Game.Window.ClientBounds.Height / Game.DefaultResolution.Y),
                             SpriteEffects.None,
                             0.5f);

            textBox.Draw(spriteBatch, Game.fontManager.FontColor, Game.fontManager.FontOffset);
        }

    }
}
