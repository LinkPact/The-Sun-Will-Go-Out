using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class AutoSaveHandler
    {
        private readonly string AutoSaveMessage = "The game has been saved.";
        private readonly Color MessageColor = Color.LightGreen;

        private Game1 game;
        private SpriteFont font;

        private Vector2 position
        {
            get
            {
                return new Vector2(game.camera.Position.X + windowSize.X / 2 - textSize.X,
                    game.camera.Position.Y - windowSize.Y / 2 + textSize.Y);
            }
        }
        private Vector2 windowSize;
        private Vector2 textSize;

        private static bool showText;
        private static float hideTime;

        public AutoSaveHandler(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            font = FontManager.GetFontStatic(14);
            textSize = font.MeasureString(AutoSaveMessage);
            windowSize = new Vector2(Game1.ScreenSize.X, Game1.ScreenSize.Y);
        }

        public void Update(GameTime gameTime)
        {
            if (showText && StatsManager.PlayTime.HasOverworldTimePassed(hideTime))
            {
                showText = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (showText)
            {
                spriteBatch.DrawString(font, AutoSaveMessage, position, MessageColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }

        public static void DisplayAutoSaveMessage(int milliseconds)
        {
            hideTime = StatsManager.PlayTime.GetFutureOverworldTime(milliseconds);
            showText = true;
        }
    }
}
