﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public static class TextUtils
    {
        // Text scroll variables
        private static Dictionary<string, string> stringBuffer = new Dictionary<string, string>();
        private static List<string> garbageStringKeys = new List<string>();
        private static readonly int CHARS_TO_APPEND = 1;

        public static string WordWrap(SpriteFont font, string txt, int width)
        {
            string line = string.Empty;
            string returnString = string.Empty;
            string[] wordArray = txt.Split(' ');

            foreach (string word in wordArray)
            {
                if (word.Contains('\n'))
                {
                    returnString = returnString + line;
                    line = string.Empty;
                }

                else if (font.MeasureString(line + word).Length() > width)
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

        /*
         *  ScrollText() 
         * 
         *  Returns the specified string with progressivly more characters each time the
         *  function is called. Use this before drawing string to the screen with DrawString().
         */
        public static string ScrollText(string str, bool flush, out bool finished)
        {
            StringBuilder strBuilder;
            string temp = "";

            if (!stringBuffer.Keys.Contains<string>(str))
            {
                stringBuffer.Add(str, "");
            }
            else
            {
                stringBuffer.TryGetValue(str, out temp);
            }

            strBuilder = new StringBuilder(temp);

            if (flush)
            {
                strBuilder.Append(str.Substring(strBuilder.Length, str.Length - strBuilder.Length));
                finished = true;
                garbageStringKeys.Add(str);
                return strBuilder.ToString();
            }

            // Checks if there's room to add characters
            if (str.Length - strBuilder.Length >= CHARS_TO_APPEND)
            {
                strBuilder.Append(str.Substring(strBuilder.Length, CHARS_TO_APPEND));
            }

            // Adds any "left over" characters
            if (str.Length - strBuilder.Length < CHARS_TO_APPEND)
            {
                strBuilder.Append(str.Substring(strBuilder.Length, str.Length - strBuilder.Length));
            }

            temp = strBuilder.ToString();

            if (str.Length - strBuilder.Length <= 0)
            {
                garbageStringKeys.Add(str);
                finished = true;
            }
            else
            {
                stringBuffer[str] = temp;
                finished = false;
            }

            return temp;
        }

        /*
         *  RefreshTextScrollBuffer()
         *  
         *  Removes finished strings from ScrollText()'s buffer. 
         *  Should be called anytime a scrolling message is cleared from the screen.
         */
        public static void RefreshTextScrollBuffer()
        {
            foreach (string str in garbageStringKeys)
            {
                stringBuffer.Remove(str);
            }

            garbageStringKeys.Clear();
        }

        //Method for creating a TextBox with predefined text
        public static TextBox CreateTextBox(SpriteFont font,
                                            Rectangle textRect,
                                            bool origin,
                                            bool scroll,
                                            string text)
        {
            TextBox tempTextBox = new TextBox(font, new Rectangle(textRect.X,
                                                                  textRect.Y,
                                                                  textRect.Width,
                                                                  textRect.Height),
                                                                  text,
                                                                  origin);

            tempTextBox.SetScrolling(scroll);
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

        public static List<String> SplitHashTagText(String text)
        {
            List<String> tempList = new List<String>();

            String[] split = text.Split('#');
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].Trim();
                tempList.Add(split[i]);
            }

            return tempList;
        }

        public static int GetSplitCount(params string[] text)
        {
            int count = 0;

            foreach (string str in text)
            {
                String[] split = str.Split('#');

                for (int i = 0; i < split.Length; i++)
                {
                    split[i] = split[i].Trim();
                    count++;
                }
            }

            return count;
        }

        // Deletion includes characters 'sign1' & 'sign2'
        public static string RemoveTextBetween(string text, char sign1, char sign2)
        {
            string result = text;
            int first, last;

            if (!text.Contains(sign1)
                || !text.Contains(sign2))
            {
                return result;
            }

            first = text.IndexOf(sign1);
            last = text.LastIndexOf(sign2) + 1;

            result = text.Remove(first, last - first);
            return result;
        }
    }
}
