using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class RegularPlating : PlayerPlating
    {
        
        public RegularPlating(Game1 Game):
            base(Game)
        {
            Setup();
        }

        public RegularPlating(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Balanced plating with decent defense and decent agility";
        }

        private void Setup()
        {
            Name = "Regular Plating";

            Armor = 250.0f;
            CurrentOverworldHealth = Armor;

            Speed = 0.2f;
            Acceleration = 0.025f;
            PrimarySlots = 2;

            Value = 300;

            StatsManager.ApplyDifficultyOnPlating(this);
        }
    }
}
