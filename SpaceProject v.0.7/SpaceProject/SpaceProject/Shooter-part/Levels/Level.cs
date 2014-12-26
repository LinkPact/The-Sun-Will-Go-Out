using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceProject
{
    public enum MissionType
    {
        rebel,
        pirate,
        dark,
        power,
        none
    }

    public abstract class Level
    {
        #region declaration

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int LevelWidth { get; set; }
        public int LevelHeight { get { return 600; } }
        public int RelativeOrigin
        {
            get
            { return WindowWidth / 2 - LevelWidth / 2; }
        }
        public int BorderDrawStart {
            get { return RelativeOrigin - 800; }
        }
        //private float DrawLayer = 0.7f;

        protected PlayerVerticalShooter player;

        protected Sprite spriteSheet;
        protected Game1 Game;

        protected MissionType missionType;
        public MissionType GetMissionType() { return missionType; }

        public string Name;
        protected SpriteFont font1;
        protected Random random;
        //protected SpawnController spawnController;
        protected BackgroundManager backgroundManager;
        private Sprite border;

        protected LevelObjective levelObjective;

        protected float victoryTime;
        protected float maxTime;
        protected string EndText;

        protected int killCountForVictory;
        protected int letThroughCountForLoss;

        protected Boolean winOnFinish;
        private bool levelGivenUp;

        protected float customStartTime;
        public void SetCustomStartTime(float customStartTime) { this.customStartTime = customStartTime; }
        private Boolean isCustomStartSet;
        public Boolean IsCustomStartSet() { return isCustomStartSet; }

        private bool firstIterationGameOver;

        public int LevelLoot;

        protected Song levelSong;

        protected List<LevelEvent> untriggeredEvents = new List<LevelEvent>();
        private List<LevelEvent> activeEvents = new List<LevelEvent>();
        private List<LevelEvent> deadEvents = new List<LevelEvent>();

        public Boolean HasBossEvents()
        {
            foreach (LevelEvent event_ in untriggeredEvents)
            {
                if (event_ is BossLevelEvent)
                    return true;
            }

            foreach (LevelEvent event_ in activeEvents)
            {
                if (event_ is BossLevelEvent)
                    return true;
            }
            return false;
        }

        bool eventsOver;

        public Vector2 PlayerPosition { get { return player.Position; } }

        #region runtimeVariables
        public int enemiesKilled;
        public int enemiesLetThrough;
        public int totalLevelEnemyCount;
        public float playTime;
        public float PlayTimeRounded { get { return (int)(playTime / 1000);} private set { } }
        #endregion

        private LevelLogger levelLogger;
        private static Boolean isLogging = true;
        public static Boolean IsLogging { get { return isLogging; } }
        private static float accumulatedShipDamage;
        public static void AddShipDamage(float damage) { accumulatedShipDamage += damage; }
        private static float accumulatedShieldDamage;
        public static void AddShieldDamage(float damage) { accumulatedShieldDamage += damage; }

        #endregion

        protected Level(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, MissionType missionType)
        {
            this.Game = Game;
            border = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/BorderSprite"), new Rectangle(5, 5, 1, 1));
            this.spriteSheet = spriteSheet;
            this.player = player;
            random = new Random();
            customStartTime = -1;
            this.missionType = missionType;

            levelObjective = LevelObjective.Finish;
            winOnFinish = true;
        }

        public void finishLevel_DEVELOPONLY()
        {
            if (StatsManager.gameMode != GameMode.develop)
                throw new ArgumentException("This function should ONLY be called for debug purposes // Jakob");

            levelObjective = LevelObjective.KillNumber;
            killCountForVictory = 0;
        }

        public bool IsMapCompleted
        {
            get
            {
                return (eventsOver && !CheckLivingEnemies() && player.HP > 0);
            }
        }

        public bool IsObjectiveCompleted
        {
            get
            {
                bool isLevelCompleted = false;
                switch (levelObjective)
                {
                    case LevelObjective.Time:
                        {
                            if (playTime > victoryTime && player.HP > 0)
                                isLevelCompleted = true;

                            break;
                        }
                    case LevelObjective.KillPercentage:
                    case LevelObjective.KillNumber:
                    case LevelObjective.KillNumberOrSurvive:
                    case LevelObjective.KillPercentageOrSurvive:
                        {
                            if (enemiesKilled >= killCountForVictory)
                                isLevelCompleted = true;

                            break;
                        }
                    case LevelObjective.Boss:
                        {
                            Boolean hasLevelBossesLeft = HasBossEvents();
                            isLevelCompleted = !hasLevelBossesLeft;
                            break;
                        }
                }

                if (winOnFinish && IsMapCompleted)
                    isLevelCompleted = true;

                return isLevelCompleted;
            }
        }
        
        public bool IsGameOver
        {
            get
            {
                bool IsGameOver = false;
                if (player.IsKilled || levelGivenUp)
                {
                    IsGameOver = true;
                }

                switch (levelObjective)
                {
                    case LevelObjective.CountMayNotPass:
                        {
                            if (enemiesLetThrough >= letThroughCountForLoss)
                                IsGameOver = true;
                            break;
                        }
                    case LevelObjective.KillPercentage:
                    case LevelObjective.KillNumber:
                        {
                            if (IsMapCompleted && !IsObjectiveCompleted)
                                IsGameOver = true;
                            break;
                        }
                }

                return IsGameOver;
            }
        }

        public virtual void Initialize()
        {
            levelLogger = new LevelLogger(accumulatedShipDamage.ToString() + "/" + accumulatedShieldDamage.ToString());
            
            Game.stateManager.shooterState.gameObjects.Clear();
            CreatePlayer();

            WindowWidth = Game.Window.ClientBounds.Width;
            WindowHeight = Game.Window.ClientBounds.Height;

            levelGivenUp = false;

            if (customStartTime == -1)
            {
                playTime = 0;
                isCustomStartSet = false;
            }
            else
            {
                customStartTime *= 1000;
                playTime = customStartTime;
                isCustomStartSet = true;
            }

            Game.stateManager.shooterState.backgroundObjects.Clear();
            backgroundManager = new BackgroundManager(Game, player, this);
            backgroundManager.Initialize(BackgroundType.deadSpace);

            font1 = Game.fontManager.GetFont(14);

            player.AmassedCopper = 0;
            player.AmassedGold = 0;
            player.AmassedTitanium = 0;

            firstIterationGameOver = true;

            untriggeredEvents.Clear();
            activeEvents.Clear();

            eventsOver = false;

            enemiesKilled = 0;
            enemiesLetThrough = 0;

            if (levelSong != null) Game.musicManager.PlayMusic(Music.Asteroids);

            CalculateLevelEnemyCount();
        }

        private void CreatePlayer()
        {
            player = new PlayerVerticalShooter(Game, spriteSheet);
            player.Initialize();
            player.SetLevelWidth(LevelWidth);
            Game.stateManager.shooterState.gameObjects.Add(player);
        }

        protected void CustomStartSetup()
        {
            List<LevelEvent> remove = new List<LevelEvent>();
            foreach (LevelEvent event_ in untriggeredEvents)
            {
                if (event_ is PointLevelEvent)
                {
                    if (event_.StartTime < customStartTime)
                    {
                        remove.Add(event_);
                    }
                }
                else if (event_ is LastingLevelEvent)
                {
                    if (event_.StartTime < customStartTime)
                    {
                        if (((LastingLevelEvent)event_).IsTimeDuringEvent(customStartTime))
                        {
                            ((LastingLevelEvent)event_).CutDown(customStartTime);
                        }
                        else
                        {
                            remove.Add(event_);
                        }
                    }
                }
            }

            foreach (LevelEvent event_ in remove)
            {
                untriggeredEvents.Remove(event_);
            }
            remove.Clear();
        }

        private void ManageEvents(GameTime gameTime)
        {
            foreach (LevelEvent event_ in untriggeredEvents)
            {
                event_.AttemptTrigger(playTime);

                if (event_.RetrieveTriggerStatus().Equals("Running")) activeEvents.Add(event_);
            }

            foreach (LevelEvent event_ in activeEvents)
            {
                if (untriggeredEvents.Contains(event_)) untriggeredEvents.Remove(event_);

                event_.Run(gameTime);
                
                if (event_.RetrieveTriggerStatus().Equals("Completed")) deadEvents.Add(event_);
            }

            foreach (LevelEvent event_ in deadEvents)
            {
                activeEvents.Remove(event_);
            }
            deadEvents.Clear();

            if (untriggeredEvents.Count == 0 && activeEvents.Count == 0)
            {
                eventsOver = true;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsGameOver && !levelGivenUp)
            {
                playTime += gameTime.ElapsedGameTime.Milliseconds;
                ManageEvents(gameTime);
            }
            else
                GameIsOver();

            if (!IsObjectiveCompleted && !IsMapCompleted)
                SpawnControlUpdate(gameTime);
            else
            {
                if (IsObjectiveCompleted)
                {
                    MapCompletionLogic();
                    player.TempInvincibility = 1000000;
                }
                else
                {
                    player.TempInvincibility = 1000000;
                }
            }

            backgroundManager.Update(gameTime);
        }

        private void MapCompletionLogic()
        {
            EndText = "";
            if (missionType.Equals(MissionType.pirate))
                EndText = "You earned: " + (int)(LevelLoot * StatsManager.moneyFactor) + " rupees. \n";
            EndText += "Press 'Enter' to continue..";

            if (StatsManager.gameMode == GameMode.hardcore)
                StatsManager.ReduceOverwordHealthToVerticalHealth(player);

            if (ControlManager.CheckKeypress(Keys.Enter))
            {
                LeaveLevel();
            }
        }

        private void LeaveLevel()
        {
            if (missionType.Equals(MissionType.pirate))
                StatsManager.AddLoot((int)(LevelLoot * StatsManager.moneyFactor));
                //StatsManager.Rupees += LevelLoot;
            ReturnToPreviousScreen();
        }

        private void GameIsOver()
        {
            if (firstIterationGameOver)
            {
                firstIterationGameOver = false;
                StatsManager.ReduceShipLife(40);
                if (StatsManager.GetShipLife() <= 0)
                    Game.stateManager.ChangeState("OutroState");
            }
            if (ControlManager.CheckKeypress(Keys.R))
            {
                //this.ResetLevel();
                //Game.stateManager.shooterState.Initialize();
                this.Initialize();
            }

            else if (ControlManager.CheckPress(RebindableKeys.Action2))
            {
                ReturnToPreviousScreen();
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            backgroundManager.Draw(spriteBatch);

            if (LevelWidth < Game.Resolution.X)
            {
                //left border
                //spriteBatch.Draw(
                //    border.Texture,
                //    new Vector2(((float)Game.Window.ClientBounds.Width - (float)LevelWidth) / 2 - (float)Game.Window.ClientBounds.Width / 8, 0),
                //    border.SourceRectangle,
                //    Color.Black,
                //    0.0f,
                //    new Vector2(0, 0),
                //    new Vector2((float)Game.Window.ClientBounds.Width / 8, (float)Game.Window.ClientBounds.Height),
                //    SpriteEffects.None,
                //    DrawLayer);

                spriteBatch.Draw(
                    border.Texture,
                    new Vector2(800 + BorderDrawStart, (WindowHeight - Game.DefaultResolution.Y) / 2),
                    new Rectangle(0, 0, 2, 1),
                    Color.DarkOrange,
                    0.0f,
                    new Vector2(0, 0),
                    new Vector2(1, WindowHeight - (WindowHeight - Game.DefaultResolution.Y)),
                    SpriteEffects.None,
                    0.8f);

                // right Border
                //spriteBatch.Draw(
                //    border.Texture,
                //    new Vector2((float)Game.Window.ClientBounds.Width - ((float)Game.Window.ClientBounds.Width - (float)LevelWidth) / 2, 0),
                //    border.SourceRectangle,
                //    Color.Black,
                //    0.0f,
                //    new Vector2(0, 0),
                //    new Vector2((float)Game.Window.ClientBounds.Width / 8, (float)Game.Window.ClientBounds.Height),
                //    SpriteEffects.None,
                //    DrawLayer);

                spriteBatch.Draw(
                    border.Texture,
                    new Vector2(RelativeOrigin + LevelWidth, (WindowHeight - Game.DefaultResolution.Y) / 2),
                    new Rectangle(0, 0, 2, 1),
                    Color.DarkOrange,
                    0.0f,
                    new Vector2(0, 0),
                    new Vector2(1, WindowHeight - (WindowHeight - Game.DefaultResolution.Y)),
                    SpriteEffects.None,
                    0.8f);

            }
            
            if (Game.Window.ClientBounds.Height > 600)
            {
                //upper border
                //spriteBatch.Draw(border.Texture, new Vector2(((float)Game.Window.ClientBounds.Width - (float)LevelWidth) / 2, 0), 
                //    border.SourceRectangle, Color.Black, 0.0f, Vector2.Zero, 
                //    new Vector2(LevelWidth, (float)(Game.Window.ClientBounds.Height - 600) / 2),  SpriteEffects.None, DrawLayer + 0.01f);

                spriteBatch.Draw( border.Texture, new Vector2((Game.Window.ClientBounds.Width - LevelWidth) / 2,
                                                             (float)(WindowHeight - Game.DefaultResolution.Y) / 2),
                    new Rectangle(0, 0, 1, 2), Color.DarkOrange, 0.0f, new Vector2(0, 0), new Vector2(LevelWidth + 1, 1),
                    SpriteEffects.None, 0.8f);
                
                // lower border
                //spriteBatch.Draw(border.Texture,
                //    new Vector2(((float)Game.Window.ClientBounds.Width - (float)LevelWidth) / 2, (float)Game.Window.ClientBounds.Height - (float)(Game.Window.ClientBounds.Height - 600) / 2),
                //    border.SourceRectangle, Color.Black, 0.0f, Vector2.Zero,
                //    new Vector2(LevelWidth, (float)(Game.Window.ClientBounds.Height - 600) / 2),
                //    SpriteEffects.None, DrawLayer);

                spriteBatch.Draw(
                    border.Texture,
                    new Vector2((float)(Game.Window.ClientBounds.Width - LevelWidth) / 2, WindowHeight - (float)(WindowHeight - Game.DefaultResolution.Y) / 2),
                    new Rectangle(0, 0, 1, 2),
                    Color.DarkOrange,
                    0.0f,
                    new Vector2(0, 0),
                    new Vector2(LevelWidth + 1, 1),
                    SpriteEffects.None,
                    0.8f);
            }
        }
        
        public void DrawEndMessage(SpriteBatch spriteBatch)
        {
            if(IsObjectiveCompleted)
                spriteBatch.DrawString(font1, "Level Completed!\n\n" + EndText,
                                       new Vector2(Game.Window.ClientBounds.Width / 2,
                                                   Game.Window.ClientBounds.Height / 2) + Game.fontManager.FontOffset,
                                       Game.fontManager.FontColor,
                                       0f,
                                       font1.MeasureString("Level Completed!\n\n" + EndText) / 2,
                                       1f,
                                       SpriteEffects.None,
                                       0.9f);

            else if (IsGameOver)
                spriteBatch.DrawString(font1, "You are dead!\n\nPress 'R' to try again '" + ControlManager.GetKeyName(RebindableKeys.Action2) + "' to go back.", 
                                       new Vector2(Game.Window.ClientBounds.Width / 2,
                                                   Game.Window.ClientBounds.Height / 2) + Game.fontManager.FontOffset,
                                       Game.fontManager.FontColor,
                                       0f,
                                       font1.MeasureString("You are dead!\n\nPress R to try again or '" + ControlManager.GetKeyName(RebindableKeys.Action2) + "' to go back.") / 2,
                                       1f,
                                       SpriteEffects.None,
                                       1f);

            else if (levelGivenUp)
                spriteBatch.DrawString(font1, "You gave up!\n\nPress 'R' to try again or '" + ControlManager.GetKeyName(RebindableKeys.Action2) + "' to go back.",
                                       new Vector2(Game.Window.ClientBounds.Width / 2,
                                                   Game.Window.ClientBounds.Height / 2) + Game.fontManager.FontOffset,
                                       Game.fontManager.FontColor,
                                       0f,
                                       font1.MeasureString("You are dead!\n\nPress R to try again or '" + ControlManager.GetKeyName(RebindableKeys.Action2) + "' to go back.") / 2,
                                       1f,
                                       SpriteEffects.None,
                                       1f);
        }

        #region setVictoryConditions

        public void SetCustomVictoryCondition(LevelObjective objective, int objectiveValue)
        {
            switch (objective)
            {
                case LevelObjective.Finish:
                    {
                        SetVictoryToFinish();
                        break;
                    }
                case LevelObjective.Time:
                    {
                        SetVictoryToTime(objectiveValue);
                        break;
                    }
                case LevelObjective.KillNumber:
                    {
                        SetVictoryToKillCount(objectiveValue);
                        break;
                    }
                case LevelObjective.CountMayNotPass:
                    {
                        SetVictoryToLetThroughCount(objectiveValue);
                        break;
                    }
                case LevelObjective.KillPercentage:
                    {
                        CalculateLevelEnemyCount();
                        int killCount = (int)((float)totalLevelEnemyCount * ((float)objectiveValue / 100));
                        SetVictoryToKillCount(killCount);
                        break;
                    }
                case LevelObjective.KillNumberOrSurvive:
                    {
                        SetVictoryToKillNumberOrSurvive(objectiveValue);
                        break;
                    }
                case LevelObjective.KillPercentageOrSurvive:
                    {
                        CalculateLevelEnemyCount();
                        int killCount = (int)((float)totalLevelEnemyCount * ((float)objectiveValue / 100));
                        SetVictoryToKillNumberOrSurvive(killCount);
                        break;
                    }
                case LevelObjective.Boss:
                    {
                        SetVictoryToKillBossEvents(objectiveValue);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Non-implemented objective encountered!");
                    }
            }
        }

        private void SetVictoryToTime(float time)
        {
            levelObjective = LevelObjective.Time;
            victoryTime = time * 1000;
        }
        private void SetVictoryToFinish() 
        {
            levelObjective = LevelObjective.Finish;
            winOnFinish = true;
        }
        private void SetVictoryToKillCount(int objectiveValue)
        {
            levelObjective = LevelObjective.KillNumber;
            killCountForVictory = objectiveValue;
            winOnFinish = false;
        }
        private void SetVictoryToLetThroughCount(int objectiveValue)
        {
            levelObjective = LevelObjective.CountMayNotPass;
            letThroughCountForLoss = objectiveValue;
            winOnFinish = true;
        }
        private void SetVictoryToKillNumberOrSurvive(int objectiveValue)
        {
            levelObjective = LevelObjective.KillNumberOrSurvive;
            killCountForVictory = objectiveValue;
            winOnFinish = true;
        }
        private void SetVictoryToKillBossEvents(int objectiveValue)
        {
            levelObjective = LevelObjective.Boss;
            winOnFinish = true;
        }
        #endregion

        public virtual void SpawnControlUpdate(GameTime gameTime)
        {
            List<GameObjectVertical> listRef = Game.stateManager.shooterState.gameObjects;
        }
        
        public virtual void ResetLevel()
        { }
        
        public virtual void ReturnToPreviousScreen()
        {
            if (GameStateManager.previousState.Equals("StationState"))
                Game.stateManager.stationState.LoadStationData(
                    Game.stateManager.overworldState.GetStation(StationState.PreviousStation));
            else if (GameStateManager.previousState.Equals("PlanetState"))
                Game.stateManager.planetState.LoadPlanetData(
                    Game.stateManager.overworldState.GetPlanet(PlanetState.PreviousPlanet));

            Game.stateManager.ChangeState(GameStateManager.previousState);
        }
        
        protected bool CheckLivingEnemies()
        {
            List<GameObjectVertical> listRef = Game.stateManager.shooterState.gameObjects;

            //Kollar efter levande 
            for (int n = 0; n < listRef.Count; n++)
            {
                if (listRef[n].ObjectClass == "enemy")
                {
                    return true;
                }
            }
            return false;
        }

        protected bool CheckLivingFriends()
        {
            List<GameObjectVertical> listRef = Game.stateManager.shooterState.gameObjects;

            //Kollar efter levande 
            for (int n = 0; n < listRef.Count; n++)
            {
                if (listRef[n].ObjectClass == "friend" || listRef[n].ObjectClass == "player")
                {
                    return true;
                }
            }

            return false;
        }

        public virtual String GetObjectiveString()
        {
            String returnString = "Objective: ";
                       
            switch (levelObjective)
            {
                case LevelObjective.Finish:
                    {
                        returnString += "Finish the level";
                        break;
                    }
                case LevelObjective.CountMayNotPass:
                    {
                        int leftToLetThrough = letThroughCountForLoss - enemiesLetThrough;
                        if (leftToLetThrough < 0)
                            leftToLetThrough = 0;

                        returnString += "Don't let " + leftToLetThrough + " enemies pass";
                        break;
                    }
                case LevelObjective.KillPercentage:
                case LevelObjective.KillNumber:
                    {
                        int leftToKill = killCountForVictory - enemiesKilled;
                        if (leftToKill < 0)
                            leftToKill = 0;

                        returnString += "Kill " + leftToKill + " enemies";
                        break;
                    }
                case LevelObjective.KillPercentageOrSurvive:
                case LevelObjective.KillNumberOrSurvive:
                    {
                        int leftToKill = killCountForVictory - enemiesKilled;
                        if (leftToKill < 0)
                            leftToKill = 0;

                        returnString += "Kill " + leftToKill + " enemies or survive";
                        break;
                    }
                case LevelObjective.Time:
                    {
                        int timeLeft = (int)(victoryTime / 1000 - PlayTimeRounded);
                        if (timeLeft < 0)
                            timeLeft = 0;

                        returnString += " Survive for " + timeLeft + " seconds";
                        break;
                    }
            }
            return returnString;
        }

        protected void CalculateLevelEnemyCount()
        {
            int totalEnemies = 0;
            foreach (LevelEvent evnt in untriggeredEvents)
            {
                totalEnemies += evnt.RetrieveCreatures().Count;
            }

            totalLevelEnemyCount = totalEnemies;
        }

        public void GiveUpLevel()
        {
            levelGivenUp = true;
        }

        public void SetFreighterMaxHP(float MaxHP)
        {
            foreach (GameObjectVertical obj in Game.stateManager.shooterState.gameObjects)
            {
                if (obj is FreighterAlly)
                {
                    ((FreighterAlly)obj).HPmax = MaxHP;
                }
            }
        }

        public void SetFreighterHP(float HP)
        {
            foreach (GameObjectVertical obj in Game.stateManager.shooterState.gameObjects)
            {
                if (obj is FreighterAlly)
                {
                    ((FreighterAlly)obj).HP = HP;
                }
            }
        }

        public float GetFreighterHP()
        {
            foreach (GameObjectVertical obj in Game.stateManager.shooterState.gameObjects)
            {
                if (obj is FreighterAlly)
                {
                    return ((FreighterAlly)obj).HP;
                }
            }

            return -1;
        }

        // Insert position in relative to 
        protected float GetPos(float pos, float max, float window)
        {
            return ((pos + 0.5f) / max) * window;
        }

    }
}
