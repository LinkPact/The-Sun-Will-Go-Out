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
        public ArriveAtLocationObjective(Game1 game, Mission mission, String description) :
            base(game, mission, description)
        { }

        public ArriveAtLocationObjective(Game1 game, Mission mission, String description, EventTextCapsule eventTextCapsule) :
            base(game, mission, description)
        {
            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;

            if (eventTextCapsule.Portraits.Count > 0)
            {
                SetupPortraits(eventTextCapsule.Portraits, eventTextCapsule.PortraitTriggers);
            }
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
            else if (Destination is SubInteractiveObject)
            {
                return (CollisionDetection.IsRectInRect(game.player.Bounds, Destination.Bounds) &&
                    (ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter))
                    && !PopupHandler.IsMenuOpen);
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
