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

        public ResponseObjective(Game1 game, Mission mission, String description, GameObjectOverworld destination,
            ResponseTextCapsule responseTextCapsule) :
            base(game, mission, description, destination)
        {
            responseEventText = responseTextCapsule.ResponseEvents.Key;
            responseEvents = responseTextCapsule.ResponseEvents;
            actions = responseTextCapsule.Actions;
        }

        public override void OnActivate()
        {
            base.OnActivate();

            mission.MissionHelper.ShowEvent(responseEventText);
            mission.MissionHelper.ShowResponse(responseEvents.Value);
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
