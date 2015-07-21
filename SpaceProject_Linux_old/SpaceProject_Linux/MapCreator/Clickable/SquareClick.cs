using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    public class SquareClick
    {
        public Boolean isLeftClicked;
        public SquareData targetData;
        public Coordinate coordinate;

        public SquareClick(Boolean isLeftClicked, SquareData targetData, Coordinate coordinate)
        {
            this.isLeftClicked = isLeftClicked;
            this.targetData = targetData;
            this.coordinate = coordinate;
        }

    }
}
