using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    public class ExplosionParticle
    {
        private Game1 game;
        private static Random random = new Random();
        //private Sprite spriteSheet;

        private Sprite sprite;

        private Color color;
        private int r = 255;
        private int g = 255;
        private int b = 255;

        private Vector2 position;
        private float speed;
        private float startingSpeed;
        private Vector2 direction;

        private Vector2 startingDirection;

        private Vector2 centerPoint;
        private float scale;

        private int lifeTime;
        public int LifeTime { get { return lifeTime; } private set { ;} }

        private float leftRemovalPos;
        private float rightRemovalPos;

        public ExplosionParticle(Game1 game, Sprite spriteSheet, Vector2 startingPos, Vector2 startingDir,
                                 float startingSpeed, float size, bool randomDir, int index)
        {
            speed = 0.024f + (float)(random.NextDouble() * 0.02f);
            lifeTime = (50 + random.Next(75));

            CommonSetup(game, spriteSheet, startingPos, startingDir, startingSpeed, size, randomDir, index);
        }

        public ExplosionParticle(Game1 game, Sprite spriteSheet, Vector2 startingPos, Vector2 startingDir,
                                 float startingSpeed, float size, bool randomDir, int index, 
                                 float fragmentSpeed, int fragmentLifeTime)
        {
            speed = fragmentSpeed;
            lifeTime = fragmentLifeTime;
        
            CommonSetup(game, spriteSheet, startingPos, startingDir, startingSpeed, size, randomDir, index);
        }

        private void CommonSetup(Game1 game, Sprite spriteSheet, Vector2 startingPos, Vector2 startingDir,
                                 float startingSpeed, float size, bool randomDir, int index)
        {
            this.game = game;
            sprite = spriteSheet.GetSubSprite(new Rectangle(532, 0, 10, 11));
            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2,
                                      sprite.SourceRectangle.Value.Height / 2);

            scale = size / 30;


            if (!randomDir)
                direction = new Vector2(-1f + (0.25f * index), -1f + (0.25f * index));
            else
                direction = new Vector2(-1f + (random.Next(16) * 0.125f),
                                        -1f + (random.Next(16) * 0.125f));

            position = startingPos;
            startingDirection = startingDir;
            this.startingSpeed = startingSpeed;

            leftRemovalPos = (game.Window.ClientBounds.Width - game.stateManager.shooterState.CurrentLevel.LevelWidth) / 2 - 50;
            rightRemovalPos = (game.Window.ClientBounds.Width - game.stateManager.shooterState.CurrentLevel.LevelWidth) / 2 + game.stateManager.shooterState.CurrentLevel.LevelWidth + 50;

        }

        public void Update(GameTime gameTime)
        {
            Vector2 s1 = startingDirection * startingSpeed;
            Vector2 s2 = direction * speed;

            position += (s1 + s2) * gameTime.ElapsedGameTime.Milliseconds;

            lifeTime--;

            if (g > 69)
                g -= 3;
            if (b > 5)
                b -= 5;

            if (position.X < leftRemovalPos || position.X > rightRemovalPos)
            {
                lifeTime = 0;
            }

            else if (position.Y < (game.Window.ClientBounds.Height - 600) / 2 ||
                position.Y > (game.Window.ClientBounds.Height - 600) / 2 + 600)
            {
                lifeTime = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            color = new Color(r, g, b);

            if (lifeTime > 0)
                spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle,
                    color, 0f, centerPoint, scale, SpriteEffects.None, 0.65f); 
        }

        private void setFixedDirection(int i)
        {

        }
    }
}
