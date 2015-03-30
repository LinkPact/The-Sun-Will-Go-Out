using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class ItemTransportObjective : Objective
    {
        private Item item;

        public ItemTransportObjective(Game1 game, Mission mission, String description, Item item) :
            base(game, mission, description)
        {
            Setup(item);
        }

        public ItemTransportObjective(Game1 game, Mission mission, String description,
            Item item, EventTextCapsule eventTextCapsule) :
            base(game, mission, description)
        {
            Setup(item);

            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveFailedEventText = eventTextCapsule.FailedText;
        }

        private void Setup(Item item)
        {
            this.item = item;
        }

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
                return (ShipInventoryManager.ShipItems.Contains(item) 
                    && mission.MissionHelper.IsPlayerOnPlanet(Destination.name));
            }
            else if (Destination is Station)
            {
                return (ShipInventoryManager.ShipItems.Contains(item) 
                    && mission.MissionHelper.IsPlayerOnStation(Destination.name));
            }
            else
            {
                return (ShipInventoryManager.ShipItems.Contains(item) 
                    && CollisionDetection.IsRectInRect(game.player.Bounds, Destination.Bounds));
            }
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            ShipInventoryManager.RemoveItem(item);
        }

        public override bool Failed()
        {
            if (Destination is Planet 
                && mission.MissionHelper.IsPlayerOnPlanet(Destination.name))
            {
                return !ShipInventoryManager.ShipItems.Contains(item);
            }
            else if (Destination is Station
                && mission.MissionHelper.IsPlayerOnStation(Destination.name))
            {
                return !ShipInventoryManager.ShipItems.Contains(item);
            }
            else if (CollisionDetection.IsRectInRect(game.player.Bounds, Destination.Bounds))
            {
                return !ShipInventoryManager.ShipItems.Contains(item);
            }

            return false;
        }

        public override void OnFailed()
        {
            base.OnFailed();
        }
    }
}
