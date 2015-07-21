using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    public class ResponseTextCapsule
    {
        private KeyValuePair<EventText, List<EventText>> responseEvents;
        public KeyValuePair<EventText, List<EventText>> ResponseEvents { get { return responseEvents; } private set { ;} }

        private SortedDictionary<int, System.Action> actions;
        public SortedDictionary<int, System.Action> Actions { get { return actions; } private set { ;} }

        private EventTextCanvas eventTextCanvas;
        public EventTextCanvas EventTextCanvas { get { return eventTextCanvas; } private set { ; } }

        public ResponseTextCapsule(EventText eventText, List<EventText> responses, List<System.Action> actions, EventTextCanvas canvas)
        {
            responseEvents = new KeyValuePair<EventText, List<EventText>>(eventText, responses);
            this.actions = new SortedDictionary<int, System.Action>();

            for (int i = 0; i < actions.Count; i++)
            {
                this.actions.Add(i, actions[i]);
            }

            eventTextCanvas = canvas;
        }
    }
}
