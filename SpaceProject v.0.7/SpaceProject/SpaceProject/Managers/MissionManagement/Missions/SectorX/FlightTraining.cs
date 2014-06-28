using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class FlightTraining : Mission
    {
        private enum FlightTrainingState
        { 
            uninitialized = 0,
            A_ready = 1,
            A_running = 2,
            A_completed = 3,
            B_ready = 4,
            B_running = 5,
            B_completed = 6,
            C_ready = 7,
            C_running = 8,
            C_completed = 9,
            failed = 10
        }

        //public int getProgress { get { return (int)flightTrainingState; } set { flightTrainingState = (FlightTrainingState)value; } }

        private FlightTrainingState flightTrainingState;

        public FlightTraining(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            EventArray = new string[6, 1];

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");
            EventArray[1, 0] = configFile.GetPropertyAsString(section, "EventText2", "");
            EventArray[2, 0] = configFile.GetPropertyAsString(section, "EventText3", "");
            EventArray[3, 0] = configFile.GetPropertyAsString(section, "EventText4", "");
            EventArray[4, 0] = configFile.GetPropertyAsString(section, "EventText5", "");
            EventArray[5, 0] = configFile.GetPropertyAsString(section, "EventText6", "");
            //EventArray[6, 0] = configFile.GetPropertyAsString(section, "EventText7", "");

            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText3", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText4", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText5", ""));

            SideMissilesWeapon sideMissiles = new SideMissilesWeapon(Game, ItemVariety.regular);
            RewardItems.Add(sideMissiles);

            RestartAfterFail();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            flightTrainingState = FlightTrainingState.uninitialized;
        }

        public override void OnLoad()
        { }

        public override void MissionLogic()
        {
            base.MissionLogic();

            String firstMission = "flightTraining_1";
            String secondMission = "flightTraining_2";
            String thirdMission = "flightTraining_3";

            //First part
            if (EventBuffer.Count <= 0
                && flightTrainingState == FlightTrainingState.uninitialized 
                && GameStateManager.currentState == "StationState"
                && Game.stateManager.stationState.Station.name.ToLower().Equals("border station")
                && Game.stateManager.stationState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            {
                flightTrainingState = FlightTrainingState.A_ready;
            }

            if (flightTrainingState == FlightTrainingState.A_ready
                && GameStateManager.currentState == "OverworldState")
            {
                Game.stateManager.shooterState.BeginLevel(firstMission);
                flightTrainingState = FlightTrainingState.A_running;
            }

            if (flightTrainingState == FlightTrainingState.A_running 
                && Game.stateManager.shooterState.GetLevel(firstMission).IsObjectiveCompleted)
            {
                updateLogic = true;
                //ShowEventAndUpdateProgress(0, 0);

                //Game.stateManager.ChangeState("OverworldState");
                flightTrainingState = FlightTrainingState.A_completed;
            }

            //Second part
            if (flightTrainingState == FlightTrainingState.A_completed
                && GameStateManager.currentState == "StationState" 
                && Game.stateManager.stationState.Station.name.ToLower().Equals("border station")
                && Game.stateManager.stationState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            {
                ShowEvent(new List<int> { 0, 1, 2 });
                flightTrainingState = FlightTrainingState.B_ready;
            }

            if (flightTrainingState == FlightTrainingState.B_ready
                && GameStateManager.currentState == "OverworldState")
            {
                Game.stateManager.shooterState.BeginLevel(secondMission);
                flightTrainingState = FlightTrainingState.B_running;
            }

            if (flightTrainingState == FlightTrainingState.B_running
                && Game.stateManager.shooterState.GetLevel(secondMission).IsObjectiveCompleted)
            {
                updateLogic = true;
                //ShowEventAndUpdateProgress(1, 0);

                //Game.stateManager.ChangeState("OverworldState");
                flightTrainingState = FlightTrainingState.B_completed;
            }

            //Third part
            if (flightTrainingState == FlightTrainingState.B_completed
                && GameStateManager.currentState == "StationState"
                && Game.stateManager.stationState.Station.name.ToLower().Equals("border station")
                && Game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            {
                ShowEvent(new List<int> { 3, 4, 5 });
                flightTrainingState = FlightTrainingState.C_ready;
            }

            if (flightTrainingState == FlightTrainingState.C_ready
                && GameStateManager.currentState == "OverworldState")
            {
                Game.stateManager.shooterState.BeginLevel(thirdMission);
                flightTrainingState = FlightTrainingState.C_running;
            }

            if (flightTrainingState == FlightTrainingState.C_running
                && Game.stateManager.shooterState.GetLevel(thirdMission).IsObjectiveCompleted)
            {
                //updateLogic = true;
                //ShowEvent(2, 0);

                //Game.stateManager.ChangeState("OverworldState");
                flightTrainingState = FlightTrainingState.C_completed;
            }

            //

            //Fail mission
            if ((flightTrainingState == FlightTrainingState.A_running
                || flightTrainingState == FlightTrainingState.B_running
                || flightTrainingState == FlightTrainingState.C_running)
                && GameStateManager.currentState == "OverworldState")
            {
                //flightTrainingState = FlightTrainingState.failed;
                //updateLogic = true;
                //ShowEvent(2, 0);

                MissionManager.MarkMissionAsFailed("Flight Training");
            }

            //Win mission
            if (flightTrainingState == FlightTrainingState.C_completed
                && Game.stateManager.shooterState.GetLevel(firstMission).IsObjectiveCompleted)
            {
                updateLogic = true;

                //ShowEventAndUpdateProgress(3, 0);
                //ShowEvent(new List<int> { 6 });
                
                MissionManager.MarkMissionAsCompleted(this.MissionName);
                //Game.stateManager.ChangeState("OverworldState");
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
