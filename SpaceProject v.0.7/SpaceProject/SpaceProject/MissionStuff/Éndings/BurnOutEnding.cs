using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BurnOutEnding
    {
        private Game1 game;
        private Sprite spriteSheet;
        private Sprite blackness;
        private float blacknessScale;
        private readonly float BLACKNESS_SCALE_INCREASE = 0.00065f;
        private readonly float BLACKNESS_MAX_SCALE = 10f;
        private Color blacknessColor;

        protected readonly int EXPLOSION_PARTICLES = 75;
        private int waves;
        private readonly int WAVE_DELAY = 50;
        private int waveTimer;

        private bool activated;
        private bool finished;
        public bool Finished { get { return finished; } private set { ;} }

        private List<BurnOutExplosionParticle> particles;
        private List<BurnOutExplosionParticle> oldParticles;
        private Vector2 screenCenter;

        public BurnOutEnding(Game1 game, Sprite spriteSheet)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;
        }

        public void Initialize()
        {
            particles = new List<BurnOutExplosionParticle>();
            oldParticles = new List<BurnOutExplosionParticle>();
            blacknessColor = Color.White;
            blackness = spriteSheet.GetSubSprite(new Rectangle(164, 43, 163, 163));
            blacknessScale = 1f;
        }

        public void Activate(Vector2 screenCenter, int waves)
        {
            activated = true;
            this.screenCenter = screenCenter;
            for (int i = 0; i < EXPLOSION_PARTICLES; i++)
            {
                BurnOutExplosionParticle par = new BurnOutExplosionParticle();
                par.Initialize(screenCenter, spriteSheet);
                particles.Add(par);
            }

            this.waves = waves;
        }

        public void Update(GameTime gameTime)
        {
            waveTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (waves > 0
                && waveTimer < 0)
            {
                for (int i = 0; i < EXPLOSION_PARTICLES; i++)
                {
                    BurnOutExplosionParticle par = new BurnOutExplosionParticle();
                    par.Initialize(screenCenter, spriteSheet);
                    particles.Add(par);
                }

                waves--;
                waveTimer = WAVE_DELAY;
            }

            foreach (BurnOutExplosionParticle par in particles)
            {
                if (par.age > 0)
                {
                    par.Update(gameTime);
                }
                else
                {
                    oldParticles.Add(par);
                }
            }

            foreach (BurnOutExplosionParticle par in oldParticles)
            {
                particles.Remove(par);
            }

            oldParticles.Clear();

            if (activated)
            {
                if (blacknessScale < BLACKNESS_MAX_SCALE)
                {
                    blacknessScale += BLACKNESS_SCALE_INCREASE * gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    if (blacknessColor.R > 0)
                    {
                        blacknessColor.R -= 1;
                    }
                    if (blacknessColor.G > 0)
                    {
                        blacknessColor.G -= 1;
                    }
                    if (blacknessColor.B > 0)
                    {
                        blacknessColor.B -= 1;
                    }

                    if (blacknessColor.R <= 0
                        && blacknessColor.G <= 0
                        && blacknessColor.B <= 0)
                    {
                        finished = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BurnOutExplosionParticle par in particles)
            {
                par.Draw(spriteBatch);
            }

            spriteBatch.Draw(blackness.Texture, screenCenter, blackness.SourceRectangle, blacknessColor,
                0f, blackness.CenterPoint, blacknessScale, SpriteEffects.None, 0.999f);
        }

        private class BurnOutExplosionParticle
        {
            private readonly float INIT_EXPLOSION_SPEED = 0.15f;
            private readonly float SPEED_DECREASE = 0.00012f;
            private readonly float MIN_SPEED = 0.1f;
            private readonly float INIT_SCALE = 1f;
            private readonly float MIN_SCALE = 0.01f;
            private readonly float SCALE_DECREASE = 0.0001f;
            private readonly Color INIT_COLOR = new Color(100, 168, 255);
            private readonly byte COLOR_DECREASE = 2;

            public int age;

            private Sprite sprite;
            private float speed;

            private Direction dir;
            private Vector2 position;
            private float scale;
            private Color color;
            private int colorIndex;

            public void Initialize(Vector2 pos, Sprite spriteSheet)
            {
                scale = INIT_SCALE;
                speed = INIT_EXPLOSION_SPEED;
                position = pos;
                sprite = spriteSheet.GetSubSprite(new Rectangle(0, 47, 159, 152));
                dir = new Direction(MathFunctions.GetRandomDirection());
                color = INIT_COLOR;
                age = 6000 + StaticFunctions.GetRandomValue(3000);
            }

            public void Update(GameTime gameTime)
            {
                age -= gameTime.ElapsedGameTime.Milliseconds;

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

                position += (speed * dir.GetDirectionAsVector()) * gameTime.ElapsedGameTime.Milliseconds;

                if (scale > MIN_SCALE)
                    scale -= SCALE_DECREASE * gameTime.ElapsedGameTime.Milliseconds;

                if (speed > MIN_SPEED)
                {
                    speed -= SPEED_DECREASE * gameTime.ElapsedGameTime.Milliseconds;
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle, color,
                    0f, sprite.CenterPoint, scale, SpriteEffects.None, 1f);
            }
        }
    }
}
