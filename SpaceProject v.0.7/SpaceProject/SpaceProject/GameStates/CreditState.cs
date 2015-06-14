using System;
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
            int xOffset = 10;
            int yOffset = 50;

            spriteFont = Game.fontManager.GetFont(14);
            textBox = TextUtils.CreateTextBox(spriteFont, new Rectangle(xOffset, yOffset, Game.Window.ClientBounds.Width - 20,
                                        Game.Window.ClientBounds.Height - 20), false, false,
                                        "Design and development:\nDaniel Alm Grundstrom\nJakob Willforss\nJohan Philipsson\n\n"+
                                        "Visuals:\nDaniel Alm Grundstrom\n\n" +
                                        "Music:\nDaniel Alm Grundstrom\nJakob Willforss\n\n" +
                                        "Portraits:\nJosefin Voigt (many thanks!)\n\n" +
                                        "Fonts:\nIceland by Cyreal\nISL Jupiter by Isurus Labs\n\n" +
                                        "Space sounds:\nSpace Music Ambient by evanjones4\n\n" +
                                        "Sound effects:\n'menu click' by fins (Creative Commons 0 Licence)\n'Hover 2' by plasterbrain (Creative Commons 0 Licence)\n'" + 
                                        "ship fire' by Nbs Dark (Creative Commons 0 Licence)\n'Thruster_Level_II' by nathanshadow (Sampling+ Licence)\n\n" +
                                        "..and many thanks to all our testers, we really appreciate it.\nTell us, and your name will be here in the final version!");
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
            if (ControlManager.GamepadReady == false)
            {
                if (ControlManager.CheckPress(RebindableKeys.Action1) 
                    || ControlManager.CheckKeyPress(Keys.Enter)
                    || ControlManager.CheckPress(RebindableKeys.Action2)
                    || ControlManager.CheckKeyPress(Keys.Escape))
                {
                    Game.stateManager.ChangeState("MainMenuState");
                }
            }

            if (ControlManager.GamepadReady == true)
            {            
                if (ControlManager.CurrentGamepadState.IsButtonDown(ControlManager.GamepadAction))
                {
                    Game.stateManager.ChangeState("MainMenuState");
                }
            }

            //if (ControlManager.IsLeftMouseButtonClicked())
            //{
            //    Game.stateManager.ChangeState("MainMenuState");
            //}
            
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
