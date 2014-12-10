using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Peye : Planet
    {
        public Peye(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {          
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(308, 692, 177, 177));

            base.Initialize();

            // SX_PoisonPlanet
            PlanetCodeName = "SX_Peye";
            ColonyCodeName = "SX_Peye_Colony1";

            LoadPlanetData(PlanetCodeName);
            ShopSetup();
        }

        private void ShopSetup()
        {
            AddShopEntry(new ShopInventoryEntry(ShipPartType.AdvancedLaser, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.ProximityLaser, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.AdvancedBeam, ShipPartAvailability.common, ItemVariety.random));

            AddShopEntry(new ShopInventoryEntry(ShipPartType.MineLayer, ShipPartAvailability.common, ItemVariety.high));

            AddShopEntry(new ShopInventoryEntry(ShipPartType.BulletShield, ShipPartAvailability.common, ItemVariety.high));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.CollisionShield, ShipPartAvailability.common, ItemVariety.high));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.WeaponBoostEnergyCell, ShipPartAvailability.common, ItemVariety.high));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.ShieldBoostEnergyCell, ShipPartAvailability.common, ItemVariety.high));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.LightPlating, ShipPartAvailability.common, ItemVariety.high));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.HeavyPlating, ShipPartAvailability.common, ItemVariety.high));

            SetShopFilling(ShopFilling.sparse);
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
