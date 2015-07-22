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
        public BasicPlating(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "A low quality plating providing basic defense and speed";
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
            Tier = TierType.Average;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(600, 100, 100, 100));
        }
    }
}
