using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    class DurationSquareDataChain
    {
        public DurationSquareData head;
        public DurationSquareData tail;
        public List<DurationSquareData> body;

        public List<DurationSquareData> complete;

        public EnemyType enemyType;
        public DurationEventType eventType;
        //public Movement movementType;

        public List<Coordinate> coordinates;
        public Coordinate startCoordinate;
        public Coordinate endCoordinate;

        public DurationSquareDataChain(EnemyType enemyType, DurationEventType eventType, 
            Movement movementType, DurationSquareData head, DurationSquareData tail, 
            List<DurationSquareData> body, Coordinate startCoordinate, Coordinate endCoordinate)
        {
            this.enemyType = enemyType;
            this.eventType = eventType;

            this.head = head;
            this.tail = tail;

            this.body = new List<DurationSquareData>();
            this.body = body;

            complete = new List<DurationSquareData>();
            complete.Add(head);
            complete.AddRange(body);
            complete.Add(tail);
            foreach (DurationSquareData data in complete)
            {
                data.enemyType = enemyType;
                data.movement = movementType;
                data.durationEventType = eventType;
                data.eventData = new DurationEventData(ActiveData.durationEventState);
            }

            this.startCoordinate = startCoordinate;
            this.endCoordinate = endCoordinate;

            coordinates = new List<Coordinate>();
            for (int y = startCoordinate.Y; y <= endCoordinate.Y; y++)
            {
                coordinates.Add(new Coordinate(startCoordinate.X, y));
            }
        }

        public DurationSquareDataChain(String loadText, Coordinate startCoordinate, Coordinate endCoordinate)
        {
            this.head = new DurationSquareData(loadText);
            this.tail = new DurationSquareData(loadText);

            this.body = new List<DurationSquareData>();
            for (int pos = (startCoordinate.Y + 1); pos < endCoordinate.Y; pos++)
            {
                body.Add(new DurationSquareData(loadText));
            }
            
            this.enemyType = head.enemyType;
            this.eventType = head.durationEventType;

            complete = new List<DurationSquareData>();
            complete.Add(head);
            complete.AddRange(body);
            complete.Add(tail);

            this.startCoordinate = startCoordinate;
            this.endCoordinate = endCoordinate;

            coordinates = new List<Coordinate>();
            for (int y = startCoordinate.Y; y <= endCoordinate.Y; y++)
            {
                coordinates.Add(new Coordinate(startCoordinate.X, y));
            }
        }

        public DurationSquareData GetDataFromCoordinate(Coordinate otherCoordinates)
        {
            if (otherCoordinates.X != coordinates[0].X)
            {
                return null;
            }
            else if (otherCoordinates.Y >= coordinates[0].Y && otherCoordinates.Y <= coordinates[coordinates.Count - 1].Y)
            {
                for (int n = 0; n < coordinates.Count; n++)
                {
                    if (otherCoordinates.Y == coordinates[n].Y)
                        return complete[n];
                }
            }

            return null;
        }

        public Boolean IsOutside(int xBoundary, int yBoundary)
        {
            if (coordinates[0].X >= xBoundary || coordinates[0].X < 0)
            {
                return true;
            }
            else
            {
                foreach (Coordinate coord in coordinates)
                {
                    if (coord.Y >= yBoundary || coord.Y < 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            String returnString = "C:" + coordinates[0].X + "," + coordinates[0].Y + " " + 
                coordinates[coordinates.Count - 1].X + "," + coordinates[coordinates.Count - 1].Y + ":";
            returnString += "E:" + head.ToString();
            return returnString;
        }
    }
}
