using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /*
     *  AutoPilotObjective.cs
     * 
     *  Objective for handling objectives in which the player ship is automatically 
     *  controlled to the destination.
     */
    public class AutoPilotObjective : Objective
    {
        private List<OverworldShip> ships;
        private float speed;
        private Vector2 companionStartingPos;
        private Dictionary<string, List<float>> timedMessages;
        private float nextMessageTime;
        private bool realTime;
        private int realTimeSwitchIndex;

        public AutoPilotObjective(Game1 game, Mission mission, String description, float speed) :
            base(game, mission, description)
        {
        }

        public AutoPilotObjective(Game1 game, Mission mission, String description,
            float speed, List<OverworldShip> companions,
            Vector2 companionStartingPos, Dictionary<string, List<float>> timedMessages,
            EventTextCapsule eventTextCapsule, bool realTime = true) :
            base(game, mission, description)
        {
            this.speed = speed;
            ships = companions;
            this.companionStartingPos = companionStartingPos;
            this.timedMessages = timedMessages;
            this.realTime = realTime;

            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;
        }

        public AutoPilotObjective(Game1 game, Mission mission, String description,
            float speed, List<OverworldShip> companions,
            Vector2 companionStartingPos, Dictionary<string, List<float>> timedMessages, bool realTime = true) :
            base(game, mission, description)
        {
            this.speed = speed;
            ships = companions;
            this.companionStartingPos = companionStartingPos;
            this.timedMessages = timedMessages;
            this.realTime = realTime;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnMissionStart()
        {
            base.OnMissionStart();
        }

        public override void OnActivate()
        {
            base.OnActivate();

            OverworldShip.FollowPlayer = false;
            game.player.DisableControls();

            // Starts companion ships and disables player controls
            foreach (OverworldShip ship in ships)
            {
                ship.Start();
                ship.speed = speed;
            }

            // Sets starting time of first message
            if (timedMessages.Keys.Count > 0)
            {
                nextMessageTime = GetNextMessageStartTime();
            }
        }

        public override void Update(PlayTime playTime)
        {
            base.Update(playTime);

            // Updates player direction and speed
            game.player.Direction.RotateTowardsPoint(game.player.position, Destination.position, 0.2f);
            game.player.speed = speed;

            // Displays timed messages
            if (timedMessages.Keys.Count > 0
                && StatsManager.PlayTime.HasOverworldTimePassed(nextMessageTime))
            {
                if (realTime)
                {
                    game.messageBox.DisplayRealtimeMessage(timedMessages.Keys.First(), GetNextMessageDuration());
                }
                else
                {
                    realTimeSwitchIndex++;
                    game.messageBox.DisplayMessage(timedMessages.Keys.First(), false);
                    if (realTimeSwitchIndex >= 2)
                    {
                        realTime = true;
                    }
                }
                timedMessages.Remove(timedMessages.Keys.First());

                if (timedMessages.Keys.Count > 0)
                {
                    nextMessageTime = GetNextMessageStartTime();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool Completed()
        {
            return (Vector2.Distance(game.player.position, Destination.position) < 500);
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            game.player.EnableControls();
            foreach (OverworldShip ship in ships)
            {
                ship.Wait();
            }

            OverworldShip.FollowPlayer = true;
            game.player.EnableControls();
        }

        public override bool Failed()
        {
            return false;
        }

        public override void OnFailed()
        {
            base.OnFailed();
        }

        public override void Reset()
        {
            base.Reset();
        }

        private float GetNextMessageStartTime()
        {
            List<float> value;
            float startTime;

            timedMessages.TryGetValue(timedMessages.Keys.First(), out value);
            startTime = value[0];
            return StatsManager.PlayTime.GetFutureOverworldTime(startTime);
        }

        private float GetNextMessageDuration()
        {
            List<float> value;

            timedMessages.TryGetValue(timedMessages.Keys.First(), out value);

            return value[1];
        }
    }
}
