using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BaseState : GameState
    {
        protected Sprite spriteSheet;

        protected Planet planet;
        protected Station station;

        protected Vector2 nameStringPosition;
        protected Vector2 nameStringOrigin;

        public Sprite SpriteSheet { get { return spriteSheet; } }

        public GameObjectOverworld GetBase()
        {
            if (this is PlanetState)
            {
                return planet;
            }

            else
            {
                return station;
            }
        }

        protected BaseState(Game1 Game, string Name) :
            base (Game, Name) 
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            nameStringPosition = new Vector2(Game.Window.ClientBounds.Width / 2, 30);         
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet.Texture,
                             new Vector2(0, 0),
                             new Rectangle(0, 486, 1280, 720),
                             Color.White,
                             0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width / 1280,
                                 Game.Window.ClientBounds.Height / 720),
                             SpriteEffects.None,
                             0.0f);

            base.Draw(spriteBatch);
        }
    }
}
