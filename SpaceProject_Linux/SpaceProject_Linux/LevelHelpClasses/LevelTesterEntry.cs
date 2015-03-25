using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Linux
{
    class LevelTesterEntry
    {
        private LevelEntry levelEntry;
        private String filepath;
        private String identifier;
        private Keys entryKey;
        private int standardEquip;
        private MissionType missionType;

        public LevelTesterEntry(LevelEntry levelEntry, Keys entryKey, int standardEquip = -1)
        {
            this.levelEntry = levelEntry;

            this.filepath = levelEntry.FilePath;
            this.identifier = levelEntry.Identifier;
            this.entryKey = entryKey;
            this.standardEquip = standardEquip;
            this.missionType = levelEntry.MissionType;
        }

        public LevelEntry GetLevelEntry()
        {
            return levelEntry;
        }

        public String GetPath()
        {
            return filepath;
        }

        public Keys GetKey()
        {
            return entryKey;
        }

        public String GetDescription()
        {
            return identifier;
        }

        public MissionType GetMissionType()
        {
            return missionType;
        }

        public String GetDescriptionWithKey()
        {
            return identifier + ", " + entryKey.ToString();
        }

        public int GetStandardEquip()
        {
            return standardEquip;
        }
    }
}
