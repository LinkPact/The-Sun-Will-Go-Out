using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SpaceProject.MapCreator;

namespace SpaceProject.MapCreator
{
    /* This class is a general class used to read data from textfiles created by MapCreator
     * The target save file is entered in the constructor (filePath)
     * together with the name through which the level will be identified (identifyer)
     */
    class MapCreatorLevel : Level
    {
        private LevelData data;

        public MapCreatorLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, 
            String identifier, String filePath, MissionType missionType, Boolean isTestRun = false)
            : base(Game, spriteSheet, player1, missionType)
        {
            Setup(identifier, filePath, isTestRun);
        }

        private void Setup(String identifier, String filePath, Boolean isTestRun)
        {
            this.Name = identifier;

            if (isTestRun)
                LevelLoader.ReadMapCreatorLevelFile(filePath);
            else
                LevelLoader.ReadLevelFile(filePath);
            
            String[] textData;
            if (LevelLoader.HasNewOutput())
            {
                textData = LevelLoader.GetNewOutput();
                 data = new LevelData(textData);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            
            //TODO: Read this data from file
            SetCustomVictoryCondition(levelObjective, 0);
            ////

            if (data != null)
                BuildLevel();
            else
                throw new System.ArgumentException("Non-valid sourcefile");

            if (IsCustomStartSet())
                CustomStartSetup();

            CalculateLevelEnemyCount();
            player.SetLevelWidth(LevelWidth);
        }

        //Creates level from information contained in the LevelData file,
        //which is created from the savefile in the constructur.
        private void BuildLevel()
        {
            float[] timeVector = data.GetTimeVector();
            SquareData[,] dataGrid = data.PointDataGrid;
            List<DurationSquareDataChain> durationChains = data.DurationChains;
            LevelWidth = data.WidthInPixels;
            
            BuildPointPart(dataGrid, timeVector);
            BuildDurationPart(durationChains, timeVector);

            if (data.objective != LevelObjective.Finish)
                SetCustomVictoryCondition(data.objective, data.objectiveValue);

            levelObjective = data.objective;

        }

        private void BuildPointPart(SquareData[,] dataGrid, float[] timeVector)
        {
            for (int h = 0; h < data.height; h++)
            {
                for (int w = 0; w < data.pointWidth; w++)
                {
                    String squareString = dataGrid[w, h].ToString();
                    squareString = Regex.Replace(squareString, @" ", "");

                    if (!squareString.Substring(0, 1).Equals("0"))
                    {
                        float position = GetPos(w, data.pointWidth, LevelWidth);
                        CreatePointEvent(squareString, position, 1000 * timeVector[h]);
                    }
                }
            }
        }
        
        private float GetPos(float pos, float max, float window)
        {
            return ((pos + 0.5f) / max) * window;
        }

        private void BuildDurationPart(List<DurationSquareDataChain> durationChains, float[] timeVector)
        {
            foreach (DurationSquareDataChain chain in durationChains)
            {
                String chainString = chain.head.ToString();
                chainString = Regex.Replace(chainString, @" ", "");

                if (!chainString.Substring(0, 1).Equals("0"))
                {
                    float startTime = timeVector[chain.startCoordinate.Y];
                    float endTime = timeVector[chain.endCoordinate.Y];
                    CreateDurationEvent(chain.head.ToString(), startTime, endTime);
                }
            }
        }

        private void CreatePointEvent(String state, float pos, float time)
        {
            //To prevent enemies at position 0 from being removed
            if (pos == 0)
                pos = 1;
            //

            String enemyData = state.Substring(0, 1);
            String movementData = state.Substring(1, 2);
            String eventData = state.Substring(3, 1);

            SortedDictionary<string, int> settings = new SortedDictionary<string, int>();

            MatchCollection matches = Regex.Matches(state, @"([XYWHD])(\d+)");
            foreach (Match match in matches)
            {
                String letter = match.Groups[1].Value;
                String valueString = match.Groups[2].Value;
                int value = Convert.ToInt32(valueString);
                settings.Add(letter, value);
            }
            //String enemy = SaveLoadStringLibrary.GetEnemyTypeFromSaveString(enemyData);
            String enemy = enemyData;
            EnemyType identifier = DataConversionLibrary.GetEnemyEnumFromString(enemy);
            Movement movement = DataConversionLibrary.GetMovementEnumFromString(movementData);

            #region switchEventData
            switch (eventData)
            {
                case "0":
                    {
                        LevelEvent lvEv = new SingleEnemy(Game, player, spriteSheet, this, identifier, time, pos);
                        SetMovement(lvEv, movement);
                        untriggeredEvents.Add(lvEv);
                        break;
                    }
                // The horizontal event forces a horizontal movement
                case "h":
                    {
                        int direction = settings["D"];
                        int yPos = settings["Y"];

                        LevelEvent lvEv = new SingleHorizontalEnemy(Game, player, spriteSheet, this, identifier, time, direction, yPos);

                        if (direction == 1)
                            SetMovement(lvEv, Movement.RightHorizontal);
                        else
                            SetMovement(lvEv, Movement.LeftHorizontal);
                        
                        untriggeredEvents.Add(lvEv);
                        break;
                    }
                case "l":
                    {
                        int width = settings["W"];
                        int xspacing = settings["X"];
                        LevelEvent lvEv = new LineFormation(Game, player, spriteSheet, this, identifier, time, width, xspacing, pos);

                        SetMovement(lvEv, movement);

                        untriggeredEvents.Add(lvEv);
                        break;
                    }
                case "s":
                    {
                        int width = settings["W"];
                        int xspacing = settings["X"];
                        int height = settings["H"];
                        int yspacing = settings["Y"];
                        LevelEvent lvEv = new SquareFormation(Game, player, spriteSheet, this, identifier, time, width, height, xspacing, yspacing, pos);

                        SetMovement(lvEv, movement);

                        untriggeredEvents.Add(lvEv);
                        break;
                    }
                case "t":
                    {
                        int height = settings["H"];
                        int xspacing = settings["X"];
                        int yspacing = settings["Y"];
                        LevelEvent lvEv = new VFormation(Game, player, spriteSheet, this, identifier, time, height, xspacing, yspacing, pos);

                        SetMovement(lvEv, movement);

                        untriggeredEvents.Add(lvEv);
                        break;
                    }
            }
            #endregion
        }

        private Boolean SetMovement(LevelEvent lvEv, Movement movement)
        {
            if (movement != Movement.None)
            {
                SetupCreature setup = new SetupCreature();
                setup.SetMovement(movement);
                lvEv.CreatureSetup(setup);
                return true;
            }
            return false;
        }

        private void CreateDurationEvent(String state, float startTime, float endTime)
        {
            state = Regex.Replace(state, " ", "");

            String enemyData = state.Substring(0, 1);
            String movementData = state.Substring(1, 2);
            String eventData = state.Substring(3, 1);

            SortedDictionary<string, int> settings = new SortedDictionary<string, int>();

            MatchCollection matches = Regex.Matches(state, @"([A-Z])(\d+)");
            foreach (Match match in matches)
            {
                String letter = match.Groups[1].Value;
                String valueString = match.Groups[2].Value;
                int value = Convert.ToInt32(valueString);
                settings.Add(letter, value);
            }
            //String enemy = SaveLoadStringLibrary.GetEnemyTypeFromSaveString(enemyData);
            String enemy = enemyData;
            EnemyType identifier = DataConversionLibrary.GetEnemyEnumFromString(enemy);
            Movement movement = DataConversionLibrary.GetMovementEnumFromString(movementData);

            switch (eventData)
            {
                case "E":
                    {
                        int density = settings["D"];

                        LevelEvent lvEv = new EvenSwarm(Game, player, spriteSheet, this, identifier, 1000 * startTime, 
                            1000 * (endTime - startTime), density);
                        SetMovement(lvEv, movement);
                        untriggeredEvents.Add(lvEv);
                        break;
                    }
                case "G":
                    {
                        int startDensity = settings["S"];
                        int peakDensity = settings["P"];
                        int peakTime = settings["T"];

                        LevelEvent lvEv = new GradientSwarm(Game, player, spriteSheet, this, identifier, 1000 * startTime, 
                            1000 * (endTime - startTime), peakTime, startDensity, peakDensity);
                        SetMovement(lvEv, movement);
                        untriggeredEvents.Add(lvEv);
                        break;
                    }
            }
        }
    }
}
