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
                return (PlayerHasItem() && mission.MissionHelper.IsPlayerOnPlanet(Destination.name));
            }
            else if (Destination is Station)
            {
                return (PlayerHasItem() && mission.MissionHelper.IsPlayerOnStation(Destination.name));
            }
            else
            {
                return (PlayerHasItem() && CollisionDetection.IsRectInRect(game.player.Bounds, Destination.Bounds));
            }
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            RemoveItemFromInventory();
        }

        public override bool Failed()
        {
            if (Destination is Planet 
                && mission.MissionHelper.IsPlayerOnPlanet(Destination.name))
            {
                return !PlayerHasItem();
            }
            else if (Destination is Station
                && mission.MissionHelper.IsPlayerOnStation(Destination.name))
            {
                return !PlayerHasItem();
            }
            else if (CollisionDetection.IsRectInRect(game.player.Bounds, Destination.Bounds))
            {
                return !PlayerHasItem();
            }

            return false;
        }

        public override void OnFailed()
        {
            base.OnFailed();
        }

        private bool PlayerHasItem()
        {
            foreach (Item i in ShipInventoryManager.ShipItems)
            {
                if (i.GetType().Equals(item.GetType()))
                {
                    return true;
                }
            }

            return false;
        }

        private void RemoveItemFromInventory()
        {
            foreach (Item i in ShipInventoryManager.ShipItems)
            {
                if (i.GetType().Equals(item.GetType()))
                {
                    ShipInventoryManager.RemoveItem(i);
                    break;
                }
            }
        }
    }
}
