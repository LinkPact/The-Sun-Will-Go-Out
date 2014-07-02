using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Lavis : Planet
    {
        public Lavis(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {           
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(506, 384, 203, 203));

            hasColony = true;

            base.Initialize();

            // Old name: SX_BluePlanet
            PlanetCodeName = "SX_Lavis";
            ColonyCodeName = "SX_Lavis_Colony1";

            LoadPlanetData(PlanetCodeName);

            resourceTypes.Add("Titanium");
            resourceCount.Add(500);
            ShopSetup();
        }

        private void ShopSetup()
        {
            AddShopEntry(new ShopInventoryEntry(ShipPartType.PlasmaEnergyCell, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.RegularBomb, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.WaveBeam, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.MultipleShot, ShipPartAvailability.common, ItemVariety.random));

            SetShopFilling(ShopFilling.regular);
        }

        public override void Update(GameTime gameTime)
        {            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsUsed == true)
            {          
                base.Draw(spriteBatch);
            }
        }
    }
}
