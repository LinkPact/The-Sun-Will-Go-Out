using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class BorderStation : Station
    {
        private const float itemSpreadFactor = 0.1f;

        public BorderStation(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 523, 93, 93));
            base.Initialize();
            StationCodeName = "BO_Border_Station";
            LoadStationData(StationCodeName);
            ShopSetup();
        }

        private void ShopSetup()
        {
            //shopFilling = ShopFilling.sparse;
            shopFilling = ShopFilling.filled;

            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.BasicLaser, ShipPartAvailability.common, ItemVariety.random));
            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.DualLaser, ShipPartAvailability.common, ItemVariety.random));
            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.PunyTurret, ShipPartAvailability.common, ItemVariety.random));

            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.LightPlating, ShipPartAvailability.common, ItemVariety.random));
            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.HeavyPlating, ShipPartAvailability.common, ItemVariety.random));

            InventoryItemSetup(shopFilling);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
