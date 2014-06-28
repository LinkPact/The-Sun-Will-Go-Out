using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public abstract class PlayerPlating : ShipPart
    {
        protected Sprite spriteSheet;

        public float Armor;
        public float Speed;
        public float Acceleration;
        public int PrimarySlots;

        public float CurrentOverworldHealth;

        public void LoadHealth(int loadedHealth)
        {
            if (loadedHealth == -1)
                CurrentOverworldHealth = Armor;
            else
                CurrentOverworldHealth = loadedHealth;
        }

        protected PlayerPlating(Game1 Game):
            base(Game)
        {
            Setup();
        }

        protected PlayerPlating(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        private void Setup()
        {
            this.spriteSheet = Game.spriteSheetVerticalShooter;
            Kind = "Plating";
            CurrentOverworldHealth = Armor;
        }

        public virtual void Initialize()
        {
        }

        protected override void SetShipPartVariety(double percent, double quality)
        {
            if (quality < 0) { quality = 0; }

            Armor = (float)quality * Armor;
            Value = (int)(Value * ((float)quality) * ((float)quality));
        }

        protected override List<String> GetInfoText()
        {
            List<String> infoText = new List<String>();

            infoText.Add(Name);
            infoText.Add("Armor: " + Math.Round((double)Armor, 0).ToString() + " units");
            infoText.Add("Speed: " + Math.Round((double)1000 * Speed, 0).ToString() + " units");
            infoText.Add("Size: " + (10 * PrimarySlots).ToString() + " feet");
            infoText.Add("Value: " + Math.Round((double)Value, 0).ToString() + " Rupees");

            infoText.Add("");
            infoText.Add(GetDescription());

            return infoText;
        }

        public override String RetrieveSaveData()
        {
            return "emptyitem";
        }

        public void ApplyDifficulty(GameMode gameMode)
        {
            if (gameMode == GameMode.hardcore)
            {
                Armor *= 0.1f;
                if (Armor > 40)
                    Armor = 40;

            }
            else if (gameMode == GameMode.easy)
            {
                Armor *= 3f;
            }

            CurrentOverworldHealth = Armor;
        }
    }
}
