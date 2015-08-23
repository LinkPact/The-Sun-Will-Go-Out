using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum TierType
    {
        None = 0,
        Low = 1,
        Average = 2,
        Good = 3,
        Great = 4,
        Excellent = 5
    }

    public abstract class Item
    {
        protected Game1 Game;
        protected SpriteFont font;

        public String Name;
        public String Kind;
        public float Weight;
        public float Value;
        public TierType Tier;

        public int ShopColor;

        protected Random random;

        protected Sprite displaySprite;

        protected Item(Game1 Game)
        {
            this.Game = Game;
            Weight = 1.0f;
            Value = 0;
            Tier = TierType.None;
            Name = "";
            Kind = "";

            random = new Random();
        }

        public abstract String RetrieveSaveData();

        public void DisplayInventoryInfo(SpriteBatch spriteBatch, SpriteFont font, Color textColor)
        {
            Vector2 position = new Vector2(54, Game1.ScreenSize.Y / 2 + 52);
            DisplayInfo(spriteBatch, font, position, textColor, Game1.ScreenSize.X / 2 - 100);
        }

        public void DisplayInfo(SpriteBatch spriteBatch, SpriteFont font,
            Vector2 startingPos, Color color, int wordWrapWidth)
        {
            List<String> infoText = GetInfoText();
            float deltaY = 15;
            float titleSpacing = 5;

            // Draw title
            spriteBatch.DrawString(font, infoText[0], new Vector2(startingPos.X,
                startingPos.Y) + Game.fontManager.FontOffset,
                color, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

            // Draw item stats, skips title so starts at index 1
            for (int n = 1; n < infoText.Count; n++)
            {
                String text = infoText[n];
                if (n == infoText.Count - 1)
                {
                    text = "Description: " + infoText[n];
                    text = TextUtils.WordWrap(font, infoText[n], wordWrapWidth);   
                }

                spriteBatch.DrawString(font, text, new Vector2(startingPos.X,
                    startingPos.Y + deltaY * n + titleSpacing) + Game.fontManager.FontOffset,
                    color, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }

            if (displaySprite != null) 
            {
                float imageOffset = 192;
                spriteBatch.Draw(displaySprite.Texture, new Vector2(startingPos.X + imageOffset, startingPos.Y), displaySprite.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }

        protected abstract List<String> GetInfoText();

        protected abstract String GetDescription();
    }
}
