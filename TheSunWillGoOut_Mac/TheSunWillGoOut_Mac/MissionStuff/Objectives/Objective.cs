using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
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

        private System.Action failLogic;

        private String description;
        public String Description { get { return description; } set { description = value; } }

        private GameObjectOverworld destination;
        public GameObjectOverworld Destination { get { return destination; } set {destination = value; } }

        protected EventTextCanvas eventTextCanvas;
        protected EventText objectiveCompletedEventText;
        protected EventText objectiveFailedEventText;

        protected bool isOnCompletedCalled;

        private bool usePortraits;
        private List<PortraitID> portraits;
        private List<int> portraitTriggers;

        protected Objective(Game1 game, Mission mission, String description)
        {
            this.game = game;
            this.mission = mission;
            this.description = description;
            destination = mission.Destinations[mission.Objectives.Count];
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
                    if (usePortraits)
                    {
                        if (portraits != null
                            && portraits.Count > 0)
                        {
                            PopupHandler.DisplayPortraitMessage(portraits, portraitTriggers, objectiveCompletedEventText.Text);
                        }
                        else
                        {
                            PopupHandler.DisplayPortraitMessage(portraits[0], objectiveCompletedEventText.Text);
                        }
                    }
                    else
                    {
                        PopupHandler.DisplayMessage(objectiveCompletedEventText.Text);
                    }
                }
            }
        }

        public void SetFailLogic(System.Action failLogic)
        {
            this.failLogic = failLogic;
        }

        public abstract bool Failed();

        public virtual void OnFailed()
        {
            if (failLogic == null)
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

                MissionManager.MarkMissionAsFailed(mission.MissionID);
                mission.CurrentObjective = null;
            }
            else
            {
                failLogic.Invoke();
            }

            mission.OnFailed();

            if (mission.IsRestartAfterFail())
            {
                MissionManager.ResetMission(mission.MissionID);
            }
        }

        public void SetupPortraits(List<PortraitID> portraits, List<int> portraitTriggers)
        {
            usePortraits = true;
            this.portraits = portraits;
            this.portraitTriggers = portraitTriggers;
        }
    }
}
