using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// Class with help-functions for mission-logic

namespace SpaceProject
{
    public enum LevelStartCondition
    {
        Immediately,
        TextCleared,
        EnteringOverworld,
        OnStartMission
    }

    public class MissionHelper
    {
        private Game1 game;
        private Mission mission;

        private String levelToStart;
        private LevelStartCondition levelStartCondition;

        public MissionHelper(Game1 game, Mission mission) 
        {
            this.game = game;
            this.mission = mission;
        }

        public void Initialize()
        {
            levelToStart = "";
        }

        public void Update()
        {
            if (levelToStart != "")
            {
                switch (levelStartCondition)
                {
                    case LevelStartCondition.Immediately:
                        game.stateManager.shooterState.BeginLevel(levelToStart);
                        levelToStart = "";
                        break;

                    case LevelStartCondition.TextCleared:
                        if (IsTextCleared())
                        {
                            game.stateManager.shooterState.BeginLevel(levelToStart);
                            levelToStart = "";
                        }
                        break;

                    case LevelStartCondition.EnteringOverworld:
                        if (GameStateManager.currentState == "OverworldState")
                        {
                            game.stateManager.shooterState.BeginLevel(levelToStart);
                            levelToStart = "";
                        }
                        break;

                    case LevelStartCondition.OnStartMission:
                        {
                            if (mission.MissionHelper.HasStartMissionTextBeenDisplayed())
                            {
                                mission.MissionHelper.StartLevel(levelToStart);
                                levelToStart = "";
                            }
                        }
                        break;
                }
            }
        }

        public bool IsPlayerOnStation(String stationName)
        {
            return (GameStateManager.currentState == "StationState"
                && game.stateManager.stationState.Station.name.ToLower().Equals(stationName.ToLower()));
        }

        public bool IsPlayerOnPlanet(String planetName)
        {
            return (GameStateManager.currentState == "PlanetState"
                && game.stateManager.planetState.Planet.name.ToLower().Equals(planetName.ToLower()));
        }

        public bool IsLevelCompleted(String levelName)
        {
            return game.stateManager.shooterState.GetLevel(levelName).IsObjectiveCompleted;
        }

        public bool IsLevelFailed(String levelName)
        {
            return (game.stateManager.shooterState.GetLevel(levelName).IsGameOver 
                && GameStateManager.currentState == "OverworldState");
        }

        // Returns true if all mission-text displayed in station/planet-state is cleared by player
        public bool IsTextCleared()
        {
            return (mission.EventBuffer.Count <= 0 && game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm);
        }

        public bool HasTextBeenDisplayed(int eventIndex)
        {
            //return (mission.EventArray[eventIndex, 0] == ""
            //    && IsTextCleared());

            return (mission.EventList[eventIndex].Key == ""
                && IsTextCleared());
        }

        public bool HasStartMissionTextBeenDisplayed()
        {
            return (mission.MissionText.EndsWith("/ok")
                && IsTextCleared());
        }

        public void ShowEvent(String eventText)
        {
            if (!mission.EventBuffer.Contains(eventText))
            {
                if (eventText != "")
                    MissionManager.MissionEventBuffer.Add(eventText);

                eventText = "";
            }
        }

        public void ShowEvent(List<String> eventText)
        {
            for (int i = 0; i < eventText.Count; i++)
            {
                if (!mission.EventBuffer.Contains(eventText[i]))
                {
                    if (eventText[i] != "")
                        MissionManager.MissionEventBuffer.Add(eventText[i]);

                    eventText[i] = "";
                }
            }
        }

        public void ShowResponse(int eventIndex, int responseIndex)
        {
            //if (!mission.ResponseBuffer.Contains(mission.EventArray[eventIndex, responseIndex]))
            //{
            //    if (mission.EventArray[eventIndex, responseIndex] != "")
            //    {
            //        game.stateManager.stationState.SubStateManager.MissionMenuState.ActiveMission = mission;
            //        mission.ResponseBuffer.Add(mission.EventArray[eventIndex, responseIndex]);
            //    }
            //
            //    mission.EventArray[eventIndex, responseIndex] = "";
            //}

            if (!mission.ResponseBuffer.Contains(mission.EventList[eventIndex].Value[responseIndex]))
            {
                if (mission.EventList[eventIndex].Value[responseIndex] != "")
                {
                    game.stateManager.stationState.SubStateManager.MissionMenuState.ActiveMission = mission;
                    mission.ResponseBuffer.Add(mission.EventList[eventIndex].Value[responseIndex]);
                }

                mission.EventList[eventIndex].Value[responseIndex] = "";
            }
        }

        public void ShowResponse(int eventIndex, List<int> responseIndices)
        {
            //for (int i = 0; i < responseIndices.Count; i++)
            //{
            //    if (!mission.ResponseBuffer.Contains(mission.EventArray[eventIndex, responseIndices[i]]))
            //    {
            //        if (mission.EventArray[eventIndex, responseIndices[i]] != "")
            //        {
            //            mission.ResponseBuffer.Add(mission.EventArray[eventIndex, responseIndices[i]]);
            //            game.stateManager.stationState.SubStateManager.MissionMenuState.ActiveMission = mission;
            //        }
            //
            //        mission.EventArray[eventIndex, responseIndices[i]] = "";
            //    }
            //}

            for (int i = 0; i < responseIndices.Count; i++)
            {
                if (!mission.ResponseBuffer.Contains(mission.EventList[eventIndex].Value[responseIndices[i]]))
                {
                    if (mission.EventList[eventIndex].Value[responseIndices[i]] != "")
                    {
                        mission.ResponseBuffer.Add(mission.EventList[eventIndex].Value[responseIndices[i]]);
                        game.stateManager.stationState.SubStateManager.MissionMenuState.ActiveMission = mission;
                    }

                    mission.EventList[eventIndex].Value[responseIndices[i]] = "";
                }
            }
        }

        public void StartLevel(String levelName)
        {
            game.stateManager.shooterState.BeginLevel(levelName);
        }

        public void StartLevelAfterCondition(String levelName, LevelStartCondition levelStartCondition)
        {
            levelToStart = levelName;
            this.levelStartCondition = levelStartCondition;
        }

        public void ClearResponseText()
        {
            mission.ResponseBuffer.Clear();
        }

        public bool AllObjectivesCompleted()
        {
            for (int i = mission.ObjectiveIndex; i < mission.Objectives.Count; i++)
            {
                if (!mission.Objectives[i].Completed())
                {
                    return false;
                }

            }

            return true;
        }
    }
}
