using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class CloseInOnLocationObjective : Objective
    {
        private int radius;

        public CloseInOnLocationObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, int radius) :
            base(game, mission, description, destination)
        {
            this.radius = radius;
        }

        public CloseInOnLocationObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, int radius, EventTextCapsule eventTextCapsule) :
            base(game, mission, description, destination)
        {
            this.radius = radius;

            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;
        }

        private void Setup()
        { }

        public override void OnActivate()
        {
            base.OnActivate();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(PlayTime playTime)
        {
            base.Update(playTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool Completed()
        {
            if (isOnCompletedCalled)
            {
                return true;
            }

            return CollisionDetection.IsPointInsideCircle(game.player.position, Destination.position, radius);
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
