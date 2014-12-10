using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    class LevelTesterEntry
    {
        private String filepath;
        private String description;
        private Keys entryKey;
        private int standardEquip;

        public LevelTesterEntry(String filepath, String description, Keys entryKey, int standardEquip = -1)
        {
            this.filepath = filepath;
            this.description = description;
            this.entryKey = entryKey;
            this.standardEquip = standardEquip;
        }

        public String GetPath()
        {
            return "testlevels\\" + filepath;
        }

        public Keys GetKey()
        {
            return entryKey;
        }

        public String GetDescription()
        {
            return description;
        }

        public String GetDescriptionWithKey()
        {
            //return filepath + ", " + description + ", " + entryKey.ToString();
            return description + ", " + entryKey.ToString();
        }

        public int GetStandardEquip()
        {
            return standardEquip;
        }
    }
}
