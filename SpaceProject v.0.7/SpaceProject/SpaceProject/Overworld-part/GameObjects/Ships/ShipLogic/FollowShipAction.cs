using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class FollowShipAction : ShipAction
    {
        OverworldShip ship;
        GameObjectOverworld target;

        public FollowShipAction(OverworldShip ship, GameObjectOverworld target)
        {
            this.ship = ship;
            this.target = target;
        }

        public override void Update(GameTime gameTime)
        {
            // Adjust course towards target
            if (target.position != Vector2.Zero)
            {
                ship.Direction.RotateTowardsPoint(ship.position, target.position, 0.2f);
                ship.AddParticle();
            }
            else
                ship.Direction = Direction.Zero;
        }
    }
}
