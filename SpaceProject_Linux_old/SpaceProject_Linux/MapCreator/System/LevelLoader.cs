﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic;

namespace SpaceProject_Linux
{
    public static class LevelLoader
    {
        private static Boolean hasNewOutput;
        private static String[] loadedLines;

        public static void ReadLevelFile(String input)
        {
            String path = "Levels/" + input + ".lvl";

            try
            {
                loadedLines = File.ReadAllLines(@path);
            }
            catch //(FileNotFoundException e)
            {
                return;
            }

            hasNewOutput = true;
        }

        public static void ReadMapCreatorLevelFile(String input)
        {
            String path = "MapCreatorLevels/" + input + ".lvl";

            try
            {
                loadedLines = File.ReadAllLines(@path);
            }
            catch //(FileNotFoundException e)
            {
                return;
            }

            hasNewOutput = true;
        }

        public static Boolean HasNewOutput()
        {
            return hasNewOutput;
        }

        public static String[] GetNewOutput()
        {
            hasNewOutput = false;
            return loadedLines;
        }
    }   
}