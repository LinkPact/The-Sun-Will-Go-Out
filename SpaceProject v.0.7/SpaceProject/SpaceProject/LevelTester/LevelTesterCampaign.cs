using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class LevelTesterCampaign
    {
        private int currentLevel;
        private List<CampaignEntry> campaignLevels = new List<CampaignEntry>();

        public LevelTesterCampaign()
        {
            currentLevel = 0;
        }

        public void RunNextLevel()
        {
        
        }

        private class CampaignEntry
        {
            LevelTesterEntry testerEntry;

            public CampaignEntry(LevelTesterEntry testerEntry)
            {
                this.testerEntry = testerEntry;



            }
        }
    }
}
