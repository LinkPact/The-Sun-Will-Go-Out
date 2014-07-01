using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class ArriveAtLocationObjective : Objective
    {
        public ArriveAtLocationObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination) :
            base(game, mission, description, destination)
        { }

        public ArriveAtLocationObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, EventTextCapsule textCapsule) :
            base(game, mission, description, destination)
        {
            foreach (String s in textCapsule.CompletedText)
            {
                this.objectiveCompletedEventText.Add(s);
            }

            this.eventTextCanvas = textCapsule.EventTextCanvas;
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

        public override void Update()
        {
            base.Update();
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

            if (Destination is Planet)
            {
                return mission.MissionHelper.IsPlayerOnPlanet(Destination.name);
            }
            else if (Destination is Station)
            {
                return mission.MissionHelper.IsPlayerOnStation(Destination.name);
            }
            else
            {
                return CollisionDetection.IsRectInRect(game.player.Bounds, Destination.Bounds);
            }
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
