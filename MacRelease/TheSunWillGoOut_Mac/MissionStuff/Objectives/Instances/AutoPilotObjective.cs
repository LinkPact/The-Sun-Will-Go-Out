using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
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
        private bool useRealTimeSwitch;
        private int realTimeSwitchIndex;
        private int activateRealTimeSwitchIndex;
        private List<List<PortraitID>> portraits;
        private List<List<int>> portraitTriggers;
        private bool skip;

        public AutoPilotObjective(Game1 game, Mission mission, String description, float speed) :
            base(game, mission, description)
        {
        }

        public AutoPilotObjective(Game1 game, Mission mission, String description,
            float speed, List<OverworldShip> companions,
            Vector2 companionStartingPos,
            EventTextCapsule eventTextCapsule, bool realTime = true) :
            base(game, mission, description)
        {
            this.speed = speed;
            ships = companions;
            this.companionStartingPos = companionStartingPos;
            this.realTime = realTime;

            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;

            if (eventTextCapsule.Portraits.Count > 0)
            {
                SetupPortraits(eventTextCapsule.Portraits, eventTextCapsule.PortraitTriggers);
            }
        }

        public AutoPilotObjective(Game1 game, Mission mission, String description,
            float speed, List<OverworldShip> companions,
            Vector2 companionStartingPos, bool realTime = true) :
            base(game, mission, description)
        {
            this.speed = speed;
            ships = companions;
            this.companionStartingPos = companionStartingPos;
            this.realTime = realTime;
        }

        public override void Initialize()
        {
            base.Initialize();

            portraits = new List<List<PortraitID>>();
            portraitTriggers = new List<List<int>>();
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
            if (!HasAnyShipArrived())
            {
                game.player.Direction.SetDirection(new Vector2(
                    Destination.position.X - game.player.position.X,
                    Destination.position.Y - game.player.position.Y));

                if (ControlManager.CheckPress(RebindableKeys.Pause)
                    && GameStateManager.currentState.Equals("OverworldState"))
                {
                    PopupHandler.DisplaySelectionMenu("Do you want to skip travel?",
                        new List<string>() { "Yes", "No" },
                        new List<System.Action>(){
                            delegate 
                            {
                                SkipForward();
                            },
                            delegate {}
                        });
                }
            }
            game.player.speed = speed;

            // Displays timed messages
            if (timedMessages.Keys.Count > 0
                && StatsManager.PlayTime.HasOverworldTimePassed(nextMessageTime))
            {
                if (portraits.Count > 0)
                {
                    if (realTime)
                    {
                        PopupHandler.DisplayRealtimePortraitMessage(GetNextMessageDuration(), portraits[0].ToArray(), 
                            portraitTriggers[0], timedMessages.Keys.First());
                    }
                    else
                    {
                        realTimeSwitchIndex++;
                        PopupHandler.DisplayPortraitMessage(portraits[0], portraitTriggers[0], timedMessages.Keys.First());
                        if (useRealTimeSwitch
                            && realTimeSwitchIndex >= activateRealTimeSwitchIndex)
                        {
                            realTime = true;
                        }
                    }

                    portraits.RemoveAt(0);
                    portraitTriggers.RemoveAt(0);
                }
                else
                {
                    if (realTime)
                    {
                        PopupHandler.DisplayRealtimeMessage(GetNextMessageDuration(), timedMessages.Keys.First());
                    }
                    else
                    {
                        realTimeSwitchIndex++;
                        PopupHandler.DisplayMessage(timedMessages.Keys.First());
                        if (useRealTimeSwitch
                            && realTimeSwitchIndex >= activateRealTimeSwitchIndex)
                        {
                            realTime = true;
                        }
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
            return HasAnyShipArrived() || skip;
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

        public void SetTimedMessages(Dictionary<string, List<float>> timedMessages,
            List<List<PortraitID>> portraits, List<List<int>> portraitTriggers)
        {
            if (portraits != null)
            {
                this.portraits = portraits;
                this.portraitTriggers = portraitTriggers;
            }

            this.timedMessages = timedMessages;
        }

        public void SetActivateRealTimeIndex(int index)
        {
            useRealTimeSwitch = true;
            activateRealTimeSwitchIndex = index;
        }

        private bool HasAnyShipArrived()
        {
            foreach (OverworldShip ship in ships)
            {
                if (ship.HasArrived)
                {
                    return true;
                }
            }

            return false;
        }

        private void SkipForward()
        {
            game.player.position = Destination.position;
            game.camera.cameraPos = game.player.position;
            PopupHandler.SkipRealTimeMessages();
            ships[0].HasArrived = true;
            for (int i = 0; i < portraits.Count; i++)
            {
                PopupHandler.DisplayPortraitMessage(portraits[0], portraitTriggers[0], timedMessages.Keys.First());
                portraits.RemoveAt(0);
                portraitTriggers.RemoveAt(0);
                timedMessages.Remove(timedMessages.Keys.First());
            }
            skip = true;
        }
    }
}
