using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    class WaitTimeAction : ShipAction
    {
        OverworldShip ship;
        int waitTime;
        int counter;

        public WaitTimeAction(OverworldShip ship, int time)
        {
            this.ship = ship;
            this.waitTime = time;
            counter = 0;
        }

        public override void Reset()
        {
            counter = 0;
            Finished = false;
        }

        public override void Update(GameTime gameTime)
        {
            counter += gameTime.ElapsedGameTime.Milliseconds;
            ship.destination = Vector2.Zero;
            if (counter >= waitTime)
            {
                Finished = true;
            }
        }
    }
}
