using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject.MapCreator
{
    class DurationSquareGrid : SquareGrid
    {
        #region declaration
        private List<DurationSquareDataChain> durationDataChains;
        private SquareClick markedClick;

        private MouseState previous;
        private MouseState current;
        private Boolean isRightClicked;
        #endregion

        public DurationSquareGrid(Game1 game, Sprite spriteSheet, Vector2 position, SquareData[,] data_)
            : base(game, spriteSheet, position, data_)
        {
            //durationData = new List<DurationSquareDataChain>();
            spacing = 10;
        }

        public void Initialize(SquareData[,] dataGrid, List<DurationSquareDataChain> dataChains)
        {
            durationDataChains = dataChains;
            CreateSquareGrid(dataGrid, dataChains);
        }

        //Creates square grid linked to given SquareData-CHAIN
        protected void CreateSquareGrid(SquareData[,] data, List<DurationSquareDataChain> newDurationData)
        {
            int localWidth = data.GetLength(0);
            int localHeight = data.GetLength(1);

            grid = new DurationSquare[localWidth, localHeight];

            for (int n = 0; n < localWidth; n++)
            {
                for (int m = 0; m < localHeight; m++)
                {
                    DurationSquareData existingData = CheckChainForPresentData(newDurationData, new Coordinate(n, m));
                    if (existingData != null)
                    {
                        data[n,m] = existingData;
                    }

                    Square s = new DurationSquare(data[n, m], GetSquarePos(n, m), spriteSheet, new Coordinate(n, m));
                    s.Initialize();
                    grid[n, m] = s;
                }
            }
        }

        private DurationSquareData CheckChainForPresentData(List<DurationSquareDataChain> durationData, Coordinate coordinate)
        {
            DurationSquareData data = null;

            foreach (DurationSquareDataChain chain in durationData)
            {
                data = chain.GetDataFromCoordinate(coordinate);

                if (data != null)
                {
                    break;
                }
            }

            return data;
        }

        public override void UpdateGridSize(int newWidth, int newHeight)
        {
            RemoveChainsOutside(newWidth, newHeight);

            SquareData[,] newData = new DurationSquareData[newWidth, newHeight];
            ClearDataGrid(newData);

            for (int n = 0; n < newWidth; n++)
            {
                for (int m = 0; m < newHeight; m++)
                {
                    newData[n, m] = new DurationSquareData();
                }
            }
            data = newData;
            width = newWidth;
            height = newHeight;

            CreateSquareGrid(data, durationDataChains);
        }
        private void RemoveChainsOutside(int newWidth, int newHeight)
        {
            List<DurationSquareDataChain> removeList = new List<DurationSquareDataChain>();
            foreach (DurationSquareDataChain chain in durationDataChains)
            {
                if (chain.IsOutside(newWidth, newHeight))
                {
                    removeList.Add(chain);
                }
            }

            foreach (DurationSquareDataChain chain in removeList)
            {
                durationDataChains.Remove(chain);
            }
            removeList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMouse();

            game.Window.Title = "isSquareMarked?: " + (markedClick != null).ToString() + " Number of added chains?: " + durationDataChains.Count;

            SquareClick squareClick = null;

            for (int n = 0; n < GetWidth(); n++)
            {
                for (int m = 0; m < GetHeight(); m++)
                {
                    grid[n, m].Update(gameTime);
                    grid[n, m].SetOffset(new Vector2(0, LevelMechanics.GetCurrentOffset()));

                    if (grid[n, m].HasClickInformation())
                        squareClick = grid[n, m].GetClickInformation();
                }
            }

            if (squareClick != null)
                ClickLogic(squareClick);
            else if (isRightClicked && IsSquareMarked())
                markedClick = null;
        }

        #region clear
        public void Clear()
        {
            ClearChainList(durationDataChains);
            ClearDataGrid(data);
            CreateSquareGrid(data, durationDataChains);
        }
        protected void ClearDataGrid(SquareData[,] newData)
        {
            int localWidth = newData.GetLength(0);
            int localHeight = newData.GetLength(1);

            for (int n = 0; n < localWidth; n++)
            {
                for (int m = 0; m < localHeight; m++)
                {
                    SquareData sd = new DurationSquareData();
                    sd.Initialize();
                    newData[n, m] = sd;
                }
            }
        }
        private void ClearChainList(List<DurationSquareDataChain> dataChain)
        {
            List<DurationSquareDataChain> copy = new List<DurationSquareDataChain>();
            foreach (DurationSquareDataChain chain in dataChain)
            {
                copy.Add(chain);
            }
            foreach (DurationSquareDataChain chain in copy)
            {
                dataChain.Remove(chain);
            }
            copy.Clear();
        }
        #endregion

        #region chainlogic
        private DurationSquareDataChain GetDataChain(SquareClick squareClick)
        {
            DurationSquareDataChain durationChain = new DurationSquareDataChain(ActiveData.enemyState, ActiveData.durationEventState, ActiveData.movementState, 
                    (DurationSquareData)(markedClick.targetData), (DurationSquareData)(squareClick.targetData), GetRange(markedClick.coordinate, squareClick.coordinate),
                    markedClick.coordinate, squareClick.coordinate);

            return durationChain;
        }

        private void EraseChain(DurationSquareDataChain targetChain)
        {
            foreach (DurationSquareData dataTarget in targetChain.complete)
            {
                dataTarget.Initialize();
            }
            durationDataChains.Remove(targetChain);
        }

        private DurationSquareDataChain GetTargetedChain(SquareClick squareClick)
        { 
            Coordinate targetedCoord = squareClick.coordinate;

            foreach (DurationSquareDataChain dataChain in durationDataChains)
            {
                foreach (Coordinate chainCoord in dataChain.coordinates)
                {
                    if (chainCoord.Equals(squareClick.coordinate))
                    {
                        return dataChain;
                    }
                }
            }
            return null;
        }

        private List<DurationSquareData> GetRange(Coordinate startCoordinate, Coordinate endCoordinate)
        {
            if (startCoordinate.X != endCoordinate.X)
                throw new ArgumentException("Invalid coordinate range entered!");

            List<DurationSquareData> bodyData = new List<DurationSquareData>();
            for (int y = startCoordinate.Y + 1; y < endCoordinate.Y; y++)
            {
                DurationSquareData target = (DurationSquareData)(grid[startCoordinate.X,y].data);
                bodyData.Add(target);
            }

            return bodyData;
        }
        #endregion

        #region clicklogic
        private void UpdateMouse()
        {
            previous = current;
            current = Mouse.GetState();

            if (previous.RightButton == ButtonState.Pressed && current.RightButton == ButtonState.Released)
            {
                isRightClicked = true;
            }
            else
            {
                isRightClicked = false;
            }
        }

        private void ClickLogic(SquareClick squareClick)
        {
            if (ClickIsValid(squareClick))
            {
                if (squareClick.isLeftClicked)
                    LeftClick(squareClick);
                else
                    RightClick(squareClick);
            }
            else
            {
                markedClick = null;
            }
        }

        private Boolean ClickIsValid(SquareClick squareClick)
        {
            if (markedClick != null)
            {
                if (squareClick.coordinate.X != markedClick.coordinate.X)
                    return false;
            }

            return true;
        }

        private void LeftClick(SquareClick squareClick)
        {
            if (squareClick.targetData.enemyType == EnemyType.none && !IsSquareMarked())
            {
                markedClick = squareClick;
            }
            else if (squareClick.targetData.enemyType == EnemyType.none && IsSquareMarked() && squareClick.coordinate.Y >= markedClick.coordinate.Y)
            {
                //This is where the datachains are created
                DurationSquareDataChain durationChain = GetDataChain(squareClick);

                durationDataChains.Add(durationChain);
                markedClick = null;
            }
            else
            {
                markedClick = null;
            }
        }

        private void RightClick(SquareClick squareClick)
        {
            if (IsSquareMarked())
            {
                markedClick = null;
            }
            else
            {
                DurationSquareDataChain targetedChain = GetTargetedChain(squareClick);
                if (targetedChain != null)
                    EraseChain(targetedChain);
            }
        }

        private Boolean IsSquareMarked()
        {
            return markedClick != null;
        }

        #endregion
    }
}
