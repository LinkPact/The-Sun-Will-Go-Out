using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Class representing data for each event
    public abstract class EventData
    {
        //public PointEventType assignedEventType;
        public SortedDictionary<string, int> eventSettings;

        public EventData()
        { }

        public List<String> GetEventSettings()
        {
            List<String> eventStrings = new List<String>();
            
            foreach (KeyValuePair<string, int> entry in eventSettings)
            {
                eventStrings.Add(entry.Key + " " + entry.Value);
            }

            return eventStrings;
        }
    }
}
