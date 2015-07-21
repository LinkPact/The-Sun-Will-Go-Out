using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceProject_Linux
{
    class BurnOutExplosionParticle
    {
        // Constants
        private static readonly float INIT_EXPLOSION_SPEED = 0.15f;
        private static readonly float SPEED_DECREASE = 0.00012f;
        private static readonly float MIN_SPEED = 0.1f;

        private static readonly float INIT_SCALE = 1f;
        private static readonly float MIN_SCALE = 0.01f;
        private static readonly float SCALE_DECREASE = 0.0001f;

        private static readonly Color INIT_COLOR = new Color(100, 168, 255);
        private static readonly byte COLOR_DECREASE = 2;

        private static readonly int BASE_LIFE_TIME = 6000;
        private static readonly int RANDOM_LIFE_TIME = 3000;
        // ----

        // Variables
        private Random random;

        private Sprite sprite;

        private float speed;
        private Direction direction;
        private Vector2 position;
        private float scale;
        private Color color;

        public int lifeTime;
        private int colorIndex;
        // ----

        public void Initialize(Vector2 position, Sprite spriteSheet)
        {
            this.position = position;
            random = new Random();
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 47, 159, 152));

            scale = INIT_SCALE;
            speed = INIT_EXPLOSION_SPEED;
            direction = new Direction(MathFunctions.GetRandomDirection());
            color = INIT_COLOR;
            lifeTime = BASE_LIFE_TIME + random.Next(RANDOM_LIFE_TIME);
        }

        public void Update(GameTime gameTime)
        {
            lifeTime -= gameTime.ElapsedGameTime.Milliseconds;
            position += (speed * direction.GetDirectionAsVector()) * gameTime.ElapsedGameTime.Milliseconds;

            if (scale > MIN_SCALE)
                scale -= SCALE_DECREASE * gameTime.ElapsedGameTime.Milliseconds;

            if (speed > MIN_SPEED)
            {
                speed -= SPEED_DECREASE * gameTime.ElapsedGameTime.Milliseconds;
            }

            ColorTransition();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle, color,
                0f, sprite.CenterPoint, scale, SpriteEffects.None, 1f);
        }

        private void ColorTransition()
        {
            if (colorIndex == 0)
            {
                if (color.R < 135 + COLOR_DECREASE * 3)
                    color.R += (byte)(COLOR_DECREASE * 3);
                if (color.G < 206 + COLOR_DECREASE * 3)
                    color.G += (byte)(COLOR_DECREASE * 3);
                if (color.B < 235 + COLOR_DECREASE * 3)
                    color.B += (byte)(COLOR_DECREASE * 3);

                if (color.R >= 135 + COLOR_DECREASE * 3
                    && color.G >= 206 + COLOR_DECREASE * 3
                    && color.B >= 235 + COLOR_DECREASE * 3)
                {
                    colorIndex++;
                }
            }

            else if (colorIndex == 1)
            {
                if (color.R < 255 - COLOR_DECREASE * 2)
                    color.R += (byte)(COLOR_DECREASE * 2);
                if (color.G < 255 - COLOR_DECREASE * 2)
                    color.G += (byte)(COLOR_DECREASE * 2);
                if (color.B < 255 - COLOR_DECREASE * 2)
                    color.B += (byte)(COLOR_DECREASE * 2);

                if (color.R >= 255 - COLOR_DECREASE * 2
                    && color.G >= 255 - COLOR_DECREASE * 2
                    && color.B >= 255 - COLOR_DECREASE * 2)
                {
                    colorIndex++;
                }
            }

            else if (colorIndex == 2)
            {
                if (color.B > COLOR_DECREASE)
                    color.B -= COLOR_DECREASE;

                if (color.B <= COLOR_DECREASE)
                {
                    colorIndex++;
                }
            }

            else if (colorIndex == 3)
            {
                if (color.R < 255 - COLOR_DECREASE)
                    color.R += COLOR_DECREASE;
                if (color.G > 69 + COLOR_DECREASE)
                    color.G -= COLOR_DECREASE;
                if (color.B > COLOR_DECREASE)
                    color.B -= COLOR_DECREASE;

                if (color.R >= 255 - COLOR_DECREASE
                    && color.G <= 69 + COLOR_DECREASE
                    && color.B <= COLOR_DECREASE)
                {
                    colorIndex++;
                }
            }

            else if (colorIndex == 4)
            {
                if (color.R > COLOR_DECREASE)
                    color.R -= COLOR_DECREASE;
                if (color.G > COLOR_DECREASE)
                    color.G -= COLOR_DECREASE;
            }
        }
    }
}