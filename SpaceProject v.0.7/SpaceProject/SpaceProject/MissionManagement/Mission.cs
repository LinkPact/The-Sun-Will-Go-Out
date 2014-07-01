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
        private string planetName;
        //private string stationName;
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
            get { return planetName; }
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

            this.section = section;
        }

        protected Mission(Game1 Game, string section)
        {
            this.Game = Game;

            configFile = new ConfigFile();
            configFile.Load("Data/missiondata.dat");
            this.configSection = section;

            eventBuffer = MissionManager.MissionEventBuffer;
            responseBuffer = MissionManager.MissionResponseBuffer;
            acceptText = new string[1];

            missionName = "";
            missionText = "";
            posResponse = "";
            negResponse = "";
            completedText = "";
            failedText = "";

            missionState = StateOfMission.Available;

            moneyReward = 0;
            progressReward = 0;
            reputationReward = 0;
            rewardItems = new List<Item>();

            objectives = new List<Objective>();
            objectiveDescriptions = new List<string>();

            OverviewMenuState = Game.stateManager.planetState.SubStateManager.OverviewMenuState;
            MiningMenuState = Game.stateManager.planetState.SubStateManager.MiningMenuState;
            MissionMenuState = Game.stateManager.planetState.SubStateManager.MissionMenuState;
            PlanetInfoMenuState = Game.stateManager.planetState.SubStateManager.InfoMenuState;
            RumorsMenuState = Game.stateManager.planetState.SubStateManager.RumorsMenuState;
            ShopMenuState = Game.stateManager.planetState.SubStateManager.ShopMenuState;

        }

        public virtual void Initialize()
        {
            missionHelper = new MissionHelper(Game, this);
            missionHelper.Initialize();

            objectives = new List<Objective>();

            planetName = configFile.GetPropertyAsString(configSection, "Planet", "");
            missionName = configFile.GetPropertyAsString(configSection, "Name", "");
            missionText = configFile.GetPropertyAsString(configSection, "Text", "");
            posResponse = configFile.GetPropertyAsString(configSection, "PosResponse", "");
            negResponse = configFile.GetPropertyAsString(configSection, "NegResponse", "");
            acceptText[0] = configFile.GetPropertyAsString(configSection, "Accept", "");
            acceptFailedText = configFile.GetPropertyAsString(configSection, "FailAccept", ""); 
            completedText = configFile.GetPropertyAsString(configSection, "Success", "");
            objectiveCompleted = configFile.GetPropertyAsString(configSection, "ObjectiveCompleted", "");
            failedText = configFile.GetPropertyAsString(configSection, "Fail", "");
            objectiveFailed = configFile.GetPropertyAsString(configSection, "ObjectiveFailed", "");
            
            moneyReward = configFile.GetPropertyAsInt(configSection, "Reward", 0);
            progressReward = configFile.GetPropertyAsInt(configSection, "Progress", 0);
            reputationReward = configFile.GetPropertyAsInt(configSection, "Reputation", 0);

            missionState = (StateOfMission)configFile.GetPropertyAsInt(configSection, "State", 0);
        }

        public virtual void StartMission()
        { }

        public virtual void OnLoad()
        { }

        public virtual void OnReset()
        {
            for (int i = 0; i < EventArray.GetLength(0); i++)
            {
                for (int j = 0; j < EventArray.GetLength(1); j++)
                {
                    EventArray[i, j] = configFile.GetPropertyAsString(section, "EventText" + (i + 1), "");
                }
            }

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

        public virtual void Draw(SpriteBatch spriteBatch) { }

        private void RefreshCurrentObjective()
        {
            if (objectives.Count > 0)
            {
                int previousObjectiveIndex = objectives.IndexOf(currentObjective);
                currentObjective = objectives[objectiveIndex];

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
    }
}
