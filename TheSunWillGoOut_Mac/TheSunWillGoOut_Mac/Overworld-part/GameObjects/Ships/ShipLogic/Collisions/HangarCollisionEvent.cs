using System;
using System.Collections.Generic;

namespace SpaceProject_Mac
{
    class HangarCollisionEvent : CollisionEvent
    {
        private List<string> messages;

        public HangarCollisionEvent(Game1 game, OverworldShip ship, GameObjectOverworld target)
            : base(game, ship, target) 
        {
            messages = new List<string>();
            messages.Add("Die rebel scum!");
            messages.Add("You are deamed a traitor to the alliance. You will pay for your crimes!");
            messages.Add("Resistance is futile, surrender now!");
        }

        public override void Invoke()
        {
            if (StatsManager.reputation < 0)
            {
                ship.Explode();
                game.stateManager.overworldState.RemoveOverworldObject(ship);
                game.stateManager.shooterState.BeginHangarLevel();
                PopupHandler.DisplayMessage(GetMessage());
            }
        }

        private string GetMessage()
        {
            return messages[game.random.Next(0, messages.Count)];
        }
    }
}
