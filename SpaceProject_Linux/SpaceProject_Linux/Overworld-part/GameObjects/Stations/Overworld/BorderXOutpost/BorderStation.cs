﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Linux
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
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.BasicLaser, ShipPartAvailability.common, ItemVariety.random));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.DualLaser, ShipPartAvailability.common, ItemVariety.random));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.PunyTurret, ShipPartAvailability.common, ItemVariety.random));
            //
            //SetShopFilling(ShopFilling.sparse);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
