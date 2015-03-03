using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class TimedMessageObjective : Objective
    {
        private List<string> messages = new List<string>();
        private float messageDelay;
        private float messageTime;

        public TimedMessageObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, String message, float messageDelay, float startTime) :
            base(game, mission, description, destination)
        {
            messages.Add(message);
            this.messageDelay = messageDelay;
            messageTime = startTime;
        }

        public TimedMessageObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, String message, float messageDelay, float startTime,
            EventTextCapsule eventTextCapsule) :
            base(game, mission, description, destination)
        {
            messages.Add(message);
            this.messageDelay = messageDelay;
            messageTime = startTime;

            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;
        }

        public TimedMessageObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, List<String> messages, float messageDelay, float startTime) :
            base(game, mission, description, destination)
        {
            foreach (string str in messages)
            {
                this.messages.Add(str);
            }
            this.messageDelay = messageDelay;
            messageTime = startTime;
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
                PopupHandler.DisplayRealtimeMessage(messageDelay, messages.ToArray());
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
                && PopupHandler.TextBufferEmpty);
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
    }
}
