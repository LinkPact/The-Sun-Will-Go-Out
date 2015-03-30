using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public class EventText
    {
        private String text;
        public String Text { get { return text; } set { text = value; } }

        private bool displayed;
        public bool Displayed { get { return displayed; } set { displayed = value; } }

        public EventText(String text)
        {
            this.text = text;
        }
    }
}
