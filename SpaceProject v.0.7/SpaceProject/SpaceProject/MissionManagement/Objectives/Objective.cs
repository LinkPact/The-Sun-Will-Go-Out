using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum EventTextCanvas
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

        protected EventTextCanvas eventTextCanvas;
        protected List<String> objectiveCompletedEventText = new List<String>();
        protected List<String> objectiveFailedEventText = new List<String>();

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
            if (isOnCompletedCalled)
            {
                if (mission.MissionHelper.IsTextCleared())
                {
                    if (!mission.MissionHelper.AllObjectivesCompleted())
                    {
                        mission.ObjectiveIndex++;
                    }
                }
            }

            else if (Failed())
            {
                OnFailed();
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

            if (objectiveCompletedEventText.Count > 0
                && objectiveCompletedEventText[0] != "")
            {
                if (eventTextCanvas.Equals(EventTextCanvas.BaseState))
                {
                    mission.MissionHelper.ShowEvent(objectiveCompletedEventText);
                }
                else if (eventTextCanvas.Equals(EventTextCanvas.MessageBox))
                {
                    game.messageBox.DisplayMessage(objectiveCompletedEventText);
                }
            }
        }

        public abstract bool Failed();
        public virtual void OnFailed()
        {
            if (objectiveFailedEventText.Count > 0
                && objectiveFailedEventText[0] != "")
            {
                if (eventTextCanvas.Equals(EventTextCanvas.BaseState))
                {
                    mission.MissionHelper.ShowEvent(objectiveFailedEventText);
                }
                else if (eventTextCanvas.Equals(EventTextCanvas.MessageBox))
                {
                    game.messageBox.DisplayMessage(objectiveFailedEventText);
                }
            }

            MissionManager.MarkMissionAsFailed(mission.MissionName);
            mission.CurrentObjective = null;
        }
    }
}
