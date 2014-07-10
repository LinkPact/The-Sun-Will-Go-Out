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

        public bool HasTextBeenDisplayed(EventText eventText)
        {
            return eventText.Displayed;
        }

        public bool HasStartMissionTextBeenDisplayed()
        {
            return (mission.MissionText.EndsWith("/ok")
                && IsTextCleared());
        }

        public void ShowEvent(EventText eventText)
        {
            String[] substrings = eventText.Text.Split('#');

            foreach (String subStr in substrings)
            {
                MissionManager.MissionEventBuffer.Add(subStr);
            }

            eventText.Displayed = true;

            ClearResponseText();
        }

        public void ShowEvent(EventText eventText, bool clearResponse)
        {
            String[] substrings = eventText.Text.Split('#');

            foreach (String subStr in substrings)
            {
                MissionManager.MissionEventBuffer.Add(subStr);
            }

            eventText.Displayed = true;

            if (clearResponse)
            {
                ClearResponseText();
                mission.MissionResponse = 0;
            }
        }

        public void ShowEvent(List<EventText> eventText)
        {
            for (int i = 0; i < eventText.Count; i++)
            {
                ShowEvent(eventText[i]);
            }
        }

        public void ShowResponse(EventText responseText)
        {
            game.stateManager.stationState.SubStateManager.MissionMenuState.ActiveMission = mission;
            mission.ResponseBuffer.Add(responseText.Text);

            responseText.Displayed = true;
        }

        public void ShowResponse(List<EventText> responseText)
        {
            for (int i = 0; i < responseText.Count; i++)
            {
                ShowResponse(responseText[i]);
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

        public bool IsResponseTextCleared()
        {
            return mission.ResponseBuffer.Count == 0;
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
