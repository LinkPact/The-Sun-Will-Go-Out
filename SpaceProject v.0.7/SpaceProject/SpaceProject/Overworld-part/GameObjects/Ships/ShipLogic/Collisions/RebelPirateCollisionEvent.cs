using System;
using System.Collections.Generic;

namespace SpaceProject
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
                game.stateManager.overworldState.RemoveOverworldObject(ship);
                PopupHandler.DisplayMessage(GetMessage());
                game.stateManager.shooterState.BeginRebelPirateLevel();
            }
        }

        private string GetMessage()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return messages[r.Next(0, messages.Count)];
        }
    }
}
