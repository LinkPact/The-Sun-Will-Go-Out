using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public abstract class PlayerEnergyCell : ShipPart
    {
        protected Sprite spriteSheet;

        public float Capacity;
        public float Recharge;

        protected PlayerEnergyCell(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            this.spriteSheet = Game.spriteSheetVerticalShooter;
        }

        protected PlayerEnergyCell(Game1 Game) :
            base(Game)
        {
            this.spriteSheet = Game.spriteSheetVerticalShooter;
        }

        public virtual void Initialize()
        { }

        protected override void SetShipPartVariety(double percent, double quality)
        {
            if (quality < 0) { quality = 0; }

            Capacity = (float)quality * Capacity;
            Recharge = (float)quality * Recharge;

            Value = (int)(Value * ((float)quality) * ((float)quality));
        }

        protected override List<String> GetInfoText()
        {
            List<String> infoText = new List<String>();

            infoText.Add(Name);
            infoText.Add("Capacity: " + Math.Round((double)Capacity, 0).ToString() + " energy");
            infoText.Add("Recharge rate: " + Math.Round((double)Recharge, 1).ToString() + " energy/second");
            infoText.Add("Value: " + Math.Round((double)Value, 0).ToString() + " Rupees");
            infoText.Add("Quality: " + Variety.ToString());

            infoText.Add("");
            infoText.Add(GetDescription());

            return infoText;
        }

        public override String RetrieveSaveData()
        {
            return "emptyitem";
        }
    }
}
