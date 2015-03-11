using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public enum OutroType
    {
        GameOver,
        RebelEnd,
        AllianceEnd,
        OnYourOwnEnd
    }

    public class OutroState : GameState
    {
        private readonly int TextBoxWidth = 600;

        private TextBox textBox;
        private SpriteFont spriteFont;

        private float txtSpeed;
        private const float txtMaxSpeed = 0.75f;
        private const float txtMinSpeed = 0.15f;
        private Texture2D backdrop;

        public OutroState(Game1 Game, string name) :
            base(Game, name)
        {
            spriteFont = Game.fontManager.GetFont(16);
            textBox = TextUtils.CreateTextBoxConfig(spriteFont,
                                                    new Rectangle((Game.Window.ClientBounds.Width - TextBoxWidth) / 2,
                                                                  Game.Window.ClientBounds.Height,
                                                                  TextBoxWidth,
                                                                  Game.Window.ClientBounds.Height - 20),
                                                    "Data/storydata.dat",
                                                    false);
        }

        public override void Initialize()
        {
            txtSpeed = 0.15f;
            base.Initialize();
            backdrop = Game.Content.Load<Texture2D>("Overworld-Sprites/introBackdrop");
        }

        public override void Update(GameTime gameTime)
        {
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

        public void SetOutroType(OutroType outroType)
        {
            string section = "";
            int paragraphs = 0;
            List<string> tempList = new List<string>();

            switch (outroType)
            {
                case OutroType.GameOver:
                    section = "gameover";
                    paragraphs = 1;
                    ActiveSong = Music.none;
                    break;

                case OutroType.RebelEnd:
                    section = "rebelending";
                    paragraphs = 5;
                    ActiveSong = Music.Outro;
                    break;

                case OutroType.AllianceEnd:
                    section = "allianceending";
                    paragraphs = 7;
                    ActiveSong = Music.Outro;
                    break;

                case OutroType.OnYourOwnEnd:
                    section = "onyourownending";
                    paragraphs = 8;
                    ActiveSong = Music.GoingOut;
                    break;
            }

            for (int i = 1; i <= paragraphs; i++ )
            {
                tempList.Add(section + i.ToString());
            }
                
            textBox.LoadFormattedText(section,
                                     tempList,
                                     null);
        }
    }
}
