using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class ArriveAtLocationObjective : Objective
    {
        public ArriveAtLocationObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination) :
            base(game, mission, description, destination)
        { }

        public ArriveAtLocationObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, EventTextCapsule eventTextCapsule) :
            base(game, mission, description, destination)
        {
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

            if (Destination is Planet)
            {
                return mission.MissionHelper.IsPlayerOnPlanet(Destination.name);
            }
            else if (Destination is Station)
            {
                return mission.MissionHelper.IsPlayerOnStation(Destination.name);
            }
            else if (Destination is Battlefield)
            {
                return (CollisionDetection.IsRectInRect(game.player.Bounds, Destination.Bounds) &&
                    (ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)));
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
