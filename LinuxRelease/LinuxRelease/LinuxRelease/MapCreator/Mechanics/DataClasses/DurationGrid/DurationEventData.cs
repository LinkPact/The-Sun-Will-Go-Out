using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
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
