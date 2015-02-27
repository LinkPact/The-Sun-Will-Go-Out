﻿using System;
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

        public AutoPilotObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, float speed) :
            base(game, mission, description, destination)
        {
        }

        public AutoPilotObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, float speed, List<OverworldShip> companions,
            Vector2 companionStartingPos, Dictionary<string, List<float>> timedMessages,
            EventTextCapsule eventTextCapsule) :
            base(game, mission, description, destination)
        {
            this.speed = speed;
            ships = companions;
            this.companionStartingPos = companionStartingPos;
            this.timedMessages = timedMessages;

            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;
        }

        public AutoPilotObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, float speed, List<OverworldShip> companions,
            Vector2 companionStartingPos, Dictionary<string, List<float>> timedMessages) :
            base(game, mission, description, destination)
        {
            this.speed = speed;
            ships = companions;
            this.companionStartingPos = companionStartingPos;
            this.timedMessages = timedMessages;

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

            OverworldShip.FollowPlayer = true;

            // Starts companion ships and disables player controls
            foreach (OverworldShip ship in ships)
            {
                ship.Start();
                ship.speed = speed;
            }

            ControlManager.DisableControls();

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
            game.player.speed = speed + 0.013f;

            // Displays timed messages
            if (timedMessages.Keys.Count > 0
                && StatsManager.PlayTime.HasOverworldTimePassed(nextMessageTime))
            {
                game.messageBox.DisplayRealtimeMessage(GetNextMessageDuration(), timedMessages.Keys.First());
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

            ControlManager.EnableControls();
            foreach (OverworldShip ship in ships)
            {
                ship.Wait();
            }

            OverworldShip.FollowPlayer = true;
        }

        public override bool Failed()
        {
            return false;
        }

        public override void OnFailed()
        {
            base.OnFailed();
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
