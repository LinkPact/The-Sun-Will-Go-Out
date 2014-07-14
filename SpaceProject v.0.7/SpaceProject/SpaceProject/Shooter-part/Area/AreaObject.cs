using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public abstract class AreaObject : GameObjectVertical
    {
        protected Vector2 sourcePosition;

        protected AreaObject(Game1 game, Vector2 position)
            : base(game)
        {
            this.sourcePosition = position;
        }

        public abstract Boolean IsOverlapping(AnimatedGameObject obj);
    }
}
