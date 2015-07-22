using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class RebelBaseShop : ShopStation
    {
        public RebelBaseShop(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(273, 523, 59, 59));
            base.Initialize();
            StationCodeName = "OW_Rebel_Base_Shop";
            LoadStationData(StationCodeName);
            ShopSetup();
        }

        private void ShopSetup()
        {
            hasShop = true;

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.FragmentMissile, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.Beam, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.Burster, ShipPartAvailability.common, ItemVariety.Regular));

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.SideMissiles, ShipPartAvailability.common, ItemVariety.Regular));
            //AddMandatoryItem(new ShopInventoryEntry(ShipPartType.FieldDamage, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.Disruptor, ShipPartAvailability.common, ItemVariety.Regular));

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.AdvancedEnergyCell, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.AdvancedPlating, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.AdvancedShield, ShipPartAvailability.common, ItemVariety.Regular));

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