using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class PointSquareGrid : SquareGrid
    {
        #region declaration

        #endregion

        public PointSquareGrid(Game1 game, Sprite spriteSheet, Vector2 position, SquareData[,] data_)
            : base (game, spriteSheet, position, data_)
        {
            spacing = 15;
        }

        public void Initialize(SquareData[,] data)
        {
            //InitializeDataGrid(data);
            CreateSquareGrid(data);
        }

        protected void ClearDataGrid(SquareData[,] newData)
        {
            int localWidth = newData.GetLength(0);
            int localHeight = newData.GetLength(1);

            for (int n = 0; n < localWidth; n++)
            {
                for (int m = 0; m < localHeight; m++)
                {
                    SquareData sd = new PointSquareData();
                    sd.Initialize();
                    newData[n, m] = sd;
                }
            }
        }

        //Creates square grid linked to given SquareData-grid
        protected void CreateSquareGrid(SquareData[,] data)
        {
            int localWidth = data.GetLength(0);
            int localHeight = data.GetLength(1);

            grid = new Square[localWidth, localHeight];

            for (int n = 0; n < localWidth; n++)
            {
                for (int m = 0; m < localHeight; m++)
                {
                    Square s = new PointSquare((PointSquareData)data[n, m], GetSquarePos(n, m), spriteSheet, new Coordinate(n,m));
                    s.Initialize();
                    grid[n, m] = s;
                }
            }
        }

        public override void UpdateGridSize(int newWidth, int newHeight)
        {
            SquareData[,] newData = new PointSquareData[newWidth, newHeight];
            ClearDataGrid(newData);

            for (int n = 0; n < newWidth; n++)
            {
                for (int m = 0; m < newHeight; m++)
                {
                    if (n < data.GetLength(0) && m < data.GetLength(1))
                    {
                        newData[n, m] = data[n, m];
                    }
                    else
                    {
                        newData[n, m] = new PointSquareData();
                    }
                }
            }
            data = newData;
            width = newWidth;
            height = newHeight;
            CreateSquareGrid(data);
        }

        public override void Update(GameTime gameTime)
        {
            for (int n = 0; n < GetWidth(); n++)
            {
                for (int m = 0; m < GetHeight(); m++)
                {
                    grid[n, m].Update(gameTime);
                    grid[n, m].SetOffset(new Vector2(0, LevelMechanics.GetCurrentOffset()));
                }
            }
        }

        public void Clear()
        {
            ClearDataGrid(data);
            CreateSquareGrid(data);
        }
    }
}
