using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using System.Globalization;

namespace SpaceProject_Linux
{
    //Class that will contain the levels gathered data as text.
    //Will be able to interact both with textfiles and other classes.
    class LevelData
    {
        #region declaration
        public int height;
        public int pointWidth;
        public int durationWidth;
        public float duration;

        public LevelObjective objective;
        public int objectiveValue;

        private int widthInPixels;
        public int WidthInPixels
        {
            get { return widthInPixels; }
            set { widthInPixels = value; }
        }

        private SquareData[,] pointDataGrid;
        public SquareData[,] PointDataGrid
        {
            get { return pointDataGrid; }
            set { pointDataGrid = value; }
        }

        private SquareData[,] durationDataGrid;
        public SquareData[,] DurationDataGrid
        {
            get { return durationDataGrid; }
            set { durationDataGrid = value; }
        }

        private List<DurationSquareDataChain> durationChains;
        public List<DurationSquareDataChain> DurationChains
        {
            get { return durationChains; }
            set { durationChains = value; }
        }

        private float[] timeVector;

        private String[] textData;
        #endregion

        //Called when new data is created from text file
        public LevelData(String[] textData)
        {
            this.textData = textData;
            LoadDataFromFile(textData);
        }

        //Called when new data is created from inside program
        public LevelData()
        {
            height = 10;
            pointWidth = 6;
            durationWidth = 3;
            pointDataGrid = new PointSquareData[pointWidth, height];
            durationDataGrid = new DurationSquareData[durationWidth, height];
            durationChains = new List<DurationSquareDataChain>();
            objective = LevelObjective.Finish;

            duration = 10;
            widthInPixels = 400;

            CalculateTimeVector();

            InitializePointGridData();
            InitializeDurationGridData();
        }

        private void InitializePointGridData()
        {
            for (int n = 0; n < pointWidth; n++)
            {
                for (int m = 0; m < height; m++)
                {
                    SquareData s = new PointSquareData();
                    pointDataGrid[n, m] = s;
                }
            }
        }

        private void InitializeDurationGridData()
        {
            for (int n = 0; n < durationWidth; n++)
            {
                for (int m = 0; m < height; m++)
                {
                    SquareData s = new DurationSquareData();
                    durationDataGrid[n, m] = s;
                }
            }
        }

        public float[] GetTimeVector()
        {
            CalculateTimeVector();
            return timeVector;
        }

        private void CalculateTimeVector()
        {
            timeVector = new float[height];
            for (int n = 0; n < height; n++)
            {
                timeVector[n] = (float)(Math.Round((Decimal)(n * duration / height), 1, MidpointRounding.AwayFromZero));
            }
        }

        #region loading
        
        //Takes a string vector and inputs its data into the LevelData instance.
        public void LoadDataFromFile(String[] data)
        {
            textData = data;
            
            List<String> dataTransfer = new List<String>();
            foreach (String str in data)
            {
                dataTransfer.Add(str);
            }

            String header = textData[0];
            String dataLine = textData[2];

            int localGridWidth = GetGridWidth(dataLine);
            int localGridHeight = GetGridHeight(data);

            pointWidth = localGridWidth;
            height = localGridHeight;

            timeVector = new float[localGridHeight];
            pointDataGrid = new SquareData[localGridWidth, localGridHeight];
            durationChains = new List<DurationSquareDataChain>();

            LoadHeader(dataTransfer, header);
            LoadPoints(dataTransfer, localGridWidth, localGridHeight);
            LoadDuration(dataTransfer);
        }
        
        private void LoadHeader(List<String> dataTransfer, String header)
        {
            dataTransfer.RemoveAt(0);
            
            //Header
            MatchCollection headerMatches = Regex.Matches(header, @":(\w+)\s(\w+\|\w+|\w+)");
            foreach (Match match in headerMatches)
            {
                switch (match.Groups[1].Value)
                {
                    case "duration":
                        {
                            String dur = match.Groups[2].Value;
                            duration = Convert.ToInt32(dur);
                            break;
                        }
                    case "width":
                        {
                            String w = match.Groups[2].Value;
                            widthInPixels = Convert.ToInt32(w);
                            break;
                        }
                    case "durGridWidth":
                        {
                            String str = match.Groups[2].Value;
                            durationWidth = Convert.ToInt32(str);
                            break;
                        }
                    case "levelObjective":
                        {
                            String str = match.Groups[2].Value;
                            LoadLevelObjective(str);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void LoadLevelObjective(String loadPattern)
        {
            Match objectiveModeMatch = Regex.Match(loadPattern, @"^(\w+)|");
            objective = MathFunctions.ParseEnum<LevelObjective>(objectiveModeMatch.Value);
            Match objectiveValMatch = Regex.Match(loadPattern, @"([A-Z])(\d+)");
            objectiveValue = Convert.ToInt32(objectiveValMatch.Groups[2].Value);
        }
        
        private void LoadPoints(List<String> dataTransfer, int localGridWidth, int localGridHeight)
        {
            dataTransfer.RemoveAt(0);
            for (int m = 0; m < localGridHeight; m++)
            {
                String dataLine = dataTransfer[0];
                dataTransfer.RemoveAt(0);
                MatchCollection gridMatches = Regex.Matches(dataLine, @"(\w):([^:]+):");

                foreach (Match match in gridMatches)
                {
                    switch (match.Groups[1].Value)
                    {
                        case "T":
                            {
                                String time = match.Groups[2].Value;
                                timeVector[m] = Convert.ToSingle(time, CultureInfo.InvariantCulture);
                                break;
                            }
                        case "E":
                            {
                                String gridLineMatch = match.Groups[2].Value;
                                MatchCollection lineMatches = Regex.Matches(gridLineMatch, @" (\w+)");

                                int position = 0;
                                foreach (Match match_ in lineMatches)
                                {
                                    String elementData = match_.Groups[1].Value;
                                    SquareData sd = new PointSquareData(elementData);
                                    pointDataGrid[position, m] = sd;
                                    position++;
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        private void LoadDuration(List<String> dataTransfer)
        {
            LoadChains(dataTransfer);
            LoadGrid();
        }
        
        private void LoadChains(List<String> dataTransfer)
        {
            dataTransfer.RemoveAt(0);
            while (dataTransfer.Count > 0)
            {
                String dataLine = dataTransfer[0];
                dataTransfer.RemoveAt(0);

                MatchCollection gridMatches = Regex.Matches(dataLine, @"C:(\d+),(\d+) (\d+),(\d+):E: (\w+)");

                foreach (Match match in gridMatches)
                {
                    int startX = Convert.ToInt32(match.Groups[1].Value);
                    int startY = Convert.ToInt32(match.Groups[2].Value);
                    int endX = Convert.ToInt32(match.Groups[3].Value);
                    int endY = Convert.ToInt32(match.Groups[4].Value);
                    String elementData = match.Groups[5].Value;

                    DurationSquareDataChain dataChain = new DurationSquareDataChain(elementData,
                        new Coordinate(startX, startY), new Coordinate(endX, endY));
                    durationChains.Add(dataChain);
                }
            }
        }
        
        private void LoadGrid()
        {
            durationDataGrid = new DurationSquareData[durationWidth, height];
            InitializeDurationGridData();
        }

        //Calculates number of relevant elements in line using regexp
        private int GetGridWidth(String inputLine)
        {
            String gridString = Regex.Match(inputLine, @"E:(.+):end").Groups[1].ToString();
            MatchCollection matches = Regex.Matches(gridString, @" \w+");

            return matches.Count;
        }

        private int GetGridHeight(String[] data)
        {
            int height = 0;
            String currentLine = data[height + 2];
            while (true)
            {
                Match match = Regex.Match(currentLine, @"Duration");
                if (match.Success || height == data.Length - 2)
                {
                    break;
                }

                height++;
                currentLine = data[height + 2];
            }

            return height;
        }
        #endregion

        #region saving
        public List<String> GetDataAsText()
        {


            String header = "Header " +
                ":duration " + duration +
                ":width " + widthInPixels +
                ":durGridWidth " + durationDataGrid.GetLength(0) +
                ":levelObjective " + GetObjectiveSaveString() +
                ":";

            List<String> gridStrings = GetGridAsStrings();            
            List<String> chainStrings = GetChainsAsStrings();
            List<String> output = new List<String>();

            output.Add(header);
            output.Add(gridStrings[0]);

            //Entering gridpart
            for (int n = 0; n < gridStrings.Count - 1; n++)
            {
                String entry = "T: " + timeVector[n].ToString() + ":E:" + gridStrings[n + 1] + ":end";
                output.Add(entry);
            }

            output.Add(chainStrings[0]);

            //Entering chainpart
            for (int n = 1; n < chainStrings.Count; n++)
            {
                output.Add(chainStrings[n].ToString());
            }

            return output;
        }

        private String GetObjectiveSaveString()
        {
            return objective.ToString() + "|" + "X" + objectiveValue;
        }

        private List<String> GetGridAsStrings()
        {
            List<String> stringList = new List<String>();
            stringList.Add("Point");

            for (int n = 0; n < height; n++)
            {
                stringList.Add("");
                for (int m = 0; m < pointWidth; m++)
                {
                    String s = stringList[n + 1];
                    s += pointDataGrid[m, n].ToString();
                    stringList[n + 1] = s;                    
                }
            }

            return stringList;
        }

        private List<String> GetChainsAsStrings()
        {
            List<String> stringList = new List<String>();
            stringList.Add("Duration");

            foreach (DurationSquareDataChain chain in durationChains)
            {
                stringList.Add(chain.ToString());
            }

            return stringList;
        }
        #endregion
    }
}