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
        //private enum FlightTrainingState
        //{ 
        //    uninitialized = 0,
        //    A_ready = 1,
        //    A_running = 2,
        //    A_completed = 3,
        //    B_ready = 4,
        //    B_running = 5,
        //    B_completed = 6,
        //    C_ready = 7,
        //    C_running = 8,
        //    C_completed = 9,
        //    failed = 10
        //}

        //public int getProgress { get { return (int)flightTrainingState; } set { flightTrainingState = (FlightTrainingState)value; } }

        //private FlightTrainingState flightTrainingState;

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

            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText3", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText4", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText5", ""));

            SideMissilesWeapon sideMissiles = new SideMissilesWeapon(Game, ItemVariety.regular);
            RewardItems.Add(sideMissiles);

            RestartAfterFail();
        }

        public override void Initialize()
        {
            base.Initialize();

            Station borderStation = Game.stateManager.overworldState.getStation("Border Station");

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                "flightTraining_1", LevelStartCondition.EnteringOverworld));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                new EventTextCapsule(new List<String> { EventArray[0, 0], EventArray[1, 0] , EventArray[2, 0]},
                    null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                "flightTraining_2", LevelStartCondition.EnteringOverworld));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                new EventTextCapsule(new List<String> { EventArray[3, 0], EventArray[4, 0], EventArray[5, 0] },
                    null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], borderStation,
                "flightTraining_3", LevelStartCondition.EnteringOverworld));
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
        }

        public override void OnLoad()
        { }

        public override void MissionLogic()
        {
            base.MissionLogic();

            //String firstMission = "flightTraining_1";
            //String secondMission = "flightTraining_2";
            //String thirdMission = "flightTraining_3";
            //
            ////First part
            //if (flightTrainingState == FlightTrainingState.uninitialized 
            //    && GameStateManager.currentState == "StationState"
            //    && Game.stateManager.stationState.Station.name.ToLower().Equals("border station"))
            //{
            //    missionHelper.StartLevelAfterCondition(firstMission, LevelStartCondition.EnteringOverworld);
            //}
            //
            //if (missionHelper.IsLevelCompleted(firstMission))
            //{
            //    updateLogic = true;
            //    flightTrainingState = FlightTrainingState.A_completed;
            //}
            //
            ////Second part
            //if (flightTrainingState == FlightTrainingState.A_completed
            //    && missionHelper.IsPlayerOnStation("Border Station"))
            //{
            //    missionHelper.ShowEvent(new List<int> { 0, 1, 2 });
            //    missionHelper.StartLevelAfterCondition(secondMission, LevelStartCondition.EnteringOverworld);
            //}
            //
            //if (missionHelper.IsLevelCompleted(secondMission))
            //{
            //    updateLogic = true;
            //    flightTrainingState = FlightTrainingState.B_completed;
            //}
            //
            ////Third part
            //if (flightTrainingState == FlightTrainingState.B_completed
            //    && missionHelper.IsPlayerOnStation("Border Station"))
            //{
            //    missionHelper.ShowEvent(new List<int> { 3, 4, 5 });
            //    missionHelper.StartLevelAfterCondition(thirdMission, LevelStartCondition.EnteringOverworld);
            //}
            //
            //if (missionHelper.IsLevelCompleted(thirdMission))
            //{
            //    flightTrainingState = FlightTrainingState.C_completed;
            //}
            //
            ////
            //
            ////Fail mission
            //if (missionHelper.IsLevelFailed(firstMission)
            //    || missionHelper.IsLevelFailed(secondMission)
            //    || missionHelper.IsLevelFailed(thirdMission))
            //{
            //    MissionManager.MarkMissionAsFailed("Flight Training");
            //}
            //
            ////Win mission
            //if (flightTrainingState == FlightTrainingState.C_completed)
            //{
            //    updateLogic = true;
            //    MissionManager.MarkMissionAsCompleted(this.MissionName);
            //}
            
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
