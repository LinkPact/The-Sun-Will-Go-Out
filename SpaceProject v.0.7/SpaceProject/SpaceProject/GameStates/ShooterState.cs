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

        private Random random;
        public Sprite spriteSheet;
        private float timeSinceStarted;

        //Game management
        private PlayerVerticalShooter player;

        //Declaration of level(s)
        private LevelResourceGather resourceLevel;

        private List<Level> levels = new List<Level>();
        private Level currentLevel;

        public Level CurrentLevel { get { return currentLevel; } }

        public List<GameObjectVertical> gameObjects = new List<GameObjectVertical>(); //Contains all used GameObjects
        private List<GameObjectVertical> deadGameObjects = new List<GameObjectVertical>();

        public List<GameObjectVertical> backgroundObjects = new List<GameObjectVertical>();
        private List<GameObjectVertical> deadBackgroundObjects = new List<GameObjectVertical>();

        private float windowHeightOffset = 0;
        public float WindowHeightOffset { get { return windowHeightOffset; } private set { ;} }

        #endregion

        public ShooterState(Game1 Game, String name):
            base(Game, name)
        {
            this.Game = Game;
            Class = "play";

            //Load texture
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/ShooterSheet"));
            
            //Player
            player = new PlayerVerticalShooter(Game, spriteSheet);

            // (Levels used for development only)
            levels.Add(new JakobsDevelopLevel(Game, spriteSheet, player, MissionType.rebel));
            //

            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "mapCreator2", "map1", MissionType.none));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "mapCreator", "xExperimental", MissionType.none));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "finalBattle", "xFinalBattle", MissionType.none));

            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "DeathByMeteor", "XDeathByMeteor", MissionType.rebel));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "flightTraining_1", "XFlightTraining_1", MissionType.none));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "flightTraining_2", "XFlightTraining_2", MissionType.none));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "flightTraining_3", "XFlightTraining_3", MissionType.none));

            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "hardLevel", "XHardLevel", MissionType.rebel));

            //Levels
            levels.Add(new LevelMeteor(Game, spriteSheet, player, MissionType.none));
            levels.Add(new ExperimentLevel(Game, spriteSheet, player, MissionType.none));
            levels.Add(new EliminationLevel(Game, spriteSheet, player, MissionType.none));
            levels.Add(new DanneLevel(Game, spriteSheet, player, MissionType.none));
            levels.Add(new EscortLevel(Game, spriteSheet, player, MissionType.none));
            resourceLevel = new LevelResourceGather(Game, spriteSheet, player, MissionType.none);
            levels.Add(resourceLevel);
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "DefendColony", "XDefendColony", MissionType.power));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "AstroScan", "XAstroscan", MissionType.none));

            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "AstroDodger", "XAstroDodger", MissionType.dark));

            levels.Add(new FirstMissionLevel(Game, spriteSheet, player, "FirstMissionLevel", "XFirstMission", MissionType.none));
            levels.Add(new SecondMissionLevel(Game, spriteSheet, player, "SecondMissionlvl1", "XSecondMissionlvl3", MissionType.rebel));
            levels.Add(new SecondMissionLevel(Game, spriteSheet, player, "SecondMissionlvl2", "XSecondMissionlvl2", MissionType.rebel));
            levels.Add(new SecondMissionLevel(Game, spriteSheet, player, "SecondMissionlvl3", "XSecondMissionlvl1", MissionType.rebel));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "Main_TheAlliancelvl", "XMain_TheAlliance", MissionType.power)); 

            //TEST
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "EscortTest1", "escortTest1", MissionType.rebel));

            //Pirate Levels
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateLevel1", "PirateLevel1", MissionType.pirate));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateLevel2", "PirateLevel2", MissionType.pirate));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateLevel3", "PirateLevel3", MissionType.pirate));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateLevel4", "PirateLevel4", MissionType.pirate));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateLevel5", "PirateLevel5", MissionType.pirate));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateLevel6", "PirateLevel6", MissionType.pirate));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateLevel7", "PirateLevel7", MissionType.pirate));
            levels.Add(new MapCreatorLevel(Game, spriteSheet, player, "PirateLevel8", "PirateLevel8", MissionType.pirate));
        }

        public override void Initialize()
        {
            if (windowHeightOffset == 0 
                && Game.Window.ClientBounds.Height > 600)
            {
                windowHeightOffset = ((float)Game.Window.ClientBounds.Height - 600) / 2;
            }

            random = new Random();

            gameObjects.Clear();
            backgroundObjects.Clear();

            // Removed by Jakob 140701, kill after two weeks
            //if (!gameObjects.Contains(player))
            //    gameObjects.Add(player);

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
                case MissionType.rebel:
                    {
                        ActiveSong = Music.Rebels;
                        break;
                    }
                case MissionType.pirate:
                    {
                        ActiveSong = Music.Asteroids;
                        break;
                    }
                case MissionType.dark:
                    {
                        ActiveSong = Music.DarkSpace;
                        break;
                    }
                case MissionType.power:
                    {
                        ActiveSong = Music.PowerSong;
                        break;
                    }
                case MissionType.none:
                    {
                        ActiveSong = Music.Asteroids;
                        break;
                    }
            }
        }

        public void SetupLevelTestRun(String fileName, float startTime)
        {
            //In order to remove duplicates when ran several times
            List<Level> removeList = new List<Level>();
            foreach (Level level in levels)
            {
                if (level.Name.Equals("testRun"))
                {
                    removeList.Add(level);
                }
            }
            foreach (Level level in removeList)
            {
                levels.Remove(level);
            }
            removeList.Clear();

            Boolean isTestRun = true;
            Level lv = new MapCreatorLevel(Game, spriteSheet, player, "testRun", fileName, MissionType.none, isTestRun);
            lv.SetCustomStartTime(startTime);
            levels.Add(lv);
        }

        public override void OnLeave()
        {
            gameObjects.Clear();

            MediaPlayer.Stop();
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
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, objectiveString, new Vector2(xBase, yBase), Color.DarkOliveGreen);

            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, "Enemies killed: " + currentLevel.enemiesKilled, new Vector2(xBase, yBase + 20), Color.DarkOliveGreen);
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, "Enemies escaped: " + currentLevel.enemiesLetThrough, new Vector2(xBase, yBase + 40), Color.DarkOliveGreen);
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, "Enemy count: " + currentLevel.totalLevelEnemyCount, new Vector2(xBase, yBase + 60), Color.DarkOliveGreen);
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, "Elapsed time: " + currentLevel.PlayTimeRounded, new Vector2(xBase, yBase + 80), Color.DarkOliveGreen);
        }

        private void CheckKeyboard()
        {
            if (ControlManager.CheckPress(RebindableKeys.Pause) && timeSinceStarted > 200)
                Game.messageBox.DisplayMenu();

            //if (ControlManager.CheckKeypress(Keys.Y))
            //    Game.messageBox.DisplayMessage("Hello world!");

            //if (ControlManager.CheckKeypress(Keys.F1))
            //    AlliedShip.ShowSightRange = !AlliedShip.ShowSightRange;

            if (StatsManager.gameMode == GameMode.develop)
                DevelopCommands();
        }

        private void DevelopCommands()
        { 
            if (ControlManager.CheckKeypress(Keys.F))
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

                    if (GlobalMathFunctions.IsOneOfType<AreaDamage>(obj1, obj2))
                    {
                        PerformAreaDamage(obj1, obj2);
                    }
                    else
                    {
                        if (GlobalMathFunctions.IsOneOfType<AreaCollision>(obj1.AreaCollision, obj2.AreaCollision)
                            && GlobalMathFunctions.IsOneOfType<PlayerBullet>(obj1, obj2))
                        {
                            PerformAreaCollision(obj1, obj2);
                        }

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

        private void PerformAreaCollision(GameObjectVertical obj1, GameObjectVertical obj2)
        {
            if (obj1.HasAreaCollision() && obj2.HasAreaCollision())
                return;

            if (obj1.HasAreaCollision())
            {
                if (((AreaCollision)(obj1.AreaCollision)).IsOverlapping((AnimatedGameObject)obj2))
                {
                    CollisionHandlingVerticalShooter.GameObjectsCollision(obj1, obj2);
                }
            }
            else
            {
                if (((AreaCollision)(obj2.AreaCollision)).IsOverlapping((AnimatedGameObject)obj1))
                {
                    CollisionHandlingVerticalShooter.GameObjectsCollision(obj2, obj1);
                }
            }
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

        public void BeginLevel(string levelName)
        {
            foreach (Level level in levels)
                if (level.Name == levelName)
                {
                    currentLevel = level;
                    Game.stateManager.shooterState.Initialize();
                    Game.stateManager.ChangeState("ShooterState");
                    return;
                }

            throw new ArgumentException("No suitable level found!");
        }

        public void BeginPirateLevel()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            String levelName = "PirateLevel" + r.Next(1, 9);
            BeginLevel(levelName);
        }

        public Level GetLevel(string levelName)
        {
            foreach (Level level in levels)
                if (level.Name == levelName)
                    return level;

            return null;
        }

    }
}