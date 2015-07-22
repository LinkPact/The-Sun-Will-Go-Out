using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class ShipManagerText
    {
        #region init
        private Game1 Game;

        //Denotes current active position of cursor.
        private static int layer;
        private static int layer1pos;
        private static int layer2pos;

        private static Vector2 topDisplayPos;
        private static Vector2 equippedDisplayPos;

        private static Vector2 equippedStartPos;
        private static Vector2 ownedStartPos;

        private const int fontSize = 16;
        private const int ySpacing = 23;

        #endregion
        public ShipManagerText(Game1 Game)
        {
            this.Game = Game;
        }

        public void Initialize()
        {
            FontManager.FontColorStatic = Game.fontManager.FontColor;

            topDisplayPos = new Vector2(Game1.ScreenSize.X * 3/4, 30);
            equippedDisplayPos = new Vector2(Game1.ScreenSize.X / 2 + 30, 70);

            equippedStartPos = new Vector2(Game1.ScreenSize.X / 2 + 50, 93);
            ownedStartPos = new Vector2(Game1.ScreenSize.X / 2 + 30, 150);
        }

        public void Update(GameTime gameTime, int layer_, int layer1pos_, int layer3pos_)
        {
            layer = layer_;
            layer1pos = layer1pos_;
            layer2pos = layer3pos_;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DisplayGeneralInfo(spriteBatch);
        }

        public void DisplayGeneralInfo(SpriteBatch spriteBatch)
        {
            //Money
            spriteBatch.DrawString(FontManager.GetFontStatic(16),
                "Money: " + (int)StatsManager.Crebits + " Crebits",
                new Vector2(Game1.ScreenSize.X / 2 + Game1.ScreenSize.X / 4, Game1.ScreenSize.Y * 3 / 4 + 30) + FontManager.FontOffsetStatic,
                FontManager.FontColorStatic,
                0f,
                FontManager.GetFontStatic(16).MeasureString("Money: " + (int)StatsManager.Crebits + " Crebits") / 2,
                1f,
                SpriteEffects.None,
                1f);
        }
        
        public static void DisplayBackInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Back", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Back") / 2, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Leave inventory", equippedDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
        }

        public static void DisplayPrimaryWeaponInfo1(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Primary weapon slot 1", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Primary weapon slot 1") / 2, 1.0f, SpriteEffects.None, 0.5f);
            DisplayList(spriteBatch, "Owned weapons", ShipInventoryManager.GetAvailablePrimaryWeapons(1), equippedDisplayPos + FontManager.FontOffsetStatic, ySpacing, 1);
        }

        public static void DisplayPrimaryWeaponInfo2(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Primary weapon slot 2", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Primary weapon slot 2") / 2, 1.0f, SpriteEffects.None, 0.5f);
            DisplayList(spriteBatch, "Owned weapons", ShipInventoryManager.GetAvailablePrimaryWeapons(2), equippedDisplayPos + FontManager.FontOffsetStatic, ySpacing, 2);
        }

        public static void DisplaySecondaryInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Secondary weapon", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Secondary weapon") / 2, 1.0f, SpriteEffects.None, 0.5f);
            DisplayList(spriteBatch, "Owned secondary", ShipInventoryManager.OwnedSecondary, equippedDisplayPos + FontManager.FontOffsetStatic, ySpacing);
        }

        public static void DisplayPlatingInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Platings", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Plating") / 2, 1.0f, SpriteEffects.None, 0.5f);
            DisplayList(spriteBatch, "Owned platings", ShipInventoryManager.OwnedPlatings, equippedDisplayPos + FontManager.FontOffsetStatic, ySpacing);
        }
        
        public static void DisplayEnergyCellInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Energy cell", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Energy cell") / 2, 1.0f, SpriteEffects.None, 0.5f);
            DisplayList(spriteBatch, "Owned energy cells", ShipInventoryManager.OwnedEnergyCells, equippedDisplayPos + FontManager.FontOffsetStatic, ySpacing);
        }
        
        public static void DisplayShieldInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Shields", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Shields") / 2, 1.0f, SpriteEffects.None, 0.5f);
            DisplayList(spriteBatch, "Owned shields", ShipInventoryManager.OwnedShields, equippedDisplayPos + FontManager.FontOffsetStatic, ySpacing);
        }

        private static void DisplayList(SpriteBatch spriteBatch, String tag, List<ShipPart> partList, Vector2 startPosition, float deltaY, int slot = 0)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), tag, startPosition, FontManager.FontColorStatic);

            int pos = 1;
            foreach (ShipPart part in partList)
            {
                string name = part.Name;
                Color color = Color.White;

                if (!part.Kind.ToLower().Equals("primary")
                    && ShipInventoryManager.IsEquipped(part))
                {
                    name += " [equipped]";
                }

                else if (part.Kind.ToLower().Equals("primary")
                    && ShipInventoryManager.IsEquippedAt(part, slot))
                {
                    name += " [equipped]";
                }

                if (layer == 2
                    && layer2pos == partList.IndexOf(part))
                {
                    color = FontManager.FontSelectColor1;
                }

                spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), name, new Vector2(startPosition.X + 20, startPosition.Y + pos * deltaY), color);
                pos++;
            }

            if (layer == 2)
            {
                Color color = Color.White;
                if (layer2pos == partList.Count)
                {
                    color = FontManager.FontSelectColor1;
                }

                spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Back", new Vector2(startPosition.X + 20, startPosition.Y + pos * deltaY), color);
            }
        }
        
        private static bool IsEquipped(Item item)
        {
            if (item == ShipInventoryManager.equippedEnergyCell || item == ShipInventoryManager.equippedSecondary ||
                item == ShipInventoryManager.equippedShield || item == ShipInventoryManager.equippedPlating)
            {
                return true;
            }

            foreach (Item primary in ShipInventoryManager.equippedPrimaryWeapons)
            {
                if (item == primary) { return true; }
            }

            return false;
        }
    }
}
