using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Highfence : Planet
    {
        public Highfence(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(1, 835, 244, 243));
            base.Initialize();

            // Old name: SX_DustPlanet
            PlanetCodeName = "SX_Highfence";
            ColonyCodeName = "SX_Highfence_Colony1";

            LoadPlanetData(PlanetCodeName);
            ShopSetup();
        }

        private void ShopSetup()
        {
            shopFilling = ShopFilling.filled;

            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.DualLaser, ShipPartAvailability.ubiquitous, ItemVariety.random));
            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.SpreadBullet, ShipPartAvailability.common, ItemVariety.random));

            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.RegularEnergyCell, ShipPartAvailability.common, ItemVariety.random));
            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.RegularShield, ShipPartAvailability.common, ItemVariety.random));

            InventoryItemSetup(shopFilling);
        }
    }
}
