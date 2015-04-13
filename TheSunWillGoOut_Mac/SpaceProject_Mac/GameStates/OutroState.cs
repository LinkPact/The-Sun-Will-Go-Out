using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
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
        private readonly int ParagraphTime = 7000;
        private readonly byte ColorAlpha = 4;

        private int textHeight;

        private ConfigFile outroFile;
        private List<string> text;
        private string currentLine;
        private bool fadeOut;
        private Color textColor;
        private int textTimer;

        private TextBox textBox;
        private SpriteFont spriteFont;

        private bool scrolling;
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

            outroFile = new ConfigFile();
            outroFile.Load("Data/storydata.dat");

            text = new List<string>();
        }

        public override void Update(GameTime gameTime)
        {
            if (ControlManager.CheckKeyPress(Keys.Escape))
            {
                Game.Restart();
            }

            if (scrolling)
            {
                UpdateScrolling(gameTime);
            }
            else
            {
                if (textTimer % 5 == 0
                    && text.Count > 0)
                {
                    bool temp;
                    currentLine = TextUtils.ScrollText(text[0], false, out temp);
                }

                UpdateFading(gameTime);
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

            if (scrolling)
            {
                textBox.Draw(spriteBatch, Game.fontManager.FontColor, Game.fontManager.FontOffset);
            }
            else if (text.Count > 0)
            {
                spriteBatch.DrawString(spriteFont, currentLine, Game.ScreenCenter, textColor, 0f,
                    spriteFont.MeasureString(text[0]) / 2, 1f, SpriteEffects.None, 1f);
            }
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
                    scrolling = true;
                    break;

                case OutroType.RebelEnd:
                    section = "rebelending";
                    paragraphs = 5;
                    ActiveSong = Music.Outro;
                    scrolling = true;
                    break;

                case OutroType.AllianceEnd:
                    section = "allianceending";
                    paragraphs = 7;
                    ActiveSong = Music.Outro;
                    scrolling = true;
                    break;

                case OutroType.OnYourOwnEnd:
                    section = "onyourownending";
                    paragraphs = 8;
                    ActiveSong = Music.GoingOut;
                    scrolling = false;
                    textTimer = ParagraphTime;
                    textColor = new Color(0, 0, 0, 0);
                    fadeOut = false;
                    currentLine = "";
                    break;
            }

            for (int i = 1; i <= paragraphs; i++ )
            {
                tempList.Add(section + i.ToString());
            }

            if (scrolling)
            {
                textBox.LoadFormattedText(section,
                                         tempList,
                                         null);
                textHeight = (int)spriteFont.MeasureString(TextUtils.WordWrap(spriteFont, textBox.GetText(), TextBoxWidth)).Y;
            }
            else
            {
                for (int i = 1; i <= paragraphs; i++)
                {
                    text.Add(outroFile.GetPropertyAsString(section, section + i, "I AM ERROR!"));
                }

                text.Add("The End");
            }
        }

        private void UpdateScrolling(GameTime gameTime)
        {
            CheckInput();

            if (txtSpeed > txtMaxSpeed)
            {
                txtSpeed = txtMaxSpeed;
            }

            else if (txtSpeed < txtMinSpeed)
            {
                txtSpeed = txtMinSpeed;
            }

            textBox.TextBoxPosY -= txtSpeed;

            if (textBox.TextBoxPosY + textHeight < 0)
            {
                Game.Restart();
            }
        }

        private void UpdateFading(GameTime gameTime)
        {
            textTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (fadeOut)
            {
                if (textTimer % 2 == 0
                    && textColor.A > ColorAlpha * 2)
                {
                    textColor.R -= (byte)(ColorAlpha * 2);
                    textColor.G -= (byte)(ColorAlpha * 2);
                    textColor.B -= (byte)(ColorAlpha * 2);
                    textColor.A -= (byte)(ColorAlpha * 2);
                }
            }

            else
            {
                if (textTimer % 2 == 0
                    && textColor.A < 255 - ColorAlpha)
                {
                    textColor.R += ColorAlpha;
                    textColor.G += ColorAlpha;
                    textColor.B += ColorAlpha;
                    textColor.A += ColorAlpha;
                }
            }

            if (textTimer <= ParagraphTime / 3)
            {
                fadeOut = true;
            }

            if (textTimer <= 0
                && text.Count > 0)
            {
                text.RemoveAt(0);
                textTimer = ParagraphTime;
                textColor = new Color(0, 0, 0, 0);
                fadeOut = false;
                currentLine = "";
            }

            else if (text.Count <= 0)
            {
                Game.Restart();
            }
        }

        private void CheckInput()
        {
            if (ControlManager.CheckHold(RebindableKeys.Up))
            {
                txtSpeed += 0.01f;
            }

            else
                txtSpeed -= 0.01f;
        }
    }
}
