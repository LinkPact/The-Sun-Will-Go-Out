using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum EventTextFormat
    {
        BaseState,
        MessageBox
    }

    public abstract class Objective
    {
        protected Game1 game;
        protected Mission mission;

        private String description;
        public String Description { get { return description; } set { description = value; } }

        private GameObjectOverworld destination;
        public GameObjectOverworld Destination { get { return destination; } set {destination = value; } }

        protected EventTextFormat eventTextFormat;
        protected List<int> endEventIndices = new List<int>();

        protected bool isOnCompletedCalled;

        protected Objective(Game1 game, Mission mission, String description, GameObjectOverworld destination)
        {
            this.game = game;
            this.mission = mission;
            this.description = description;
            this.destination = destination;
        }

        public virtual void OnActivate() 
        {
            mission.ObjectiveDestination = destination;
        }

        public virtual void Initialize()
        {

        }

        public virtual void Reset()
        {
            isOnCompletedCalled = false;
        }

        public virtual void Update()
        {
            if (Failed())
            {
                OnFailed();
            }

            else if (isOnCompletedCalled)
            {
                if (mission.MissionHelper.IsTextCleared())
                {
                    if (!mission.MissionHelper.AllObjectivesCompleted())
                    {
                        mission.ObjectiveIndex++;
                    }
                }
            }

            else if (Completed())
            {
                OnCompleted();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public abstract bool Completed();
        public virtual void OnCompleted() 
        {
            isOnCompletedCalled = true;

            if (endEventIndices.Count > 0)
            {
                if (eventTextFormat.Equals(EventTextFormat.BaseState))
                {
                    mission.MissionHelper.ShowEvent(endEventIndices);
                }
                else if (eventTextFormat.Equals(EventTextFormat.MessageBox))
                {
                    for (int i = 0; i < endEventIndices.Count; i++)
                    {
                        game.messageBox.DisplayMessage(mission.EventArray[i, 0]);
                    }
                }
            }
        }

        public abstract bool Failed();
        public virtual void OnFailed()
        {
            MissionManager.MarkMissionAsFailed(mission.MissionName);
            mission.CurrentObjective = null;
        }
    }
}
