using System;
using System.Collections.Generic;

namespace SpaceProject_Mac
{
    class DisplayFreighterMessageOE : OverworldEvent
    {
        private List<String> text = new List<String>();
        private string[] messages = new string[] {
        "Grettings traveler!\nWe are currently transporting potatoes.",
        "Grettings!\nWe are making transports of supplies to the colonies.",
        "Grettings!\nWe are making transports of supplies to the colonies.",
        "What we're transporting is none of your business.",
        "Don't disturb us.\nOur mission is of great importance to the alliance."};

        public DisplayFreighterMessageOE(Game1 game) :
            base()
        {
            this.text.Add(messages[game.random.Next(0,messages.Length)]);
        }

        public override Boolean Activate() 
        {
            PopupHandler.DisplayMessage(text.ToArray());
            return true;
        }
    }
}
