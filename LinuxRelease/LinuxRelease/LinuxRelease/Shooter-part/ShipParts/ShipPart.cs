using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum  ItemVariety
{
    None,
    Low,
    Regular,
    High,
    Random,
}

namespace SpaceProject
{
    public abstract class ShipPart : Item
    {
        //General constants, perhaps temporarely located here
        public static float lowItemSpreadFactor = 0.1f;
        public static float regularItemSpreadFactor = 0.2f;
        public static float highItemSpreadFactor = 0.3f;
        public static float veryHighItemSpreadFactor = 0.4f;

        public static float poorQuality = 0.7f;
        public static float lowQuality = 0.8f;
        public static float regularQuality = 1;
        public static float goodQuality = 1.2f;
        public static float greatQuality = 1.3f;

        private static float greatQualThres = 0.05f;
        private static float goodQualThres = 0.25f;
        private static float regQualThres = 0.75f;
        //private static float lowQualThres = 1.00f;

        private ItemVariety variety;
        public ItemVariety Variety { get { return variety; } }

        private Boolean isVarietySet = false;

        public Boolean needExternalRandomization = false;

        protected ShipPart(Game1 Game, ItemVariety variety)
            : base(Game)
        {
            this.variety = variety;
        }

        public virtual ItemVariety GetShipPartVariety()
        {
            return variety;
        }

        protected abstract void SetShipPartVariety(double variation, double quality);

        public void SetShipPartVariety(ItemVariety new_variety = ItemVariety.None)
        {
            // Hack-around here to not break the parts of the code that directly
            // applies item variety to the ship part. I hope to rebuild the system
            // further later on so that variety only is set in the constructor. // Jakob 150319

            if (new_variety != ItemVariety.None)
            {
                this.variety = new_variety;
            }

            if (!isVarietySet)
            {
                isVarietySet = true;
            }
            else
            {
                return;
            }

            switch (variety)
            { 
                case ItemVariety.Regular:
                    {
                        SetShipPartVariety(ShipPart.regularItemSpreadFactor, ShipPart.regularQuality);
                        break;
                    }
                case ItemVariety.Low:
                    {
                        SetShipPartVariety(ShipPart.regularItemSpreadFactor, ShipPart.lowQuality);
                        break;
                    }
                case ItemVariety.High:
                    {
                        SetShipPartVariety(ShipPart.regularItemSpreadFactor, ShipPart.greatQuality);
                        break;
                    }
                case ItemVariety.Random:
                    {
                        // Recursive call with the obtained proper quality
                        ItemVariety var = GetRandomVariety();
                        SetShipPartVariety(var);
                        break;
                    }
                case ItemVariety.None:
                    {
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Not implemented!");
                    }
            }
        }

        public void SetWorkingRandom(Random externalRandom)
        {
            this.random = externalRandom;
        }

        private ItemVariety GetRandomVariety()
        {
            double randNbr = MathFunctions.GetExternalRandomDouble();

            if (randNbr < goodQualThres)
                return ItemVariety.High;
            else if (randNbr < regQualThres)
                return ItemVariety.Regular;
            else //if (randNbr < regQualThres)
                return ItemVariety.Low;
        }

        private double GetQualityNumber()
        {
            double randNbr = MathFunctions.GetExternalRandomDouble();

            if (randNbr < greatQualThres)
                return greatQuality;
            else if (randNbr < goodQualThres)
                return goodQuality;
            else if (randNbr < regQualThres)
                return regularQuality;
            else
                return lowQuality;
        }
    }
}
