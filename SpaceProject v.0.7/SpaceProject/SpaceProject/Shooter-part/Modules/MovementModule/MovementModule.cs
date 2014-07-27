using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public abstract class MovementModule
    {
        protected Random random;
        protected float windowWidth;
        protected float windowHeight;

        public MovementModule(Game1 game)
        {
            random = new Random();
            windowWidth = game.Window.ClientBounds.Width;
            windowHeight = game.Window.ClientBounds.Height;
        }

        public abstract void Setup(GameObjectVertical obj);

        public abstract void Update(GameTime gameTime, GameObjectVertical obj);
    }
}
