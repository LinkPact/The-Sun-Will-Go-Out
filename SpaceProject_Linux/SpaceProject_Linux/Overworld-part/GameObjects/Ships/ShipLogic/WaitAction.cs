using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    class WaitAction : ShipAction
    {
        OverworldShip ship;
        private Func<Boolean> condition;
        private float originalSpeed;        // DANNE: Added a few lines to reset speed when WaitAction is finished

        public WaitAction(OverworldShip ship, Func<Boolean> condition)
        {
            this.ship = ship;
            this.condition = condition;
            originalSpeed = ship.speed;
        }

        public override void Update(GameTime gameTime)
        {
            if (condition.Invoke())
            {
                Finished = true;
                ship.speed = originalSpeed;
            }
            else
            {
                ship.speed = 0;
                ship.destination = Vector2.Zero;
            }
        }
    }
}
