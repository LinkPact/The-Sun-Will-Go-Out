using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class BaseManagerText
    {
        #region init
        private Game1 Game;

        //private SpriteFont fontRegular;
        //private SpriteFont smallFont;

        //Denotes current active position of cursor.
        int layer;
        int var1;
        int var2;
        int var3;

        int section;

        private Color txtColor;
        #endregion
        public BaseManagerText(Game1 Game)
        {
            this.Game = Game;
        }
        public void Initialize()
        {
            txtColor = Game.fontManager.FontColor;
            //fontRegular = Game.Content.Load<SpriteFont>("Vertical-Sprites/font14");
            //smallFont = Game.Content.Load<SpriteFont>("Vertical-Sprites/font12");
            //smallFont = Game.fontManager.GetFont(14);
            //fontRegular = Game.fontManager.GetFont(16);
        }
        public void Update(GameTime gameTime, int layer, int var1, int var2, int var3, int section)
        {
            this.layer = layer;
            this.var1 = var1;
            this.var2 = var2;
            this.var3 = var3;
            this.section = section;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            DisplayBaseItems(spriteBatch);
            if (var1 == 0) { DisplayInventory(spriteBatch); }
            else if (var1 == 1) { DisplayShipInfo(spriteBatch); }
            //else if (var1 == 2) { DisplayShipInfo(spriteBatch); }
            //else if (var1 == 3) { DisplayEnergyCellInfo(spriteBatch); }
            //else if (var1 == 4) { DisplayShieldInfo(spriteBatch); }
            //else if (var1 == 5) { DisplayInventory(spriteBatch); }
        }
        public void DisplayBaseItems(SpriteBatch spriteBatch)
        {
            int itemCount = BaseInventoryManager.BaseItems.Count;

            spriteBatch.DrawString(Game.fontManager.GetFont(16), "Base items", new Vector2(200, 170) + Game.fontManager.FontOffset, txtColor, 0, Game.fontManager.GetFont(16).MeasureString("Base items") / 2, 1.0f, SpriteEffects.None, 0.5f);

            if (section == 1)
            {
                if (layer == 3 && BaseInventoryManager.BaseItems[var2].Kind != "Empty" && section == 1)
                    spriteBatch.DrawString(Game.fontManager.GetFont(14), "Trash", new Vector2(30, 565) + Game.fontManager.FontOffset, txtColor, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                else
                    spriteBatch.DrawString(Game.fontManager.GetFont(14), "Trash", new Vector2(30, 565) + Game.fontManager.FontOffset, Color.Gray, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }

            for (int n = 0; n < 20; n++)
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(14), BaseInventoryManager.BaseItems[n].Name, new Vector2(30, 180 + n * 19) + Game.fontManager.FontOffset, txtColor);
            }

            for (int n = 20; n < BaseInventoryManager.BaseItems.Count; n++)
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(14), BaseInventoryManager.BaseItems[n].Name, new Vector2(220, 180 + (n - 20) * 19) + Game.fontManager.FontOffset, txtColor);
            }
        }
        public void DisplayInventory(SpriteBatch spriteBatch)
        {
            int itemCount = ShipInventoryManager.ShipItems.Count;

            if (var2 > itemCount) var2 = itemCount - 1;

            spriteBatch.DrawString(Game.fontManager.GetFont(16), "Inventory", new Vector2(600, 30) + Game.fontManager.FontOffset, txtColor, 0, Game.fontManager.GetFont(16).MeasureString("Inventory") / 2, 1.0f, SpriteEffects.None, 0.5f);

            if (layer == 3 && ShipInventoryManager.ShipItems[var2] != ShipInventoryManager.equippedEnergyCell && ShipInventoryManager.ShipItems[var2] != BaseInventoryManager.equippedShip
                && ShipInventoryManager.ShipItems[var2].Kind != "Empty")
                spriteBatch.DrawString(Game.fontManager.GetFont(16), "Trash", new Vector2(420, 400) + Game.fontManager.FontOffset, txtColor, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            else
                spriteBatch.DrawString(Game.fontManager.GetFont(16), "Trash", new Vector2(420, 400) + Game.fontManager.FontOffset, Color.Gray, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

            int columnSize = 14;
            int inventory = ShipInventoryManager.inventorySize;

            if (inventory > columnSize)
            {
                for (int n = 0; n < columnSize; n++)
                {
                    if (IsEquipped(ShipInventoryManager.ShipItems[n]))
                        spriteBatch.DrawString(Game.fontManager.GetFont(16), ShipInventoryManager.ShipItems[n].Name, new Vector2(420, 40 + n * 23) + Game.fontManager.FontOffset, Color.Blue);
                    else
                        spriteBatch.DrawString(Game.fontManager.GetFont(16), ShipInventoryManager.ShipItems[n].Name, new Vector2(420, 40 + n * 23) + Game.fontManager.FontOffset, txtColor);
                }

                for (int n = 0; n < inventory - columnSize; n++)
                {
                    if (IsEquipped(ShipInventoryManager.ShipItems[n + columnSize]))
                        spriteBatch.DrawString(Game.fontManager.GetFont(16), ShipInventoryManager.ShipItems[n + columnSize].Name, new Vector2(600, 40 + n * 23) + Game.fontManager.FontOffset, Color.Blue);
                    else
                        spriteBatch.DrawString(Game.fontManager.GetFont(16), ShipInventoryManager.ShipItems[n + columnSize].Name, new Vector2(600, 40 + n * 23) + Game.fontManager.FontOffset, txtColor);
                }
            }
            else
            {
                for (int n = 0; n < inventory; n++)
                {
                    if (IsEquipped(ShipInventoryManager.ShipItems[n]))
                        spriteBatch.DrawString(Game.fontManager.GetFont(16), ShipInventoryManager.ShipItems[n].Name, new Vector2(420, 40 + n * 23) + Game.fontManager.FontOffset, Color.Blue);
                    else
                        spriteBatch.DrawString(Game.fontManager.GetFont(16), ShipInventoryManager.ShipItems[n].Name, new Vector2(420, 40 + n * 23) + Game.fontManager.FontOffset, txtColor);
                }
            }

            //Old non-working code. Saved to be removed later.
            /*if (section == 0)
            {
                if (ShipInventoryManager.ShipItems.Count > var2)
                {
                    if (layer == 3 && ShipInventoryManager.ShipItems[var2] != ShipInventoryManager.equippedEnergyCell && ShipInventoryManager.ShipItems[var2] != ShipInventoryManager.equippedShip
                        && ShipInventoryManager.ShipItems[var2].Kind != "Empty")
                        spriteBatch.DrawString(fontRegular, "Trash", new Vector2(430, 400), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(fontRegular, "Trash", new Vector2(430, 400), Color.Gray, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                }
            }

            for (int n = 0; n < ShipInventoryManager.inventorySize / 2; n++)
            {
                if (IsEquipped(ShipInventoryManager.ShipItems[n]))
                    spriteBatch.DrawString(fontRegular, ShipInventoryManager.ShipItems[n].Name, new Vector2(430, 40 + n * 23), Color.Blue);
                else
                    spriteBatch.DrawString(fontRegular, ShipInventoryManager.ShipItems[n].Name, new Vector2(430, 40 + n * 23), Color.Black);
            }

            for (int n = ShipInventoryManager.inventorySize / 2; n < ShipInventoryManager.inventorySize; n++)
            {
                if (IsEquipped(ShipInventoryManager.ShipItems[n]))
                    spriteBatch.DrawString(fontRegular, ShipInventoryManager.ShipItems[n].Name, new Vector2(630, 40 + (n - 14) * 23), Color.Blue);
                else
                    spriteBatch.DrawString(fontRegular, ShipInventoryManager.ShipItems[n].Name, new Vector2(630, 40 + (n - 14) * 23), Color.Black);
            }*/
        }
        public void DisplayShipInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game.fontManager.GetFont(16), "Hangar", new Vector2(600, 30) + Game.fontManager.FontOffset, txtColor, 0, Game.fontManager.GetFont(16).MeasureString("Ships") / 2, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(Game.fontManager.GetFont(16), "Current", new Vector2(430, 70) + Game.fontManager.FontOffset, txtColor);

            for (int n = 0; n < 1; n++)
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(16), BaseInventoryManager.equippedShip.Name, new Vector2(450, 93 + n * 23) + Game.fontManager.FontOffset, txtColor);
            }

            spriteBatch.DrawString(Game.fontManager.GetFont(16), "Owned ships", new Vector2(430, 150) + Game.fontManager.FontOffset, txtColor);

            for (int n = 0; n < BaseInventoryManager.ownedShips.Count; n++)
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(16), BaseInventoryManager.ownedShips[n].Name, new Vector2(450, 173 + n * 23) + Game.fontManager.FontOffset, txtColor);
            }
        }
        private bool IsEquipped(Item item)
        {
            if (item == ShipInventoryManager.equippedEnergyCell || item == ShipInventoryManager.equippedSecondary ||
                item == ShipInventoryManager.equippedShield || item == BaseInventoryManager.equippedShip)
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