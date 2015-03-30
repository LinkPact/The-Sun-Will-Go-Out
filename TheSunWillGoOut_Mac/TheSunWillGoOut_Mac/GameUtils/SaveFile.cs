using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace SpaceProject_Mac
{
    public class SaveFile
    {
        private Game1 Game;

        private SortedDictionary<String, SortedDictionary<String, String>> configData =
            new SortedDictionary<String, SortedDictionary<String, String>>();

        private String currentSection = "";

        public SaveFile(Game1 Game)
        {
            this.Game = Game;
        }

        public bool Save(String filePath, String fileName, String className, SortedDictionary<String, String> properties)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(filePath, fileName), true))
            {
                writer.WriteLine("[" + className.ToLower() + "]");

                foreach (KeyValuePair<String, String> entry in properties)
                {
                    writer.WriteLine(entry.Key.ToLower() + " = " + entry.Value);
                }

                writer.WriteLine("");
                writer.Close();
            }

            return true;
        }

        public void NewSavefile(String filePath, String filename)
        {
            File.Create(Path.Combine(filePath, filename)).Dispose();
        }

        public void EmptySaveFile(String filePath, String filename)
        {
            File.WriteAllText(Path.Combine(filePath, filename), "");
        }

        public static bool CheckIfFileExists(String filePath, String filename)
        {
            return File.Exists(Path.Combine(filePath, filename));
        }

        public bool Load(String filePath, String fileName)
        {
            TextReader reader;
            bool parsedCorrectly = true;
            configData.Clear();

            try
            {
                reader = new StreamReader(Path.Combine(filePath, fileName));

                String currentLine;

                while ((currentLine = reader.ReadLine()) != null)
                {
                    parsedCorrectly = Parse(currentLine);

                    if (parsedCorrectly == false)
                    {
                        break;
                    }
                }

                reader.Close();

                return parsedCorrectly;
            }
            catch (FileNotFoundException ex)
            {
                String errMsg = ex.Message;
                return false;
            }
        }

        public int GetPropertyAsInt(String section, String name, int defaultValue)
        {
            section = section.ToLower();
            name = name.ToLower();

            if (configData.ContainsKey(section) == false)
                return defaultValue;

            if (configData[section].ContainsKey(name) == false)
                return defaultValue;

            return Convert.ToInt32(configData[section][name]);
        }

        public float GetPropertyAsFloat(String section, String name, float defaultValue)
        {
            // Making sure everything is lower case letters before comparing/searching
            section = section.ToLower();
            name = name.ToLower();

            if (configData.ContainsKey(section) == false)
                return defaultValue;

            if (configData[section].ContainsKey(name) == false)
                return defaultValue;

            String tempFloatString = configData[section][name];
            float tmp = Convert.ToSingle(tempFloatString, CultureInfo.InvariantCulture);
            return Convert.ToSingle(tempFloatString, CultureInfo.InvariantCulture);
        }

        public String GetPropertyAsString(String section, String name, String defaultValue)
        {
            // Making sure everything is lower case letters before comparing/searching
            section = section.ToLower();
            name = name.ToLower();

            if (configData.ContainsKey(section) == false)
                return defaultValue;

            if (configData[section].ContainsKey(name) == false)
                return defaultValue;

            return configData[section][name];
        }

        public bool GetPropertyAsBool(String section, String name, bool defaultValue)
        {
            // Making sure everything is lower case letters before comparing/searching
            section = section.ToLower();
            name = name.ToLower();

            if (configData.ContainsKey(section) == false)
                return defaultValue;

            if (configData[section].ContainsKey(name) == false)
                return defaultValue;

            String tempBool = configData[section][name].ToLower();

            if (tempBool == "true")
                return true;
            else if (tempBool == "false")
                return false;
            else
                return defaultValue;
        }

        public bool ContainsSection(String section)
        {
            section = section.ToLower();

            if (configData.ContainsKey(section) == true)
                return true;

            return false;
        }

        private bool Parse(String line)
        {
            // Remove trailing whitespace
            line = line.TrimStart(new char[] { ' ', '\t' });
            line = line.TrimEnd(new char[] { ' ', '\t' });

            // Line is empty
            if (line.Count() == 0)
                return true;

            // Line is comment
            if (line[0] == ';')
                return true;

            // Section
            if (line[0] == '[')
            {
                String sectionName = line.Substring(1, line.LastIndexOf(']') - 1);

                sectionName = sectionName.TrimStart(new char[] { ' ', '\t' });
                sectionName = sectionName.TrimEnd(new char[] { ' ', '\t' });

                if (configData.ContainsKey(sectionName))
                {
                    return false;
                }
                else
                {
                    sectionName = sectionName.ToLower();
                    currentSection = sectionName;

                    configData.Add(sectionName, new SortedDictionary<String, String>());
                    return true;
                }
            }

            if (currentSection == "")
                return false;

            if (line.LastIndexOf('=') == -1)
                return false;

            String currentPropertyName = line.Substring(0, line.LastIndexOf('='));
            String currentPropertyValue = line.Substring(line.LastIndexOf('=') + 1);

            currentPropertyName = currentPropertyName.TrimStart(new char[] { ' ', '\t' });
            currentPropertyName = currentPropertyName.TrimEnd(new char[] { ' ', '\t' });

            currentPropertyValue = currentPropertyValue.TrimStart(new char[] { ' ', '\t' });
            currentPropertyValue = currentPropertyValue.TrimEnd(new char[] { ' ', '\t' });

            if (currentPropertyName.Count() == 0 || currentPropertyValue.Count() == 0)
                return false;

            currentPropertyName = currentPropertyName.ToLower();

            if (configData[currentSection].ContainsKey(currentPropertyName))
            {
                return false;
            }
            else
            {
                configData[currentSection].Add(currentPropertyName, currentPropertyValue);
            }

            return true;
        }

        public Item GetItemFromSavefile(string section)
        {
            float quantity = 0;
            ItemVariety var;
            ShipPart fooItem;

            switch (GetPropertyAsString(section, "name", "error"))
            {
                // Weapons
                case "basic laser":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new BasicLaserWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "beam":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new BeamWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "drill beam":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new DrillBeamWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "flame shot":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new FlameShotWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "multiple shot":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new MultipleShotWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "spread bullet":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new SpreadBulletWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "ballistic laser":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new BallisticLaserWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "minelayer":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new MineLayerWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "advanced laser":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new AdvancedLaserWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "proximity laser":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new ProximityLaserWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "wave beam":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new WaveBeamWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "dual laser":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new DualLaserWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                // Secondary
                case "homingmissile":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new HomingMissileWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "bomb":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new RegularBombWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "sidemissiles":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new SideMissilesWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "fragmentmissile":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new FragmentMissileWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "puny turret":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new PunyTurretWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "turret":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new TurretWeapon(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                // Shields
                case "regular shield":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new RegularShield(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "plasma shield":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new AdvancedShield(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "durable shield":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new BasicShield(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "collision shield":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new CollisionShield(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "bullet shield":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new BulletShield(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                // Energy
                case "regular cell":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new RegularEnergyCell(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "durable cell":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new BasicEnergyCell(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "plasma cell":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new AdvancedEnergyCell(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "weapon boost cell":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new WeaponBoostEnergyCell(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "shield boost cell":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new ShieldBoostEnergyCell(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                // Plating
                case "regular plating":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new RegularPlating(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "light plating":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new LightPlating(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                case "heavy plating":
                    var = (ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true);
                    fooItem = new HeavyPlating(Game);
                    fooItem.SetShipPartVariety(var);
                    return fooItem;
                // Resource
                case "copper":
                    quantity = Game.saveFile.GetPropertyAsInt(section, "quantity", 0);
                    return new CopperResource(Game, quantity);
                case "gold":
                    quantity = Game.saveFile.GetPropertyAsInt(section, "quantity", 0);
                    return new GoldResource(Game, quantity);
                case "titanium":
                    quantity = Game.saveFile.GetPropertyAsInt(section, "quantity", 0);
                    return new TitaniumResource(Game, quantity);
                // Other items
                case "fine whiskey":
                    quantity = Game.saveFile.GetPropertyAsInt(section, "quantity", 0);
                    return new FineWhiskey(Game, quantity);
                case "---":
                    return new EmptyItem(Game);
                default:
                    throw new ArgumentException("Not implemented!");
            }
        }

        public Item CreateItemFromSector(string section)
        {
            String className = GetPropertyAsString(section, "name", "error");
            Item fooItem = CreateItem(className);

            if (fooItem is ShipPart)
            {
                ((ShipPart)fooItem).SetShipPartVariety((ItemVariety)Enum.Parse(typeof(ItemVariety), GetPropertyAsString(section, "variety", "none"), true));

                if (fooItem is PlayerPlating)
                    ((PlayerPlating)fooItem).LoadHealth(GetPropertyAsInt(section, "currenthealth", -1));
            }
            else if (fooItem is QuantityItem)
            {
                ((QuantityItem)fooItem).Quantity = Game.saveFile.GetPropertyAsInt(section, "quantity", 0);
            }
            return fooItem;
        }

        private Item CreateItem(string classname)
        {
            Type t = Type.GetType(classname);
            if (t == null)
            {
                throw new Exception("Type " + classname + " not found.");
            }
            Item i = (Item)Activator.CreateInstance(t, new object[] { Game, null });
            return i;
        }
    }
}