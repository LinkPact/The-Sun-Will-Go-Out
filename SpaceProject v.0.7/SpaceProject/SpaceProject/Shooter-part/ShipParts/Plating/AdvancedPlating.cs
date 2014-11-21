﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class AdvancedPlating : PlayerPlating
    {
        
        public AdvancedPlating(Game1 Game):
            base(Game)
        {
            Setup();
        }

        public AdvancedPlating(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "High quality plating for top-level defence";
        }

        private void Setup()
        {
            Name = "Advanced Plating";

            Armor = 1000.0f;
            CurrentOverworldHealth = Armor;

            Speed = 0.17f;
            Acceleration = 0.03f;
            PrimarySlots = 2;

            Value = 2000;

            StatsManager.ApplyDifficultyOnPlating(this);
        }
    }
}
