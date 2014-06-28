using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class GreyPlanet: Planet
    {
        public GreyPlanet(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 22, 703, 703));

            base.Initialize();

            PlanetCodeName = "SY_GreyPlanet";
            ColonyCodeName = "SY_GreyPlanet_Colony1";

            LoadPlanetData(PlanetCodeName);
            
            shopInventory.Add(new CopperResource(this.Game, 500));
            shopInventory.Add(new GoldResource(this.Game, 300));
            shopInventory.Add(new TitaniumResource(this.Game, 100));
            shopInventory.Add(new FineWhiskey(this.Game, 10));
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
