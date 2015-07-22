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

        private List<PortraitID> portraits;
        public List<PortraitID> Portraits { get { return portraits; } }

        private List<int> portraitTriggers;
        public List<int> PortraitTriggers { get { return portraitTriggers; } }

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
            portraits = new List<PortraitID>();
            portraitTriggers = new List<int>();
        }

        public EventTextCapsule(EventText completedText, EventText failedText,
            EventTextCanvas eventTextCanvas, PortraitID portrait) :
            this(completedText, failedText, eventTextCanvas)
        {
            portraits.Add(portrait);
        }

        public EventTextCapsule(EventText completedText, EventText failedText,
            EventTextCanvas eventTextCanvas, List<PortraitID> portraits, List<int> portraitTriggers) :
            this(completedText, failedText, eventTextCanvas)
        {
            this.portraits = portraits;
            this.portraitTriggers = portraitTriggers;
        }
    }
}
