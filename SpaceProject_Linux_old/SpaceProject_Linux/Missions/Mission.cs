﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace SpaceProject_Linux
{
    public enum StateOfMission
    { 
        Unavailable,            // 0
        Available,              // 1
        Active,                 // 2
        Completed,              // 3
        Failed,                 // 4
        CompletedDead,          // 5
        FailedDead              // 6
    }

    public enum TextType
    {
        Introduction,
        Event,
        Completed,
        Failed
    }

    public abstract class Mission
    {
        protected Game1 Game;
        protected Sprite spriteSheet;
        protected string section;

        protected bool updateLogic;

        protected ConfigFile configFile;
        private string configSection = "";

        private List<KeyValuePair<EventText, List<EventText>>> eventList;

        private List<string> eventBuffer;
        private List<string> responseBuffer;

        public int MissionResponse;

        protected int progress;

        private MissionID missionID;
        private string missionName;
        private string locationName;
        private string endLocationName;

        private string introductionText;
        private string posResponse;
        private string negResponse;
        private string[] acceptText;
        private int acceptIndex;
        private string acceptFailedText;
        private string completedText;
        private string objectiveCompleted;
        private string failedText;
        private string objectiveFailed;

        protected Boolean isMainMission = false;
        private Boolean restartAfterFail = false;

        private StateOfMission missionState;

        protected int moneyReward;
        private int progressReward;
        private int reputationReward;

        private List<Item> rewardItems;

        protected List<Objective> objectives;
        private Objective currentObjective;
        private int objectiveIndex;
        private List<string> objectiveDescriptions;
        private string currentObjectiveDescription;

        protected string tempString = "";

        protected MineMenuState MiningMenuState;
        protected MissionMenuState MissionMenuState;
        protected OverviewMenuState OverviewMenuState;
        protected InfoMenuState PlanetInfoMenuState;
        protected RumorsMenuState RumorsMenuState;
        protected ShopMenuState ShopMenuState;

        protected List<GameObjectOverworld> destinations;
        protected GameObjectOverworld objectiveDestination;
        public GameObjectOverworld ObjectiveDestination { get { return objectiveDestination; } set { objectiveDestination = value; } }
        public List<GameObjectOverworld> Destinations { get { return destinations; } private set { ; } }

        // Change this to true if your mission requires an available slot to accept.
        protected bool requiresAvailableSlot;

        protected MissionHelper missionHelper;
        public MissionHelper MissionHelper { get { return missionHelper; } private set { ;} }

        #region Properties

        public bool UpdateLogic { get { return updateLogic; } set { updateLogic = value; } }

        public Boolean IsMainMission
        {
            get { return isMainMission; }
        }

        public string ConfigSection
        {
            get { return configSection; }
        }

        public List<KeyValuePair<EventText, List<EventText>>> EventList
        {
            get { return eventList; }
            set { eventList = value; }
        }

        public List<string> EventBuffer
        {
            get { return eventBuffer; }
            set { eventBuffer = value; }
        }

        public List<string> ResponseBuffer
        {
            get { return responseBuffer; }
            set { responseBuffer = value; }
        }

        public MissionID MissionID
        {
            get { return missionID; }
        }

        public string MissionName
        {
            get { return missionName; }
        }

        public string LocationName
        {
            get { return locationName; }
        }

        public string EndLocationName
        {
            get
            {
                if (endLocationName == null
                    || endLocationName.Equals(""))
                {
                    return locationName;
                }
                else
                {
                    return endLocationName;
                }
            }
        }

        public string IntroductionText
        {
            get { return introductionText; }
            set { introductionText = value; }
        }

        public List<Objective> Objectives
        {
            get { return objectives; }
        }

        public List<string> ObjectiveDescriptions
        {
            get { return objectiveDescriptions; }
            set { objectiveDescriptions = value; }
        }

        public int ObjectiveIndex
        {
            get { return objectiveIndex; }
            set { objectiveIndex = value; }
        }

        public Objective CurrentObjective { get { return currentObjective; } set { currentObjective = value; } }

        public string CurrentObjectiveDescription
        {
            get { return currentObjectiveDescription; }
            set { currentObjectiveDescription = value; }
        }

        public string PosResponse
        {
            get { return posResponse; }
        }

        public string NegResponse
        {
            get { return negResponse; }
        }

        public string[] AcceptText
        {
            get { return acceptText; }
            set { acceptText = value; }
        }

        public int AcceptIndex
        {
            get { return acceptIndex; }
            set { acceptIndex = value; }
        }

        public string AcceptFailedText
        {
            get { return acceptFailedText; }
        }

        public string CompletedText
        {
            get { return completedText; }
            set { completedText = value; }
        }

        public string ObjectiveCompleted
        {
            get { return objectiveCompleted; }
            set { objectiveCompleted = value; }
        }

        public string FailedText
        {
            get { return failedText; }
        }

        public string ObjectiveFailed
        {
            get { return objectiveFailed; }
            set { objectiveFailed = value; }
        }

        public StateOfMission MissionState
        {
            get { return missionState; }
            set { missionState = value; }
        }

        public int MoneyReward
        {
            // Adjusted for difficulty by moneyFactor
            get { return (int)(moneyReward * StatsManager.moneyFactor); }
        }

        public int ProgressReward
        {
            get { return progressReward; }
        }

        public int ReputationReward
        {
            get { return reputationReward; }
        }

        public List<Item> RewardItems
        {
            get { return rewardItems; }
            set { rewardItems = value; }
        }

        protected void RestartAfterFail()
        {
            restartAfterFail = true;
        }

        public Boolean IsRestartAfterFail()
        {
            return restartAfterFail;
        }

        public bool RequiresAvailableSlot { get { return requiresAvailableSlot; } }

        #endregion

        protected Mission(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            this(Game, section, missionID)
        {
            if (spriteSheet != null)
                this.spriteSheet = spriteSheet;
        }

        protected Mission(Game1 Game, string section, MissionID missionID)
        {
            this.Game = Game;
            this.section = section;
            this.missionID = missionID;
            configFile = new ConfigFile();
            configFile.Load("Data/missiondata.dat");
        }

        public virtual void Initialize()
        {
            missionHelper = new MissionHelper(Game, this);
            missionHelper.Initialize();

            objectives = new List<Objective>();
            objectiveDescriptions = new List<string>();

            rewardItems = new List<Item>();

            OverviewMenuState = Game.stateManager.planetState.SubStateManager.OverviewMenuState;
            MiningMenuState = Game.stateManager.planetState.SubStateManager.MiningMenuState;
            MissionMenuState = Game.stateManager.planetState.SubStateManager.MissionMenuState;
            PlanetInfoMenuState = Game.stateManager.planetState.SubStateManager.InfoMenuState;
            RumorsMenuState = Game.stateManager.planetState.SubStateManager.RumorsMenuState;
            ShopMenuState = Game.stateManager.planetState.SubStateManager.ShopMenuState;

            eventBuffer = MissionManager.MissionEventBuffer;
            responseBuffer = MissionManager.MissionResponseBuffer;
            acceptText = new string[1];

            LoadMissionData();

            SetMissionType();
        }

        public virtual void StartMission()
        { }

        public virtual void OnLoad()
        { }

        public virtual void OnReset()
        {
            ResetEventText();
            ObjectiveIndex = 0;

            for (int i = 0; i < objectives.Count; i++)
            {
                objectives[i].Destination = destinations[i];
                objectives[i].Reset();
            }
        }

        public virtual void OnFailed()
        {

        }

        public virtual void MissionLogic()
        {
            RefreshCurrentObjective();
            missionHelper.Update();
        }

        public virtual void DisplayMissionInfo(SpriteBatch spriteBatch, SpriteFont font)
        {
            string tempString = "";

            spriteBatch.DrawString(font,
                                   MissionName,
                                   new Vector2(Game.ScreenSize.X / 4, Game.ScreenSize.Y * 1 / 3 + 20) + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   0,
                                   font.MeasureString(MissionName) / 2,
                                   1.0f,
                                   SpriteEffects.None,
                                   0.5f);

            if (MissionState.Equals(StateOfMission.Active))
                tempString = "[Objective] ";

            else if (MissionState.Equals(StateOfMission.CompletedDead))
                tempString = "[Completed] ";

            else if (MissionState.Equals(StateOfMission.FailedDead))
                tempString = "[Failed] ";

            spriteBatch.DrawString(font,
                                   TextUtils.WordWrap(font, (tempString + CurrentObjectiveDescription), (Game.ScreenSize.X / 2) - 10),
                                   new Vector2(10, (Game.ScreenSize.Y * 1 / 3) + 40) + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   0,
                                   new Vector2(0, 0),
                                   1.0f,
                                   SpriteEffects.None,
                                   0.5f);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (currentObjective != null)
            {
                currentObjective.Draw(spriteBatch);
            }
        }

        private void RefreshCurrentObjective()
        {
            if (objectives.Count > 0)
            {
                int previousObjectiveIndex = objectives.IndexOf(currentObjective);

                currentObjective = objectives[objectiveIndex];
                currentObjectiveDescription = currentObjective.Description;

                if (previousObjectiveIndex != objectives.IndexOf(currentObjective))
                {
                    currentObjective.OnActivate();
                    currentObjectiveDescription = currentObjective.Description;
                }

                currentObjective.Update(StatsManager.PlayTime);

                if (missionHelper.AllObjectivesCompleted())
                {
                    MissionManager.MarkMissionAsCompleted(this.MissionID);
                }
            }
        }

        public abstract int GetProgress();
        public abstract void SetProgress(int progress);

        private void LoadMissionData()
        {
            List<String> lines = configFile.GetAllLinesInSection(section);

            SetMissionDataFromFile(lines);
        }

        private void SetMissionDataFromFile(List<String> lines)
        {
            eventList = new List<KeyValuePair<EventText, List<EventText>>>();

            for (int linePos = 0; linePos < lines.Count; linePos++)
            {
                String line = lines[linePos];

                Match match = Regex.Match(line, @"([a-zA-Z]+).*=\s+(.+)");
                String key = match.Groups[1].Value;
                String value = match.Groups[2].Value;

                if (key == "EventText")
                {
                    if (!value.Contains("Response"))
                    {
                        KeyValuePair<EventText, List<EventText>> newEventEntry
                            = new KeyValuePair<EventText, List<EventText>>(new EventText(value), new List<EventText>());

                        while (lines[linePos + 1].Contains("Response"))
                        {
                            linePos++;

                            Match responseMatch = Regex.Match(lines[linePos], @"(\w+)\s*=\s*(.+)");
                            String responseValue = responseMatch.Groups[2].Value;
                            newEventEntry.Value.Add(new EventText(responseValue));
                        }
                        EventList.Add(newEventEntry);
                    }
                }
                else
                {
                    SetMissionValues(key, value);
                }
            }
        }

        private void SetMissionValues(String key, String value)
        {
            switch (key)
            {
                case "Location":
                    {
                        locationName = value;
                        break;
                    }
                case "EndLocation":
                    {
                        endLocationName = value;
                        break;
                    }
                case "Name":
                    {
                        missionName = value;
                        break;
                    }
                case "Introduction":
                    {
                        introductionText = value;
                        break;
                    }
                case "PosResponse":
                    {
                        posResponse = value;
                        break;
                    }
                case "NegResponse":
                    {
                        negResponse = value;
                        break;
                    }
                case "Accept":
                    {
                        acceptText[0] = value;
                        break;
                    }
                case "FailAccept":
                    {
                        acceptFailedText = value;
                        break;
                    }
                case "Success":
                    {
                        completedText = value;
                        break;
                    }
                case "ObjectiveText":
                    {
                        ObjectiveDescriptions.Add(value);
                        break;
                    }
                case "ObjectiveCompleted":
                    {
                        objectiveCompleted = value;
                        break;
                    }
                case "ObjectiveFailed":
                    {
                        objectiveFailed = value;
                        break;
                    }
                case "Reward":
                    {
                        moneyReward = Int32.Parse(value);
                        break;
                    }
                case "Progress":
                    {
                        progressReward = Int32.Parse(value);
                        break;
                    }
                case "Reputation":
                    {
                        reputationReward = Int32.Parse(value);
                        break;
                    }
                case "State":
                    {
                        missionState = (StateOfMission)Int32.Parse(value);
                        break;
                    }
                case "Fail":
                    {
                        failedText = value;
                        break;
                    }
                //default:
                //    {
                //        //throw new ArgumentException("Unhandled information loaded");
                //    }
            }
        }

        private void ResetEventText()
        {
            foreach (KeyValuePair<EventText, List<EventText>> index in EventList)
            {
                if (index.Key.Text.Contains("/ok"))
                {
                    index.Key.Text = index.Key.Text.Replace("/ok", "");
                }
            }

            if (introductionText.Contains("/ok"))
            {
                introductionText = introductionText.Replace("/ok", "");
            }
        }

        public EventText GetEvent(int eventID)
        {
            return EventList[eventID].Key;
        }

        protected EventText GetResponse(int eventID, int responseIndex)
        {
            return EventList[eventID].Value[responseIndex];
        }

        protected List<EventText> GetAllResponses(int eventID)
        {
            return EventList[eventID].Value;
        }

        public void ReplaceObjectiveText(TextType textType, string ID, Object replacement, int index = 0)
        {
            switch (textType)
            {
                case TextType.Introduction:
                    if (introductionText.Contains(ID))
                    {
                        introductionText =
                            introductionText.Replace(ID, replacement.ToString());
                    }
                    break;

                case TextType.Event:
                    if (eventList[index].Key.Text.Contains(ID))
                    {
                        eventList[index].Key.Text =
                            eventList[index].Key.Text.Replace(ID, replacement.ToString());
                    }
                    break;

                case TextType.Completed:
                    if (completedText.Contains(ID))
                    {
                        completedText =
                            completedText.Replace(ID, replacement.ToString());
                    }
                    break;

                case TextType.Failed:
                    if (failedText.Contains(ID))
                    {
                        failedText =
                            failedText.Replace(ID, replacement.ToString());
                    }
                    break;
            }
        }

        protected abstract void SetDestinations();
        protected abstract void SetupObjectives();

        private void SetMissionType()
        {
            StringBuilder stringBuilder = new StringBuilder(missionName);

            if (isMainMission)
            {
                stringBuilder.Insert(0, "Main - ");
            }
            else
            {
                stringBuilder.Insert(0, "Secondary - ");
            }

            missionName = stringBuilder.ToString();
        }

        protected void AddDestination(GameObjectOverworld destination, int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                destinations.Add(destination);
            }
        }
    }
}
