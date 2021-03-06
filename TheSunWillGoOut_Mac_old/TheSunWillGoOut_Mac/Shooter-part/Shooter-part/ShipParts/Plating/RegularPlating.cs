﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class RegularPlating : PlayerPlating
    {
        public RegularPlating(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Mid-range plating offering decent defense and speed";
        }

        private void Setup()
        {
            Name = "Regular Plating";

            armor = 550.0f;
            CurrentOverworldHealth = Armor;

            Speed = 0.19f;
            Acceleration = 0.03f;
            PrimarySlots = 2;

            Value = 600;
            Tier = TierType.Good;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(900, 100, 100, 100));
        }
    }
}
