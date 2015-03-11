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
    public class ShooterState : GameState
    {
        #region declaration

        public Sprite spriteSheet;
        private float timeSinceStarted;

        //Game management
        private PlayerVerticalShooter player;

        //Declaration of level(s)
        private List<Level> levels = new List<Level>();
        private Level currentLevel;
        public Level CurrentLevel { get { return currentLevel; } }
        private Level testRunLevel;
        public Boolean IsTestRunLevelCompleted() 
        { 
            return testRunLevel.IsObjectiveCompleted;
        }

        public List<GameObjectVertical> gameObjects = new List<GameObjectVertical>(); //Contains all used GameObjects
        private List<GameObjectVertical> deadGameObjects = new List<GameObjectVertical>();

        public List<GameObjectVertical> backgroundObjects = new List<GameObjectVertical>();
        private List<GameObjectVertical> deadBackgroundObjects = new List<GameObjectVertical>();

        private float windowHeightOffset = 0;
        public float WindowHeightOffset { get { return windowHeightOffset; } private set { ;} }

        #endregion

        public ShooterState(Game1 Game, String name) :
            base(Game, name)
        {
            this.Game = Game;
            Class = "play";

            //Load texture
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/ShooterSheet"));
            
            //Player
            player = new PlayerVerticalShooter(Game, spriteSheet);

            // Create new log file
            LevelLogger.InitializeNewLogfile();

            // (Levels used for development only)
            //levels.Add(new JakobsDevelopLevel(Game, spriteSheet, player, MissionType.rebelpirate, "JakobDevelop"));
            //

            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "mapCreator2", "map1", MissionType.none));
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "mapCreator", "xExperimental", MissionType.none));
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "finalBattle", "xFinalBattle", MissionType.none));


            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "hardLevel", "XHardLevel", MissionType.rebelpirate));

            //Levels
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "DefendColony", "XDefendColony", MissionType.power));
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "AttackOnRebelStation", "P2AttackOnRebelStation", MissionType.none));


            //levels.Add(new FirstMissionLevel(Game, spriteSheet, player, "FirstMissionLevel", "XFirstMission", MissionType.none));
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "ScoutingLevel", "XScoutingLevel", MissionType.none));
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateAmbush", "XPirateAmbush", MissionType.none));
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateAnnihilation", "XPirateAnnihilation", MissionType.none));
            //levels.Add(new SecondMissionLevel(Game, spriteSheet, player, "SecondMissionlvl1", "XSecondMissionlvl3", MissionType.rebelpirate));
            //levels.Add(new SecondMissionLevel(Game, spriteSheet, player, "SecondMissionlvl2", "XSecondMissionlvl2", MissionType.rebelpirate));
            //levels.Add(new SecondMissionLevel(Game, spriteSheet, player, "SecondMissionlvl3", "XSecondMissionlvl1", MissionType.rebelpirate));
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "Main_TheAlliancelvl", "XMain_TheAlliance", MissionType.power));
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "Main10_AllianceDefence", "XMain10_level1", MissionType.power));
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "CoverBlown", "XCoverBlown", MissionType.power));

            //TEST
            //levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "EscortTest1", "escortTest1", MissionType.rebelpirate));


            // Unsure about if this is to be used, seems like a nice idea though! // Jakob
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "LonelyAsteroidEncounter", "LonelyAsteroidLevel", MissionType.none));

            //////////// USED LEVELS //////////////

            var levelDict = ShooterState.GetMissionPathDict();

            //Pirate Levels
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["rp1"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["rp2"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["rp3"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["rp4"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["rp5"]));

            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["ap1"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["ap2"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["ap3"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["ap4"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["ap5"]));

            levels.Add(new HangarLevel(Game, spriteSheet, player, levelDict["hangar1"]));
            levels.Add(new HangarLevel(Game, spriteSheet, player, levelDict["hangar2"]));
            levels.Add(new HangarLevel(Game, spriteSheet, player, levelDict["hangar3"]));

            // Side mission levels

            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["s_flightTraining_1"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["s_flightTraining_2"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["s_flightTraining_3"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["s_deathByMeteor"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["s_astroScan"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["s_astroDodger"]));

            // Main mission levels
            levels.Add(new RebelsInTheMeteors(Game, spriteSheet, player, levelDict["1_1"]));
            levels.Add(new SecondMissionLevel(Game, spriteSheet, player, levelDict["2_1"]));
            levels.Add(new SecondMissionLevel(Game, spriteSheet, player, levelDict["2_2"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["3_1"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["3_2"]));
            levels.Add(new Infiltration1Level(Game, spriteSheet, player, levelDict["4_1"]));
            levels.Add(new Infiltration2Level(Game, spriteSheet, player, levelDict["4_2"]));
            levels.Add(new Retaliation1Level(Game, spriteSheet, player, levelDict["5_1"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["5_2"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["6_1"]));
            levels.Add(new InTheNameOfScienceLevel2(Game, spriteSheet, player, levelDict["6_2"]));
            levels.Add(new Information1Level(Game, spriteSheet, player, levelDict["7_1"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["8o_1"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["8o_2"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["8r_1"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["8r_2"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["8a_1"]));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, levelDict["8a_2"]));
            
        }
        
        // Creates and returns a dictionary linking level names to string paths
        public static Dictionary<String, LevelEntry> GetMissionPathDict()
        {
            //var pathDict = new Dictionary<String, String>();
            var entryDict = new Dictionary<String, LevelEntry>();

            // Main

            entryDict.Add("1_1", new LevelEntry("RebelsInTheMeteors",   "mainlevels/m1_RebelsAsteroids",        MissionType.regularalliance));
            entryDict.Add("2_1", new LevelEntry("FreighterEscort1",     "mainlevels/m2_FreighterEscort_lv1",    MissionType.regularalliance));
            entryDict.Add("2_2", new LevelEntry("FreighterEscort2",     "mainlevels/m2_FreighterEscort_lv2",    MissionType.regularalliance));
            entryDict.Add("3_1", new LevelEntry("DefendColonyBreak",    "mainlevels/m3_DefendColony_lv1",       MissionType.power));
            entryDict.Add("3_2", new LevelEntry("DefendColonyHold",     "mainlevels/m3_DefendColony_lv2",       MissionType.power));

            entryDict.Add("4_1", new LevelEntry("Infiltration1",        "mainlevels/m4_infiltration_lv1",       MissionType.dark));
            entryDict.Add("4_2", new LevelEntry("Infiltration2",        "mainlevels/m4_infiltration_lv2",       MissionType.dark));
            entryDict.Add("5_1", new LevelEntry("Retribution1",         "mainlevels/m5_retribution_lv1",        MissionType.regularrebel));
            entryDict.Add("5_2", new LevelEntry("Retribution2",         "mainlevels/m5_retribution_lv2",        MissionType.regularrebel));
            entryDict.Add("6_1", new LevelEntry("Itnos_1",              "mainlevels/m6_itnos_lv1",              MissionType.regularrebel));
            entryDict.Add("6_2", new LevelEntry("Itnos_2",              "mainlevels/m6_itnos_lv2",              MissionType.regularrebel));
            entryDict.Add("7_1", new LevelEntry("Information",          "mainlevels/m7_information",            MissionType.dark));

            entryDict.Add("8o_1", new LevelEntry("OnYourOwn_1",         "mainlevels/m8a_OYO_lv1",               MissionType.goingout));
            entryDict.Add("8o_2", new LevelEntry("OnYourOwn_2",         "mainlevels/m8a_OYO_lv2",               MissionType.goingout));
            entryDict.Add("8r_1", new LevelEntry("RebelBranch_1",       "mainlevels/m8b_rebels_lv1",            MissionType.regularrebel));
            entryDict.Add("8r_2", new LevelEntry("RebelBranch_2",       "mainlevels/m8b_rebels_lv2",            MissionType.regularrebel));
            entryDict.Add("8a_1", new LevelEntry("AllianceBranch_1",    "mainlevels/m8c_alliance_lv1",          MissionType.regularrebel));
            entryDict.Add("8a_2", new LevelEntry("AllianceBranch_2",    "mainlevels/m8c_alliance_lv2",          MissionType.regularrebel));

            // Side

            entryDict.Add("s_flightTraining_1", new LevelEntry("flightTraining_1", "sidelevels/FlightTraining_1",   MissionType.regularalliance));
            entryDict.Add("s_flightTraining_2", new LevelEntry("flightTraining_2", "sidelevels/FlightTraining_2",   MissionType.regularalliance));
            entryDict.Add("s_flightTraining_3", new LevelEntry("flightTraining_3", "sidelevels/FlightTraining_3",   MissionType.regularalliance));
            entryDict.Add("s_deathByMeteor",    new LevelEntry("DeathByMeteor",    "sidelevels/DeathByMeteor",      MissionType.regularrebel));
            entryDict.Add("s_astroScan",        new LevelEntry("AstroScan",        "sidelevels/Astroscan",          MissionType.dark));
            entryDict.Add("s_astroDodger",      new LevelEntry("AstroDodger",      "sidelevels/AstroDodger",        MissionType.dark));

            // Pirates

            entryDict.Add("rp1", new LevelEntry("RebelPirate1",         "pirates/RebelPirate1",                 MissionType.rebelpirate));
            entryDict.Add("rp2", new LevelEntry("RebelPirate2",         "pirates/RebelPirate2",                 MissionType.rebelpirate));
            entryDict.Add("rp3", new LevelEntry("RebelPirate3",         "pirates/RebelPirate3",                 MissionType.rebelpirate));
            entryDict.Add("rp4", new LevelEntry("RebelPirate4",         "pirates/RebelPirate4",                 MissionType.rebelpirate));
            entryDict.Add("rp5", new LevelEntry("RebelPirate5",         "pirates/RebelPirate5",                 MissionType.rebelpirate));

            entryDict.Add("ap1", new LevelEntry("AlliancePirate1",      "pirates/AlliancePirate1",              MissionType.regularalliance));
            entryDict.Add("ap2", new LevelEntry("AlliancePirate2",      "pirates/AlliancePirate2",              MissionType.regularalliance));
            entryDict.Add("ap3", new LevelEntry("AlliancePirate3",      "pirates/AlliancePirate3",              MissionType.regularalliance));
            entryDict.Add("ap4", new LevelEntry("AlliancePirate4",      "pirates/AlliancePirate4",              MissionType.regularalliance));
            entryDict.Add("ap5", new LevelEntry("AlliancePirate5",      "pirates/AlliancePirate5",              MissionType.regularalliance));

            entryDict.Add("hangar1", new LevelEntry("hangar1",          "pirates/hangar1",                      MissionType.regularalliance));
            entryDict.Add("hangar2", new LevelEntry("hangar2",          "pirates/hangar2",                      MissionType.regularalliance));
            entryDict.Add("hangar3", new LevelEntry("hangar3",          "pirates/hangar3",                      MissionType.regularalliance));

            return entryDict;
        }

        public override void Initialize()
        {
            if (windowHeightOffset == 0 
                && Game.Window.ClientBounds.Height > 600)
            {
                windowHeightOffset = ((float)Game.Window.ClientBounds.Height - 600) / 2;
            }

            gameObjects.Clear();
            backgroundObjects.Clear();

            foreach (GameObjectVertical obj in gameObjects)
                obj.Initialize();
            //-------------

            timeSinceStarted = 0;
        }

        public override void OnEnter()
        {
            if (!gameObjects.Contains(player))
                gameObjects.Add(player);

            currentLevel.Initialize();
            MusicSelection();

            base.OnEnter();
        }

        private void MusicSelection()
        {
            switch ((currentLevel).GetMissionType())
            {
                case MissionType.rebelpirate:
                    {
                        ActiveSong = Music.Stars;
                        break;
                    }
                case MissionType.alliancepirate:
                    {
                        ActiveSong = Music.Stars;
                        break;
                    }
                case MissionType.goingout:
                    {
                        ActiveSong = Music.GoingOut;
                        break;
                    }
                case MissionType.power:
                    {
                        ActiveSong = Music.PowerSong;
                        break;
                    }
                case MissionType.dark:
                    {
                        ActiveSong = Music.DarkPiano;
                        break;
                    }
                case MissionType.regularalliance:
                    {
                        ActiveSong = Music.AllianceBattle;
                        break;
                    }
                case MissionType.regularrebel:
                    {
                        ActiveSong = Music.Falling;
                        break;
                    }
                case MissionType.none:
                    {
                        ActiveSong = Music.SpaceAmbience;
                        break;
                    }
            }
        }

        public void SetupMapCreatorTestRun(String filePath, float startTime)
        {
            StatsManager.RestoreShipHealthToMax();
            Boolean isMapCreatorRun = true;
            testRunLevel = new MapCreatorLevel(Game, spriteSheet, player, "testRun", filePath, MissionType.none, isMapCreatorRun);
            testRunLevel.SetCustomStartTime(startTime);
            levels.Add(testRunLevel);
        }

        public void SetupTestLevelRun(LevelEntry levelEntry, float startTime)
        {
            StatsManager.RestoreShipHealthToMax();
            Boolean isMapCreatorRun = false;
            testRunLevel = new MapCreatorLevel(Game, spriteSheet, player, levelEntry.FilePath, levelEntry.FilePath, levelEntry.MissionType, isMapCreatorRun);
            testRunLevel.SetCustomStartTime(startTime);
            levels.Add(testRunLevel);
        }

        public override void OnLeave()
        {
            gameObjects.Clear();

            MediaPlayer.Stop();
            Popup.delayTimer = 50;
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceStarted += gameTime.ElapsedGameTime.Milliseconds;

            HandleGameObjects(gameTime); 
            HandleBackgroundObjects(gameTime); 
            currentLevel.Update(gameTime);
            CheckKeyboard();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentLevel.Draw(spriteBatch);

            //Draws all GameObjects used in this level.
            foreach (GameObjectVertical obj in gameObjects)
            {
                obj.Draw(spriteBatch);
            }

            //Draws background objects
            foreach (GameObjectVertical obj in backgroundObjects)
            {
                obj.Draw(spriteBatch);
            }

            if (StatsManager.gameMode == GameMode.develop)
                DisplayLevelModeInformation(spriteBatch);

            currentLevel.DrawEndMessage(spriteBatch);
        }

        private void DisplayLevelModeInformation(SpriteBatch spriteBatch)
        {
            float xBase = 350f;
            float yBase = 100f;

            String objectiveString = currentLevel.GetObjectiveString();

            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, "Level name: " + currentLevel.Identifier, new Vector2(xBase, yBase), Color.DarkOliveGreen);
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, objectiveString, new Vector2(xBase, yBase + 20), Color.DarkOliveGreen);

            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, "Enemies killed: " + currentLevel.enemiesKilled, new Vector2(xBase, yBase + 40), Color.DarkOliveGreen);
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, "Enemies escaped: " + currentLevel.enemiesLetThrough, new Vector2(xBase, yBase + 60), Color.DarkOliveGreen);
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, "Enemy count: " + currentLevel.totalLevelEnemyCount, new Vector2(xBase, yBase + 80), Color.DarkOliveGreen);
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, "Elapsed time: " + currentLevel.PlayTimeRounded, new Vector2(xBase, yBase + 100), Color.DarkOliveGreen);
        }

        private void CheckKeyboard()
        {
            if (ControlManager.CheckPress(RebindableKeys.Pause) && timeSinceStarted > 200)
                PopupHandler.DisplayMenu();

            if (ControlManager.CheckKeyPress(Keys.Y))
            {
                //Game.messageBox.DisplayMessage("Hello world!");
                Game.soundEffectsManager.MutateLaserSound_DEVELOP();
            }

            //if (ControlManager.CheckKeypress(Keys.F1))
            //    AlliedShip.ShowSightRange = !AlliedShip.ShowSightRange;

            if (StatsManager.gameMode == GameMode.develop || StatsManager.gameMode == GameMode.campaign)
                DevelopCommands();
        }

        private void DevelopCommands()
        { 
            if (ControlManager.CheckKeyPress(Keys.F))
            {
                currentLevel.finishLevel_DEVELOPONLY();
            }
        }

        private void HandleGameObjects(GameTime gameTime)
        {
            int count = 0;
            if (gameObjects != null)
                count = gameObjects.Count;

            CheckCollision(count);
            UpdateObjects(gameTime, count);
            ClearDeadObjects(count);
        }

        private void CheckCollision(int objectCount)
        {
            for (int n = 0; n < objectCount; n++)
            {
                for (int m = n + 1; m < objectCount; m++)
                {
                    GameObjectVertical obj1 = gameObjects[n];
                    GameObjectVertical obj2 = gameObjects[m];

                    Boolean pixelCollisionNeedsEvaluation = true;

                    if (MathFunctions.IsOneOfType<AreaDamage>(obj1, obj2))
                    {
                        PerformAreaDamage(obj1, obj2);
                        pixelCollisionNeedsEvaluation = false;
                    }
                    else if(MathFunctions.IsOneOfType<AreaShieldCollision>(obj1.AreaCollision, obj2.AreaCollision)
                            && MathFunctions.IsOneOfType<PlayerBullet>(obj1, obj2))
                    {
                        pixelCollisionNeedsEvaluation = PerformShieldAreaCollision(obj1, obj2);
                    }
                    
                    if (pixelCollisionNeedsEvaluation)
                    {
                        if (CollisionDetection.VisiblePixelsColliding(obj1.Bounding, obj2.Bounding,
                            ((AnimatedGameObject)(obj1)).CurrentAnim.CurrentFrame,
                            ((AnimatedGameObject)obj2).CurrentAnim.CurrentFrame, 
                            obj1.CenterPoint, obj2.CenterPoint))
                        {
                            CollisionHandlingVerticalShooter.GameObjectsCollision(obj1, obj2);
                        }
                    }
                }
            }
        }

        private void PerformAreaDamage(GameObjectVertical obj1, GameObjectVertical obj2)
        {
            if (obj1 is AreaDamage && obj2 is AreaDamage)
            {
                return;
            }

            if (obj1 is AreaDamage)
            {
                if (((AreaDamage)(obj1)).IsOverlapping((AnimatedGameObject)obj2))
                {
                    CollisionHandlingVerticalShooter.GameObjectsCollision(obj1, obj2);
                }
            }
            else
            {
                if (((AreaDamage)(obj2)).IsOverlapping((AnimatedGameObject)obj1))
                {
                    CollisionHandlingVerticalShooter.GameObjectsCollision(obj2, obj1);
                }
            }
        }

        private Boolean PerformShieldAreaCollision(GameObjectVertical obj1, GameObjectVertical obj2)
        {
            if (obj1.HasAreaCollision() && obj2.HasAreaCollision())
                return false;

            if (obj1.HasAreaCollision())
            {
                if (((EnemyShip)obj1).ShieldCanTakeHit(obj2.Damage))
                {
                    if (((AreaShieldCollision)(obj1.AreaCollision)).IsOverlapping((PlayerBullet)obj2))
                    {
                        CollisionHandlingVerticalShooter.GameObjectsCollision(obj1.AreaCollision, obj2);
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (((EnemyShip)obj2).ShieldCanTakeHit(obj1.Damage))
                {
                    if (((AreaShieldCollision)(obj2.AreaCollision)).IsOverlapping((PlayerBullet)obj1))
                    {
                        CollisionHandlingVerticalShooter.GameObjectsCollision(obj2.AreaCollision, obj1);
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateObjects(GameTime gameTime, int objectCount)
        {
            for (int n = 0; n < objectCount; n++)
            {
                gameObjects[n].Update(gameTime);
            }        
        }
        
        private void ClearDeadObjects(int objectCount)
        {
            for (int n = 0; n < objectCount; n++)
            {
                GameObjectVertical obj1 = gameObjects[n];

                if (obj1.IsKilled || obj1.IsOutside)
                {
                    deadGameObjects.Add(obj1);

                    if (obj1.IsKilled && obj1 is EnemyShip)
                    {
                        currentLevel.LevelLoot += ((EnemyShip)obj1).GetLoot();
                        currentLevel.enemiesKilled++;

                        if (obj1.LastHitBy == Tracker.Player
                            && !(obj1 is Meteorite))
                        {
                            currentLevel.enemiesKilledByPlayer++;
                        }
                    }

                    if ((obj1.IsOutside && obj1 is EnemyShip))
                    {
                        currentLevel.enemiesLetThrough++;
                    }
                }
            }

            for (int n = 0; n < deadGameObjects.Count; n++)
            {
                gameObjects.Remove(deadGameObjects[n]);
            }
            deadGameObjects.Clear();
        }

        private void HandleBackgroundObjects(GameTime gameTime)
        {
            int count = 0;
            if (backgroundObjects != null)
            {
                count = backgroundObjects.Count;

                if (backgroundObjects.Count > 0)
                {

                    for (int n = 0; n < count; n++)
                    {
                        backgroundObjects[n].Update(gameTime);
                    }

                    //Ta bort doda objekt
                    for (int n = 0; n < count; n++)
                    {
                        if (backgroundObjects[n].IsKilled || backgroundObjects[n].IsOutside)
                        {
                            deadBackgroundObjects.Add(backgroundObjects[n]);
                        }
                    }

                    for (int n = 0; n < deadBackgroundObjects.Count; n++)
                    {
                        backgroundObjects.Remove(deadBackgroundObjects[n]);
                    }
                    deadBackgroundObjects.Clear();
                }
            }
        }

        public void BeginTestLevel()
        {
            if (testRunLevel == null)
            {
                throw new ArgumentException("No test level assigned!");
            }

            currentLevel = testRunLevel;
            Game.stateManager.shooterState.Initialize();
            Game.stateManager.ChangeState("ShooterState");
        }

        public void BeginLevel(string levelName)
        {
            foreach (Level level in levels)
            {
                if (level.Identifier == levelName)
                {
                    currentLevel = level;
                    Game.stateManager.shooterState.Initialize();
                    Game.stateManager.ChangeState("ShooterState");
                    return;
                }
            }

            throw new ArgumentException("No suitable level found!");
        }

        public void BeginPirateLevel()
        {
            String levelName = "PirateLevel" + Game.random.Next(1, 9);
            BeginLevel(levelName);
        }

        public void BeginHangarLevel()
        {
            String levelName = "hangar" + Game.random.Next(1, 4);
            BeginLevel(levelName);
        }

        public void BeginRebelPirateLevel()
        {
            String levelName = "RebelPirate" + Game.random.Next(1, 4);
            BeginLevel(levelName);
        }

        public void BeginAlliancePirateLevel()
        {
            String levelName = "AlliancePirate" + Game.random.Next(1, 4);
            BeginLevel(levelName);
        }

        public Level GetLevel(string levelName)
        {
            foreach (Level level in levels)
                if (level.Identifier == levelName)
                    return level;

            return null;
        }
    }
}