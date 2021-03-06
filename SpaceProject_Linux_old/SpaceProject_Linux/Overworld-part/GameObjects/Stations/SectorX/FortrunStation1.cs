﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class FortrunStation1 : Station
    {
        public FortrunStation1(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(779, 1075, 291, 292));
            base.Initialize();
            StationCodeName = "SX_Fortrun_1";
            LoadStationData(StationCodeName);
            
            ShopSetup();
        }

        private void ShopSetup()
        {
            hasShop = true;

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.RegularEnergyCell, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.RegularShield, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.RegularPlating, ShipPartAvailability.common, ItemVariety.regular));

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.MultipleShot, ShipPartAvailability.common, ItemVariety.regular));
            //AddMandatoryItem(new ShopInventoryEntry(ShipPartType.Beam, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.BallisticLaser, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.WaveBeam, ShipPartAvailability.common, ItemVariety.regular));

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.Turret, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.HomingMissile, ShipPartAvailability.common, ItemVariety.regular));

            //AddShopEntry(new ShopInventoryEntry(ShipPartType.RegularEnergyCell, ShipPartAvailability.common, ItemVariety.random));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.RegularShield, ShipPartAvailability.common, ItemVariety.random));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.RegularPlating, ShipPartAvailability.common, ItemVariety.random));

            SetShopFilling(ShopFilling.veryFilled);
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
