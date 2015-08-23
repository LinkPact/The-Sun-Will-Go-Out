using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public abstract class AreaObject : AnimatedGameObject
    {
        protected AreaObject(Game1 game, Vector2 position)
            : base(game, game.spriteSheetVerticalShooter)
        {
            Position = position;
        }

        public abstract Boolean IsOverlapping(AnimatedGameObject obj);
    }
}
