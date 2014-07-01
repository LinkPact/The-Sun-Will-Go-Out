using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class EventTextCapsule
    {
        private List<String> compeletedText;
        public List<String> CompletedText { get { return compeletedText; } set { compeletedText = value; } }

        private List<String> failedText;
        public List<String> FailedText { get { return failedText; } set { failedText = value; } }

        private EventTextCanvas eventTextCanvas;
        public EventTextCanvas EventTextCanvas { get { return eventTextCanvas; } set { eventTextCanvas = value; } }

        public EventTextCapsule(List<String> completedText, List<String> failedText, EventTextCanvas eventTextCanvas)
        {
            this.compeletedText = completedText;
            this.FailedText = failedText;
            this.eventTextCanvas = eventTextCanvas;

            if (completedText == null)
            {
                completedText = new List<String>();
            }
            if (failedText == null)
            {
                failedText = new List<String>();
            }
        }
    }
}
