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
        
        public BasicPlating(Game1 Game):
            base(Game)
        {
            Setup();
        }

        public BasicPlating(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "A low quality plating, covering basic needs";
        }

        private void Setup()
        {
            Name = "Basic Plating";

            Armor = 350.0f;
            CurrentOverworldHealth = Armor;

            Speed = 0.17f;
            Acceleration = 0.03f;
            PrimarySlots = 2;

            Value = 200;

            StatsManager.ApplyDifficultyOnPlating(this);
        }
    }
}
