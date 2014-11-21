using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class HangarCollisionEvent : CollisionEvent
    {
        public HangarCollisionEvent(Game1 game, OverworldShip ship, GameObjectOverworld target)
            : base(game, ship, target) { }

        public override void Invoke()
        {
            game.stateManager.overworldState.RemoveOverworldObject(ship);
            game.messageBox.DisplayMessage("You should have stayed in the warm comfort of you home planet. Surrender your cargo peacefully or take the consequences.", false);
            game.stateManager.shooterState.BeginHangarLevel();
        } 
    }
}
