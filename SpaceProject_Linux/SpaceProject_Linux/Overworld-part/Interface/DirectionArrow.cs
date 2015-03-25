using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    class DirectionArrow
    {
        private Vector2 targetCoordinate;
        private Sprite sprite;

        public float rotation;
        private Boolean isMainMission;

        public DirectionArrow(Sprite spriteSheet, Vector2 target, Vector2 playerpos, Boolean isMainMission)
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(45, 35, 15, 8));
            this.targetCoordinate = new Vector2(target.X, target.Y);

            SetRotation(playerpos);

            this.isMainMission = isMainMission;
        }

        private void SetRotation(Vector2 playerPosition)
        {
            Vector2 targetDir = new Vector2(targetCoordinate.X - playerPosition.X, targetCoordinate.Y - playerPosition.Y);
            Vector2 targetDirScaled = MathFunctions.ScaleDirection(targetDir);
            double radiansDir = MathFunctions.RadiansFromDir(targetDirScaled);
            rotation = (float)(radiansDir - Math.PI / 2);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawCenter)
        {
            float arrowOffset = -92;
            float drawLayer = 0.912f;

            Color tint = GetColor(isMainMission);

            spriteBatch.Draw(sprite.Texture, drawCenter, sprite.SourceRectangle, tint, rotation,
                new Vector2(sprite.SourceRectangle.Value.Width / 2, arrowOffset),
                1f, SpriteEffects.None, drawLayer);
        }

        private Color GetColor(Boolean isMain)
        {
            if (!isMain)
            {
                return Color.Silver;
            }
            else
            {
                return Color.Gold;
            }
        }
    }
}
