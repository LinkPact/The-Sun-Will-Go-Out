using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class AlliancePirateCollisionEvent : CollisionEvent
    {
         private List<string> messages;

         public AlliancePirateCollisionEvent(Game1 game, OverworldShip ship, GameObjectOverworld target)
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
                game.stateManager.overworldState.RemoveOverworldObject(ship);
                game.messageBox.DisplayMessage(0, GetMessage());
                game.stateManager.shooterState.BeginAlliancePirateLevel();
            }
        }

        private string GetMessage()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return messages[r.Next(0, messages.Count)];
        }
    }
}
