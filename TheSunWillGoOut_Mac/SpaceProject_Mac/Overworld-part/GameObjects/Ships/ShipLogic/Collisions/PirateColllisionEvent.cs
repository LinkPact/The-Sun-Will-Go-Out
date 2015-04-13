using System;
using System.Collections.Generic;

namespace SpaceProject_Mac
{
    class PirateColllisionEvent : CollisionEvent
    {
        public PirateColllisionEvent(Game1 game, OverworldShip ship, GameObjectOverworld target)
            : base(game, ship, target) { }

        public override void Invoke()
        {
            ship.Explode();
            game.stateManager.overworldState.RemoveOverworldObject(ship);
            game.stateManager.shooterState.BeginPirateLevel();
            PopupHandler.DisplayMessage("You should have stayed in the warm comfort of you home planet. Surrender your cargo peacefully or take the consequences.");
        }
    }
}
