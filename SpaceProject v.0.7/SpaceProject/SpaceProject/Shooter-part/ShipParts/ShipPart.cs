using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum  ItemVariety
{
    regular,
    low,
    high,
    random,
    none
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

        public static float poorQuality = 0.6f;
        public static float lowQuality = 0.8f;
        public static float regularQuality = 1;
        public static float goodQuality = 1.2f;
        public static float greatQuality = 1.4f;

        private static float greatQualThres = 0.05f;
        private static float goodQualThres = 0.25f;
        private static float regQualThres = 0.75f;
        //private static float lowQualThres = 1.00f;

        private ItemVariety variety;
        public ItemVariety Variety { get { return variety; } }

        public Boolean needExternalRandomization = false;

        protected ShipPart(Game1 Game) 
            : base(Game)
        {
            variety = ItemVariety.none;
        }

        protected ShipPart(Game1 Game, ItemVariety variety)
            : base(Game)
        {
            SetShipPartVariety(variety);
        }

        public virtual ItemVariety GetShipPartVariety()
        {
            return variety;
        }

        protected abstract void SetShipPartVariety(double variation, double quality);

        public void SetShipPartVariety(ItemVariety variety)
        {
            this.variety = variety;

            switch (variety)
            { 
                case ItemVariety.regular:
                    {
                        SetShipPartVariety(ShipPart.regularItemSpreadFactor, ShipPart.regularQuality);
                        break;
                    }
                case ItemVariety.low:
                    {
                        SetShipPartVariety(ShipPart.regularItemSpreadFactor, ShipPart.lowQuality);
                        break;
                    }
                case ItemVariety.high:
                    {
                        SetShipPartVariety(ShipPart.regularItemSpreadFactor, ShipPart.goodQuality);
                        break;
                    }
                case ItemVariety.random:
                    {
                        double quality = GetQualityNumber();
                        SetShipPartVariety(ShipPart.regularItemSpreadFactor, quality);
                        break;
                    }
                case ItemVariety.none:
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

        private double GetQualityNumber()
        {
            double randNbr = GlobalMathFunctions.GetExternalRandomDouble();

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
