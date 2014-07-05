using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class EventTextCapsule
    {
        private List<String> completedText;
        public List<String> CompletedText { get { return completedText; } set { completedText = value; } }

        private List<String> failedText;
        public List<String> FailedText { get { return failedText; } set { failedText = value; } }

        private EventTextCanvas eventTextCanvas;
        public EventTextCanvas EventTextCanvas { get { return eventTextCanvas; } set { eventTextCanvas = value; } }

        public EventTextCapsule(List<String> completedText, List<String> failedText, EventTextCanvas eventTextCanvas)
        {
            this.completedText = completedText;
            this.FailedText = failedText;
            this.eventTextCanvas = eventTextCanvas;

            if (completedText == null)
            {
                this.completedText = new List<String>();
            }
            if (failedText == null)
            {
                this.failedText = new List<String>();
            }
        }
    }
}
