using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class WaitAction : ShipAction
    {
        OverworldShip ship;
        Vector2 tempDestination;
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
                tempDestination = ship.destination;
                ship.destination = Vector2.Zero;
            }
        }
    }
}
