﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class PeyeShop : Station
    {
        public PeyeShop(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 1390, 62, 62));
            base.Initialize();
            StationCodeName = "OW_Peye_Shop";
            LoadStationData(StationCodeName);

            ShopSetup();
        }

        private void ShopSetup()
        {
            hasShop = true;

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.AdvancedLaser, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.ProximityLaser, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.AdvancedBeam, ShipPartAvailability.common, ItemVariety.regular));

            //AddMandatoryItem(new ShopInventoryEntry(ShipPartType.MineLayer, ShipPartAvailability.common, ItemVariety.regular));

            //AddShopEntry(new ShopInventoryEntry(ShipPartType.BulletShield, ShipPartAvailability.common, ItemVariety.high));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.CollisionShield, ShipPartAvailability.common, ItemVariety.high));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.WeaponBoostEnergyCell, ShipPartAvailability.common, ItemVariety.high));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.ShieldBoostEnergyCell, ShipPartAvailability.common, ItemVariety.high));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.LightPlating, ShipPartAvailability.common, ItemVariety.high));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.HeavyPlating, ShipPartAvailability.common, ItemVariety.high));

            SetShopFilling(ShopFilling.regular);
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