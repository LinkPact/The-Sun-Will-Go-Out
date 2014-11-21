using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BaseInventoryManager
    {
        #region declaration

        private Game1 Game;
        private Sprite spriteSheet;

        private static EmptyItem emptyItem;
        private static EmptyWeapon emptyWeapon;
        private static EmptyShield emptyShield;
        private static int columnSize;
        public static int ColumnSize { get { return columnSize; } private set { } }

        private static List<Item> baseItems = new List<Item>();
        public static List<Item> BaseItems { get { return baseItems; } private set { } }
        private static int itemCount;
        public static int ItemCount { get { return itemCount; } private set { } }
        
        public static PlayerPlating equippedShip;
        public static List<PlayerPlating> ownedShips = new List<PlayerPlating>();

        public static int inventorySize;

        //Pre-existing baseitems
        private BasicLaserWeapon basicLaser;
        private MultipleShotWeapon multipleShot;
        private RegularBombWeapon bomb;
        private RegularPlating regularShip;
        private BasicEnergyCell durableCell;
        private AdvancedShield plasmaShield;
        private TitaniumResource titanium;
        
        //Ships
        private RegularPlating regularShip1;
        private LightPlating lightShip;
        private HeavyPlating heavyShip;

        public static List<int> ownCounts = new List<int>();

        #endregion
        public BaseInventoryManager(Game1 Game)
        {
            this.Game = Game;
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/ShooterSheet"));
            emptyItem = new EmptyItem(Game);
        }
        
        public void Initialize()
        {
            inventorySize = 40;
            columnSize = 20;

            emptyWeapon = new EmptyWeapon(Game);
            emptyShield = new EmptyShield(Game);

            //Weapons the player have from the beginning
            basicLaser = new BasicLaserWeapon(Game);
            multipleShot = new MultipleShotWeapon(Game);
            bomb = new RegularBombWeapon(Game);
            
            //Ships and other owned from the beginning
            regularShip = new RegularPlating(Game);
            durableCell = new BasicEnergyCell(Game);
            plasmaShield = new AdvancedShield(Game);

            //Resources
            titanium = new TitaniumResource(this.Game, 100);

            //Adding to base-list
            baseItems.Add(basicLaser);
            baseItems.Add(multipleShot);
            baseItems.Add(bomb);
            baseItems.Add(regularShip);
            baseItems.Add(durableCell);
            baseItems.Add(plasmaShield);
            baseItems.Add(titanium);

            //Ships owned from the beginning
            regularShip1 = new RegularPlating(Game);
            lightShip = new LightPlating(Game);
            heavyShip = new HeavyPlating(Game);

            ownedShips.Add(regularShip1);
            //ownedShips.Add(lightShip);
            //ownedShips.Add(heavyShip);

            equippedShip = regularShip1;

            //

            UpdateLists(emptyItem);
        }
        
        public static void AddItem(Item addedItem)
        {
            int firstEmptyPos = -1;

            for (int n = 0; n < baseItems.Count; n++)
            {
                if (baseItems[n].Kind == "Empty")
                {
                    firstEmptyPos = n;
                    break;
                }
            }

            if (firstEmptyPos != -1)
            {
                baseItems.RemoveAt(firstEmptyPos);
                baseItems.Insert(firstEmptyPos, addedItem);
            }

            UpdateLists(emptyItem);
        }
        
        public static void InsertItem(int position, Item item)
        {
            baseItems.Insert(position, item);
            UpdateLists(emptyItem);
        }
        
        public static void SwitchItems(int position1, int position2)
        {
            Item tempItem1 = BaseInventoryManager.BaseItems[position1];
            Item tempItem2 = BaseInventoryManager.BaseItems[position2];

            baseItems.RemoveAt(position1);
            baseItems.Insert(position1, tempItem2);
            baseItems.RemoveAt(position2);
            baseItems.Insert(position2, tempItem1);
        }
        
        public static void RemoveItemAt(int position)
        {
            baseItems.RemoveAt(position);
            //baseItems.Insert(position, emptyItem);
            UpdateLists(emptyItem);
        }
        
        public static void RemoveItem(Item item)
        {
            baseItems.Remove(item);
            UpdateLists(emptyItem);
        }
        
        public static void ChangeShip(int position)
        {
            ShipInventoryManager.CompressInventory();

            List<int> toBeMoved = new List<int>();
            //if (ShipInventoryManager.ItemCount > StatsManger.ownedShips[position].InventorySlots)
            //{
            //    for (int n = BaseInventoryManager.ownedShips[position].InventorySlots; n < ShipInventoryManager.ItemCount; n++)
            //    {
            //        toBeMoved.Add(n);
            //    }
            //}
            for (int n = toBeMoved.Count; n > 0; n--)
            {
                MoveToBase(n);
            }
            
            BaseInventoryManager.equippedShip = BaseInventoryManager.ownedShips[position];
            UpdateLists(emptyItem);
        }
        
        private static void MoveToBase(int invShipPos)
        {
            AddItem(ShipInventoryManager.ShipItems[invShipPos]);
            ShipInventoryManager.RemoveItemAt(invShipPos);
        }
        
        private void SwitchBetweenInventories(int invShipPos, int invBasePos)
        {
            Item invItem = ShipInventoryManager.ShipItems[invShipPos];
            Item baseItem = BaseInventoryManager.BaseItems[invBasePos];

            ShipInventoryManager.RemoveItemAt(invShipPos);
            ShipInventoryManager.InsertItem(invShipPos, baseItem);
            BaseInventoryManager.RemoveItemAt(invBasePos);
            BaseInventoryManager.InsertItem(invBasePos, invItem);
        }
        
        private static void UpdateLists(EmptyItem emptyItem)
        {
            ShipInventoryManager.UpdateLists(emptyItem);

            if (baseItems.Count < inventorySize)
            {
                for (int n = baseItems.Count; n < inventorySize; n++)
                    baseItems.Add(emptyItem);
            }
            else if (baseItems.Count > inventorySize)
            {
                for (int n = baseItems.Count; n > inventorySize; n--)
                    baseItems.RemoveAt(baseItems.Count - 1);
            }

            int itemCounter = 0;
            foreach (Item item in baseItems)
            {
                if (item.Kind != "empty")
                    itemCounter += 1;
                        
            }
            itemCount = itemCounter;

            ownCounts.Clear();
        }
        
        public static void CheckQuantityCount(Game1 Game)
        {
            float copperTotalRest = 0;
            float goldTotalRest = 0;
            float titaniumTotalRest = 0;
            float fineWhiskeyTotalRest = 0;

            float rest = 0;
            float quantity;
            float maxQuantity;

            bool checkAgain = false;

            for (int i = 0; i < baseItems.Count; i++)
            {
                if (baseItems[i] is QuantityItem)
                {

                    quantity = ((QuantityItem)baseItems[i]).Quantity;
                    maxQuantity = ((QuantityItem)baseItems[i]).MaxQuantity;

                    if (quantity > maxQuantity)
                    {
                        rest = quantity - maxQuantity;
                        ((QuantityItem)baseItems[i]).Quantity -= rest;

                    check:

                        if (rest > maxQuantity)
                        {
                            switch (baseItems[i].Kind.ToLower())
                            {
                                case "resource":
                                    {
                                        switch (baseItems[i].Name.ToLower())
                                        {
                                            case "copper":
                                                AddItem(new CopperResource(Game, maxQuantity));
                                                break;

                                            case "gold":
                                                AddItem(new GoldResource(Game, maxQuantity));
                                                break;

                                            case "titanium":
                                                AddItem(new TitaniumResource(Game, maxQuantity));
                                                break;
                                        }
                                    }
                                    break;

                                case "spirit":
                                    {
                                        switch (baseItems[i].Name.ToLower())
                                        {
                                            case "fine whiskey":
                                                AddItem(new FineWhiskey(Game, maxQuantity));
                                                break;
                                        }
                                    }
                                    break;
                            }

                            rest -= maxQuantity;

                            goto check;
                        }

                    }

                    if (baseItems[i].Name == "Copper")
                    {
                        copperTotalRest += rest;
                        rest = 0;
                    }

                    else if (baseItems[i].Name == "Gold")
                    {
                        goldTotalRest += rest;
                        rest = 0;
                    }

                    else if (baseItems[i].Name == "Titanium")
                    {
                        titaniumTotalRest += rest;
                        rest = 0;
                    }

                    else if (baseItems[i].Name == "Fine Whiskey")
                    {
                        fineWhiskeyTotalRest += rest;
                        rest = 0;
                    }
                }
            }

            if (copperTotalRest > 0)
            {
                for (int i = 0; i < baseItems.Count; i++)
                {
                    if (baseItems[i].Name == "Copper")
                    {
                        if (((QuantityItem)baseItems[i]).Quantity < ((QuantityItem)baseItems[i]).MaxQuantity)
                        {
                            ((QuantityItem)baseItems[i]).Quantity += copperTotalRest;
                            copperTotalRest = 0;
                            checkAgain = true;
                        }
                    }
                }

                if (copperTotalRest > 0)
                {
                    AddItem(new CopperResource(Game, copperTotalRest));
                    copperTotalRest = 0;
                    checkAgain = true;
                }
            }

            if (goldTotalRest > 0)
            {
                for (int i = 0; i < baseItems.Count; i++)
                {
                    if (baseItems[i].Name == "Gold")
                    {
                        if (((QuantityItem)baseItems[i]).Quantity < ((QuantityItem)baseItems[i]).MaxQuantity)
                        {
                            ((QuantityItem)baseItems[i]).Quantity += goldTotalRest;
                            goldTotalRest = 0;
                            checkAgain = true;
                        }
                    }
                }

                if (goldTotalRest > 0)
                {
                    AddItem(new GoldResource(Game, goldTotalRest));
                    goldTotalRest = 0;
                    checkAgain = true;
                }
            }

            if (titaniumTotalRest > 0)
            {
                for (int i = 0; i < baseItems.Count; i++)
                {
                    if (baseItems[i].Name == "Titanium")
                    {
                        if (((QuantityItem)baseItems[i]).Quantity < ((QuantityItem)baseItems[i]).MaxQuantity)
                        {
                            ((QuantityItem)baseItems[i]).Quantity += titaniumTotalRest;
                            titaniumTotalRest = 0;
                            checkAgain = true;
                        }
                    }
                }

                if (titaniumTotalRest > 0)
                {
                    AddItem(new TitaniumResource(Game, titaniumTotalRest));
                    titaniumTotalRest = 0;
                    checkAgain = true;
                }
            }

            if (fineWhiskeyTotalRest > 0)
            {
                for (int i = 0; i < baseItems.Count; i++)
                {
                    if (baseItems[i].Name == "Fine Whiskey")
                    {
                        if (((QuantityItem)baseItems[i]).Quantity < ((QuantityItem)baseItems[i]).MaxQuantity)
                        {
                            ((QuantityItem)baseItems[i]).Quantity += fineWhiskeyTotalRest;
                            fineWhiskeyTotalRest = 0;
                            checkAgain = true;
                        }
                    }
                }

                if (fineWhiskeyTotalRest > 0)
                {
                    AddItem(new FineWhiskey(Game, fineWhiskeyTotalRest));
                    fineWhiskeyTotalRest = 0;
                    checkAgain = true;
                }
            }

            if (checkAgain)
            {
                CheckQuantityCount(Game);
            }
        }

        //public void Save()
        //{
        //    equippedShip.Save();
        //}
        //
        //public void Load()
        //{
        //    equippedShip.Load();
        //}*/
    }
}
