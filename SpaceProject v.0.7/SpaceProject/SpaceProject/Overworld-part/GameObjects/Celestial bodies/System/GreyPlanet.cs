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
        //float centerX;
        //float centerY;
        //float radius;
        //float speed;
        //float speedScale;
        //float angle;
 
        public GreyPlanet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(1, 545, 700, 700));

            base.Initialize();

            PlanetCodeName = "S1_GreyPlanet";
            ColonyCodeName = "S1_GreyPlanet_Colony1";

            LoadPlanetData(PlanetCodeName);

            //ObjectName = "Grey Planet";
            //ObjectPosition = new Vector2(4080, 3110);
            

            //planetMass = 0.7f;
            //planetTemp = -136;
            //planetGravity = 0.4f;
            //planetOrbit = 5.7f;
            //planetRotation = 15;
            //planetSurface = "Rock";

            //centerX = 2400;
            //centerY = 1785;
            //radius = 1350f;
            //speed = 600f;
            //speedScale = (0.001f * 2 * (float)Math.PI) / speed;
            
            shopInventory.Add(new CopperResource(this.Game, 500));
            shopInventory.Add(new GoldResource(this.Game, 300));
            shopInventory.Add(new TitaniumResource(this.Game, 100));
            shopInventory.Add(new FineWhiskey(this.Game, 10));
        }

        public override void Update(GameTime gameTime)
        {
            //angle = (float)gameTime.TotalGameTime.TotalMilliseconds * speedScale;
            //
            //ObjectAngle += (float)Math.PI * 0.01f / 180;

            //System.Diagnostics.Trace.WriteLine(gameTime.TotalGameTime.TotalMilliseconds);

            //double xCoord = centerX + Math.Sin(angle) * radius;
            //double yCoord = centerY + Math.Cos(angle) * radius;
            //
            //ObjectPosition = new Vector2((float)xCoord, (float)yCoord);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);  
        }

        // Appends items to item pool
        public override void UpdateItemPool()
        {
            itemPool = null;
        }
    }
}
