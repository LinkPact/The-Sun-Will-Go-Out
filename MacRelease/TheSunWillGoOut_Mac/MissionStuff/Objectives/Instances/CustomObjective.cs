using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class CustomObjective : Objective
    {
        private System.Action activateAction;
        private System.Action updateAction;
        private Func<Boolean> completedCondition;
        private Func<Boolean> failedCondition;

        public CustomObjective(Game1 game, Mission mission, String description, EventTextCapsule eventTextCapsule):
            base(game, mission, description)
        {
            SetupEventText(eventTextCapsule);
            SetupLogic(delegate { }, delegate { },
                delegate { return GameStateManager.currentState.Equals("OverworldState"); },
                delegate { return false; });
        }

        public CustomObjective(Game1 game, Mission mission, String description, System.Action activateLogic,
            System.Action updateLogic, Func<Boolean> completedCondition, Func<Boolean> failedCondition) :
            base(game, mission, description)
        {
            SetupLogic(activateLogic, updateLogic, completedCondition, failedCondition);
        }

        public CustomObjective(Game1 game, Mission mission, String description,
            EventTextCapsule eventTextCapsule, System.Action activateLogic,
            System.Action updateLogic, Func<Boolean> completedCondition, Func<Boolean> failedCondition) :
            base(game, mission, description)
        {
            SetupEventText(eventTextCapsule);
            SetupLogic(activateLogic, updateLogic, completedCondition, failedCondition);
        }

        private void SetupLogic(System.Action activateLogic, System.Action updateLogic,
            Func<Boolean> completedConditions, Func<Boolean> failedCondition)
        {
            this.activateAction = activateLogic;
            this.updateAction = updateLogic;
            this.completedCondition = completedConditions;
            this.failedCondition = failedCondition;
        }

        private void SetupEventText(EventTextCapsule eventTextCapsule)
        {
            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;

            if (eventTextCapsule.Portraits.Count > 0)
            {
                SetupPortraits(eventTextCapsule.Portraits, eventTextCapsule.PortraitTriggers);
            }
        }

        public override void OnActivate()
        {
            base.OnActivate();

            if (activateAction != null)
            {
                activateAction.Invoke();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(PlayTime playTime)
        {
            base.Update(playTime);

            if (updateAction != null)
            {
                updateAction.Invoke();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool Completed()
        {
            if (completedCondition != null)
            {
                return completedCondition.Invoke();
            }

            return false;
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
        }

        public override bool Failed()
        {
            if (failedCondition != null)
            {
                return failedCondition.Invoke();
            }

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
    }
}
