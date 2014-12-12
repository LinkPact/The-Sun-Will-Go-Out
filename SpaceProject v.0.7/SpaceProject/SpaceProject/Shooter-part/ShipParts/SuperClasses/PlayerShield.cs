using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public abstract class PlayerShield : ShipPart
    {
        protected Sprite spriteSheet;

        public float Capacity;
        public float ConversionFactor;
        public float Regeneration;

        protected float collisionDamageFactor;
        protected float bulletDamageFactor;
        
        protected PlayerShield(Game1 Game):
            base(Game)
        {
            this.spriteSheet = Game.spriteSheetVerticalShooter;
            SetDefaultValues();
        }

        protected PlayerShield(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            this.spriteSheet = Game.spriteSheetVerticalShooter;
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            collisionDamageFactor = 1;
            bulletDamageFactor = 1;
        }

        protected override void SetShipPartVariety(double percent, double quality)
        {
            if (quality < 0) throw new ArgumentException("Quality can never be negative!");

            Capacity = (float)quality * Capacity;
            ConversionFactor = (float)(1/quality) * ConversionFactor;
            Regeneration = (float)quality * Regeneration;

            Value = (int)(Value * ((float)quality) * ((float)quality));
        }

        protected override List<String> GetInfoText()
        {
            List<String> infoText = new List<String>();

            infoText.Add(Name);
            infoText.Add("Capacity: " + Math.Round((double)Capacity, 0).ToString() + " units");
            infoText.Add("Regeneration: " + Math.Round((double)Regeneration, 1).ToString() + " units/second");
            //infoText.Add("Conversion factor: " + Math.Round((double)ConversionFactor, 1).ToString() + " energy/unit");
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

        public float GetShieldDamage(GameObjectVertical obj)
        {
            if (obj is EnemyBullet)
            {
                return obj.Damage * bulletDamageFactor;
            }
            else if (obj is EnemyShip)
            {
                return obj.Damage * collisionDamageFactor;
            }
            else
            {
                return obj.Damage;
            }
        }
    }
}
