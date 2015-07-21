using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    public class LevelEntry
    {
        private String identifier;
        public String Identifier { get { return identifier; } }

        private String filePath;
        public String FilePath { get { return filePath; } }

        private MissionType missionType;
        public MissionType MissionType { get { return missionType; } }

        public LevelEntry(String identifier, String path, MissionType missionType)
        {
            this.identifier = identifier;
            this.filePath = path;
            this.missionType = missionType;
        }

    }
}
