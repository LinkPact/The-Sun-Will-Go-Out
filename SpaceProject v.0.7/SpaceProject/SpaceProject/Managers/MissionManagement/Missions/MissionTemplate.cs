using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    //Change class-name to the name of the mission 
    class MissionName : Mission
    {
        //Add MissionItems here
        private MedicalSupplies medicalSupplies;    //Example

        // Add rewarditems here
        private RegularEnergyCell veryFatcell;      //Example

        public MissionName(Game1 Game, string section) :
            base(Game, section)
        { 
            //Initialize EventArray here
            EventArray = new string[1, 1];

            //create Items here
            medicalSupplies = new MedicalSupplies(this.Game);   //Example

            //initialize reward items and add them to the "rewardlist" here
            veryFatcell = new RegularEnergyCell(Game);       //Example
            veryFatcell.Capacity = 2000;
            veryFatcell.Recharge = 50;
            RewardItems.Add(veryFatcell);                                               //

            //Add text to be displayed as objectives here
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));

            //If you want text to be displayed during the mission (currently only when entering a Colony), add it here
            EventArray[0,0] = configFile.GetPropertyAsString(section, "EventText1", "");   //index 0,0   //Example
            EventArray[1,0] = configFile.GetPropertyAsString(section, "EventText2", "");   //index 1,0   //
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        //If you want anything specific to happen when you start the mission, 
        //for example add an item to the ship inventory, add it here
        public override void StartMission()
        {
            ShipInventoryManager.AddItem(medicalSupplies);  //Example
        }

        //Add logic for the mission here
        public override void MissionLogic()
        {
            if (Game.stateManager.currentGameState.Name == "PlanetOverviewState" &&             //Example
                Game.stateManager.planetState.Planet.name == "Red Planet")        
            {
                //Be sure to use this format when adding events to the eventbuffer
                if (!EventBuffer.Contains(EventArray[0,0]))
                {
                    if (EventArray[0,0] != "")
                    {
                        EventBuffer.Add(EventArray[0,0]);
                        CurrentObjectiveDescription = ObjectiveDescriptions[1];       // <-- This changes the text that is displayed in the mission screen         
                    }

                    EventArray[0,0] = "";

                }

                if (Game.stateManager.stationState.Station.name.Equals("Red Planet Station"))
                {
                    ShipInventoryManager.RemoveItem(medicalSupplies);                          
                }
            }

            if (Game.stateManager.currentGameState.Name == "PlanetOverviewState" &&
                Game.stateManager.planetState.Planet.name == "Grey Planet")
            {
                if (CurrentObjectiveDescription.Equals(ObjectiveDescriptions[1]))
                {
                    MissionManager.MarkMissionAsCompleted(this.MissionName);
                    CurrentObjectiveDescription = ObjectiveCompleted;
                }
            }                                                                                   //

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
