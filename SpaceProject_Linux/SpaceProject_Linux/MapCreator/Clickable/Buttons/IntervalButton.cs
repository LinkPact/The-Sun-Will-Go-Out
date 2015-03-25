using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.RegularExpressions;

namespace SpaceProject_Linux
{
    public class IntervalButton : Clickable
    {
        private IntervalButton previous;
        private IntervalButton subsequent;

        private float previousTime;
        private float subsequentTime;
        private float currentTime;

        public IntervalButton(Vector2 position, IntervalButton previous, 
            IntervalButton subsequent, float time)
            : base(position)
        {
            this.position = position;
            this.currentTime = time;

            this.previous = previous;
            this.subsequent = subsequent;

            if (previous != null)
                previousTime = previous.previousTime;
            else
                previousTime = 0;

            if (subsequent != null)
                subsequentTime = subsequent.subsequentTime;
            else
                subsequentTime = currentTime;

            displayText = currentTime.ToString();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsLeftClicked())
            {
                UpdateTimes();
                float inputTime = GetNewFloat("Enter new time:", "Interval: " +
                previousTime.ToString() + "-" + subsequentTime.ToString(), displayText);

                if (inputTime != -1)
                {
                    if (inputTime < previousTime)
                        inputTime = previousTime;
                    else if (inputTime > subsequentTime)
                        inputTime = subsequentTime;
                    
                    currentTime = inputTime;
                }

                displayText = currentTime.ToString();
            }
        }

        private void UpdateTimes()
        {
            if (previous != null)
                previousTime = previous.currentTime;
            if (subsequent != null)
                subsequentTime = subsequent.currentTime;
        }

        private float GetNewTime()
        {
            //String newTimeString =  Microsoft.VisualBasic.Interaction.InputBox("Enter new time:", "Interval: " + 
            //    previousTime.ToString() + "-" + subsequentTime.ToString(), displayText, -1, -1);
            //
            //Match matchFloat = Regex.Match(newTimeString, @"^\d+(\.d+)?$");
            //
            //float inputFloat;
            //if (matchFloat.Success)
            //{
            //    inputFloat = (float)(Convert.ToDouble(newTimeString));
            //    if (inputFloat > 0)
            //        return inputFloat;
            //}
            //    return -1;

            throw new Exception("Currently unsupported!");
        }

        public override String ToString()
        {
            return currentTime.ToString();
        }

        public void SetNext(IntervalButton nextButton)
        {
            subsequent = nextButton;
        }

        public void SetMaxTime(int newMaxTime)
        {
            if (subsequent == null)
                subsequentTime = newMaxTime;
        }

        public float GetTime()
        {
            return currentTime;
        }
    }
}
