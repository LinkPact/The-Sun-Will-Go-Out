using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class EventTextCapsule
    {
        private EventText completedText;
        public EventText CompletedText { get { return completedText; } set { completedText = value; } }

        private EventText failedText;
        public EventText FailedText { get { return failedText; } set { failedText = value; } }

        private EventTextCanvas eventTextCanvas;
        public EventTextCanvas EventTextCanvas { get { return eventTextCanvas; } set { eventTextCanvas = value; } }

        public EventTextCapsule(EventText completedText, EventText failedText, EventTextCanvas eventTextCanvas)
        {
            this.completedText = completedText;
            this.FailedText = failedText;
            this.eventTextCanvas = eventTextCanvas;

            if (completedText == null)
            {
                this.completedText = new EventText("");
            }
            if (failedText == null)
            {
                this.failedText = new EventText("");
            }
        }
    }
}
