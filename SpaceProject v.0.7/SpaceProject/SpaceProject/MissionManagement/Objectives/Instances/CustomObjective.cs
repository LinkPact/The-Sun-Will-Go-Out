using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class CustomObjective : Objective
    {
        private System.Action activateAction;
        private System.Action updateAction;
        private Func<Boolean> completedAction;
        private Func<Boolean> failedAction;

        public CustomObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, EventTextCapsule eventTextCapsule, System.Action onActivate,
            System.Action update, Func<Boolean> completed, Func<Boolean> failed) :
            base(game, mission, description, destination)
        {
            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;

            this.activateAction = onActivate;
            this.updateAction = update;
            this.completedAction = completed;
            this.failedAction = failed;
        }

        private void Setup()
        { }

        public override void OnActivate()
        {
            base.OnActivate();

            activateAction.Invoke();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            base.Update();

            updateAction.Invoke();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool Completed()
        {
            return completedAction.Invoke();
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
        }

        public override bool Failed()
        {
            return failedAction.Invoke();
        }

        public override void OnFailed()
        {
            base.OnFailed();
        }
    }
}
