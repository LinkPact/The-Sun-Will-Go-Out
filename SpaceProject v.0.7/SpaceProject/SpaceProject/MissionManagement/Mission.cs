using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
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

    public abstract class Mission
    {
        protected Game1 Game;
        protected Sprite spriteSheet;
        protected string section;

        protected bool updateLogic;

        protected ConfigFile configFile;
        private string configSection;

        private string[,] eventArray;
        private List<string> eventBuffer;
        private List<string> responseBuffer;

        public int MissionResponse;

        protected int progress;

        private string missionName;
        private string locationName;
        private string missionText;
        private string posResponse;
        private string negResponse;
        private string[] acceptText;
        private int acceptIndex;
        private string acceptFailedText;
        private string completedText;
        private string objectiveCompleted;
        private string failedText;
        private string objectiveFailed;

        private Boolean restartAfterFail = false;

        private StateOfMission missionState;

        private int moneyReward;
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

        protected GameObjectOverworld objectiveDestination;
        public GameObjectOverworld ObjectiveDestination { get { return objectiveDestination; } set { objectiveDestination = value; } }

        // Change this to true if your mission requires an available slot to accept.
        protected bool requiresAvailableSlot;

        protected MissionHelper missionHelper;
        public MissionHelper MissionHelper { get { return missionHelper; } private set { ;} }

        #region Properties

        public bool UpdateLogic { get { return updateLogic; } set { updateLogic = value;} }

        public string ConfigSection
        {
            get { return configSection; }
        }

        public string[,] EventArray
        {
            get { return eventArray; }
            set { eventArray = value; }
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

        public string MissionName
        {
            get { return missionName; }
        }

        public string PlanetName
        {
            get { return locationName; }
        }

        public string MissionText
        {
            get { return missionText; }
            set { missionText = value; }
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
            get { return moneyReward; }
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

        public bool RequiresAvailableSlot { get { return requiresAvailableSlot; } private set { ; } }

        #endregion

        protected Mission(Game1 Game, string section, Sprite spriteSheet) :
            this(Game, section)
        {
            if (spriteSheet != null)
                this.spriteSheet = spriteSheet;

            this.Game = Game;
            this.section = section;
            configFile = new ConfigFile();
            configFile.Load("Data/missiondata.dat");
        }

        protected Mission(Game1 Game, string section)
        {
            this.Game = Game;
            this.section = section;
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
        }

        public virtual void StartMission()
        { }

        public virtual void OnLoad()
        { }

        public virtual void OnReset()
        {
            ResetEventText();

            foreach (Objective obj in objectives)
            {
                obj.Reset();
            }
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
                                   new Vector2(Game.Window.ClientBounds.Width / 4, Game.Window.ClientBounds.Height * 1 / 3 + 20) + Game.fontManager.FontOffset,
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
                                   TextUtils.WordWrap(font, (tempString + CurrentObjectiveDescription), (Game.Window.ClientBounds.Width / 2) - 10),
                                   new Vector2(10, (Game.Window.ClientBounds.Height * 1 / 3) + 40) + Game.fontManager.FontOffset,
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

                currentObjective.Update();

                if (missionHelper.AllObjectivesCompleted())
                {
                    MissionManager.MarkMissionAsCompleted(this.MissionName);
                }
            }
        }

        public abstract int GetProgress();
        public abstract void SetProgress(int progress);

        private void LoadMissionData()
        {
            List<String> lines = configFile.GetAllLinesInSection(section);

            int eventCounter = 0;
            int responseCounter = 1;

            for (int i = 0; i < lines.Count; i++)
            {
                String[] split = lines[i].Split('=');
                split[0] = split[0].Trim();
                split[0] = split[0] + "=";

                if (split[0].Contains("EventText"))
                {
                    if (!split[0].Contains("Response"))
                    {
                        if (!lines[i + 1].Contains("Response"))
                        {
                            eventCounter++;
                        }
                        continue;
                    }

                    else
                    {
                        responseCounter++;

                        if (!lines[i + 1].Contains("Response"))
                        {
                            eventCounter++;
                        }
                    }
                }
            }

            eventArray = new String[eventCounter, responseCounter];

            eventCounter = 0;
            responseCounter = 1;

            for (int i = 0; i < lines.Count; i++)
            {
                String[] split = lines[i].Split('=');
                split[0] = split[0].Trim();
                split[1] = split[1].Trim();
                split[0] = split[0] + "=";

                if (split[0].Contains("Location="))
                {
                    locationName = split[1];
                }

                else if (split[0].Contains("Name="))
                {
                    missionName = split[1];
                }

                else if (split[0].Contains("Introduction="))
                {
                     missionText = split[1];
                }

                else if (split[0].Contains("PosResponse="))
                {
                    posResponse = split[1];
                }

                else if (split[0].Contains("NegResponse="))
                {
                    negResponse = split[1];
                }

                else if (split[0].Contains("Accept="))
                {
                    acceptText[0] = split[1];
                }

                else if (split[0].Contains("FailAccept="))
                {
                    acceptFailedText = split[1];
                }

                else if (split[0].Contains("Success="))
                {
                    completedText = split[1];
                }

                else if (split[0].Contains("ObjectiveText"))
                {
                    ObjectiveDescriptions.Add(split[1]);
                }

                else if (split[0].Contains("ObjectiveCompleted="))
                {
                    objectiveCompleted = split[1];
                }

                else if (split[0].Contains("ObjectiveFailed="))
                {
                    objectiveFailed = split[1];
                }

                else if (split[0].Contains("Reward="))
                {
                    moneyReward = Int32.Parse(split[1]);
                }

                else if (split[0].Contains("Progress="))
                {
                    progressReward = Int32.Parse(split[1]);
                }

                else if (split[0].Contains("Reputation="))
                {
                    reputationReward = Int32.Parse(split[1]);
                }

                else if (split[0].Contains("State="))
                {
                    missionState = (StateOfMission)Int32.Parse(split[1]);
                }

                else if (split[0].Contains("Fail="))
                {
                    failedText = split[1];
                }

                else if (split[0].Contains("EventText"))
                {
                    if (!split[0].Contains("Response"))
                    {
                        EventArray[eventCounter, 0] = split[1];
                        if (!lines[i + 1].Contains("Response"))
                        {
                            eventCounter++;
                        }
                        continue;
                    }

                    else
                    {
                        EventArray[eventCounter, responseCounter] = split[1];
                        responseCounter++;

                        if (!lines[i + 1].Contains("Response"))
                        {
                            eventCounter++;
                        }
                    }
                }
            }
        }

        private void ResetEventText()
        {
            List<String> lines = configFile.GetAllLinesInSection(section);

            int eventCounter = 0;
            int responseCounter = 1;

            for (int i = 0; i < lines.Count; i++)
            {
                String[] split = lines[i].Split('=');
                split[0] = split[0].Trim();
                split[1] = split[1].Trim();
                split[0] = split[0] + "=";

                if (split[0].Contains("EventText"))
                {
                    if (!split[0].Contains("Response"))
                    {
                        EventArray[eventCounter, 0] = split[1];
                        if (!lines[i + 1].Contains("Response"))
                        {
                            eventCounter++;
                        }
                        continue;
                    }

                    else
                    {
                        EventArray[eventCounter, responseCounter] = split[1];
                        responseCounter++;

                        if (!lines[i + 1].Contains("Response"))
                        {
                            eventCounter++;
                        }
                    }
                }
            }
        }
    }
}
