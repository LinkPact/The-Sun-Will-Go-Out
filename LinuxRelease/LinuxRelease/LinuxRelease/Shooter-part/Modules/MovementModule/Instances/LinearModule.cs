using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class LinearModule : MovementModule
    {

        public LinearModule(Game1 game)
            : base(game)
        { }

        public override void Setup(GameObjectVertical obj)
        {
            obj.DirectionY = 1.0f;
            obj.DirectionX = 0.0f;
        }

        public override void Update(GameTime gameTime, GameObjectVertical obj)
        { }
    }
}
