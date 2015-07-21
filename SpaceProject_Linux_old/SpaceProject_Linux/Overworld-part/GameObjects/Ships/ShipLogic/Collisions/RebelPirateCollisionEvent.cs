using System;
using System.Collections.Generic;

namespace SpaceProject_Linux
{
    class RebelPirateCollisionEvent : CollisionEvent
    {
        private List<string> messages;

        public RebelPirateCollisionEvent(Game1 game, OverworldShip ship, GameObjectOverworld target)
            : base(game, ship, target) 
        {
            messages = new List<string>();
            messages.Add("The alliance must be destroyed!");
            messages.Add("Death to the alliance!");
            messages.Add("The Alliance are destroying our homes. You must pay!");
        }

        public override void Invoke()
        {
            if (StatsManager.reputation >= 0)
            {
                ship.Explode();
                game.stateManager.overworldState.RemoveOverworldObject(ship);
                game.stateManager.shooterState.BeginRebelPirateLevel();
                PopupHandler.DisplayMessage(GetMessage());
            }
        }

        private string GetMessage()
        {
            return messages[game.random.Next(0, messages.Count)];
        }
    }
}
