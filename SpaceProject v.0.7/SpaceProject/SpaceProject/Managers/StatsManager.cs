﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;

namespace SpaceProject
{
    public enum GameMode
    { 
        normal,
        easy,
        hardcore,
        develop
    }

    //Central class where variables for player and current game are stored global.
    public class StatsManager
    {
        public const float lootFactor = 0.33f;
        
        #region declaration
        private Game1 Game;
        private Sprite spriteSheet;

        public static float PlayTime;

        //Statsvariabler relaterade till spelaren
        private static PlayerPlating plating;

        //private static float shipLifeMax;
        private static float shipLife;
        
        public static float ShieldMax;
        public static int statPoints;

        public static int MaxFusionCells;
        public static int EmergencyFusionCell;
        public static int[] FusionCells;

        public static int InventorySlots = 24;
        public static int cargoCapacity = 1000;

        // Money
        public static int Rupees;

        // Reputation (-100 rebel, 100 alliance) 
        public static int reputation;
        public static int progress;

        // Fuel
        public static float Fuel;
        public static float MaxFuel;
        
        public static List<int> equipCounts = new List<int>();
        public static List<int> ownCounts = new List<int>();

        private List<Beacon> discoveredBeacons;
        public List<Beacon> DiscoveredBeacons { get { return discoveredBeacons; } private set { ;} }

        //public static Boolean isHardcore = false;
        //public static Boolean isEasy = false;

        public static GameMode gameMode;
        #endregion
        
        public StatsManager(Game1 Game)
        {
            this.Game = Game;
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/ShooterSheet"));
            FusionCells = new int[3];
        }

        public void Initialize()
        {
            plating = ShipInventoryManager.equippedPlating;
            shipLife = plating.Armor;

            // Money
            Rupees = 100;

            // Fuel
            MaxFuel = 750;
            Fuel = 750;

            progress = 0;
            reputation = 0;

            PlayTime = 0.0f;

            MaxFusionCells = 3;
            FusionCells = new int[MaxFusionCells];
            for (int i = 0; i < MaxFusionCells; i++)
                FusionCells[i] = 1;

            discoveredBeacons = new List<Beacon>();
        }

        public static void SetEasyStats()
        {
            UpdateValues();
            gameMode = GameMode.easy;
        }

        public static void SetHardcoreStats()
        {
            UpdateValues();
            gameMode = GameMode.hardcore;
        }

        public static void SetNormalStats()
        {
            UpdateValues();
            gameMode = GameMode.normal;
        }

        public static void SetDevelopStats()
        {
            UpdateValues();
            gameMode = GameMode.develop;

            Rupees = 100000;
        }

        public static void ApplyDifficultyOnPlating(PlayerPlating plating)
        {
            plating.ApplyDifficulty(gameMode);
        }

        public void Update()
        { }

        public static float Armor()
        {
            return plating.Armor;
        }
        
        public static float Speed()
        {
            return plating.Speed;
        }
        
        public static float Acceleration()
        {
            return plating.Acceleration;
        }
        
        public static float CargoCapacity()
        {
            return cargoCapacity;
        }
        
        public static float GetShipLife()
        {
            return plating.CurrentOverworldHealth;
        }

        public static void ReduceShipLife(int damage)
        {
            plating.CurrentOverworldHealth -= damage;
        }
        
        public static void RepairShip(float repairvalue)
        {
            plating.CurrentOverworldHealth += repairvalue;
            if (plating.CurrentOverworldHealth > Armor())
                plating.CurrentOverworldHealth = Armor();
        }

        public static void UpdateValues()
        {
            plating = ShipInventoryManager.equippedPlating;
        }

        public static void AddLoot(int loot)
        {
            Rupees += loot;
        }

        public static float GetShieldRegeneration()
        {
            if (ShipInventoryManager.equippedEnergyCell is WeaponBoostEnergyCell)
                return ShipInventoryManager.equippedShield.Regeneration * 0.1f;
            else if ((ShipInventoryManager.equippedEnergyCell is ShieldBoostEnergyCell))
                return ShipInventoryManager.equippedShield.Regeneration * 1.5f;
            else
                return ShipInventoryManager.equippedShield.Regeneration;
                
        }

        public static float GetEnergyRegeneration()
        {
            if (ShipInventoryManager.equippedEnergyCell is WeaponBoostEnergyCell)
                return ShipInventoryManager.equippedEnergyCell.Recharge * 1.2f;
            else if ((ShipInventoryManager.equippedEnergyCell is ShieldBoostEnergyCell))
                return ShipInventoryManager.equippedEnergyCell.Recharge * 0.5f;
            else
                return ShipInventoryManager.equippedEnergyCell.Recharge;

        }

        public bool FusionCellsDepleted()
        {
            for (int i = 0; i < MaxFusionCells; i++)
            {
                if (FusionCells[i] > 0)
                    return false;
            }

            return true;
        }

        public void RemoveFusionCell()
        {
            for (int i = MaxFusionCells - 1; i >= 0; i--)
            {
                if (FusionCells[i] > 0)
                {
                    FusionCells[i]--;
                    break;
                }
            }
        }

        public void Save()
        {
            SortedDictionary<String, String> saveData = new SortedDictionary<string, string>();
            saveData.Add("playtime", Convert.ToString(PlayTime, CultureInfo.InvariantCulture));
            saveData.Add("rupees", Convert.ToString(Rupees, CultureInfo.InvariantCulture));
            saveData.Add("progress", Convert.ToString(progress, CultureInfo.InvariantCulture));
            saveData.Add("reputation", Convert.ToString(reputation, CultureInfo.InvariantCulture));
            // Removed 14-06-24 as the current life now is handled in the armors themselves / Jakob
            //saveData.Add("shiplife", Convert.ToString(shipLife, CultureInfo.InvariantCulture));
            saveData.Add("shipfuel", Convert.ToString(Fuel, CultureInfo.InvariantCulture));

            saveData.Add("gamemode", Convert.ToString(gameMode, CultureInfo.InvariantCulture));

            //Fusion cell part
            String fusionSaveKey = "";
            String fusionSaveValue = "";
            for (int i = 0; i < FusionCells.Length; i++)
            {
                fusionSaveKey = "fusioncell" + (i + 1).ToString();
                fusionSaveValue = FusionCells[i].ToString();
                saveData.Add(fusionSaveKey, fusionSaveValue);
            }
            
            saveData.Add("emergency fusion cells", EmergencyFusionCell.ToString());

            if (discoveredBeacons.Count > 0)
            {
                StringBuilder beacons = new StringBuilder("");
                for (int i = 0; i < discoveredBeacons.Count; i++)
                {
                    beacons.Append(discoveredBeacons[i].name)
                    .Append("/");
                }

                saveData.Add("beacons", beacons.ToString());
            }

            else
            {
                saveData.Add("beacons", "none");
            }

            Game.saveFile.Save("save.ini", "statsmanager", saveData);
        }

        public void Load()
        {
            PlayTime = Game.saveFile.GetPropertyAsFloat("statsmanager", "playtime", 0);
            Rupees = Game.saveFile.GetPropertyAsInt("statsmanager", "rupees", 0);
            progress = Game.saveFile.GetPropertyAsInt("statsmanager", "progress", 0);
            reputation = Game.saveFile.GetPropertyAsInt("statsmanager", "reputation", 0);
            // Removed 14-06-24 as the current life now is handled in the armors themselves / Jakob
            //shipLife = Game.saveFile.GetPropertyAsFloat("statsmanager", "shiplife", 0);
            Fuel = Game.saveFile.GetPropertyAsFloat("statsmanager", "shipfuel", 0);

            gameMode = GlobalFunctions.ParseEnum<GameMode>(Game.saveFile.GetPropertyAsString("statsmanager", "gamemode", ""));

            String beaconLine = Game.saveFile.GetPropertyAsString("statsmanager", "beacons", "");

            if (!beaconLine.ToLower().Equals("none") && !beaconLine.ToLower().Equals(""))
            {
                String[] beaconNames = Game.saveFile.GetPropertyAsString("statsmanager", "beacons", "").Split('/');

                for (int i = 0; i < beaconNames.Length - 1; i++)
                {
                    beaconNames[i] = beaconNames[i].Trim();
                    Beacon beacon = Game.stateManager.overworldState.GetSectorX.GetBeacon(beaconNames[i]);
                    AddDiscoveredBeacon(beacon);
                    beacon.OnLoad();
                }
            }
        }

        public void AddDiscoveredBeacon(Beacon beacon)
        {
            if (!discoveredBeacons.Contains(beacon))
            {
                Game.GetBeaconMenu.AddBeacon(beacon);
                discoveredBeacons.Add(beacon);
            }
        }
    }
}