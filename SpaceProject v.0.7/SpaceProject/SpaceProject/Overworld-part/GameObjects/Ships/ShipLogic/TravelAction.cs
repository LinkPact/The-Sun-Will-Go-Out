using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class TravelAction : ShipAction
    {
        OverworldShip ship;
        GameObjectOverworld target;

        public TravelAction(OverworldShip ship, GameObjectOverworld target)
        {
            this.ship = ship;
            this.target = target;
        }

        public override void Update(GameTime gameTime)
        {
            // Check if arrived at destination
            if (CollisionDetection.IsRectInRect(ship.Bounds, target.Bounds))
            {
                Finished = true;
            }
            else
            {
                ship.destination = target.position;
                Finished = false;
            }
        }

        static public GameObjectOverworld GetRandomPlanet(Sector sector)
        {
            List<GameObjectOverworld> tempList = new List<GameObjectOverworld>();
            tempList.AddRange(sector.GetGameObjects());

            Random r = new Random(DateTime.Now.Millisecond);
            return tempList[(int)r.Next(0, tempList.Count - 1)];
        }
    }
}
