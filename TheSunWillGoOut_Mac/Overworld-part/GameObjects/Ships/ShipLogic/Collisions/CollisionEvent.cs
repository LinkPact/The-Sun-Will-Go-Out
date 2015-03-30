using System;
using System.Collections.Generic;

namespace SpaceProject_Mac
{
    public class CollisionEvent
    {
        protected Game1 game;
        protected OverworldShip ship;
        public GameObjectOverworld target;

        public CollisionEvent(Game1 game, OverworldShip ship, GameObjectOverworld target)
        {
            this.game = game;
            this.ship = ship;
            this.target = target;
        }

        public virtual void Invoke()
        {

        }
    }
}
