using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    public class TimedMessageObjective : Objective
    {
        private List<string> messages = new List<string>();
        private float messageDelay;
        private float messageTime;

        private List<PortraitID> portraits;
        private List<int> portraitTriggers;

        public TimedMessageObjective(Game1 game, Mission mission, String description, 
            float messageDelay, float startTime, params string[] messages) :
            base(game, mission, description)
        {
            foreach (string str in messages)
            {
                this.messages.Add(str);
            }
            this.messageDelay = messageDelay;
            messageTime = startTime;
        }

        public TimedMessageObjective(Game1 game, Mission mission, String description,
            float messageDelay, float startTime, PortraitID portrait, params string[] messages) :
            this(game, mission, description, messageDelay, startTime, messages)
        {
            portraitTriggers = new List<int>();
            portraits = new List<PortraitID>();
            portraits.Add(portrait);
        }

        public TimedMessageObjective(Game1 game, Mission mission, String description,
            float messageDelay, float startTime, List<PortraitID> portraits,
            List<int> portraitTriggers, params string[] messages) :
            this(game, mission, description, messageDelay, startTime, messages)
        {
            this.portraits = portraits;
            this.portraitTriggers = portraitTriggers;
        }

        public override void OnActivate()
        {
            messageTime = StatsManager.PlayTime.GetFutureOverworldTime(messageTime);
            base.OnActivate();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(PlayTime playTime)
        {
            if (messageTime != - 1
                && StatsManager.PlayTime.HasOverworldTimePassed(messageTime))
            {
                messageTime = -1;

                if (portraits != null
                    && portraits.Count > 0)
                {
                    PopupHandler.DisplayRealtimePortraitMessage(messageDelay, portraits.ToArray(),
                        portraitTriggers, messages.ToArray());
                }
                else
                {
                    PopupHandler.DisplayRealtimeMessage(messageDelay, messages.ToArray());
                }
            }
            base.Update(playTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool Completed()
        {
            return (messageTime == -1
                && PopupHandler.RealTimeTextBufferEmpty);
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
        }

        public override bool Failed()
        {
            return false;
        }

        public override void OnFailed()
        {
            base.OnFailed();
        }

        public void SetEventText(EventTextCapsule eventTextCapsule)
        {
            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;

            if (eventTextCapsule.Portraits.Count > 0)
            {
                SetupPortraits(eventTextCapsule.Portraits, eventTextCapsule.PortraitTriggers);
            }
        }
    }
}
