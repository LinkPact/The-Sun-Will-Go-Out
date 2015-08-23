using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class DisplayTextOE : OverworldEvent
    {
        private List<String> text = new List<String>();

        public DisplayTextOE(List<String> text) :
            base()
        {
            this.text = text;
        }

        public DisplayTextOE(String text) :
            base()
        {
            this.text.Add(text);
        }

        public override Boolean Activate() 
        {
            PopupHandler.DisplayMessage(text.ToArray());
            return true;
        }
    }
}
