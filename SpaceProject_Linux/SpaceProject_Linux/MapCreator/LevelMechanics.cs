using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.RegularExpressions;

namespace SpaceProject_Linux
{
    public class LevelMechanics
    {
        #region declaration
        private Game1 game;
        private Sprite spriteSheet;
        private String name;

        private LevelData data;

        private PointSquareGrid pointGrid;
        private DurationSquareGrid durationGrid;

        private TimeVector timings;
        private static Vector2 position = new Vector2(50, 550);

        private int saveStringTime;

        private static int viewFrame;
        private static int viewFrameMax;
        private static int squareRowsShown = 35;
        private static int spacing = 15;

        private float startTime = 0;
        public float GetTestStartTime() { return startTime; }
        public void SetTestStartTime( float newStartTime ) { startTime = newStartTime; }

        #endregion

        public LevelMechanics(Game1 game, Sprite spriteSheet, String name)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;
            this.name = name;
            
            saveStringTime = 0;
            data = new LevelData();
        }

        public void Initialize()
        {
            timings = new TimeVector(game, position, data.GetTimeVector());            
         
            pointGrid = new PointSquareGrid(game, spriteSheet, new Vector2(position.X + 10, position.Y), data.PointDataGrid);
            pointGrid.Initialize(data.PointDataGrid);

            durationGrid = new DurationSquareGrid(game, spriteSheet, new Vector2(position.X + 200, position.Y), data.DurationDataGrid);
            durationGrid.Initialize(data.DurationDataGrid, data.DurationChains);

            viewFrame = 0;
            UpdateViewFrame();
        }

        private void UpdateViewFrame()
        {
            viewFrameMax = (int)(pointGrid.GetHeight() / squareRowsShown);
            if (viewFrame > viewFrameMax)
                viewFrame = viewFrameMax;
        }

        public void Update(GameTime gameTime)
        {
            pointGrid.Update(gameTime);
            durationGrid.Update(gameTime);
            timings.Update(gameTime);

            if (saveStringTime > 0)
                saveStringTime -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pointGrid.Draw(spriteBatch);
            durationGrid.Draw(spriteBatch);
            timings.Draw(spriteBatch);
        }

        #region name
        public void ChangeName(String newName)
        {
            name = newName;
        }
        public String GetName()
        {
            return name;
        }
        #endregion

        public void LoadFile(String path)
        {
            String[] lines;
            LevelLoader.ReadMapCreatorLevelFile(path);
            if (LevelLoader.HasNewOutput())
            {
                lines = LevelLoader.GetNewOutput();

                LevelData levelData = new LevelData();
                levelData.LoadDataFromFile(lines);

                data = levelData;
                Initialize();
            }
            else
            { 
                //TODO: else-logic. Popup?
            }
        }

        public List<String> GetLevelAsStrings()
        {
            return data.GetDataAsText();
        }

        #region gridlogic
        public void ClearGrid()
        {
            pointGrid.Clear();
            durationGrid.Clear();
        }
        public void ChangePointGridWidth(int incr)
        {
            data.pointWidth += incr;
            if (data.pointWidth < 1) data.pointWidth = 1;
            UpdatePointGridSize(data.pointWidth, data.height);
            data.PointDataGrid = pointGrid.GetData();
            
            UpdateViewFrame();
        }
        public void ChangeDurationGridWidth(int incr)
        {
            data.durationWidth += incr;
            if (data.durationWidth < 1) data.durationWidth = 1;
            UpdateDurationGridSize(data.durationWidth, data.height);
            data.DurationDataGrid = durationGrid.GetData();

            UpdateViewFrame();
        }
        public void ChangeGridHeight(int incr)
        {
            data.height += incr;
            if (data.height < 1) data.height = 1;
            
            UpdatePointGridSize(data.pointWidth, data.height);
            data.PointDataGrid = pointGrid.GetData();
            
            UpdateDurationGridSize(data.durationWidth, data.height);
            data.DurationDataGrid = durationGrid.GetData();
            
            UpdateViewFrame();
        }
        private void UpdatePointGridSize(int columns, int rows)
        {
            pointGrid.UpdateGridSize(columns, rows);
            timings.SetTimeVector(data.GetTimeVector());
        }
        private void UpdateDurationGridSize(int columns, int rows)
        {
            durationGrid.UpdateGridSize(columns, rows);
        }
        public int GetPointGridWidth()
        {
            return pointGrid.GetWidth();
        }
        public int GetPointGridHeight()
        {
            return pointGrid.GetHeight();
        }
        public int GetDurationGridWidth()
        {
            return durationGrid.GetWidth();
        }
        
        #endregion

        #region timelogic
        public void LevelSavedEvent()
        {
            saveStringTime = 1000;
        }
        public Boolean SaveStringTime()
        {
            return saveStringTime > 0;
        }
        public static float GetTimeForCoordinate(int gridYPosition)
        {
            float time = TimeVector.GetTimeFromIndex(gridYPosition);
            return time;
        }
        #endregion

        #region levelduration
        public float GetLevelDuration()
        {
            return data.duration;
        }
        public void SetLevelDuration(float newDuration)
        {
            data.duration = newDuration;
            timings.SetTimeVector(data.GetTimeVector());
        }
        #endregion

        #region levelPixelWidth
        public int GetWidthInPixels()
        {
            return data.WidthInPixels;
        }
        public void SetWidthInPixels(int newPixelWidth)
        {
            data.WidthInPixels = newPixelWidth;
        }
        #endregion

        public void SetLevelObjective(LevelObjective objective, int objectiveInt)
        {
            data.objective = objective;
            data.objectiveValue = objectiveInt;
        }

        public String GetObjectiveDisplayString()
        {
            return data.objective.ToString() + ": " + data.objectiveValue;
        }

        public void ChangeGridViewFrame(int incr)
        {
            viewFrame += incr;
            
            if (viewFrame > viewFrameMax)
            {
                viewFrame = viewFrameMax;
            }
            else if (viewFrame < 0)
            {
                viewFrame = 0;
            }
        }

        #region viewFrames
        public static float CalculateYPos(int m)
        {
            return position.Y - (m * spacing);
        }

        public static int GetLowerPosition()
        {
            return (int)(viewFrame * squareRowsShown);
        }
        public static int GetHigherPosition()
        {
            return (int)((viewFrame + 1) * squareRowsShown);
        }

        public static int GetCurrentOffset()
        {
            return viewFrame * squareRowsShown * spacing;
        }
        public static Boolean GetUpArrowEnabled()
        {
            return viewFrame < viewFrameMax;
        }
        public static Boolean GetDownArrowEnabled()
        {
            return viewFrame > 0;
        }

        public int GetViewFrame()
        {
            return viewFrame;
        }
        public int GetViewFrameMax()
        {
            return viewFrameMax;
        }
        #endregion


    }
}
