using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class CursorCoordinate : Coordinate
    {
        private int gridWidth;
        private int gridHeight;
        private Boolean oddEnd;

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
            return (X + 1) + (Y * gridWidth) - 1;
        }

        public void MoveCursor(int x, int y)
        {
            this.x += x;
            this.y += y;

            SetXCoordinateToValid();
            SetYCoordinateToValid();
        }

        private void SetXCoordinateToValid()
        {
            if (x < 0)
            {
                if (oddEnd == false || Y != gridHeight - 1)
                    x += gridWidth;
                else
                {
                    x += gridWidth + 1;
                }
            }
            else if (x >= gridWidth)
            {
                if (oddEnd == false || Y != gridHeight - 1)
                    // 14-06-24 Why not set it to zero here? / Jakob
                    //x = x % gridWidth;
                    x = 0;
                else
                {
                    if (x >= gridWidth + 1)
                        x = 0;

                }
            }
        }

        private void SetYCoordinateToValid()
        {
            if (y < 0)
            {
                y += gridHeight;
            }
            else if (y >= gridHeight)
            {
                y = y % gridHeight;
            }
        }
    }
}
