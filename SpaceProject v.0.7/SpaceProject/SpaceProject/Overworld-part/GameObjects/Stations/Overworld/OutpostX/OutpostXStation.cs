using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class OutpostXStation : Station
    {
        public OutpostXStation(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 523, 93, 93));

            base.Initialize();

            StationCodeName = "OX_OutpostX_Station";

            LoadStationData(StationCodeName);
            ShopSetup();
        }

        private void ShopSetup()
        {
            AddShopEntry(new ShopInventoryEntry(ShipPartType.ShieldBoostEnergyCell, ShipPartAvailability.common, ItemVariety.regular));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.WeaponBoostEnergyCell, ShipPartAvailability.common, ItemVariety.regular));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.CollisionShield, ShipPartAvailability.common, ItemVariety.regular));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.BulletShield, ShipPartAvailability.common, ItemVariety.regular));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.Beam, ShipPartAvailability.common, ItemVariety.regular));

            SetShopFilling(ShopFilling.filled);
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
