using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace SpaceProject
{
    //Snodd från föreläsningarna
    public class ConfigFile
    {

        private SortedDictionary<String, SortedDictionary<String, String>> configData =
            new SortedDictionary<String, SortedDictionary<String, String>>();

        private String currentSection = "";

        public bool Load(String filePath)
        {
            TextReader reader;
            bool parsedCorrectly = true;

            try
            {
                reader = new StreamReader(filePath);

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

        public String GetPropertyOnLine(String section, int line, String defaultValue)
        {
            return defaultValue;
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

            //String tempFloatString = configData[section][name].Replace('.', ',');
            String tempFloatString = configData[section][name];
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

        private bool Parse(String line)
        {
            line = line.TrimStart(new char[] { ' ', '\t' });
            line = line.TrimEnd(new char[] { ' ', '\t' });

            if (line.Count() == 0)
                return true;

            if (line[0] == ';')
                return true;

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
    }
}

