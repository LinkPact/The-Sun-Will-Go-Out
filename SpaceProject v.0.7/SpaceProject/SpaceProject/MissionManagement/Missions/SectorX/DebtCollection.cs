using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    //Change class-name to the name of the mission 
    class DebtCollection : Mission
    {

        public DebtCollection(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        //If you want anything specific to happen when you start the mission, 
        //for example add an item to the ship inventory, add it here
        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
        }

        public override void OnLoad()
        { }

        //Add logic for the mission here
        public override void MissionLogic()
        {
            base.MissionLogic();

            if (progress == 0 &&
                missionHelper.IsPlayerOnStation("Fotrun Station I"))
            {
                progress = 1;
                missionHelper.ShowEvent(new List<String> { EventArray[0, 0], EventArray[1, 0]});
                missionHelper.ShowResponse(1, new List<int>() { 1, 2, 3, 4 });
            }
            
            if (MissionResponse != 0)
            {
                switch (MissionResponse)
                {
                    case 1:
                        {
                            if (StatsManager.Rupees > 1000)
                            {
                                missionHelper.ShowEvent(EventArray[2, 0]);
                                ObjectiveIndex = 1;
                                StatsManager.Rupees -= 1000;
                                missionHelper.ClearResponseText();
                            }
            
                            else
                            {
                                missionHelper.ShowEvent(EventArray[6, 0]);
                                MissionResponse = 0;
                            }
                            break;
                        }
            
                    case 2:
                        {
                            missionHelper.ShowEvent(EventArray[3, 0]);
                            progress = 2;
                            ObjectiveIndex = 2;
                            missionHelper.ClearResponseText();
                            break;
                        }
            
                    case 3:
                    case 4:
                        {
                            missionHelper.ShowEvent(EventArray[4, 0]);
                            missionHelper.ShowEvent(EventArray[5, 0]);
                            ObjectiveIndex = 1;
                            missionHelper.ClearResponseText();
                            break;
                        }
                }
            
            }
            
            if (missionHelper.IsPlayerOnPlanet("Highfence"))
            {
                if (progress == 1)
                    MissionManager.MarkMissionAsCompleted(this.MissionName);
            
                else if (progress == 2)
                    MissionManager.MarkMissionAsFailed(this.MissionName);
            }                                                                                   
        }

        public override int GetProgress()
        {
            return progress;
        }

        public override void SetProgress(int progress)
        {
            this.progress = progress;
        }
    }
}
