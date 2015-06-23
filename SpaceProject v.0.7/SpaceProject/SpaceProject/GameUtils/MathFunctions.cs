using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Klass dar jag placerat matematik som senare kunde komma tankas att behovas av ett flertal klasser
    public class MathFunctions
    {
        private static Random rand = new Random();

        public static double GetExternalRandomDouble()
        {
            return rand.NextDouble();
        }

        public static int GetExternalRandomInt(int lower, int inclusiveUpper)
        {
            return rand.Next(lower, inclusiveUpper + 1);
        }

        public static T PickRandomFromList<T>(List<T> list)
        {
            int randPos = rand.Next(list.Count);
            return list[randPos];
        }

        // Convert overworld coordinate to Vector2-position
        public static Vector2 CoordinateToPosition(Vector2 coordinate)
        { 
            var pixelOrigin = new Vector2(OverworldState.OVERWORLD_WIDTH / 2, OverworldState.OVERWORLD_HEIGHT / 2);
            return new Vector2(pixelOrigin.X + coordinate.X * 10, pixelOrigin.Y - coordinate.Y * 10);
        }

        //Skalar en Vector2 sa att absolutbeloppet av dess x och y blir 1, anvands framst for att se till7
        //att riktningen inte andrar absolutbelopp nar man andrar riktningen.
        public static Vector2 ScaleDirection(Vector2 dir)
        {
            if (dir.X == 0 && dir.Y == 0)
                throw new ArgumentException("Cannot scale a (0,0)-vector");

            Vector2 inVector = dir;
            
            double x = dir.X;
            double y = dir.Y;

            double scaleFactor = Math.Pow(x, 2) + Math.Pow(y, 2);

            dir.X = (float)Math.Sqrt(Math.Pow(x, 2) / scaleFactor);
            dir.Y = (float)Math.Sqrt(Math.Pow(y, 2) / scaleFactor);

            if (x < 0)
                dir.X = dir.X * -1;

            if (y < 0)
                dir.Y = dir.Y * -1;

            return dir;
        }

        public static Vector2 GetRandomDirection()
        {
            double randDir = rand.NextDouble() * Math.PI * 2;
            Vector2 vectorDir = DirFromRadians(randDir);
            return vectorDir;
        }

        public static Vector2 GetRandomDownDirection()
        {
            double randDir = rand.NextDouble() * Math.PI;
            Vector2 vectorDir = DirFromRadians(randDir);
            return vectorDir;
        }

        //Anvands vid cirkular rorelse, for att kunna hantera den abrupta overgangen mellan 360 grader och 0 grader.
        public static double DeltaRadians(double rad1, double rad2)
        {
            double delta;

            double diff = Math.Abs(rad2 - rad1);

            if (diff <= Math.PI)
                delta = rad2 - rad1;
            else if (rad2 - rad1 < -Math.PI)
                delta = (rad2 - rad1) + 2 * Math.PI;
            else //(rad2 - rad1 > Math.PI) ar resten tror jag :)
                delta = (rad2 - rad1) - 2 * Math.PI;

            return delta;
        }

        //public static double Radians { get; set; }

        //Omvandlar ett radianvarde till en Vector2
        public static Vector2 DirFromRadians(double radians)
        {
            Vector2 newDir = Vector2.Zero;

            newDir.X = (float)(Math.Cos(radians));
            newDir.Y = (float)(Math.Sin(radians));

            return newDir;
        }

        //Omvandlar en skalad Vector2 till radianer
        public static double RadiansFromDir(Vector2 dir)
        {
            double dirRadians;

            if (dir.Y > 0)
                dirRadians = Math.Acos(dir.X);
            else
                dirRadians = 2 * Math.PI - (Math.Acos(dir.X));

            if (dirRadians > 2 * Math.PI)
                dirRadians -= 2 * Math.PI;

            return dirRadians;
        }

        //Skapar riktning inom spridningsintervall angivet kring initialriktning.
        public static Vector2 SpreadDir(Vector2 dir, double spread)
        {
            double dirRadians = MathFunctions.RadiansFromDir(dir);
            dirRadians += ((rand.NextDouble() * spread) - spread / 2);

            Vector2 newDir = DirFromRadians(dirRadians);

            return newDir;
        }

        //Skapar riktning inom spridningsintervall angivet kring initialriktning.
        public static Vector2 SpreadPos(Vector2 position, float spreadLength)
        {
            double dX = rand.NextDouble() * 2 * spreadLength - spreadLength;
            double dY = rand.NextDouble() * 2 * spreadLength - spreadLength;

            position = new Vector2(position.X + (float)dX, position.Y + (float)dY);

            return position;
        }

        //Varies values randomly around a given value.
        //Can for example be used in differing weapons characteristics.
        public static float VaryValue(float initialVal, double percentVar)
        {
            if (percentVar > 1) { percentVar = 1; }
            if (percentVar < 0) { percentVar = 0; }

            float changedVal = initialVal + 2 * (float)percentVar * initialVal * (float)(rand.NextDouble() - 0.5);
            return changedVal;
        }

        public static GameObjectVertical ReturnClosestObject(GameObjectVertical object1, float range, List<GameObjectVertical> objects)
        {
            GameObjectVertical tempTarget = null;
            float tempDistance = range;

            foreach (GameObjectVertical obj in objects)
            {
                if (MathFunctions.ObjectDistance(object1, obj) < tempDistance
                     && MathFunctions.ObjectDistance(object1, obj) < range)
                {
                    tempTarget = obj;
                    tempDistance = MathFunctions.ObjectDistance(object1, tempTarget);
                }
            }

            return tempTarget;
        }

        public static GameObjectVertical ReturnClosestObject(GameObjectVertical object1, float range, List<GameObjectVertical> objects, string kind)
        {
            GameObjectVertical tempTarget = null;
            float tempDistance = range;

            foreach (GameObjectVertical obj in objects)
            {
                if (obj.ObjectClass.ToLower() == kind.ToLower() 
                    && MathFunctions.ObjectDistance(object1, obj) < tempDistance
                     && MathFunctions.ObjectDistance(object1, obj) < range)
                {
                    tempTarget = obj;
                    tempDistance = MathFunctions.ObjectDistance(object1, tempTarget);
                }
            }

            return tempTarget;
        }

        public static GameObjectVertical ReturnClosestObject(GameObjectVertical object1, float range, List<GameObjectVertical> objects, List<string> kinds)
        {
            GameObjectVertical tempTarget = null;
            float tempDistance = range;

            foreach (GameObjectVertical obj in objects)
            {
                for (int i = 0; i < kinds.Count; i++)
                {
                    if (obj.ObjectClass.ToLower() == kinds[i].ToLower() && MathFunctions.ObjectDistance(object1, obj) < tempDistance
                         && MathFunctions.ObjectDistance(object1, obj) < range)
                    {
                        tempTarget = obj;
                        tempDistance = MathFunctions.ObjectDistance(object1, tempTarget);
                    }
                }
            }

            return tempTarget;
        }

        //Calculates distance between two GameObjectVerticals
        public static float ObjectDistance(GameObjectVertical obj1, GameObjectVertical obj2)
        {
            float distance = Vector2.Distance(obj1.Position, obj2.Position);
            return distance;
        }

        public static Boolean AreObjectsOfTypes<T, U>(Object obj1, Object obj2)
        {
            if ((obj1 is T && obj2 is U)
                || (obj1 is U && obj2 is T))
            {
                return true;
            }

            return false;
        }

        public static Boolean IsOneOfType<T>(Object obj1, Object obj2)
        {
            if ((obj1 is T) || (obj2 is T))
            {
                return true;
            }

            return false;
        }

        // Inspired by post on StackOverflow, get enum value from string
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static List<double> GetSpreadDirList(double spreadRadians, double numberOfShots)
        {
            List<double> directions = new List<double>();
            double center = Math.PI / 2;

            //double degreesSpread = spreadRadians * (360 / (2 * Math.PI));

            for (double dir = center - (spreadRadians / 2); dir <= center + (spreadRadians / 2); dir += (spreadRadians / (numberOfShots - 1)))
            {
                directions.Add(dir);
            }
            return directions;
        }

        public static Vector2 ChangeDirection(Vector2 dir, Vector2 pos, Vector2 followedPos, double degreeChange)
        {
            double radians = RadiansFromDir(dir);

            Vector2 dirToFollowed = new Vector2(followedPos.X - pos.X, followedPos.Y - pos.Y);
            dirToFollowed = MathFunctions.ScaleDirection(dirToFollowed);

            double dirFolRadians;

            if (dirToFollowed.Y > 0)
                dirFolRadians = Math.Acos(dirToFollowed.X);
            else
                dirFolRadians = 2 * Math.PI - (Math.Acos(dirToFollowed.X));

            if (dirFolRadians > 2 * Math.PI)
                dirFolRadians -= 2 * Math.PI;

            if (MathFunctions.DeltaRadians(radians, dirFolRadians) > 0 && MathFunctions.DeltaRadians(radians, dirFolRadians) <= 180)
                radians += degreeChange * (Math.PI / 180);
            else if (MathFunctions.DeltaRadians(radians, dirFolRadians) < 0 && MathFunctions.DeltaRadians(radians, dirFolRadians) <= 180)
                radians -= degreeChange * (Math.PI / 180);
            else if (MathFunctions.DeltaRadians(radians, dirFolRadians) > 0 && MathFunctions.DeltaRadians(radians, dirFolRadians) > 180)
                radians -= degreeChange * (Math.PI / 180);
            else //(dirFolRadians < Radians && (dirFolRadians - Radians) > 180)
                radians += degreeChange * (Math.PI / 180);

            Vector2 newDir = Vector2.Zero;
            newDir.X = (float)(Math.Cos(radians));
            newDir.Y = (float)(Math.Sin(radians));

            return newDir;
        }
    }
}
