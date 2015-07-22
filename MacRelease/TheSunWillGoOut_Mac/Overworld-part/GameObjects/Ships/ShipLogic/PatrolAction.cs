using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class PatrolAction : ShipAction
    {
        private OverworldShip ship;
        private Sector sector;
        private Vector2 dest;
        private Random r;

        public PatrolAction(OverworldShip ship, Sector sector)
        {
            this.ship = ship;
            this.sector = sector;
            r = new Random(Guid.NewGuid().GetHashCode());
            SetRandomDest();
        }

        private void SetRandomDest()
        {
            dest = new Vector2(
                r.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
                r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));

            while (InRestrictedSpace(ship, dest) || PathCrossRestrictedSpace(ship, ship.position, dest) || dest == Vector2.Zero)
            {
                dest = new Vector2(
                    r.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
                    r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (dest == Vector2.Zero || CollisionDetection.IsPointInsideRectangle(ship.destination, ship.Bounds) || InRestrictedSpace(ship, ship.destination))
            {
                SetRandomDest();
            }

            ship.destination = dest;
        }
    }
}
