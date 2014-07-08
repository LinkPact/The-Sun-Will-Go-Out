using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class EventTextCapsule
    {
        private String completedText;
        public String CompletedText { get { return completedText; } set { completedText = value; } }

        private String failedText;
        public String FailedText { get { return failedText; } set { failedText = value; } }

        private EventTextCanvas eventTextCanvas;
        public EventTextCanvas EventTextCanvas { get { return eventTextCanvas; } set { eventTextCanvas = value; } }

        public EventTextCapsule(String completedText, String failedText, EventTextCanvas eventTextCanvas)
        {
            this.completedText = completedText;
            this.FailedText = failedText;
            this.eventTextCanvas = eventTextCanvas;

            if (completedText == null)
            {
                this.completedText = "";
            }
            if (failedText == null)
            {
                this.failedText = "";
            }
        }
    }
}
