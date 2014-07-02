using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class NewNorrland : Planet
    {
        public NewNorrland(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {          
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(869, 21, 312, 312));
            base.Initialize();

            // Old name: SX_BrownPlanet
            PlanetCodeName = "SX_New_Norrland";
            ColonyCodeName = "SX_New_Norrland_Colony1";

            LoadPlanetData(PlanetCodeName);
            ShopSetup();
        }

        private void ShopSetup()
        {
            AddShopEntry(new ShopInventoryEntry(ShipPartType.Turret, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.ProximityLaser, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.MineLayer, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.WaveBeam, ShipPartAvailability.common, ItemVariety.random));

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
