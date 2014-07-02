using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class FotrunStation2 : Station
    {
        public FotrunStation2(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(342, 871, 93, 93));
            base.Initialize();
            StationCodeName = "SX_Fotrun_2";
            LoadStationData(StationCodeName);

            ShopSetup();
        }

        private void ShopSetup()
        {
            AddShopEntry(new ShopInventoryEntry(ShipPartType.PlasmaShield, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.FragmentMissile, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.MineLayer, ShipPartAvailability.common, ItemVariety.random));
            AddShopEntry(new ShopInventoryEntry(ShipPartType.RegularEnergyCell, ShipPartAvailability.common, ItemVariety.random));

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
