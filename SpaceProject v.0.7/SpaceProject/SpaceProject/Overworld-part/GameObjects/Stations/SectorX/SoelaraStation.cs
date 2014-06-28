﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class SoelaraStation : Station
    {
        public SoelaraStation(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(247, 871, 93, 93));
            base.Initialize();
            StationCodeName = "SX_Station1_Soelara";
            LoadStationData(StationCodeName);
            ShopSetup();
        }

        private void ShopSetup()
        {
            shopFilling = ShopFilling.regular;

            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.SpreadBullet, ShipPartAvailability.common, ItemVariety.random));
            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.DurableEnergyCell, ShipPartAvailability.common, ItemVariety.random));
            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.BallisticLaser, ShipPartAvailability.uncommon, ItemVariety.random));
            shopInventoryEntries.Add(new ShopInventoryEntry(ShipPartType.SideMissiles, ShipPartAvailability.common, ItemVariety.random));

            InventoryItemSetup(shopFilling);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }        
    }
}
