using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class HelperBox
    {
        private Game1 Game;

        private Vector2 position;
        private Vector2 origin;

        private bool visible;
        private string text;

        public bool Visible { get { return visible; } set { visible = value; } }
        public string Text { get { return text; } set { text = value; } }

        private bool timedText;
        private float timeToShowText;

        public HelperBox(Game1 Game)
        {
            this.Game = Game;
            visible = false;
            text = "";
            timedText = false;
        }

        public void Update(GameTime gameTime)
        {
            if (timeToShowText > 0)
            {
                timeToShowText -= gameTime.ElapsedGameTime.Milliseconds;
                position = new Vector2(Game.camera.cameraPos.X, Game.camera.cameraPos.Y + Game.Window.ClientBounds.Height / 4);
            }

            else if (timeToShowText <= 0 && timedText)
            {
                timedText = false;
                text = "";
                visible = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible && text != "" && !Game.stateManager.overworldState.IsBurnOutEndingActivated)
            {
                if (timedText 
                    && timeToShowText > 0)
                {
                    spriteBatch.DrawString(Game.fontManager.GetFont(14), text, position + Game.fontManager.FontOffset, Game.fontManager.FontColor, 0f, origin, 1f, SpriteEffects.None, 0.9f);
                }
                else
                {
                    spriteBatch.DrawString(Game.fontManager.GetFont(14), text, position + Game.fontManager.FontOffset, Game.fontManager.FontColor, 0f, origin, 1f, SpriteEffects.None, 0.9f);
                }
            }

            if (!timedText)
                visible = false;
        }

        public void DisplayText(string text)
        {
            if (!timedText)
            {
                if (GameStateManager.currentState.Equals("OverworldState"))
                {
                    position = new Vector2(Game.camera.cameraPos.X, Game.camera.cameraPos.Y + Game.Window.ClientBounds.Height / 4);
                }

                else
                {
                    position = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 4 * 3);
                }

                origin = Game.fontManager.GetFont(14).MeasureString(text) / 2;
                visible = true;
                this.text = text;
            }
        }

        public void DisplayText(string text, int seconds)
        {
            if (timeToShowText <= 0)
            {
                timedText = true;
                timeToShowText = seconds * 1000;

                origin = Game.fontManager.GetFont(14).MeasureString(text) / 2;
                visible = true;
                this.text = text;
            }
        }
    }
}
