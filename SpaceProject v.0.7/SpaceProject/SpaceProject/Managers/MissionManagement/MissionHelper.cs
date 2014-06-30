﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// Class with help-functions for mission-logic

namespace SpaceProject
{
    public enum LevelStartCondition
    {
        TextCleared,
        EnteringOverworld
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

        public void ShowEvent(int eventArrayIndex)
        {
            if (!mission.EventBuffer.Contains(mission.EventArray[eventArrayIndex, 0]))
            {
                if (mission.EventArray[eventArrayIndex, 0] != "")
                    MissionManager.MissionEventBuffer.Add(mission.EventArray[eventArrayIndex, 0]);

                mission.EventArray[eventArrayIndex, 0] = "";
            }
        }

        public void ShowEvent(List<int> eventArrayIndices)
        {
            for (int i = 0; i < eventArrayIndices.Count; i++)
            {
                if (!mission.EventBuffer.Contains(mission.EventArray[eventArrayIndices[i], 0]))
                {
                    if (mission.EventArray[eventArrayIndices[i], 0] != "")
                        MissionManager.MissionEventBuffer.Add(mission.EventArray[eventArrayIndices[i], 0]);

                    mission.EventArray[eventArrayIndices[i], 0] = "";
                }
            }
        }

        public void ShowResponse(int eventIndex, int responseIndex)
        {
            if (!mission.ResponseBuffer.Contains(mission.EventArray[eventIndex, responseIndex]))
            {
                if (mission.EventArray[eventIndex, responseIndex] != "")
                {
                    game.stateManager.stationState.SubStateManager.MissionMenuState.ActiveMission = mission;
                    mission.ResponseBuffer.Add(mission.EventArray[eventIndex, responseIndex]);
                }

                mission.EventArray[eventIndex, responseIndex] = "";
            }
        }

        public void ShowResponse(int eventIndex, List<int> responseIndices)
        {
            for (int i = 0; i < responseIndices.Count; i++)
            {
                if (!mission.ResponseBuffer.Contains(mission.EventArray[eventIndex, responseIndices[i]]))
                {
                    if (mission.EventArray[eventIndex, responseIndices[i]] != "")
                    {
                        mission.ResponseBuffer.Add(mission.EventArray[eventIndex, responseIndices[i]]);
                        game.stateManager.stationState.SubStateManager.MissionMenuState.ActiveMission = mission;
                    }

                    mission.EventArray[eventIndex, responseIndices[i]] = "";
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
    }
}
