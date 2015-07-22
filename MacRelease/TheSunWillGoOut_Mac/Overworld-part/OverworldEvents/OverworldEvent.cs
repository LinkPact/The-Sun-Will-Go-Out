using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public abstract class OverworldEvent
    {
        private Boolean isCleared = false;

        public OverworldEvent() 
        { }

        public Boolean IsCleared()
        {
            return isCleared;
        }

        protected void ClearEvent()
        {
            isCleared = true;
        }

        public virtual void Update(Game1 game, GameTime gameTime)
        { }

        public abstract Boolean Activate();

    }
}
