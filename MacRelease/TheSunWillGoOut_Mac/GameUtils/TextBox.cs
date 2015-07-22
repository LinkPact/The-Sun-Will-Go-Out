using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class TextBox
    {
        private ConfigFile configFile;
        //private bool readConfigFile;
        private SpriteFont spriteFont;

        //private List<string> sections;
        //private string currentSection;
        //private int currentSectionIndex;

        private float textBoxPosX;
        private float textBoxPosY;
        private Rectangle textBoxRect;

        private string text;
        private string textBuffer;
        private bool useOrigin;
        private bool useScrolling;
        private bool scrollingFinished;
        private bool flushScrollText = false;
        private Vector2 textOrigin;

        #region Properties

        public float TextBoxPosX
        {
            get { return textBoxPosX; }
            set { textBoxPosX = value; }
        }

        public float TextBoxPosY
        {
            get { return textBoxPosY; }
            set { textBoxPosY = value; }
        }

        public Rectangle TextBoxRect
        {
            get { return textBoxRect; }
            set { textBoxRect = value; }
        }

        public bool Scrolling
        {
            get { return useScrolling; }
        }

        public bool FinishedScrolling
        {
            get { return scrollingFinished; }
        }

        #endregion

        //Creates a textbox with no predefined text
        public TextBox(SpriteFont font, Rectangle rect, bool origin)
        {
            configFile = new ConfigFile();
            //readConfigFile = true;

            spriteFont = font;

            textBoxPosX = rect.X;
            textBoxPosY = rect.Y;

            textBoxRect = rect;
            useOrigin = origin;
        }

        //Creates a textbox and adds the text sent in into the textbuffer before reading from the config file
        public TextBox(SpriteFont font, Rectangle rect, string text, bool origin)
        {
            configFile = null;

            spriteFont = font;

            textBoxPosX = rect.X;
            textBoxPosY = rect.Y;

            textBoxRect = rect;
            textBuffer = TextUtils.RemoveTextBetween(text, '{', '}');
            useOrigin = origin;
        }

        public void Initialize()
        {
            //sections = new List<string>();
            //currentSectionIndex = 0;
        }

        //Sets the config file to use
        public void SetSource(string path)
        {
            configFile.Load(path);
        }

        //Reads text from the config file
        public void LoadText(string section, string variable)
        { 
            textBuffer += configFile.GetPropertyAsString(section, variable, "");
            textBuffer = TextUtils.RemoveTextBetween(textBuffer, '{', '}');
        }

        //Reads text from the config file and stores string formatted as a list
        public void LoadFormattedText(string section, List<string> variableList, List<string> textList)
        {
            if (textList != null)
            {
                for (int i = 0; i < textList.Count; i++)
                {
                    textBuffer += textList[i] + ": " + configFile.GetPropertyAsString(section, variableList[i], "") + "\n\n";
                }
            }

            else
            {
                for (int i = 0; i < variableList.Count; i++)
                {
                    textBuffer += configFile.GetPropertyAsString(section, variableList[i], "") + "\n\n"; 
                }
            }

            textBuffer = TextUtils.RemoveTextBetween(textBuffer, '{', '}');
        }

        public String GetText()
        {
            return textBuffer;
        }

        public void SetText(String text)
        {
            textBuffer = TextUtils.RemoveTextBetween(text, '{', '}');
        }

        public void SetScrolling(bool scroll)
        {
            useScrolling = scroll;
        }

        public void Update(GameTime gameTime)
        {
            if (textBuffer != null)
            {
                if (useScrolling && !scrollingFinished)
                {
                    text = TextUtils.WordWrap(spriteFont,
                              TextUtils.ScrollText(textBuffer,
                                                   flushScrollText,
                                                   out scrollingFinished),
                              textBoxRect.Width);
                }
                else if (scrollingFinished)
                {
                    TextUtils.RefreshTextScrollBuffer();
                }
                else
                {
                    text = TextUtils.WordWrap(spriteFont, textBuffer, textBoxRect.Width);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color textColor, Vector2 fontOffset)
        {
            if (text != null)
            {
                if (useOrigin == true)
                    textOrigin = new Vector2(spriteFont.MeasureString(text).X / 2, 0);
                else
                    textOrigin = Vector2.Zero;

                Vector2 textBoxPosition = new Vector2(textBoxPosX, textBoxPosY);

                spriteBatch.DrawString(spriteFont,
                                       text,
                                       textBoxPosition + fontOffset,
                                       textColor,
                                       0f,
                                       textOrigin,
                                       1f,
                                       SpriteEffects.None,
                                       1f);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color textColor, Vector2 fontOffset, float layerDepth)
        {
            if (text != null)
            {
                if (useOrigin == true)
                    textOrigin = new Vector2(spriteFont.MeasureString(text).X / 2, 0);
                else
                    textOrigin = Vector2.Zero;

                Vector2 textBoxPosition = new Vector2(textBoxPosX, textBoxPosY);

                spriteBatch.DrawString(spriteFont,
                                   text,
                                   textBoxPosition + fontOffset,
                                   textColor,
                                   0f,
                                   textOrigin,
                                   1f,
                                   SpriteEffects.None,
                                   layerDepth);
            }
        }

        public void FlushText()
        {
            flushScrollText = true;
        }
    }
}
