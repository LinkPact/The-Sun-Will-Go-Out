using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    /*
     * Contains logic related to shops which are found in planets and stations.
     * Contains list describing their content.
     * 
     * Key parts
     * shopInventoryEntries - A list of entries which acts as "blue print" for the shop. 
     *      The entries describes which parts that are present in the shop, and how common they are.
     * 
     * shopInventory - A list with the currently present items in the shop inventory.
     */
    class Shop
    {
        private Game1 Game;
        private String identifyer;
        private Boolean inventoryIsFixed = true;

        protected List<Item> shopInventory;
        public List<Item> ShopInventory { get { return shopInventory; } set { shopInventory = value; } }

        protected List<Item> onEnterShopInventory;
        public List<Item> OnEnterShopInventory { get { return onEnterShopInventory; } set { onEnterShopInventory = value; } }

        protected List<Item> itemPool;
        public List<Item> ItemPool { get { return itemPool; } set { itemPool = value; } }

        protected List<ShopInventoryEntry> shopInventoryEntries;
        public void AddShopEntry(ShopInventoryEntry entry)
        {
            shopInventoryEntries.Add(entry);
        }

        protected List<ShopInventoryEntry> mandatoryShipItems = new List<ShopInventoryEntry>();
        public void AddMandatoryShipItem(ShopInventoryEntry mandatoryItem)
        {
            mandatoryShipItems.Add(mandatoryItem);
        }

        protected ShopFilling shopFilling;
        public ShopFilling ShopFilling { get { return shopFilling; } 
            set { 
                shopFilling = value;
                InventoryItemSetup(shopFilling);
            } 
        }

        public Shop(Game1 Game, String identifyer)
        {
            this.Game = Game;
            this.identifyer = identifyer;

            shopInventoryEntries = new List<ShopInventoryEntry>();
            shopInventory = new List<Item>();
            onEnterShopInventory = new List<Item>();
            itemPool = new List<Item>();

            // Set to none if not updated in Initialize-method
            shopFilling = ShopFilling.none;
        }

        private void CompressShopInventory()
        {
            List<int> removeList = new List<int>();
            for (int n = 0; n < shopInventory.Count; n++)
            {
                if (shopInventory[n].Kind == "Empty")
                    removeList.Add(n);
            }
            for (int n = removeList.Count; n > 0; n--)
            {
                shopInventory.RemoveAt(removeList[n - 1]); ;
                shopInventory.Add(new EmptyItem(Game));
            }
        }

        private void FillShopInventoryWithEmptyItem()
        {
            if (shopInventory.Count < 14)
            {
                int tempCount = shopInventory.Count;
                for (int i = 0; i < 14 - tempCount - 1; i++)
                    shopInventory.Add(new EmptyItem(Game));
            }
        }

        private int GetCurrentShipPartCount()
        {
            int count = 0;
            foreach (Item item in shopInventory)
            {
                if (!(item is EmptyItem))
                {
                    count++;
                }
            }
            return count;
        }

        public void UpdateShopInventory()
        {
            if (shopFilling == ShopFilling.none)
                throw new ArgumentException("Due to bad programming this can be called when shopfilling is none, but it shouldn't be");

            if (inventoryIsFixed)
            {
                return;
            }

            // Logic for controlling shopinventory-numbers
            int removeCount = Game.random.Next(0, 2);
            int addCount = Game.random.Next(0, 2);

            int currentItemCount = GetCurrentShipPartCount();
            int lowerThres = GetLowerFillLimit(shopFilling);
            int upperThres = GetUpperFillLimit(shopFilling);
            int preliminaryFinalCount = currentItemCount - removeCount + addCount;

            if (preliminaryFinalCount < lowerThres)
            {
                addCount += (lowerThres - preliminaryFinalCount);
            }
            else if (preliminaryFinalCount > upperThres)
            {
                removeCount += (preliminaryFinalCount - upperThres);
            }

            for (int n = 0; n < removeCount; n++)
            {
                RandomShipPartRemoval();
            }

            for (int n = 0; n < addCount; n++)
            {
                RandomShipPartAddition();
            }

            CompressShopInventory();
        }

        private void RandomShipPartRemoval()
        {
            int iterations = 0;
            Boolean foundTarget = false;

            while (!foundTarget)
            {
                int pos = Game.random.Next(shopInventory.Count);
                if (!(shopInventory[pos] is EmptyItem))
                {
                    RemovePartAt(pos);
                    foundTarget = true;
                }

                iterations++;
                if (iterations > 1000)
                {
                    throw new ArgumentOutOfRangeException("Something is wrong in this loop");
                }
            }
        }

        private void RandomShipPartAddition()
        {
            int iterations = 0;
            Boolean foundTarget = false;

            while (!foundTarget)
            {
                int pos = Game.random.Next(shopInventory.Count);
                if (shopInventory[pos] is EmptyItem)
                {
                    ShipPart part = ExtractShipPartFromItemFrequencies();
                    InsertPartAt(part, pos);
                    foundTarget = true;
                }

                iterations++;
                if (iterations > 1000)
                {
                    throw new ArgumentOutOfRangeException("You are attempting to add an item to a full shop inventory");
                }
            }
        }

        protected void InventoryItemSetup(ShopFilling filling)
        {
            FillShopInventoryWithEmptyItem();

            int items = Game.random.Next(GetLowerFillLimit(filling), GetUpperFillLimit(filling));

            for (int n = 0; n < items; n++)
            {
                if (n < mandatoryShipItems.Count)
                {
                    ShopInventoryEntry entry = mandatoryShipItems[n];

                    ShipPart part = RetrievePartFromEnum(entry.ShipPartType, Game);
                    part.SetShipPartVariety(entry.ItemVariety);
                    InsertPartAt(part, n);
                }
                else if (!inventoryIsFixed)
                {
                    ShipPart part = ExtractShipPartFromItemFrequencies();
                    InsertPartAt(part, n);
                }
            }
        }

        private void InsertPartAt(ShipPart part, int index)
        {
            shopInventory.RemoveAt(index);
            shopInventory.Insert(index, part);
            CompressShopInventory();
        }

        private void RemovePartAt(int index)
        {
            shopInventory.RemoveAt(index);
            shopInventory.Insert(index, new EmptyItem(Game));
            CompressShopInventory();
        }

        private ShipPart ExtractShipPartFromItemFrequencies()
        {
            ShipPart part = null;

            int summedItemFrequency = 0;

            // Stores an accumulated value for each ship part
            // The size of the ship parts range to previous value determines its probability for creation
            SortedDictionary<String, int> probabilityTable = new SortedDictionary<String, int>();

            foreach (ShopInventoryEntry entry in shopInventoryEntries)
            {
                switch (entry.Availability)
                {
                    case ShipPartAvailability.rare:
                        {
                            summedItemFrequency += 1;
                            probabilityTable.Add(entry.GetId(), summedItemFrequency);
                            break;
                        }
                    case ShipPartAvailability.uncommon:
                        {
                            summedItemFrequency += 2;
                            probabilityTable.Add(entry.GetId(), summedItemFrequency);
                            break;
                        }
                    case ShipPartAvailability.common:
                        {
                            summedItemFrequency += 3;
                            probabilityTable.Add(entry.GetId(), summedItemFrequency);
                            break;
                        }
                    case ShipPartAvailability.ubiquitous:
                        {
                            summedItemFrequency += 4;
                            probabilityTable.Add(entry.GetId(), summedItemFrequency);
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException("Not implemented!");
                        }
                }
            }

            int randItemValue = Game.random.Next(summedItemFrequency);

            foreach (KeyValuePair<String, int> pair in probabilityTable.OrderBy(key => key.Value))
            {
                if (pair.Value > randItemValue)
                {
                    ShopInventoryEntry entry = new ShopInventoryEntry(pair.Key);

                    part = RetrievePartFromEnum(entry.ShipPartType, Game);
                    part.SetShipPartVariety(entry.ItemVariety);
                    break;
                }
            }

            return part;
        }

        public static ShipPart RetrievePartFromEnum(ShipPartType shipPartEnum, Game1 Game, ItemVariety variety=ItemVariety.regular)
        {
            switch (shipPartEnum)
            {
                case ShipPartType.LongShot:                 return new LongShotWeapon(Game, variety);
                case ShipPartType.BasicLaser:               return new BasicLaserWeapon(Game, variety);
                case ShipPartType.DualLaser:                return new DualLaserWeapon(Game, variety);
                case ShipPartType.SpreadBullet:             return new SpreadBulletWeapon(Game, variety);
                case ShipPartType.Beam:                     return new BeamWeapon(Game, variety);
                case ShipPartType.MultipleShot:             return new MultipleShotWeapon(Game, variety);
                case ShipPartType.WaveBeam:                 return new WaveBeamWeapon(Game, variety);
                case ShipPartType.BallisticLaser:           return new BallisticLaserWeapon(Game, variety);
                case ShipPartType.FragmentMissile:          return new FragmentMissileWeapon(Game, variety);
                case ShipPartType.Burster:                  return new BursterWeapon(Game, variety);
                case ShipPartType.AdvancedLaser:            return new AdvancedLaserWeapon(Game, variety);
                case ShipPartType.ProximityLaser:           return new ProximityLaserWeapon(Game, variety);
                case ShipPartType.AdvancedBeam:             return new AdvancedBeamWeapon(Game, variety);
                case ShipPartType.FlameShot:                return new FlameShotWeapon(Game, variety);

                case ShipPartType.Turret:                   return new TurretWeapon(Game, variety);
                case ShipPartType.FieldDamage:              return new FieldDamageWeapon(Game, variety);
                case ShipPartType.SideMissiles:             return new SideMissilesWeapon(Game, variety);
                case ShipPartType.HomingMissile:            return new HomingMissileWeapon(Game, variety);
                case ShipPartType.Disruptor:                return new DisruptorWeapon(Game, variety);
                case ShipPartType.MineLayer:                return new MineLayerWeapon(Game, variety);
                case ShipPartType.RegularBomb:              return new RegularBombWeapon(Game, variety);
                case ShipPartType.PunyTurret:               return new PunyTurretWeapon(Game, variety);

                case ShipPartType.BasicPlating:             return new BasicPlating(Game, variety);
                case ShipPartType.RegularPlating:           return new RegularPlating(Game, variety);
                case ShipPartType.AdvancedPlating:          return new AdvancedPlating(Game, variety);
                case ShipPartType.HeavyPlating:             return new HeavyPlating(Game, variety);
                case ShipPartType.LightPlating:             return new LightPlating(Game, variety);

                case ShipPartType.BasicEnergyCell:          return new BasicEnergyCell(Game, variety);
                case ShipPartType.RegularEnergyCell:        return new RegularEnergyCell(Game, variety);
                case ShipPartType.AdvancedEnergyCell:       return new AdvancedEnergyCell(Game, variety);
                case ShipPartType.WeaponBoostEnergyCell:    return new WeaponBoostEnergyCell(Game, variety);
                case ShipPartType.ShieldBoostEnergyCell:    return new ShieldBoostEnergyCell(Game, variety);

                case ShipPartType.BasicShield:              return new BasicShield(Game, variety);
                case ShipPartType.RegularShield:            return new RegularShield(Game, variety);
                case ShipPartType.AdvancedShield:           return new AdvancedShield(Game, variety);
                case ShipPartType.CollisionShield:          return new CollisionShield(Game, variety);
                case ShipPartType.BulletShield:             return new BulletShield(Game, variety);

                default:
                        throw new ArgumentException("Undefined enum value! Check enum in DataEnums-class");
            }
        }

        private int GetLowerFillLimit(ShopFilling filling)
        {
            switch (filling)
            {
                case ShopFilling.verySparse:
                    {
                        return 1;
                    }
                case ShopFilling.sparse:
                    {
                        return 3;
                    }
                case ShopFilling.regular:
                    {
                        return 5;
                    }
                case ShopFilling.filled:
                    {
                        return 7;
                    }
                case ShopFilling.veryFilled:
                    {
                        return 9;
                    }
                default:
                    {
                        throw new ArgumentException("Not implemented!");
                    }
            }
        }

        private int GetUpperFillLimit(ShopFilling filling)
        {
            switch (filling)
            {
                case ShopFilling.verySparse:
                    {
                        return 3;
                    }
                case ShopFilling.sparse:
                    {
                        return 5;
                    }
                case ShopFilling.regular:
                    {
                        return 8;
                    }
                case ShopFilling.filled:
                    {
                        return 11;
                    }
                case ShopFilling.veryFilled:
                    {
                        return 14;
                    }
                default:
                    {
                        throw new ArgumentException("Not implemented!");
                    }
            }
        }

        public void SaveShop()
        {
            SortedDictionary<String, String> saveData = new SortedDictionary<string, string>();

            saveData.Add("count", shopInventory.Count.ToString());
            Game.saveFile.Save(Game1.SaveFilePath, "save.ini", "shop" + identifyer, saveData);

            for (int i = 0; i < shopInventory.Count; i++)
            {
                saveData.Clear();
                saveData.Add("name", shopInventory[i].ToString());
                saveData.Add("kind", shopInventory[i].Kind);
                if (shopInventory[i] is ShipPart)
                {
                    ShipPart tmp = (ShipPart)shopInventory[i];
                    saveData.Add("variety", tmp.GetShipPartVariety().ToString());
                }
                if (shopInventory[i] is QuantityItem)
                {
                    QuantityItem foo = (QuantityItem)shopInventory[i];
                    saveData.Add("quantity", foo.Quantity.ToString());
                }
                Game.saveFile.Save(Game1.SaveFilePath, "save.ini", "shop" + identifyer + i, saveData);
            }
        }

        public void LoadShop()
        {
            int count = Game.saveFile.GetPropertyAsInt("shop" + identifyer, "count", 0);
            shopInventory.Clear();

            for (int i = 0; i < count; i++)
            {
                //shopInventory.Add(Game.saveFile.GetItemFromSavefile("shop" + this.ToString() + i));
                shopInventory.Add(Game.saveFile.CreateItemFromSector("shop" + identifyer + i));
            }
        }
    }
}