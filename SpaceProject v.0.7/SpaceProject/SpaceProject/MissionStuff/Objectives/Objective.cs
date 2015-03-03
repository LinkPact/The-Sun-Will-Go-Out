using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

        private Vector2 destinationVector;
        public Vector2 DestinationVector { get { return destinationVector; } set { destinationVector = value; } }

        protected EventTextCanvas eventTextCanvas;
        protected EventText objectiveCompletedEventText;
        protected EventText objectiveFailedEventText;

        protected bool isOnCompletedCalled;

        protected Objective(Game1 game, Mission mission, String description, GameObjectOverworld destination)
        {
            this.game = game;
            this.mission = mission;
            this.description = description;
            this.destination = destination;
        }

        public virtual void Initialize() { }

        public virtual void OnMissionStart() { }

        // Called when this objective begins
        public virtual void OnActivate() 
        {
            mission.ObjectiveDestination = destination;
        }

        public virtual void Reset()
        {
            isOnCompletedCalled = false;
        }

        public virtual void Update(PlayTime playTime)
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

            if (objectiveCompletedEventText != null && objectiveCompletedEventText.Text != "") 
            {
                if (eventTextCanvas.Equals(EventTextCanvas.BaseState))
                {
                    mission.MissionHelper.ShowEvent(objectiveCompletedEventText);
                }
                else if (eventTextCanvas.Equals(EventTextCanvas.MessageBox))
                {
                    PopupHandler.DisplayMessage(objectiveCompletedEventText.Text);
                }
            }
        }

        public abstract bool Failed();
        public virtual void OnFailed()
        {
            if (objectiveFailedEventText != null && objectiveFailedEventText.Text != "")
            {
                if (eventTextCanvas.Equals(EventTextCanvas.BaseState))
                {
                    mission.MissionHelper.ShowEvent(objectiveFailedEventText);
                }
                else if (eventTextCanvas.Equals(EventTextCanvas.MessageBox))
                {
                    PopupHandler.DisplayMessage(objectiveFailedEventText.Text);
                }
            }

            MissionManager.MarkMissionAsFailed(mission.MissionName);
            mission.CurrentObjective = null;
        }
    }
}
