using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BasicPlating : PlayerPlating
    {
        public BasicPlating(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "A low quality plating, covering basic needs";
        }

        private void Setup()
        {
            Name = "Basic Plating";

            armor = 350.0f;
            CurrentOverworldHealth = Armor;

            Speed = 0.17f;
            Acceleration = 0.03f;
            PrimarySlots = 2;

            Value = 200;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(600, 100, 100, 100));
        }
    }
}
