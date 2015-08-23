using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class Coordinate
    {
        protected int x;
        public int X { get { return x; } set { x = value; } }

        protected int y;
        public int Y { get { return y; } set { y = value; } }

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate))
                return false;

            Coordinate other = (Coordinate)obj;
            return (X == other.X && Y == other.Y);
        }

        public override int GetHashCode()
        {
            return X^Y;
        }
    }
}
