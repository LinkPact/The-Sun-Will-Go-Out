using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class RebelStation1 : Station
    {
        public RebelStation1(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 523, 93, 93));
            base.Initialize();
            StationCodeName = "RO_Rebel_Station_1";
            LoadStationData(StationCodeName);

            ShopSetup();
        }

        private void ShopSetup()
        {
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.AdvancedEnergyCell, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.AdvancedPlating, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.AdvancedShield, ShipPartAvailability.common, ItemVariety.regular));

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.BallisticLaser, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.Beam, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.FragmentMissile, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.Burster, ShipPartAvailability.common, ItemVariety.regular));

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.SideMissiles, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.Disruptor, ShipPartAvailability.common, ItemVariety.regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.HomingMissile, ShipPartAvailability.common, ItemVariety.regular));

            //AddShopEntry(new ShopInventoryEntry(ShipPartType.AdvancedEnergyCell, ShipPartAvailability.common, ItemVariety.random));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.AdvancedPlating, ShipPartAvailability.common, ItemVariety.random));
            //AddShopEntry(new ShopInventoryEntry(ShipPartType.AdvancedShield, ShipPartAvailability.common, ItemVariety.random));

            SetShopFilling(ShopFilling.veryFilled);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
