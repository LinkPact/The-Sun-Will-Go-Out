using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class Ending
    {
        protected Game1 game;
        protected Sprite spriteSheet;

        protected bool activated;
        protected bool finished;
        public bool Finished { get { return finished; } private set { ;} }

        public Ending(Game1 game, Sprite spriteSheet)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;
        }

        public virtual void Initialize()
        {
            
        }

        public void Activate()
        {
            activated = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
