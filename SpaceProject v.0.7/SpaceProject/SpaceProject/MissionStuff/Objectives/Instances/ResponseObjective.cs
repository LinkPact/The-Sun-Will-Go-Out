using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class ResponseObjective : Objective
    {
        private EventText responseEventText;
        private KeyValuePair<EventText, List<EventText>> responseEvents;
        private SortedDictionary<int, System.Action> actions;
        private EventTextCanvas canvas;
        private bool delayResponse;

        public ResponseObjective(Game1 game, Mission mission, String description, ResponseTextCapsule responseTextCapsule) :
            base(game, mission, description)
        {
            Setup(responseTextCapsule);
        }

        public ResponseObjective(Game1 game, Mission mission, String description, 
            ResponseTextCapsule responseTextCapsule, EventTextCapsule eventTextCapsule) :
            base(game, mission, description)
        {
            Setup(responseTextCapsule);

            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            objectiveFailedEventText = eventTextCapsule.FailedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
        }

        private void Setup(ResponseTextCapsule responseTextCapsule)
        {
            responseEventText = responseTextCapsule.ResponseEvents.Key;
            responseEvents = responseTextCapsule.ResponseEvents;
            actions = responseTextCapsule.Actions;
            canvas = responseTextCapsule.EventTextCanvas;
        }

        public override void OnActivate()
        {
            base.OnActivate();

            if (canvas == EventTextCanvas.BaseState)
            {
                mission.MissionHelper.ShowEvent(responseEventText);
                mission.MissionHelper.ShowResponse(responseEvents.Value);

                if (GameStateManager.currentState.Equals("PlanetState"))
                {
                    game.stateManager.planetState.OnEnter();
                }

                else if (GameStateManager.currentState.Equals("StationState"))
                {
                    game.stateManager.stationState.OnEnter();
                }
            }

            else if (canvas == EventTextCanvas.MessageBox)
            {
                if (mission.MissionHelper.IsTextCleared())
                {
                    List<String> responses = new List<String>();
                    List<System.Action> a = new List<System.Action>();

                    foreach (EventText e in responseEvents.Value)
                    {
                        responses.Add(e.Text);
                    }

                    for (int i = 0; i < actions.Keys.Count; i++)
                    {
                        System.Action tempAction;

                        actions.TryGetValue(i, out tempAction);
                        a.Add(tempAction);
                    }


                    game.messageBox.DisplaySelectionMenu(responseEventText.Text, responses, a);
                }

                else
                {
                    delayResponse = true;
                }
            }
                
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(PlayTime playTime)
        {
            base.Update(playTime);

            if (mission.MissionResponse != 0)
            {
                if (mission.MissionResponse <= actions.Count)
                {
                    actions[mission.MissionResponse - 1].Invoke();
                }
            }

            if (delayResponse && mission.MissionHelper.IsTextCleared())
            {
                List<String> responses = new List<String>();
                List<System.Action> a = new List<System.Action>();

                foreach (EventText e in responseEvents.Value)
                {
                    responses.Add(e.Text);
                }

                for (int i = 0; i < actions.Keys.Count; i++)
                {
                    System.Action tempAction;

                    actions.TryGetValue(i, out tempAction);
                    a.Add(tempAction);
                }


                game.messageBox.DisplaySelectionMenu(responseEventText.Text, responses, a);

                delayResponse = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool Completed()
        {
            foreach (EventText e in responseEvents.Value)
            {
                if (e.Displayed && mission.MissionHelper.IsResponseTextCleared() &&
                    mission.MissionHelper.IsTextCleared())
                {
                    return true;
                }
            }

            return false;
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
