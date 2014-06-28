using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class ShipInventoryDisplayObject : MenuDisplayObject
    {
        private Coordinate coordinate;

        public ShipInventoryDisplayObject(Game1 Game, Sprite passive, Sprite active, Vector2 position, Coordinate coordinate)
            : base(Game, passive, active, position)
        {
            this.coordinate = coordinate;
        }

        public void UpdateActivity(Coordinate coordinate)
        {
            if (this.coordinate.Equals(coordinate))
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }
    }
}
