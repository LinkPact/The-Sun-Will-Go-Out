using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    class DurationEventData : EventData
    {
        public DurationEventType assignedEventType;

        public DurationEventData(DurationEventType eventType)
        {
            assignedEventType = eventType;
            eventSettings = EventEditor.GetDurationSettings(eventType);
        }

    }
}
