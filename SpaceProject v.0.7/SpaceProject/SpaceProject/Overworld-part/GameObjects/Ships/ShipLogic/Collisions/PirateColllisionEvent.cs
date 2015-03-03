using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class PirateColllisionEvent : CollisionEvent
    {
        public PirateColllisionEvent(Game1 game, OverworldShip ship, GameObjectOverworld target)
            : base(game, ship, target) { }

        public override void Invoke()
        {
            game.stateManager.overworldState.RemoveOverworldObject(ship);
            PopupHandler.DisplayMessage("You should have stayed in the warm comfort of you home planet. Surrender your cargo peacefully or take the consequences.");
            game.stateManager.shooterState.BeginPirateLevel();
        }
    }
}
