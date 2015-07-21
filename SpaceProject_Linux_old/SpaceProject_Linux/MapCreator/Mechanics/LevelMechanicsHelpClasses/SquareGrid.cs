using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace SpaceProject_Linux
{
    //Class handing an array of squares. There is not much function in this class,
    //it mostly acts as a container for the squares.
    public abstract class SquareGrid
    {
        #region declaration
        protected Game1 game;
        protected Sprite spriteSheet;

        protected Square[,] grid;
        protected SquareData[,] data;

        protected int width;
        protected int height;
        protected Vector2 position;

        protected int spacing;

        #endregion

        public SquareGrid(Game1 game, Sprite spriteSheet, Vector2 position, SquareData[,] data_)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;
            
            this.position = position;

            if (spacing == 0) spacing = 15;
            this.data = data_;
        }

        protected Vector2 GetSquarePos(int n, int m)
        {
            float calculatedY = LevelMechanics.CalculateYPos(m);
            Vector2 pos = new Vector2(n * spacing + position.X, calculatedY);
            
            return pos;
        }

        public abstract void UpdateGridSize(int newWidth, int newHeight);

        public abstract void Update(GameTime gameTime);

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int n = 0; n < GetWidth(); n++)
            {
                int lowerPos = LevelMechanics.GetLowerPosition();
                int higherPos = LevelMechanics.GetHigherPosition();

                for (int m = lowerPos; m < higherPos; m++)
                {
                    //Breaks loop if going outside bounds.
                    if (m >= GetHeight())
                        break;

                    grid[n, m].Draw(spriteBatch);
                }
            }
        }

        //Retrieve text data from squares, puts it into grid
        public new String[] ToString()
        {
            String[] stringVector = new String[height];

            for (int n = 0; n < height; n++)
            {
                for (int m = 0; m < width; m++)
                {
                    stringVector[n] += grid[m, n].ToString();
                }
            }

            return stringVector;
        }

        //Calculates number of relevant elements in line using regexp
        private int GetGridWidth(String inputLine)
        {
            MatchCollection matches = Regex.Matches(inputLine, @"\b\w+\b");
            return matches.Count;
        }

        public int GetWidth()
        {
            return grid.GetLength(0);
        }

        public int GetHeight()
        {
            return grid.GetLength(1);
        }

        public SquareData[,] GetData()
        {
            return data;
        }
    }
}
