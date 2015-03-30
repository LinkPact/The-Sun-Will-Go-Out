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

        public PatrolAction(OverworldShip ship, Sector sector)
        {
            this.ship = ship;
            this.sector = sector;
            SetRandomDest();
        }

        private void SetRandomDest()
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            dest = new Vector2(
                r.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
                r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
            }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (CollisionDetection.IsPointInsideRectangle(ship.destination, ship.Bounds))
            {
                SetRandomDest();
            }

            ship.destination = dest;
        }
    }
}
