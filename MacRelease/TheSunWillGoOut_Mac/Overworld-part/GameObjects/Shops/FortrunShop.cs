using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class FortrunShop : ShopStation
    {
        public FortrunShop(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 1390, 59, 59));
            base.Initialize();
            StationCodeName = "OW_Fortrun_Shop";
            LoadStationData(StationCodeName);

            ShopSetup();
        }

        private void ShopSetup()
        {
            hasShop = true;

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.MultipleShot, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.BallisticLaser, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.WaveBeam, ShipPartAvailability.common, ItemVariety.Regular));

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.Turret, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.HomingMissile, ShipPartAvailability.common, ItemVariety.Regular));

            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.RegularEnergyCell, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.RegularShield, ShipPartAvailability.common, ItemVariety.Regular));
            AddMandatoryItem(new ShopInventoryEntry(ShipPartType.RegularPlating, ShipPartAvailability.common, ItemVariety.Regular));

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