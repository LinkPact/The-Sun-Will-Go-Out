using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class RemoveOnCollisionEvent : CollisionEvent
    {
        public RemoveOnCollisionEvent(Game1 game, OverworldShip ship, GameObjectOverworld target)
            : base(game, ship, target) { }

        public override void Invoke()
        {
            game.stateManager.overworldState.RemoveOverworldObject(ship);
        }
    }
}
