using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class WaitAction : ShipAction
    {
        OverworldShip ship;
        private Func<Boolean> condition;

        public WaitAction(OverworldShip ship, Func<Boolean> condition)
        {
            this.ship = ship;
            this.condition = condition;
        }

        public override void Update(GameTime gameTime)
        {
            if (condition.Invoke())
            {
                Finished = true;
            }
            else
            {
                ship.speed = 0;
                ship.destination = Vector2.Zero;
            }
        }
    }
}
