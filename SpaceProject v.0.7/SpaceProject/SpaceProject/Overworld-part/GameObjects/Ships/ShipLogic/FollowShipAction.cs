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
            if (CollisionDetection.IsPointInsideRectangle(target.position, ship.view) && target.position != Vector2.Zero)
            {
                ship.destination = target.position;
                Finished = true;
            }
            else
            {
                //ship.Direction = Direction.Zero;
                //ship.destination = Vector2.Zero;
                Finished = false;
            }
        }
    }
}
