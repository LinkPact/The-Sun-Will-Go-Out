using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class CursorCoordinate : Coordinate
    {
        private int gridWidth;
        private int gridHeight;
        private Boolean oddEnd;
        private int lastY;

        public int Position { get { return ToInt(); } private set { } }

        public CursorCoordinate(int x, int y, int gridWidth, int gridHeight, Boolean oddEnd = false)
            : base (x, y)
        {
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            this.oddEnd = oddEnd;
        }

        public int ToInt()
        {
            switch (x)
            {
                case 0:
                    return 0;

                case 1:
                    if (y == 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }

                case 2:
                    return 3;

                case 3:
                    return 4;

                default:
                    throw new ArgumentException("Invalid coordinate position.");
            }
        }

        public void MoveCursor(int x, int y)
        {
            this.x += x;
            this.y += y;

            SetXCoordinateToValid();
            SetYCoordinateToValid(x);
        }

        private void SetXCoordinateToValid()
        {
            if (x < 0)
            {
                if (oddEnd == false || Y != gridHeight - 1)
                {
                    x += gridWidth;
                }
                else
                {
                    x += gridWidth + 1;
                }
            }
            else if (x >= gridWidth)
            {
                if (oddEnd == false || Y != gridHeight - 1)
                    x = 0;
                else
                {
                    if (x >= gridWidth)
                        x = 0;
                }
            }
        }

        private void SetYCoordinateToValid(int x)
        {
            if (this.x == 0 || this.x == 2)
            {
                lastY = y;
                y = 0;
            }

            else if (x != 0)
            {
                y = lastY;
            }

            if (y >= gridHeight)
            {
                y = 0;
            }

            else if (y < 0)
            {
                y = gridHeight - 1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.GetFontStatic(12), "X: " + x + " Y: " + y, new Vector2(10, 10), Color.White);
        }
    }
}
