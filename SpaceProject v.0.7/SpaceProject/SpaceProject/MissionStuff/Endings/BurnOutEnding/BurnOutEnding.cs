using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class BurnOutEnding : Ending
    {
        // Constants
        private readonly float FILTER_SCALE_INCREASE = 0.0012f;
        private readonly float FILTER_MAX_SCALE = 10f;
        private readonly Color INIT_FILTER_COLOR = Color.White;
        private readonly float INIT_FILTER_SCALE = 0.9f;

        protected readonly int EXPLOSION_PARTICLES = 50;
        private readonly int EXPLOSION_WAVE_DELAY = 50;
        // ----

        // Variables
        private Vector2 screenCenter;

        private List<BurnOutExplosionParticle> particles;
        private List<BurnOutExplosionParticle> oldParticles;

        private Sprite filter;
        private Color filterColor;
        private float filterScale;

        private int explosionWaves;
        private int explosionWaveTimer;
        // ----

        // Properties
        // ----

        public BurnOutEnding(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            particles = new List<BurnOutExplosionParticle>();
            oldParticles = new List<BurnOutExplosionParticle>();

            filter = spriteSheet.GetSubSprite(new Rectangle(164, 43, 163, 163));
            filterColor = INIT_FILTER_COLOR;
            filterScale = INIT_FILTER_SCALE;
            activated = false;
        }

        public void Activate(Vector2 screenCenter, int waves)
        {
            base.Activate();

            this.screenCenter = screenCenter;
            activated = true;
            explosionWaves = waves;
            game.soundEffectsManager.PlaySoundEffect(SoundEffects.HugeExplosion);

            AddParticles();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateWaves(gameTime);
            UpdateParticles(gameTime);
            UpdateFilter(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawParticles(spriteBatch);
            DrawFilter(spriteBatch);
        }

        private void UpdateWaves(GameTime gameTime)
        {
            explosionWaveTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (explosionWaves > 0
                && explosionWaveTimer <= 0)
            {
                AddParticles();

                explosionWaves--;
                explosionWaveTimer = EXPLOSION_WAVE_DELAY;
            }
        }

        private void UpdateParticles(GameTime gameTime)
        {
            foreach (BurnOutExplosionParticle par in particles)
            {
                if (par.lifeTime > 0)
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
        }

        private void UpdateFilter(GameTime gameTime)
        {
            if (activated)
            {
                if (filterScale < FILTER_MAX_SCALE)
                {
                    filterScale += FILTER_SCALE_INCREASE * gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    FadeFilterToBlack();
                }
            }
        }

        private void AddParticles()
        {
            for (int i = 0; i < EXPLOSION_PARTICLES; i++)
            {
                BurnOutExplosionParticle par = new BurnOutExplosionParticle();
                par.Initialize(screenCenter, spriteSheet);
                particles.Add(par);
            }
        }

        private void FadeFilterToBlack()
        {
            if (filterColor.R > 0)
            {
                filterColor.R -= 1;
            }
            if (filterColor.G > 0)
            {
                filterColor.G -= 1;
            }
            if (filterColor.B > 0)
            {
                filterColor.B -= 1;
            }

            if (filterColor.R <= 0
                && filterColor.G <= 0
                && filterColor.B <= 0)
            {
                finished = true;
            }
        }

        private void DrawParticles(SpriteBatch spriteBatch)
        {
            foreach (BurnOutExplosionParticle par in particles)
            {
                par.Draw(spriteBatch);
            }
        }

        private void DrawFilter(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(filter.Texture, screenCenter, filter.SourceRectangle, filterColor,
                0f, filter.CenterPoint, filterScale, SpriteEffects.None, 0.999f);
        }
    }
}
