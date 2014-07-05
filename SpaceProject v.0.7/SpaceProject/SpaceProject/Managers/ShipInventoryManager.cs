﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum ShipParts
    { 
        Primary,
        Secondary,
        Plating,
        EnergyCell,
        Shield
    }

    public class ShipInventoryManager
    {
        #region declaration

        private static Game1 Game;
        private static Sprite spriteSheet;

        private static EmptyItem emptyItem;
        private static EmptyWeapon emptyWeapon;
        private static EmptyShield emptyShield;

        private static List<Item> shipItems = new List<Item>();
        public static List<Item> ShipItems { get { return shipItems; } private set { } }
        private static int itemCount;
        public static int ItemCount { get { return itemCount; } private set { } }
        private static int columnSize;
        public static int ColumnSize { get { return columnSize; } private set { } }

        public static int inventorySize;
        private static float inventoryWeight;
        public static float InventoryWeight { get { return inventoryWeight; } private set { } }
        private static float maxWeight;
        public static float MaxWeight { get { return maxWeight; } private set { } }

        //Ship parts

        public static PlayerWeapon currentPrimaryWeapon;
        private static List<ShipPart> ownedPrimaryWeapons = new List<ShipPart>();
        public static List<ShipPart> OwnedPrimaryWeapons { get { return ownedPrimaryWeapons; } private set { } }
        public static List<PlayerWeapon> equippedPrimaryWeapons = new List<PlayerWeapon>();
        public static int primarySlots;

        public static PlayerWeapon equippedSecondary;
        private static List<ShipPart> ownedSecondary = new List<ShipPart>();
        public static List<ShipPart> OwnedSecondary { get { return ownedSecondary; } private set { } }

        public static PlayerPlating equippedPlating;
        public static List<ShipPart> ownedPlatings = new List<ShipPart>();
        public static List<ShipPart> OwnedPlatings { get { return ownedPlatings; } private set { } }

        public static PlayerEnergyCell equippedEnergyCell;
        public static List<ShipPart> ownedEnergyCells = new List<ShipPart>();
        public static List<ShipPart> OwnedEnergyCells { get { return ownedEnergyCells; } private set { } }

        public static PlayerShield equippedShield;
        public static List<ShipPart> ownedShields = new List<ShipPart>();
        public static List<ShipPart> OwnedShields { get { return ownedShields; } private set { } }

        public static List<int> equipCounts = new List<int>();
        public static List<int> ownCounts = new List<int>();

        public static Boolean isCheatActivated = false;

        public static Boolean isPrimaryCheatActivated       = false;
        public static Boolean isSecondaryCheatActivated     = false;
        public static Boolean isEnergyCheatActivated        = false;
        public static Boolean isShieldCheatActivated        = false;
        public static Boolean isPlatingCheatActivated       = false;
        public static Boolean isEquip1CheatActivated        = false;
        public static Boolean isEquip2CheatActivated        = false;

        #endregion

        //Test
        private GoldResource gold;

        public ShipInventoryManager(Game1 Game_)
        {
            Game = Game_;
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/ShooterSheet"));
            emptyItem = new EmptyItem(Game);
        }
        
        public void Initialize()
        {
            equippedPlating = null;

            columnSize = 14;

            emptyWeapon = new EmptyWeapon(Game);
            emptyShield = new EmptyShield(Game);

            //Initial inventory items
            BasicLaserWeapon basicLaser = new BasicLaserWeapon(Game);
            RegularEnergyCell regularCell = new RegularEnergyCell(Game);    
            RegularShield regularShield = new RegularShield(Game);
            RegularPlating regularPlating = new RegularPlating(Game);

            //Test 
            gold = new GoldResource(Game, 50);

            shipItems.Clear();

            List<PlayerWeapon> tempPrimary = new List<PlayerWeapon>();
            List<PlayerWeapon> tempSecondary = new List<PlayerWeapon>();

            shipItems.Add(basicLaser);
            shipItems.Add(regularCell);
            shipItems.Add(regularShield);
            shipItems.Add(regularPlating);

            //Test
            shipItems.Add(gold);

            //Equipped from the beginning
            primarySlots = 2;

            Random random = new Random();

            equippedPrimaryWeapons.Clear();
            equippedPrimaryWeapons.Add(basicLaser);
            equippedEnergyCell = regularCell;
            equippedShield = regularShield;
            equippedPlating = regularPlating;

            for (int n = equippedPrimaryWeapons.Count; n < primarySlots; n++)
                equippedPrimaryWeapons.Add(emptyWeapon);

            equippedSecondary = emptyWeapon;

            currentPrimaryWeapon = equippedPrimaryWeapons[0];

            //Lists with counts for the ShipManager-state
            equipCounts.Add(equippedPrimaryWeapons.Count);
            equipCounts.Add(1);
            equipCounts.Add(1);
            equipCounts.Add(1);
            equipCounts.Add(1);

            UpdateLists(emptyItem);
        }
        
        public static void AddItem(Item addedItem)
        {
            int firstEmptyPos = -1;

            for (int n = 0; n < shipItems.Count; n++)
            {
                if (shipItems[n].Kind == "Empty")
                {
                    firstEmptyPos = n;
                    break;
                }
            }

            if (firstEmptyPos != -1)
            {
                shipItems.RemoveAt(firstEmptyPos);
                shipItems.Insert(firstEmptyPos, addedItem);
            }

            UpdateLists(emptyItem);
        }
        
        public static void AddQuantityItem(Game1 Game, float quantity, string kind, string name)
        {
            for (int i = 0; i < shipItems.Count; i++)
            {
                if (shipItems[i] is QuantityItem)
                {
                    if (shipItems[i].Kind.ToLower() == kind.ToLower() &&
                        shipItems[i].Name.ToLower() == name.ToLower())
                    {
                        ((QuantityItem)shipItems[i]).Quantity += quantity;
                        CheckQuantityCount(Game);
                        return;
                    }
                }
            }

            switch (kind.ToLower())
            {
                case "resource":
                    {
                        switch (name.ToLower())
                        {
                            case "copper":
                                AddItem(new CopperResource(Game, quantity));
                                break;

                            case "gold":
                                AddItem(new GoldResource(Game, quantity));
                                break;

                            case "titanium":
                                AddItem(new TitaniumResource(Game, quantity));
                                break;
                        }
                    }
                    break;

                case "spirit":
                    {
                        switch (name.ToLower())
                        {
                            case "fine whiskey":
                                AddItem(new FineWhiskey(Game, quantity));
                                break;
                        }
                    }
                    break;
            }

            CheckQuantityCount(Game);

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

            for (int i = 0; i < shipItems.Count; i++)
            {
                if (shipItems[i] is QuantityItem)
                {

                    quantity = ((QuantityItem)shipItems[i]).Quantity;
                    maxQuantity = ((QuantityItem)shipItems[i]).MaxQuantity;

                    if (quantity > maxQuantity)
                    {
                        rest = quantity - maxQuantity;
                        ((QuantityItem)shipItems[i]).Quantity -= rest;

                    check:

                        if (rest > maxQuantity)
                        {
                            switch (shipItems[i].Kind.ToLower())
                            {
                                case "resource":
                                    {
                                        switch (shipItems[i].Name.ToLower())
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
                                        switch (shipItems[i].Name.ToLower())
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

                    if (shipItems[i].Name == "Copper")
                    {
                        copperTotalRest += rest;
                        rest = 0;
                    }

                    else if (shipItems[i].Name == "Gold")
                    {
                        goldTotalRest += rest;
                        rest = 0;
                    }

                    else if (shipItems[i].Name == "Titanium")
                    {
                        titaniumTotalRest += rest;
                        rest = 0;
                    }

                    else if (shipItems[i].Name == "Fine Whiskey")
                    {
                        fineWhiskeyTotalRest += rest;
                        rest = 0;
                    }
                }
            }

            if (copperTotalRest > 0)
            {
                for (int i = 0; i < shipItems.Count; i++)
                {
                    if (shipItems[i].Name == "Copper")
                    {
                        if (((QuantityItem)shipItems[i]).Quantity < ((QuantityItem)shipItems[i]).MaxQuantity)
                        {
                            ((QuantityItem)shipItems[i]).Quantity += copperTotalRest;
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
                for (int i = 0; i < shipItems.Count; i++)
                {
                    if (shipItems[i].Name == "Gold")
                    {
                        if (((QuantityItem)shipItems[i]).Quantity < ((QuantityItem)shipItems[i]).MaxQuantity)
                        {
                            ((QuantityItem)shipItems[i]).Quantity += goldTotalRest;
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
                for (int i = 0; i < shipItems.Count; i++)
                {
                    if (shipItems[i].Name == "Titanium")
                    {
                        if (((QuantityItem)shipItems[i]).Quantity < ((QuantityItem)shipItems[i]).MaxQuantity)
                        {
                            ((QuantityItem)shipItems[i]).Quantity += titaniumTotalRest;
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
                for (int i = 0; i < shipItems.Count; i++)
                {
                    if (shipItems[i].Name == "Fine Whiskey")
                    {
                        if (((QuantityItem)shipItems[i]).Quantity < ((QuantityItem)shipItems[i]).MaxQuantity)
                        {
                            ((QuantityItem)shipItems[i]).Quantity += fineWhiskeyTotalRest;
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
        
        public static void InsertItem(int position, Item item)
        {
            shipItems.Insert(position, item);
            UpdateLists(emptyItem);
        }
        
        public static void SwitchItems(int position1, int position2)
        {
            Item tempItem1 = ShipInventoryManager.ShipItems[position1];
            Item tempItem2 = ShipInventoryManager.ShipItems[position2];

            shipItems.RemoveAt(position1);
            shipItems.Insert(position1, tempItem2);
            shipItems.RemoveAt(position2);
            shipItems.Insert(position2, tempItem1);
        }
        
        public static void EquipItemFromSublist(ShipParts kind, int equipPos, int invPos)
        {
            switch (kind)
            {
                case ShipParts.Primary:
                    {
                        ShipInventoryManager.equippedPrimaryWeapons.RemoveAt(equipPos);
                        ShipInventoryManager.equippedPrimaryWeapons.Insert(equipPos, (PlayerWeapon)ShipInventoryManager.ownedPrimaryWeapons[invPos]);
                        break;
                    }
                case ShipParts.Secondary:
                    {
                        ShipInventoryManager.equippedSecondary = (PlayerWeapon)ShipInventoryManager.ownedSecondary[invPos];
                        break;
                    }
                case ShipParts.Plating:
                    {
                        equippedPlating = (PlayerPlating)ShipInventoryManager.ownedPlatings[invPos];
                        ChangePrimarySlots(equippedPlating.PrimarySlots);
                        StatsManager.UpdateValues();
                        break;
                    }
                case ShipParts.EnergyCell:
                    {
                        ShipInventoryManager.equippedEnergyCell = (PlayerEnergyCell)ShipInventoryManager.ownedEnergyCells[invPos];
                        break;
                    }
                case ShipParts.Shield:
                    {
                        ShipInventoryManager.equippedShield = (PlayerShield)ShipInventoryManager.ownedShields[invPos];
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("You entered inimplemented shippart!");
                    }
            }

            UpdateLists(emptyItem);
        }

        // Variant of EquipItem that searches for a free slot in equipped items. If both weapon slots are taken,
        // it equips the weapon at the first slot
        static int count = 1;
        public static void EquipItemFromInventory(String kind, int invPos)
        {
            if (kind == "Primary")
            {
                int equipPos = count;

                count++;

                if (count > 1)
                    count = 0;

                if (equippedPrimaryWeapons.Count == 1)
                {
                    equipPos = 1;
                }

                else if (equippedPrimaryWeapons.Count > 1)
                {
                    for (int i = 0; i < equippedPrimaryWeapons.Count; i++)
                    {
                        if (equippedPrimaryWeapons[i] is EmptyWeapon)
                        {
                            equipPos = i;
                            break;
                        }
                    }
                }

                ShipInventoryManager.equippedPrimaryWeapons.RemoveAt(equipPos);
                ShipInventoryManager.equippedPrimaryWeapons.Insert(equipPos, (PlayerWeapon)shipItems[invPos]);
            }
            else if (kind == "Secondary")
                ShipInventoryManager.equippedSecondary = (PlayerWeapon)shipItems[invPos];
            else if (kind == "Plating")
            {
                equippedPlating = (PlayerPlating)ShipInventoryManager.shipItems[invPos];
                ChangePrimarySlots(equippedPlating.PrimarySlots);
                StatsManager.UpdateValues();
            }
            else if (kind == "EnergyCell")
                ShipInventoryManager.equippedEnergyCell = (PlayerEnergyCell)shipItems[invPos];
            else if (kind == "Shield")
                ShipInventoryManager.equippedShield = (PlayerShield)shipItems[invPos];
            
            UpdateLists(emptyItem);
        }

        private static void ChangePrimarySlots(int newValue)
        {
            primarySlots = newValue;

            if (equippedPrimaryWeapons.Count < primarySlots)
            {
                List<PlayerWeapon> temp = new List<PlayerWeapon>();
                temp = equippedPrimaryWeapons;
                while (temp.Count < primarySlots)
                {
                    equippedPrimaryWeapons.Add(new EmptyWeapon(Game));
                }
                equippedPrimaryWeapons = temp;
            }
            else if (equippedPrimaryWeapons.Count > primarySlots)
            {
                List<PlayerWeapon> temp = new List<PlayerWeapon>();
                int currentPos = 0;
                while (temp.Count < primarySlots)
                {
                    temp.Add(equippedPrimaryWeapons[currentPos]);
                    currentPos++;
                }
                equippedPrimaryWeapons = temp;
            }

            equipCounts[0] = equippedPrimaryWeapons.Count;
        }

        public static void RemoveItemAt(int position)
        {
            if (shipItems[position] == equippedSecondary)
            {
                equippedSecondary = emptyWeapon;
            }
            else if (shipItems[position] == equippedShield)
            {
                equippedShield = emptyShield;
            }

            int count = equippedPrimaryWeapons.Count;
            for (int n = 0; n < count; n++)
            {
                if (shipItems[position] == equippedPrimaryWeapons[n])
                {
                    equippedPrimaryWeapons.RemoveAt(n);
                    equippedPrimaryWeapons.Insert(n, emptyWeapon);
                }
            }

            shipItems.RemoveAt(position);
            UpdateLists(emptyItem);
        }
        
        public static void RemoveItem(Item item)
        {
            shipItems.Remove(item);
            UpdateLists(emptyItem);
        }
        
        public static void CheckInventoryWeight()
        {
            float weight = 0;

            foreach (Item item in shipItems)
            {
                weight += item.Weight;
            }

            inventoryWeight = weight;
        }
        
        public static void CompressInventory()
        {
            List<int> removeList = new List<int>();
            for (int n = 0; n < ShipInventoryManager.ShipItems.Count; n++)
            {
                if (ShipInventoryManager.ShipItems[n].Kind == "Empty")
                    removeList.Add(n);
            }
            for (int n = removeList.Count; n > 0; n--)
            {
                ShipInventoryManager.RemoveItemAt(removeList[n - 1]); ;
            }
        }
        
        public static void UpdateLists(EmptyItem emptyItem)
        {
            inventorySize = StatsManager.InventorySlots;

            if (shipItems.Count > inventorySize)
            {
                int countRef = shipItems.Count;
                for (int n = inventorySize; n < countRef; n++)
                {
                    shipItems.RemoveAt(inventorySize);
                }
            }

            CheckInventoryWeight();
            maxWeight = StatsManager.CargoCapacity();

            ownedPrimaryWeapons.Clear();
            ownedSecondary.Clear();
            ownedPlatings.Clear();
            ownedEnergyCells.Clear();
            ownedShields.Clear();

            int itemCounter = 0;
            foreach (Item item in shipItems)
            {
                switch (item.Kind)
                {
                    case "Primary":
                        {
                            ownedPrimaryWeapons.Add((PlayerWeapon)item);
                            itemCounter += 1;
                            break;
                        }
                    case "Secondary":
                        {
                            ownedSecondary.Add((PlayerWeapon)item);
                            itemCounter += 1;
                            break;
                        }
                    case "Plating":
                        {
                            ownedPlatings.Add((PlayerPlating)item);
                            itemCounter += 1;
                            break;
                        }
                    case "EnergyCell":
                        {
                            ownedEnergyCells.Add((PlayerEnergyCell)item);
                            itemCounter += 1;
                            break;
                        }
                    case "Shield":
                        {
                            ownedShields.Add((PlayerShield)item);
                            itemCounter += 1;
                            break;
                        }
                }
            }
            itemCount = itemCounter;

            if (shipItems.Count < inventorySize)
            {
                for (int n = shipItems.Count; n < inventorySize; n++)
                    shipItems.Add(emptyItem);
            }
            else if (shipItems.Count > inventorySize)
            {
                for (int n = shipItems.Count; n > inventorySize; n--)
                    shipItems.RemoveAt(shipItems.Count - 1);
            }

            ownCounts.Clear();

            ownCounts.Add(ownedPrimaryWeapons.Count);
            ownCounts.Add(ownedSecondary.Count);
            ownCounts.Add(ownedPlatings.Count);
            ownCounts.Add(ownedEnergyCells.Count);
            ownCounts.Add(ownedShields.Count);
        }
        
        public static bool HasAvailableSlot()
        {
            foreach (Item item in shipItems)
                if (item.Kind == "Empty")
                    return true;

            return false;
        }
        
        public void Save()
        {
            SortedDictionary<String, String> saveData = new SortedDictionary<string, string>();

            saveData.Add("count", shipItems.Count.ToString());
            Game.saveFile.Save("save.ini", "shipitems", saveData);

            for (int i = 0; i < shipItems.Count; i++)
            {
                saveData.Clear();
                saveData.Add("name", shipItems[i].ToString());
                saveData.Add("kind", shipItems[i].Kind);
                if (shipItems[i] is ShipPart)
                {
                    ShipPart tmp = (ShipPart)shipItems[i];
                    saveData.Add("variety", tmp.GetShipPartVariety().ToString() );
                }
                if (shipItems[i] is QuantityItem)
                {
                    QuantityItem foo = (QuantityItem)shipItems[i];
                    saveData.Add("quantity", foo.Quantity.ToString());
                }
                if (shipItems[i] is PlayerPlating)
                {
                    PlayerPlating plating = (PlayerPlating)shipItems[i];
                    saveData.Add("currenthealth", plating.CurrentOverworldHealth.ToString()); 
                }
                Game.saveFile.Save("save.ini", "shipinv" + i, saveData);
            }

            saveData.Clear();
            for (int i = 0; i < ownedPrimaryWeapons.Count; i++) {
                for (int n = 0; n < equippedPrimaryWeapons.Count; n++) {
                    if (ownedPrimaryWeapons[i] == equippedPrimaryWeapons[n]) {
                        saveData.Add("primaryinvpos" + n, i.ToString());
                    }
                }
            }
            for (int i = 0; i < ownedSecondary.Count; i++)
            {
                if (equippedSecondary == ownedSecondary[i])
                    saveData.Add("secondaryinvpos", i.ToString());
            }
            for (int i = 0; i < ownedEnergyCells.Count; i++)
            {
                if (equippedEnergyCell == ownedEnergyCells[i])
                    saveData.Add("energyinvpos", i.ToString());
            }
            for (int i = 0; i < ownedShields.Count; i++)
            {
                if (equippedShield == ownedShields[i])
                    saveData.Add("shieldinvpos", i.ToString());
            }
            for (int i = 0; i < ownedPlatings.Count; i++)
            {
                if (equippedPlating == ownedPlatings[i])
                    saveData.Add("platinginvpos", i.ToString());
            }
            Game.saveFile.Save("save.ini", "shipequipped", saveData);

        }
        
        public void Load()
        {
            // Load inventory
            int count = Game.saveFile.GetPropertyAsInt("shipitems", "count", 0);
            shipItems.Clear();

            for (int i = 0; i < count; i++)
            {
                //InsertItem(i, Game.saveFile.GetItemFromSavefile("shipinv" + i));
                InsertItem(i, Game.saveFile.CreateItemFromSector("shipinv" + i));
            }

            // Equip Items
            int pos;
            for (int i = 0; i < equippedPrimaryWeapons.Count; i++)
            {
                pos = Game.saveFile.GetPropertyAsInt("shipequipped", "primaryinvpos" + i, -1);
                if (pos != -1)
                    EquipItemFromSublist(ShipParts.Primary, i, pos);
            }

            pos = Game.saveFile.GetPropertyAsInt("shipequipped", "secondaryinvpos", -1);
            if (pos != -1)
                EquipItemFromSublist(ShipParts.Secondary, 0, pos);

            pos = Game.saveFile.GetPropertyAsInt("shipequipped", "energyinvpos", -1);
            if (pos != -1)
                EquipItemFromSublist(ShipParts.EnergyCell, 0, pos);

            pos = Game.saveFile.GetPropertyAsInt("shipequipped", "shieldinvpos", -1);
            if (pos != -1)
                EquipItemFromSublist(ShipParts.Shield, 0, pos);
            pos = Game.saveFile.GetPropertyAsInt("shipequipped", "platinginvpos", -1);
            if (pos != -1)
                EquipItemFromSublist(ShipParts.Plating, 0, pos);
        }

        public static void ActivateCheatPrimary()
        {
            if (isPrimaryCheatActivated)
                return;

            AddItem(new DualLaserWeapon(Game));
            AddItem(new SpreadBulletWeapon(Game));
            AddItem(new MultipleShotWeapon(Game));
            AddItem(new BeamWeapon(Game));
            AddItem(new DrillBeamWeapon(Game));
            AddItem(new FlameShotWeapon(Game));
            AddItem(new BallisticLaserWeapon(Game));
            AddItem(new MineLayerWeapon(Game));
            AddItem(new AdvancedLaserWeapon(Game));
            AddItem(new ProximityLaserWeapon(Game));
            AddItem(new WaveBeamWeapon(Game));

            isPrimaryCheatActivated = true;
        }

        public static void ActivateCheatSecondary()
        {
            if (isSecondaryCheatActivated)
                return;

            AddItem(new SideMissilesWeapon(Game));
            AddItem(new RegularBombWeapon(Game));
            AddItem(new HomingMissileWeapon(Game));
            AddItem(new FragmentMissileWeapon(Game));
            AddItem(new PunyTurretWeapon(Game));
            AddItem(new TurretWeapon(Game));

            isSecondaryCheatActivated = true;
        }

        public static void ActivateCheatEnergy()
        {
            if (isEnergyCheatActivated)
                return;

            AddItem(new DurableEnergyCell(Game));
            AddItem(new PlasmaEnergyCell(Game));
            AddItem(new WeaponBoostEnergyCell(Game));
            AddItem(new ShieldBoostEnergyCell(Game));

            isEnergyCheatActivated = true;
        }

        public static void ActivateCheatShield()
        {
            if (isShieldCheatActivated)
                return;

            AddItem(new DurableShield(Game));
            AddItem(new PlasmaShield(Game));
            AddItem(new CollisionShield(Game));
            AddItem(new BulletShield(Game));

            isShieldCheatActivated = true;
        }

        public static void ActivateCheatPlating()
        {
            if (isPlatingCheatActivated)
                return;

            AddItem(new LightPlating(Game));
            AddItem(new HeavyPlating(Game));

            isPlatingCheatActivated = true;
        }

        public static void ActivateCheatEquip1()
        {
            if (isEquip1CheatActivated)
                return;

            equippedPrimaryWeapons[0] = new SpreadBulletWeapon(Game);
            equippedPrimaryWeapons[1] = new BallisticLaserWeapon(Game);
            equippedSecondary = new SideMissilesWeapon(Game);
            currentPrimaryWeapon = equippedPrimaryWeapons[0];

            equippedEnergyCell = new DurableEnergyCell(Game);
            equippedShield = new DurableShield(Game);

            isEquip1CheatActivated = true;
        }

        public static void ActivateCheatEquip2()
        {
            if (isEquip2CheatActivated)
                return;

            equippedPrimaryWeapons[0] = new ProximityLaserWeapon(Game);
            equippedPrimaryWeapons[1] = new AdvancedLaserWeapon(Game);
            equippedSecondary = new TurretWeapon(Game);
            currentPrimaryWeapon = equippedPrimaryWeapons[0];

            equippedEnergyCell = new PlasmaEnergyCell(Game);
            equippedShield = new PlasmaShield(Game);

            isEquip2CheatActivated = true;
        }

        public static void ActivateCheat()
        {
            AddItem(new DualLaserWeapon(Game));
            AddItem(new SpreadBulletWeapon(Game));
            //AddItem(new MultipleShotWeapon(Game));
            AddItem(new BeamWeapon(Game));
            AddItem(new DrillBeamWeapon(Game));
            AddItem(new FlameShotWeapon(Game));
            AddItem(new BallisticLaserWeapon(Game));
            AddItem(new MineLayerWeapon(Game));
            AddItem(new AdvancedLaserWeapon(Game));
            AddItem(new ProximityLaserWeapon(Game));
            AddItem(new WaveBeamWeapon(Game));
            
            AddItem(new SideMissilesWeapon(Game));
            AddItem(new RegularBombWeapon(Game));
            //AddItem(new HomingMissileWeapon(Game));
            //AddItem(new FragmentMissileWeapon(Game));
            //AddItem(new PunyTurretWeapon(Game));
            //AddItem(new TurretWeapon(Game));
            
            //AddItem(new DurableEnergyCell(Game));
            //AddItem(new PlasmaEnergyCell(Game));
            AddItem(new WeaponBoostEnergyCell(Game));
            AddItem(new ShieldBoostEnergyCell(Game));
            
            //AddItem(new DurableShield(Game));
            //AddItem(new PlasmaShield(Game));
            AddItem(new CollisionShield(Game));
            AddItem(new BulletShield(Game));

            AddItem(new LightPlating(Game));
            AddItem(new HeavyPlating(Game));

            StatsManager.cargoCapacity = 10000;
            maxWeight = 10000;

            isCheatActivated = true;
        }

        public static void MapCreatorEquip(int equipNbr)
        {
            equippedEnergyCell = new PlasmaEnergyCell(Game);

            switch (equipNbr)
            {
                case 1:
                    {
                        equippedPrimaryWeapons[0] = new BasicLaserWeapon(Game);
                        equippedPrimaryWeapons[1] = new DualLaserWeapon(Game);
                        equippedSecondary = new SideMissilesWeapon(Game);
                        currentPrimaryWeapon = equippedPrimaryWeapons[0];
                        break;
                    }
                case 2:
                    {
                        equippedPrimaryWeapons[0] = new DualLaserWeapon(Game);
                        equippedPrimaryWeapons[1] = new SpreadBulletWeapon(Game);
                        currentPrimaryWeapon = equippedPrimaryWeapons[0];
                        //equippedSecondary = new PunyTurretWeapon(Game);
                        //equippedEnergyCell = new PlasmaEnergyCell(Game);
                        break;
                    }
                case 3:
                    {
                        equippedPrimaryWeapons[0] = new SpreadBulletWeapon(Game);
                        equippedPrimaryWeapons[1] = new BallisticLaserWeapon(Game);
                        currentPrimaryWeapon = equippedPrimaryWeapons[0];
                        //equippedSecondary = new RegularBombWeapon(Game);
                        //equippedEnergyCell = new PlasmaEnergyCell(Game);
                        break;
                    }
                case 4:
                    {
                        equippedPrimaryWeapons[0] = new BallisticLaserWeapon(Game);
                        equippedPrimaryWeapons[1] = new MultipleShotWeapon(Game);
                        //equippedSecondary = new RegularBombWeapon(Game);
                        currentPrimaryWeapon = equippedPrimaryWeapons[0];
                        break;
                    }
                case 5:
                    {
                        equippedPrimaryWeapons[0] = new MultipleShotWeapon(Game);
                        equippedPrimaryWeapons[1] = new WaveBeamWeapon(Game);
                        currentPrimaryWeapon = equippedPrimaryWeapons[0];
                        break;
                    }
                case 6:
                    {
                        equippedPrimaryWeapons[0] = new WaveBeamWeapon(Game);
                        equippedPrimaryWeapons[1] = new MineLayerWeapon(Game);
                        currentPrimaryWeapon = equippedPrimaryWeapons[0];
                        break;
                    }
                case 7:
                    {
                        equippedPrimaryWeapons[0] = new MineLayerWeapon(Game);
                        equippedPrimaryWeapons[1] = new ProximityLaserWeapon(Game);
                        currentPrimaryWeapon = equippedPrimaryWeapons[0];
                        break;
                    }
                case 8:
                    {
                        equippedPrimaryWeapons[0] = new ProximityLaserWeapon(Game);
                        equippedPrimaryWeapons[1] = new AdvancedLaserWeapon(Game);
                        currentPrimaryWeapon = equippedPrimaryWeapons[0];
                        break;
                    }
                case 9:
                    {
                        equippedPrimaryWeapons[0] = new AdvancedLaserWeapon(Game);
                        equippedPrimaryWeapons[1] = new DrillBeamWeapon(Game);
                        currentPrimaryWeapon = equippedPrimaryWeapons[0];
                        break;
                    }
            }
        }

        public static int IndexOfItem(Item item)
        {
            if (ShipItems.Contains(item))
            {
                return ShipItems.IndexOf(item);
            }

            return -1;
        }

        public static bool IsEquipped(Item item)
        {
            if (item == equippedEnergyCell || item == equippedSecondary ||
                item == equippedShield || item == ShipInventoryManager.equippedPlating)
            {
                return true;
            }

            foreach (Item primary in equippedPrimaryWeapons)
            {
                if (item == primary) { return true; }
            }

            return false;
        }

        public static bool IsLastEquipped(Item item)
        {
            for (int i = 0; i < shipItems.Count; i++)
            {
                if (item != shipItems[i] &&
                    item.Kind.ToLower().Equals(shipItems[i].Kind.ToLower()))
                {
                    return false;
                }
            }

            switch (item.Kind.ToLower())
            {
                case "primary":
                    int count = 2;
                    for (int i = 0; i < equippedPrimaryWeapons.Count; i++)
                    {
                        if (equippedPrimaryWeapons[i] is EmptyWeapon)
                            count--;
                    }
                    if (count == 1)
                        return IsEquipped(item);

                    return false;

                default:
                    return IsEquipped(item);
            }
        }

        // Returns the index in equipped items of specified type. Returns 0 if not found
        public static int GetEquippedItemIndexOfType(String name)
        {
            for (int i = 0; i < equippedPrimaryWeapons.Count; i++)
            {
                if (name.ToLower().Equals(equippedPrimaryWeapons[i].Name.ToLower()))
                {
                    return i;
                }
            }

            return 0;
        }
    }
}