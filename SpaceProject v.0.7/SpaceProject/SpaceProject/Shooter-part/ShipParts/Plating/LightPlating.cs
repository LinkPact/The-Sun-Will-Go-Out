using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class LightPlating : PlayerPlating
    {

        public LightPlating(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public LightPlating(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Thin armor with lower durability but with higher agility";
        }

        private void Setup()
        {
            Name = "Light Plating";
         
            armor = 250.0f;
            CurrentOverworldHealth = Armor;

            Speed = 0.25f;
            Acceleration = 0.04f;
            PrimarySlots = 1;

            Value = 500;
        }

        public override void Initialize()
        { }
    }
}
