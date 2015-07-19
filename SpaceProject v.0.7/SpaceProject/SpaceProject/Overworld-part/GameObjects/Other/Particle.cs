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
        private readonly float ScaleStretch = 3f;

        protected Random rand = new Random();

        public float lifeSpawn;
        protected float opacity;

        protected float yScale;
        protected float parAngle;

        public Particle(Game1 Game, Sprite spriteSheet):
            base(Game, spriteSheet)
        {

        }

        public virtual void Initialize(GameObjectOverworld obj, Vector2 startingPoint)
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 38, 18,18));
            layerDepth = obj.layerDepth - 0.01f;

            position.X = startingPoint.X - 6 + rand.Next(12) + ((obj.Bounds.Width / 2f - obj.sprite.Width / 7.5f) * -obj.Direction.GetDirectionAsVector().X);
            position.Y = startingPoint.Y - 6 + rand.Next(12) + ((obj.Bounds.Height / 2f - obj.sprite.Height / 7.5f) * -obj.Direction.GetDirectionAsVector().Y);
            lifeSpawn = 8 + (int)rand.Next(6);
            opacity = 1;

            scale = 1;
            speed = obj.speed * 0.6f;
            maxSpeed = 4;
            color = new Color(255, 80, 80, 255);
            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2,
            sprite.SourceRectangle.Value.Height / 2);           

            Initialize();
        }


        public virtual void Update(GameTime gameTime, GameObjectOverworld obj)
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

            if (scale > 0)
            {
                scale -= 0.03f * MathFunctions.FPSSyncFactor(gameTime);
            }
            else
            {
                scale = 0;
            }

            color = GetFadingFireColor(gameTime, color) * opacity;

            if (lifeSpawn > 0)
            {
                lifeSpawn -= 1.0f * MathFunctions.FPSSyncFactor(gameTime);
            }

            yScale = scale + speed * ScaleStretch;
            parAngle = (float)((Math.PI * 90) / 180) + (float)(MathFunctions.RadiansFromDir(Direction.GetDirectionAsVector()));

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle, color,
                parAngle, centerPoint, new Vector2(scale, yScale), SpriteEffects.None, layerDepth);
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

        protected static byte UpdateSingleColor(GameTime gameTime, byte initialColor, int fadeOutRate)
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

        public void SetOpacity(float opacity)
        {
            this.opacity = opacity;
        }
    }
}
