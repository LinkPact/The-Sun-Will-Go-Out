using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class InventoryInformation
    {
        #region init
        private Game1 Game;

        //Denotes current active position of cursor.
        private static int layer;
        private static int layer1pos;
        private static int layer2pos;
        private static int section;

        //private String state;
        #endregion
        public InventoryInformation(Game1 Game)
        {
            this.Game = Game;
        }
        
        public void Initialize()
        { }

        public void Update(GameTime gameTime, int layer_, int var1_, int var2_, String state_, int section_)
        {
            layer = layer_;
            layer1pos = var1_;
            layer2pos = var2_;
            section = section_;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public static void DisplayInventoryInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2) 
            {
                if (layer2pos > ShipInventoryManager.ShipItems.Count - 1) layer2pos = 
                    ShipInventoryManager.ShipItems.Count - 1;
                if(layer2pos != -1)
                    ShipInventoryManager.ShipItems[layer2pos].DisplayInventoryInfo(spriteBatch, 
                        FontManager.GetFontStatic(14), FontManager.FontColorStatic); 
            }
        }

        public static void DisplayPrimaryWeaponInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.OwnedPrimaryWeapons.Count > 0)
                {
                    ShipInventoryManager.OwnedPrimaryWeapons[layer2pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
                }
            }
        }

        public static void DisplaySecondaryWeaponInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.OwnedSecondary.Count > 0)
                    ShipInventoryManager.OwnedSecondary[layer2pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
        }
        
        public static void DisplayPlatingInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.ownedPlatings.Count > 0)
                    ShipInventoryManager.ownedPlatings[layer2pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
        }
        
        public static void DisplayEnergyCellInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.ownedEnergyCells.Count > 0)
                    ShipInventoryManager.ownedEnergyCells[layer2pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
        }
        
        public static void DisplayShieldInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.ownedShields.Count > 0)
                    ShipInventoryManager.ownedShields[layer2pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
        }
    }
}
