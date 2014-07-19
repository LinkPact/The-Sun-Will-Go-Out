using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public abstract class AreaObject : GameObjectVertical
    {
        protected AreaObject(Game1 game, Vector2 position)
            : base(game)
        {
            Position = position;
        }

        public abstract Boolean IsOverlapping(AnimatedGameObject obj);
    }
}
