using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class GreyMoon : Planet
    {
        private float centerX;
        private float centerY;
        private float radius;
        private float orbitSpeed;
        private float speedScale;
        private float angleMoon;

        public GreyMoon(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(308, 527, 163, 163));

            base.Initialize();

            PlanetCodeName = "SX_GreyMoon";
            ColonyCodeName = "SX_GreyMoon_Colony1";

            LoadPlanetData(PlanetCodeName);

            centerX = 243063;
            centerY = 252133;
            radius = 600f;
            orbitSpeed = 600f;
            speedScale = (0.001f * 2 * (float)Math.PI) / orbitSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            angleMoon = (float)gameTime.TotalGameTime.TotalMilliseconds * speedScale;
            
            angle += (float)Math.PI * 0.01f / 180;

            double xCoord = centerX + Math.Sin(angle) * radius;
            double yCoord = centerY + Math.Cos(angle) * radius;
            
            position = new Vector2((float)xCoord, (float)yCoord);

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