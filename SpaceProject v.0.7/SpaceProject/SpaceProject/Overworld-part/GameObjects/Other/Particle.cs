using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Particle : GameObjectOverworld
    {
        private Random rand = new Random();

        public float lifeSpawn;
        private float opacity;

        public Particle(Game1 Game, Sprite spriteSheet):
            base(Game, spriteSheet)
        {

        }

        public void Initialize(GameObjectOverworld obj)
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 38, 18,18));
            layerDepth = obj.layerDepth - 0.01f;
            
            position.X = obj.position.X - 6 + (float)(rand.Next(12));
            position.Y = obj.position.Y - 6 + (float)(rand.Next(12));
            scale = 1;
            lifeSpawn = 6 + (int)rand.Next(6);
            opacity = 1;

            speed = (obj.speed * -1) * 0.05f;
            maxSpeed = 4;
            color = new Color(255, 81, 0, 255);
            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2,
            sprite.SourceRectangle.Value.Height / 2);           

            Initialize();
        }


        public void Update(GameTime gameTime, GameObjectOverworld obj)
        {
            Direction = obj.Direction;

            //Note to self: This code needs to be fixed to compensate for diagonal movement

            float particleMoveDistance = 0.5f * MathFunctions.FPSSyncFactor(gameTime);
            if (position.X < obj.position.X)
            {
                position.X += particleMoveDistance;
            }

            else if (position.X > obj.position.X)
            {
                position.X -= particleMoveDistance;
            }

            if (position.Y < obj.position.Y)
            {
                position.Y += particleMoveDistance;
            }

            else if (position.Y > obj.position.Y)
            {
                position.Y -= particleMoveDistance;
            }

            scale -= 0.025f * MathFunctions.FPSSyncFactor(gameTime);

            color = GetFadingFireColor(gameTime, color) * opacity;

            if (lifeSpawn > 0)
            {
                lifeSpawn -= 1.0f * MathFunctions.FPSSyncFactor(gameTime);
            }

            base.Update(gameTime);
        }

        public static Color GetFadingFireColor(GameTime gameTime, Color initialColor)
        {
            Color resultingColor = initialColor;

            int redFadeRate = 15;
            int greenFadeRate = 9;
            int blueFadeRate = 15;

            resultingColor.R = UpdateSingleColor(gameTime, initialColor.R, redFadeRate);
            resultingColor.G = UpdateSingleColor(gameTime, initialColor.G, greenFadeRate);
            resultingColor.B = UpdateSingleColor(gameTime, initialColor.B, blueFadeRate);
        
            return resultingColor;
        }

        private static byte UpdateSingleColor(GameTime gameTime, byte initialColor, int fadeOutRate)
        {
            byte colorFadeDelta = (byte)(fadeOutRate * MathFunctions.FPSSyncFactor(gameTime));

            if ((int)(initialColor - colorFadeDelta) > 0)
            {
                return (byte)(initialColor - colorFadeDelta);
            }
            else
            {
                return 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void SetOpacity(float opacity)
        {
            this.opacity = opacity;
        }
    }
}
