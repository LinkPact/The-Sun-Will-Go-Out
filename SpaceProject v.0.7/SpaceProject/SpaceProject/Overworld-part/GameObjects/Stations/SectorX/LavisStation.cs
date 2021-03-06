﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class LavisStation : Station
    {
        public LavisStation(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(342, 871, 93, 93));
            base.Initialize();
            StationCodeName = "SX_Station1_Lavis";
            LoadStationData(StationCodeName);
            ShopSetup();
        }

        private void ShopSetup()
        {
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.BasicLaser, ShipPartAvailability.ubiquitous, ItemVariety.Regular));
            //
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.RegularShield, ShipPartAvailability.common, ItemVariety.Regular));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.RegularEnergyCell, ShipPartAvailability.common, ItemVariety.Regular));
            //
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.DualLaser, ShipPartAvailability.uncommon, ItemVariety.Regular));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.PunyTurret, ShipPartAvailability.uncommon, ItemVariety.Regular));
            //
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.MultipleShot, ShipPartAvailability.rare, ItemVariety.Regular));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.FragmentMissile, ShipPartAvailability.rare, ItemVariety.Regular));
            //
            //SetShopFilling(ShopFilling.regular);
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
