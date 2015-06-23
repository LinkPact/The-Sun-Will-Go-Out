using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;

namespace SpaceProject
{
    public enum GameMode
    { 
        Normal,
        Easy,
        Hard,
        Hardcore,
        Develop,
        Campaign
    }

    //Central class where variables for player and current game are stored global.
    public class StatsManager
    {
        public static float moneyFactor = 1f;
        public static float damageFactor = 1f;

        #region declaration
        private Game1 Game;
        private Sprite spriteSheet;

        public static PlayTime PlayTime;

        //Statsvariabler relaterade till spelaren
        private static PlayerPlating plating { get { return ShipInventoryManager.equippedPlating; } }
        public static float Armor() { return plating.Armor; }
        
        //private static float shipLife;
        public static float ShieldMax;
        public static int statPoints;

        public static int MaxFusionCells;
        public static int EmergencyFusionCell;
        public static int[] FusionCells;

        public static int InventorySlots = 24;

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
            //plating = ShipInventoryManager.equippedPlating;

            // Money
            Rupees = 100;

            // Fuel
            MaxFuel = 750;
            Fuel = 750;

            progress = 0;
            reputation = 0;

            PlayTime = new PlayTime();

            MaxFusionCells = 3;
            FusionCells = new int[MaxFusionCells];
            for (int i = 0; i < MaxFusionCells; i++)
                FusionCells[i] = 1;

            discoveredBeacons = new List<Beacon>();
        }

        public static void SetStats()
        {
            if (gameMode.Equals(GameMode.Easy))
                SetEasyStats();
            else if (gameMode.Equals(GameMode.Normal))
                SetNormalStats();
            else if (gameMode.Equals(GameMode.Hard))
                SetHardStats();
            else if (gameMode.Equals(GameMode.Hardcore))
                SetHardcoreStats();
            else if (gameMode.Equals(GameMode.Develop))
                SetDevelopStats();
        }

        public static void SetEasyStats()
        {
            moneyFactor = 1.50f;
            damageFactor = 0.25f;
            gameMode = GameMode.Easy;
        }

        public static void SetNormalStats()
        {
            moneyFactor = 1.0f;
            damageFactor = 1.0f;
            gameMode = GameMode.Normal;
        }

        public static void SetHardStats()
        {
            moneyFactor = 1.0f;
            damageFactor = 2.0f;
            gameMode = GameMode.Hard;
        }

        public static void SetHardcoreStats()
        {
            moneyFactor = 0.8f;
            damageFactor = 1.20f;
            gameMode = GameMode.Hardcore;
        }

        public static void SetDevelopStats()
        {
            moneyFactor = 1.0f;
            damageFactor = 1.0f;
            gameMode = GameMode.Develop;

            Rupees = 100000;
        }

        public void Update()
        { }
        
        public static float Speed()
        {
            return plating.Speed;
        }
        
        public static float Acceleration()
        {
            return plating.Acceleration;
        }
        
        public static float GetShipLife()
        {
            return plating.CurrentOverworldHealth;
        }

        public static void ReduceShipLife(int damage)
        {
            plating.CurrentOverworldHealth -= damage;
        }

        public static void ReduceOverwordHealthToVerticalHealth(PlayerVerticalShooter player)
        {
            plating.CurrentOverworldHealth = player.HP;
        }

        // Sets the ships armor to an arbitrary number
        // ONLY TO BE USED FOR STRICT DEVELOP PURPOSES
        public static void SetCustomDamageFactor_DEVELOPONLY(float lifeFactor)
        {
            damageFactor = 1 / lifeFactor;
        }
        
        public static void RepairShip(float repairvalue)
        {
            plating.CurrentOverworldHealth += repairvalue;
            if (plating.CurrentOverworldHealth > Armor())
                plating.CurrentOverworldHealth = Armor();
        }

        public static void RestoreShipHealthToMax()
        {
            plating.CurrentOverworldHealth = Armor();
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
            saveData.Add("playtime", Convert.ToString(PlayTime.OverallPlayTime, CultureInfo.InvariantCulture));
            saveData.Add("overworldtime", Convert.ToString(PlayTime.OverworldTime, CultureInfo.InvariantCulture));
            saveData.Add("shooterparttime", Convert.ToString(PlayTime.ShooterPartTime, CultureInfo.InvariantCulture));
            saveData.Add("rupees", Convert.ToString(Rupees, CultureInfo.InvariantCulture));
            saveData.Add("progress", Convert.ToString(progress, CultureInfo.InvariantCulture));
            saveData.Add("reputation", Convert.ToString(reputation, CultureInfo.InvariantCulture));
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

            Game.saveFile.Save(Game1.SaveFilePath, "save.ini", "statsmanager", saveData);
        }

        public void Load()
        {
            PlayTime.OverallPlayTime = Game.saveFile.GetPropertyAsFloat("statsmanager", "playtime", 0);
            PlayTime.OverworldTime = Game.saveFile.GetPropertyAsFloat("statsmanager", "overworldtime", 0);
            PlayTime.ShooterPartTime = Game.saveFile.GetPropertyAsFloat("statsmanager", "shooterparttime", 0);
            Rupees = Game.saveFile.GetPropertyAsInt("statsmanager", "rupees", 0);
            progress = Game.saveFile.GetPropertyAsInt("statsmanager", "progress", 0);
            reputation = Game.saveFile.GetPropertyAsInt("statsmanager", "reputation", 0);
            Fuel = Game.saveFile.GetPropertyAsFloat("statsmanager", "shipfuel", 0);

            gameMode = MathFunctions.ParseEnum<GameMode>(Game.saveFile.GetPropertyAsString("statsmanager", "gamemode", ""));
            SetStats();

            String beaconLine = Game.saveFile.GetPropertyAsString("statsmanager", "beacons", "");

            if (!beaconLine.ToLower().Equals("none") && !beaconLine.ToLower().Equals(""))
            {
                String[] beaconNames = Game.saveFile.GetPropertyAsString("statsmanager", "beacons", "").Split('/');

                for (int i = 0; i < beaconNames.Length - 1; i++)
                {
                    beaconNames[i] = beaconNames[i].Trim();
                    Beacon beacon = Game.stateManager.overworldState.GetBeacon(beaconNames[i]);
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
