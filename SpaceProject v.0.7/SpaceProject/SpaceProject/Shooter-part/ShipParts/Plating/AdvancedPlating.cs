using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class AdvancedPlating : PlayerPlating
    {
        public AdvancedPlating(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "High quality plating for top-level defence and speed";
        }

        private void Setup()
        {
            Name = "Advanced Plating";

            armor = 750.0f;
            CurrentOverworldHealth = Armor;

            Speed = 0.21f;
            Acceleration = 0.03f;
            PrimarySlots = 2;

            Value = 1300;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(600, 200, 100, 100));
        }
    }
}
