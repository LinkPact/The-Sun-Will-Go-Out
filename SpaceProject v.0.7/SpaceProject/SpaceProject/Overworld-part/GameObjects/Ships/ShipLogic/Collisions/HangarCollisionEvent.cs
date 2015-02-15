using System;
using System.Collections.Generic;

namespace SpaceProject
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
            game.stateManager.overworldState.RemoveOverworldObject(ship);
            game.messageBox.DisplayMessage(0, "You should have stayed in the warm comfort of you home planet. Surrender your cargo peacefully or take the consequences.");
            game.stateManager.shooterState.BeginHangarLevel();
        }

        private string GetMessage()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return messages[r.Next(0, messages.Count)];
        }
    }
}
