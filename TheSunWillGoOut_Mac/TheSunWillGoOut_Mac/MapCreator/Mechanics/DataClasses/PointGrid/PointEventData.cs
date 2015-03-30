using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class PointEventData : EventData
    {
        public PointEventType assignedEventType;

        public PointEventData(PointEventType eventType)
            : base()
        {
            assignedEventType = eventType;
            eventSettings = EventEditor.GetPointSettings(eventType);
        }
    }
}
