using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class ShieldEffect : BackgroundObject
    {
        private Sprite sprite;

        private Vector2 position;
        private float speed;
        private Vector2 direction;
        private int lifeTime;
        public int LifeTime { get { return lifeTime; } }

        private const float radius = 50;
        private float size;
        private float sizeFactor;

        public ShieldEffect(Game1 game, Sprite spriteSheet, Vector2 pos, Vector2 dir,
            float speed, float size)
            : base(game, spriteSheet)
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(220, 180, 100, 100));
            this.position = pos;
            this.direction = dir;
            this.speed = speed;
            this.size = size;
            sizeFactor = (size * 2f) / radius;

            lifeTime = 50;

            CenterPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2,
                                    sprite.SourceRectangle.Value.Height / 2);

            transparency = 0.15f;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 movement = direction * speed;
            position += (movement) * gameTime.ElapsedGameTime.Milliseconds;

            lifeTime -= gameTime.ElapsedGameTime.Milliseconds;

            if (lifeTime < 0)
                IsKilled = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (lifeTime > 0)
                spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle,
                    Color.White * transparency, 0f, CenterPoint, sizeFactor, SpriteEffects.None, 0.65f);
        }
    }
}
