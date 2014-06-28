﻿using System;
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
            EventArray = new string[7, 5];

            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText3", ""));

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");            //index 0,0 

            EventArray[1, 0] = configFile.GetPropertyAsString(section, "EventText2", "");            //index 1,0  
            EventArray[1, 1] = configFile.GetPropertyAsString(section, "EventText2Response1", "");  //index 1,1
            EventArray[1, 2] = configFile.GetPropertyAsString(section, "EventText2Response2", "");  //index 1,2
            EventArray[1, 3] = configFile.GetPropertyAsString(section, "EventText2Response3", "");  //index 1,3
            EventArray[1, 4] = configFile.GetPropertyAsString(section, "EventText2Response4", "");  //index 1,4

            EventArray[2, 0] = configFile.GetPropertyAsString(section, "EventText3", "");            //index 2,0
            EventArray[3, 0] = configFile.GetPropertyAsString(section, "EventText4", "");            //index 3,0
            EventArray[4, 0] = configFile.GetPropertyAsString(section, "EventText5", "");            //index 4,0
            EventArray[5, 0] = configFile.GetPropertyAsString(section, "EventText6", "");            //index 5,0
            EventArray[6, 0] = configFile.GetPropertyAsString(section, "EventText7", "");            //index 6,0
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
                Game.stateManager.currentGameState.Name == "StationState" &&
                Game.stateManager.stationState.Station.name == "Fotrun Station I")
            {
                Game.stateManager.stationState.SubStateManager.MissionMenuState.ActiveMission = this;

                ShowEventAndUpdateProgress(0, 1);
                ShowEvent(1);

                ShowResponse(1, new List<int>() { 1, 2, 3, 4 });
            }

            if (MissionResponse != 0)
            {
                switch (MissionResponse)
                {
                    case 1:
                        {
                            if (StatsManager.Rupees > 1000)
                            {
                                ShowEvent(2);
                                ObjectiveIndex = 1;
                                StatsManager.Rupees -= 1000;
                                ResponseBuffer.Clear();
                            }

                            else
                            {
                                EventBuffer.Add(EventArray[6, 0]);
                                MissionResponse = 0;
                            }
                            break;
                        }

                    case 2:
                        {
                            ShowEventAndUpdateProgress(3, 2);
                            ObjectiveIndex = 2;
                            ResponseBuffer.Clear();
                            break;
                        }

                    case 3:
                    case 4:
                        {
                            ShowEvent(4);
                            ShowEvent(5);
                            ObjectiveIndex = 1;
                            ResponseBuffer.Clear();
                            break;
                        }
                }

            }

            if (Game.stateManager.currentGameState.Name == "PlanetState" &&
                Game.stateManager.planetState.Planet.name == "Highfence")
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
