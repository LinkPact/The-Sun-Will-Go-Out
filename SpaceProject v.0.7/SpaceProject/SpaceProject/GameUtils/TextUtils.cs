using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public static class TextUtils
    {
        public static string WordWrap(SpriteFont font, string txt, int width)
        {
            string line = string.Empty;
            string returnString = string.Empty;
            string[] wordArray = txt.Split(' ');

            foreach (string word in wordArray)
            {
                if (font.MeasureString(line + word).Length() > width)
                {
                    returnString = returnString + line + "\n";
                    line = string.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line; ;
        }

        //Method for creating a TextBox that loads data from a config file
        public static TextBox CreateTextBoxConfig(SpriteFont font,
                                            Rectangle textRect,
                                            string filePath,
                                            bool origin)
        {
            TextBox tempTextBox = new TextBox(font, new Rectangle(textRect.X,
                                                                  textRect.Y,
                                                                  textRect.Width,
                                                                  textRect.Height),
                                                                  origin);
            tempTextBox.Initialize();
            tempTextBox.SetSource(filePath);

            return tempTextBox;
        }

        //Method for creating a TextBox with predefined text
        public static TextBox CreateTextBox(SpriteFont font,
                                            Rectangle textRect,
                                            bool origin,
                                            string text)
        {
            TextBox tempTextBox = new TextBox(font, new Rectangle(textRect.X,
                                                                  textRect.Y,
                                                                  textRect.Width,
                                                                  textRect.Height),
                                                                  text,
                                                                  origin);
            tempTextBox.Initialize();
        
            return tempTextBox;
        }

        //Method for creating a TextBox with predefined text
        public static List<TextBox> CreateFormattedTextBoxes(SpriteFont font,
                                    Rectangle textRect, bool origin, string text)
        {
            String[] substrings = text.Split('#');

            List<TextBox> subBoxes = new List<TextBox>();

            foreach (String subStr in substrings)
            {
                TextBox tempTextBox = new TextBox(font, new Rectangle(textRect.X,
                    textRect.Y, textRect.Width, textRect.Height), subStr, origin);
                tempTextBox.Initialize();

                subBoxes.Add(tempTextBox);
            }

            return subBoxes;
        }

        //Method for creating a TextBox that reads data from a config file and read from it instantly 
        public static TextBox CreateTextBoxAndGetText(SpriteFont font,
                                                      Rectangle textRect,
                                                      string filePath,
                                                      string section,
                                                      string variable,
                                                      bool origin)
        {
            TextBox tempTextBox = new TextBox(font, new Rectangle(textRect.X,
                                                                  textRect.Y,
                                                                  textRect.Width,
                                                                  textRect.Height),
                                                                  origin);
            tempTextBox.Initialize();
            tempTextBox.SetSource(filePath);
            tempTextBox.LoadText(section, variable);

            return tempTextBox;
        }

        //Returns a formatted string when a list of MISSIONS is sent in 
        public static string ReturnStringFromList(List<Mission> list)
        {
            string tempString = "";

            if (list.Count > 0)
            {
                foreach (Mission mission in list)
                {
                    tempString += mission.MissionName + "\n";
                }
            }

            else
                tempString = "None";

            return tempString;
        }

        //Returns a formatted string when a list of STRINGS is sent in 
        public static string ReturnStringFromList(List<string> list)
        {
            string tempString = "";

            if (list.Count > 0)
            {
                foreach (string str in list)
                {
                    tempString += str + "\n";
                }
            }

            else
                tempString = "None";

            return tempString;
        }

        //Returns a formatted string when a list of STRINGS is sent in 
        public static string ReturnStringFromList(List<string> list, List<int> value)
        {
            string tempString = "";

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++ )
                {
                    tempString += list[i] + ": " + value[i] + "\n";
                }
            }

            else
                tempString = "None";

            return tempString;
        }

        public static string FormatDataBody(SpriteFont font, List<string> properties, List<string> data, float widht)
        {
            string padding = "";
            string text = "";
            int chars = 0;
            int[] charArray = new int[7];

            for (int i = 0; i < properties.Count; i++)
            {
                while (font.MeasureString(text).X < widht)
                {
                    chars++;
                    text = properties[i] + padding.PadRight(chars, ' ') + data[i] + "\n";
                }
                charArray[i] = chars;
                chars = 0;
                text = "";
            }

            for (int i = 0; i < properties.Count; i++)
            {
                text += properties[i] + padding.PadRight(charArray[i], ' ') + data[i] + "\n";
            }

            return text;
        }
    }
}
