using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class Container
    {
        protected Game1 game;

        protected Container(Game1 game)
        {
            this.game = game;
        }

        public virtual void Initialize()
        {

        }

        public virtual void Update(GameTime gameTime)
        { 
        
        }
    }
}
