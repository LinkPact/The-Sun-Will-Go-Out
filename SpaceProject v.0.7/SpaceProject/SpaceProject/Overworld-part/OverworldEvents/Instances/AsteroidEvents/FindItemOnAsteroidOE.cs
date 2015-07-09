using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class FindItemOnAsteroidOE : OverworldEvent
    {
        private Game1 Game;
        private String clearText;
        private GetItemOE getItemOE;

        private List<ShipPartType> beginningItems = new List<ShipPartType>
        { 
            ShipPartType.BasicLaser,
            ShipPartType.DualLaser,
            ShipPartType.SpreadBullet,
            ShipPartType.LongShot,
            ShipPartType.BasicPlating,
            ShipPartType.BasicEnergyCell,
            ShipPartType.BasicShield
        };

        private List<ShipPartType> alliancePhaseItems = new List<ShipPartType>
        { 
            ShipPartType.MultipleShot,
            ShipPartType.WaveBeam,
            ShipPartType.BallisticLaser,
            ShipPartType.HomingMissile,
            ShipPartType.FieldDamage,
            ShipPartType.RegularPlating,
            ShipPartType.RegularEnergyCell,
            ShipPartType.RegularShield
        };

        private List<ShipPartType> rebelPhaseItems = new List<ShipPartType>
        { 
            ShipPartType.Beam,
            ShipPartType.FragmentMissile,
            ShipPartType.Burster,
            ShipPartType.SideMissiles,
            ShipPartType.AdvancedPlating,
            ShipPartType.AdvancedEnergyCell,
            ShipPartType.AdvancedShield,
            ShipPartType.FlameShot
        };

        private List<ShipPartType> endingItems = new List<ShipPartType>
        { 
            ShipPartType.Beam,
            ShipPartType.FragmentMissile,
            ShipPartType.Burster,
            ShipPartType.SideMissiles,
            ShipPartType.AdvancedPlating,
            ShipPartType.AdvancedEnergyCell,
            ShipPartType.AdvancedShield,
            ShipPartType.ProximityLaser,
            ShipPartType.AdvancedLaser,
            ShipPartType.DualLaser
        };

        public FindItemOnAsteroidOE(Game1 Game) :
            base()
        {
            this.Game = Game;
            this.clearText = "An empty asteroid floating in space.";
        }

        public override Boolean Activate()
        {
            var eventTextList = new List<String>();
            Boolean successfullyActivated = false;

            if (!IsCleared())
            {
                var item = GetProgressBasedRandomItem(Game);
                if (getItemOE == null)
                {
                    getItemOE = new GetItemOE(item, string.Format("You found the {0}!", item.Name), "Your inventory is full!", "Cleared (is this shown?)");                
                }
                successfullyActivated = getItemOE.Activate();

                if (successfullyActivated)
                {
                    ClearEvent();
                }
            }
            else
            {
                eventTextList.Add(clearText);
                PopupHandler.DisplayMessage(eventTextList.ToArray());
            }

            return successfullyActivated;
        }

        private Item GetProgressBasedRandomItem(Game1 Game)
        {
            var currentPhase = MissionManager.GetCurrentGamePhase();

            ShipPartType progressRandomShipPart;
            List<ShipPartType> pickingPool = new List<ShipPartType>();
            pickingPool.AddRange(beginningItems);
            
            switch (currentPhase)
            {
                case GamePhase.beginning:
                    {
                        progressRandomShipPart = MathFunctions.PickRandomFromList(pickingPool);
                        break;
                    }
                case GamePhase.withAlliance:
                    {
                        pickingPool.AddRange(alliancePhaseItems);
                        progressRandomShipPart = MathFunctions.PickRandomFromList(pickingPool);
                        break;
                    }
                case GamePhase.withRebels:
                    {
                        pickingPool.AddRange(alliancePhaseItems);
                        pickingPool.AddRange(rebelPhaseItems);
                        progressRandomShipPart = MathFunctions.PickRandomFromList(pickingPool);
                        break;
                    }
                case GamePhase.ending:
                    {
                        pickingPool.AddRange(alliancePhaseItems);
                        pickingPool.AddRange(rebelPhaseItems);
                        pickingPool.AddRange(endingItems);
                        progressRandomShipPart = MathFunctions.PickRandomFromList(pickingPool);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(string.Format("Unknown argument given: {0}", currentPhase.ToString()));
                    }
            }

            ItemVariety variety = GetRandomVariety();
            return Shop.RetrievePartFromEnum(progressRandomShipPart, Game, variety);
        }

        private ItemVariety GetRandomVariety()
        {
            int randVal = MathFunctions.GetExternalRandomInt(0, 100);

            if (randVal < 60)
            {
                return ItemVariety.low;
            }
            else if (randVal < 90)
            {
                return ItemVariety.regular;
            }
            else
            {
                return ItemVariety.high;
            }
        }
    }
}
