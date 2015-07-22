using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class SlantingLineModule : MovementModule
    {
        public SlantingLineModule(Game1 game)
            : base(game)
        { }

        public override void Setup(GameObjectVertical obj)
        {
            Vector2 centerDir = new Vector2(0, 1.0f);

            double dirRadians = MathFunctions.RadiansFromDir(centerDir);
            dirRadians += random.NextDouble() * 3 * Math.PI / 24 - Math.PI / 24;

            obj.Direction = MathFunctions.DirFromRadians(dirRadians);
        }

        public override void Update(GameTime gameTime, GameObjectVertical obj)
        { }
    }
}
