﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class ShipManagerText
    {
        #region init
        private Game1 Game;

        //Denotes current active position of cursor.
        private static int layer;
        private static int layer1pos;
        private static int layer2pos;
        private static int layer3pos;

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

            topDisplayPos = new Vector2(Game.Window.ClientBounds.Width * 3/4, 30);
            equippedDisplayPos = new Vector2(Game.Window.ClientBounds.Width / 2 + 30, 70);

            equippedStartPos = new Vector2(Game.Window.ClientBounds.Width / 2 + 50, 93);
            ownedStartPos = new Vector2(Game.Window.ClientBounds.Width / 2 + 30, 150);
        }

        public void Update(GameTime gameTime, int layer_, int layer1pos_, int layer2pos_, int layer3pos_)
        {
            layer = layer_;
            layer1pos = layer1pos_;
            layer2pos = layer2pos_;
            layer3pos = layer3pos_;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DisplayGeneralInfo(spriteBatch);
            DisplayHelpText(spriteBatch);
        }

        public void DisplayGeneralInfo(SpriteBatch spriteBatch)
        {
            //Fuel
            string tempString = "Fuel: " + Math.Round(StatsManager.Fuel, 0) + " / " + StatsManager.MaxFuel + " l";
            Color tempColor;

            if (StatsManager.Fuel > StatsManager.MaxFuel / 2)
                tempColor = FontManager.FontColorStatic;

            else if (StatsManager.Fuel > StatsManager.MaxFuel / 4)
                tempColor = Color.Orange;

            else
                tempColor = Color.Red;


            spriteBatch.DrawString(FontManager.GetFontStatic(16), tempString, new Vector2(Game.Window.ClientBounds.Width / 2 + 20, Game.Window.ClientBounds.Height * 3 / 4 + 30) + FontManager.FontOffsetStatic, tempColor);

            //Money
            spriteBatch.DrawString(FontManager.GetFontStatic(16), "Money: " + (int)StatsManager.Rupees + " Rupees", new Vector2(Game.Window.ClientBounds.Width / 2 + 20, Game.Window.ClientBounds.Height * 3 / 4 + 60) + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
        }
        
        public static void DisplayPrimaryWeaponInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Primary weapons", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Primary weapon") / 2, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Equipped weapons", equippedDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);

            for (int n = 0; n < ShipInventoryManager.primarySlots; n++)
            {
                spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.equippedPrimaryWeapons[n].Name, new Vector2(equippedStartPos.X, equippedStartPos.Y + n * 23) + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
            }

            DisplayList(spriteBatch, "Owned weapons", ShipInventoryManager.OwnedPrimaryWeapons, ownedStartPos + FontManager.FontOffsetStatic, ySpacing);
        }

        public static void DisplaySecondaryInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Secondary weapons", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Secondary weapons") / 2, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Equipped weapon", equippedDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.equippedSecondary.Name, equippedStartPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
            DisplayList(spriteBatch, "Owned secondary", ShipInventoryManager.OwnedSecondary, ownedStartPos + FontManager.FontOffsetStatic, ySpacing);
        }

        public static void DisplayPlatingInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Ships", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Ships") / 2, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Equipped ship", equippedDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.equippedPlating.Name, equippedStartPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
            DisplayList(spriteBatch, "Owned platings", ShipInventoryManager.OwnedPlatings, ownedStartPos + FontManager.FontOffsetStatic, ySpacing);
        }
        
        public static void DisplayEnergyCellInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Energy cells", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Energy cells") / 2, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Equipped cell", equippedDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.equippedEnergyCell.Name, equippedStartPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
            DisplayList(spriteBatch, "Owned energy cells", ShipInventoryManager.OwnedEnergyCells, ownedStartPos + FontManager.FontOffsetStatic, ySpacing);
        }
        
        public static void DisplayShieldInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Shields", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Shields") / 2, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Equipped shield", equippedDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.equippedShield.Name, equippedStartPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
            DisplayList(spriteBatch, "Owned shields", ShipInventoryManager.OwnedShields, ownedStartPos + FontManager.FontOffsetStatic, ySpacing);
        }

        private static void DisplayList(SpriteBatch spriteBatch, String tag, List<ShipPart> partList, Vector2 startPosition, float deltaY)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), tag, startPosition, FontManager.FontColorStatic);

            int pos = 1;
            foreach (ShipPart part in partList)
            {
                spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), part.Name, new Vector2(startPosition.X + 20, startPosition.Y + pos * deltaY), FontManager.FontColorStatic);
                pos++;
            }
        }

        public static void DisplayInventory(SpriteBatch spriteBatch, Vector2 windowSize)
        {
            int itemCount = ShipInventoryManager.ShipItems.Count;

            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Inventory", topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString("Inventory") / 2, 1.0f, SpriteEffects.None, 0.5f);

            if (layer == 3 && ShipInventoryManager.ShipItems[layer2pos] != ShipInventoryManager.equippedEnergyCell && ShipInventoryManager.ShipItems[layer2pos] != ShipInventoryManager.equippedPlating
                && ShipInventoryManager.ShipItems[layer2pos].Kind != "Empty")
                spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Trash", new Vector2(windowSize.X / 2 + 20, windowSize.Y * 2 / 3) + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            else
                spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), "Trash", new Vector2(windowSize.X / 2 + 20, windowSize.Y * 2 / 3) + FontManager.FontOffsetStatic, Color.Gray, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

            int columnSize = 14;
            int inventory = ShipInventoryManager.inventorySize;

            if (inventory > columnSize)
            {
                for (int n = 0; n < columnSize; n++)
                {
                    if (IsEquipped(ShipInventoryManager.ShipItems[n]))
                        spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.ShipItems[n].Name, new Vector2(windowSize.X / 2 + 20, 40 + n * 23) + FontManager.FontOffsetStatic, Color.Blue);
                    else
                        spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.ShipItems[n].Name, new Vector2(windowSize.X / 2 + 20, 40 + n * 23) + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
                }

                for (int n = 0; n < inventory - columnSize; n++)
                {
                    if (IsEquipped(ShipInventoryManager.ShipItems[n + columnSize]))
                        spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.ShipItems[n + columnSize].Name, new Vector2(windowSize.X * 3 / 4, 40 + n * 23) + FontManager.FontOffsetStatic, Color.Blue);
                    else
                        spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.ShipItems[n + columnSize].Name, new Vector2(windowSize.X * 3 / 4, 40 + n * 23) + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
                }
            }
            else
            {
                for (int n = 0; n < inventory; n++)
                {
                    if (IsEquipped(ShipInventoryManager.ShipItems[n]))
                        spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.ShipItems[n].Name, new Vector2(windowSize.X / 2 + 20, 40 + n * 23) + FontManager.FontOffsetStatic, Color.Blue);
                    else
                        spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), ShipInventoryManager.ShipItems[n].Name, new Vector2(windowSize.X / 2 + 20, 40 + n * 23) + FontManager.FontOffsetStatic, FontManager.FontColorStatic);
                }
            }

        }

        public static void DisplayBackInfo(SpriteBatch spriteBatch)
        {
            String titleString = "Leave inventory";
            spriteBatch.DrawString(FontManager.GetFontStatic(fontSize), titleString, topDisplayPos + FontManager.FontOffsetStatic, FontManager.FontColorStatic, 0, FontManager.GetFontStatic(16).MeasureString(titleString) / 2, 1.0f, SpriteEffects.None, 0.5f);
        }

        private void DisplayHelpText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(14), ControlManager.GetKeyName(RebindableKeys.Action2) + " - Go back", new Vector2(10, Game.Window.ClientBounds.Height - FontManager.GetFontStatic(14).MeasureString(ControlManager.GetKeyName(RebindableKeys.Action2) + " - Go back").Y - 10), FontManager.FontColorStatic);
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