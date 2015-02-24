using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class DirectionArrow
    {
        private Vector2 targetCoordinate;
        private Sprite sprite;

        public float rotation;

        public DirectionArrow(Sprite spriteSheet, Vector2 target)
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(182, 29, 27, 32));
            this.targetCoordinate = new Vector2(target.X, target.Y);


        }

        public void Update(Vector2 playerPosition)
        {
            UpdateRotation(playerPosition);
            //System.Diagnostics.Debug.WriteLine(rotation);
        }

        private void UpdateRotation(Vector2 playerPosition)
        {
            Vector2 targetDir = new Vector2(targetCoordinate.X - playerPosition.X, targetCoordinate.Y - playerPosition.Y);
            Vector2 targetDirScaled = MathFunctions.ScaleDirection(targetDir);
            double radiansDir = MathFunctions.RadiansFromDir(targetDirScaled);
            rotation = (float)(radiansDir - Math.PI / 2);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 origin, Vector2 playerpos)
        {
            float arrowOffset = -68;
            float drawLayer = 0.912f;

            spriteBatch.Draw(sprite.Texture, origin, sprite.SourceRectangle, Color.White, rotation,
                new Vector2(sprite.SourceRectangle.Value.Width / 2, arrowOffset),
                1f, SpriteEffects.None, drawLayer);
        }
    }
}
