using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class GameObjectOverworld
    {
        protected Game1 Game { get; private set; }

        public bool IsUsed { get; set; }

        protected Sprite spriteSheet;

        public string Class;
        public string name;

        public Vector2 position;
        public float speed;
        private Direction direction;
        public Direction Direction { get { return direction; } set { direction = value; } } 
        public float maxSpeed;
        public bool IsDead;
        public Sprite sprite;
        public Vector2 centerPoint;
        public Color color;
        public float angle;
        public float scale;
        public SpriteEffects spriteEffect;
        public float layerDepth;               //"0" for back, "1" for front

        public Rectangle Bounds 
        {
            get
            {
                return new Rectangle((int)position.X - (int)centerPoint.X,
                                     (int)position.Y - (int)centerPoint.Y,
                                     sprite.Width, sprite.Height);
            }
        }

        public GameObjectOverworld(Game1 Game, Sprite spriteSheet)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
            direction = new Direction();
        }

        public virtual void Initialize()
        {
            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2,
                                sprite.SourceRectangle.Value.Height / 2);

            IsDead = false;
        }
       
        public virtual void Deinitialize() { }

        public virtual void FinalGoodbye() { }

        public virtual void Update(GameTime gameTime)
        {
            position += (speed * direction.GetDirectionAsVector()) * gameTime.ElapsedGameTime.Milliseconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle, color,
            angle, centerPoint, scale, spriteEffect, layerDepth);           
        }
    }
}
