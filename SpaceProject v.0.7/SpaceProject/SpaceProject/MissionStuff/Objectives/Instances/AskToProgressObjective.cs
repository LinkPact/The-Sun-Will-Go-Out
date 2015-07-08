using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class AskToProgressObjective : Objective
    {
        private bool progressObjective;
        private bool askQuestion = true;

        private EventText question;
        private EventText yesEvent;
        private EventText noEvent;

        public AskToProgressObjective(Game1 game, Mission mission, String description, EventText question,
            EventText yesEvent, EventText noEvent):
            base(game, mission, description)
        {
            this.question = question;
            this.yesEvent = yesEvent;
            this.noEvent = noEvent;
        }

        public override void OnActivate()
        {
            base.OnActivate();
        }

        public override void Update(PlayTime playTime)
        {
            base.Update(playTime);

            if (askQuestion && GameStateManager.currentState.Equals("StationState"))
            {
                askQuestion = false;

                PopupHandler.DisplaySelectionMenu(question.Text, new List<String>() { "Yes", "No" },
                new List<System.Action>()
                {
                    delegate 
                    {
                        progressObjective = true;
                        mission.MissionHelper.ShowEvent(yesEvent);
                        game.stateManager.stationState.OnEnter();
                    },
                    delegate 
                    {
                        mission.MissionHelper.ShowEvent(noEvent);
                        game.stateManager.stationState.OnEnter();
                    }
                });
            }

            else if (GameStateManager.currentState.Equals("OverworldState"))
            {
                askQuestion = true;
            }
        }

        public override bool Completed()
        {
            return progressObjective;
        }

        public override bool Failed()
        {
            return false;
        }
    }
}
