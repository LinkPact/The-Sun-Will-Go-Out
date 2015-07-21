using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    public class CheckPoint
    {

        private bool isActive;
        public bool IsActive { get { return isActive; } }

        private float startTime;

        private int duration;
        public int Duration { get { return duration; } }

        public CheckPoint(float startTime)
        {
            this.startTime = startTime;
            this.duration = 0;
        }

        public CheckPoint(float startTime, int duration)
        {
            this.startTime = startTime;
            this.duration = duration;
        }

        public void CheckActivation(int levelTime)
        {
            if (levelTime > startTime && !isActive)
                isActive = true;
        }

    }
}
