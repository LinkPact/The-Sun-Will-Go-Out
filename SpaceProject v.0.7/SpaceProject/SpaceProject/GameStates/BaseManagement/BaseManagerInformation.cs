using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class BaseManagerInformation
    {
        #region init
        private Game1 Game;

        //private SpriteFont fontRegular;

        //Denotes current active position of cursor.
        int layer;
        int var1;
        int var2;
        int var3;
        #endregion
        public BaseManagerInformation(Game1 Game)
        {
            this.Game = Game;
        }
        public void Initialize()
        {
            //fontRegular = Game.Font14;
            //fontRegular = Game.Content.Load<SpriteFont>("Vertical-Sprites/shipInfoFont");
        }
        public void Update(GameTime gameTime, int layer, int var1, int var2, int var3)
        {
            this.layer = layer;
            this.var1 = var1;
            this.var2 = var2;
            this.var3 = var3;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (var1 == 0 && layer == 3 && var3 != -1)
                DisplayInventoryInfo(spriteBatch);

            if (var1 == 1 && layer == 3)
                DisplayShipInfo(spriteBatch);

            //if (var1 == 2 && layer == 3)
            //    DisplayShipInfo(spriteBatch);
            //
            //if (var1 == 3 && layer == 3)
            //    DisplayEnergyCellInfo(spriteBatch);
            //
            //if (var1 == 4 && layer == 3)
            //    DisplayShieldInfo(spriteBatch);
            //
            //if (var1 == 5 && layer >= 2 && var3 != -1)
            //    DisplayInventoryInfo(spriteBatch);
        }
        private void DisplayInventoryInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2) 
            { 
                ShipInventoryManager.ShipItems[var2].DisplayInventoryInfo(
                spriteBatch, this.Game.fontManager.GetFont(16), this.Game.fontManager.FontColor); 
            }
            if (layer == 3) 
            { 
                ShipInventoryManager.ShipItems[var3].DisplayInventoryInfo(
                spriteBatch, this.Game.fontManager.GetFont(16), this.Game.fontManager.FontColor); 
            }
        }
        //public void DisplayPrimaryWeaponInfo(SpriteBatch spriteBatch)
        //{
        //    if (ShipInventoryManager.ownedPrimaryWeapons.Count > 0)
        //    {
        //        ShipInventoryManager.ownedPrimaryWeapons[var3].DisplayInventoryInfo(
        //            spriteBatch, this.Game.fontManager.GetFont(16), this.Game.fontManager.FontColor);
        //    }
        //}
        //public void DisplaySecondaryWeaponInfo(SpriteBatch spriteBatch)
        //{
        //    if (ShipInventoryManager.ownedSecondary.Count > 0)
        //    {
        //        ShipInventoryManager.ownedSecondary[var3].DisplayInventoryInfo(
        //            spriteBatch, this.Game.fontManager.GetFont(16), this.Game.fontManager.FontColor);
        //    }
        //}
        public void DisplayShipInfo(SpriteBatch spriteBatch)
        {
            if (BaseInventoryManager.ownedShips.Count > 0)
            {
                BaseInventoryManager.ownedShips[var3].DisplayInventoryInfo(
                    spriteBatch, this.Game.fontManager.GetFont(16), this.Game.fontManager.FontColor);
            }
        }
        public void DisplayEnergyCellInfo(SpriteBatch spriteBatch)
        {
            if (ShipInventoryManager.ownedEnergyCells.Count > 0)
            {
                ShipInventoryManager.ownedEnergyCells[var3].DisplayInventoryInfo(
                    spriteBatch, this.Game.fontManager.GetFont(16), this.Game.fontManager.FontColor);
            }
        }
        public void DisplayShieldInfo(SpriteBatch spriteBatch)
        {
            if (ShipInventoryManager.ownedShields.Count > 0)
            {
                ShipInventoryManager.ownedShields[var3].DisplayInventoryInfo(
                    spriteBatch, this.Game.fontManager.GetFont(16), this.Game.fontManager.FontColor);
            }
        }
    }
}
