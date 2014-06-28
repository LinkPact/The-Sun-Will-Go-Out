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
        private static int layer3pos;
        private static int section;

        //private String state;
        #endregion
        public InventoryInformation(Game1 Game)
        {
            this.Game = Game;
        }
        
        public void Initialize()
        { }

        public void Update(GameTime gameTime, int layer_, int var1_, 
            int var2_, int var3_, String state_, int section_)
        {
            layer = layer_;
            layer1pos = var1_;
            layer2pos = var2_;
            layer3pos = var3_;
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
                    ShipInventoryManager.ShipItems[layer2pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
            if (layer == 3) 
            {
                if (layer3pos > ShipInventoryManager.ShipItems.Count - 1) layer3pos = 
                    ShipInventoryManager.ShipItems.Count - 1;
                if(layer3pos != -1)
                    ShipInventoryManager.ShipItems[layer3pos].DisplayInventoryInfo(spriteBatch, 
                        FontManager.GetFontStatic(14), FontManager.FontColorStatic); 
            }
        }

        public static void DisplayPrimaryWeaponInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.equippedPrimaryWeapons.Count > 0)
                {
                    if (ShipInventoryManager.equippedPrimaryWeapons[layer2pos].Kind != "Empty")
                        ShipInventoryManager.equippedPrimaryWeapons[layer2pos].DisplayInventoryInfo(
                            spriteBatch, FontManager.GetFontStatic(14),
                            FontManager.FontColorStatic);
                }
            }
            else if (layer == 3)
            {
                if (ShipInventoryManager.OwnedPrimaryWeapons.Count > 0)
                {
                    ShipInventoryManager.OwnedPrimaryWeapons[layer3pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
                }
            }
        }

        public static void DisplaySecondaryWeaponInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.equippedSecondary.Kind != "Empty")
                    ShipInventoryManager.equippedSecondary.DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
            else if (layer == 3)
            {
                if (ShipInventoryManager.OwnedSecondary.Count > 0)
                    ShipInventoryManager.OwnedSecondary[layer3pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
        }
        
        public static void DisplayPlatingInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.equippedPlating.Kind != "Empty")
                    ShipInventoryManager.equippedPlating.DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
            else if (layer == 3)
            {
                if (ShipInventoryManager.ownedPlatings.Count > 0)
                    ShipInventoryManager.ownedPlatings[layer3pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
        }
        
        public static void DisplayEnergyCellInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.equippedEnergyCell.Kind != "Empty")
                    ShipInventoryManager.equippedEnergyCell.DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
            else if (layer == 3)
            {
                if (ShipInventoryManager.ownedEnergyCells.Count > 0)
                    ShipInventoryManager.ownedEnergyCells[layer3pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
        }
        
        public static void DisplayShieldInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (ShipInventoryManager.equippedShield.Kind != "Empty")
                    ShipInventoryManager.equippedShield.DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
            else if (layer == 3)
            {
                if (ShipInventoryManager.ownedShields.Count > 0)
                    ShipInventoryManager.ownedShields[layer3pos].DisplayInventoryInfo(
                        spriteBatch, FontManager.GetFontStatic(14),
                        FontManager.FontColorStatic);
            }
        }
    }
}
