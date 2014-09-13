using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class PatrolAction : ShipAction
    {
        OverworldShip ship;
        Sector sector;

        public PatrolAction(OverworldShip ship, Sector sector)
        {
            this.ship = ship;
            this.sector = sector;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (CollisionDetection.IsPointInsideRectangle(ship.destination, ship.Bounds))
            {
                Random r = new Random(DateTime.Now.Millisecond);
                ship.destination = new Vector2(
                    r.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
                    r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
            }
        }
    }
}
