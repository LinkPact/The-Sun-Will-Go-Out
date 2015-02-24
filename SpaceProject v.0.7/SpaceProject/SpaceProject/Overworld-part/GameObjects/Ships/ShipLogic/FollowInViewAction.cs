using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class FollowInViewAction : ShipAction
    {
        OverworldShip ship;
        GameObjectOverworld target;

        public FollowInViewAction(OverworldShip ship, GameObjectOverworld target)
        {
            this.ship = ship;
            this.target = target;
        }

        public override void Update(GameTime gameTime)
        {
            // Set course towards target
            if (CollisionDetection.IsPointInsideRectangle(target.position, ship.view) 
                && target.position != Vector2.Zero)
            {
                if (target is PlayerOverworld &&
                    OverworldShip.FollowPlayer &&
                    ((ship is RebelShip && StatsManager.reputation >= 0) ||
                    ((ship is AllianceShip || ship is HangarShip) && StatsManager.reputation < 0)))
                {
                    ship.destination = target.position;
                    Finished = true;
                }
                else if (!(target is PlayerOverworld))
                {
                    ship.destination = target.position;
                    Finished = true;
                }
            }
            else
            {
                Finished = false;
            }
        }
    }
}
