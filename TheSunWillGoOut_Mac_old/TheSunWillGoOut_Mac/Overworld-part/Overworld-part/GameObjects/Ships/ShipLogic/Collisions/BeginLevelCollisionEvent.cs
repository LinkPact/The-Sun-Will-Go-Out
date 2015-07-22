using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class BeginLevelCollisionEvent : CollisionEvent
    {
        // Used to determine which level starts when player runs into this ship.
        private string level;
        public string Level { set { level = value; } }

        // Message shown on encounter
        private string encounterMessage;
        public string EncounterMessage { set { encounterMessage = value; } }

        public BeginLevelCollisionEvent(Game1 game, OverworldShip ship, GameObjectOverworld target)
            : base(game, ship, target) { }

        public override void Invoke()
        {
            base.Invoke();

            game.stateManager.overworldState.RemoveOverworldObject(ship);
            game.stateManager.shooterState.BeginLevel(level);
            PopupHandler.DisplayMessage(encounterMessage);
        }
    }
}
