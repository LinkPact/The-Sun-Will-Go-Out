using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /// <summary> Library class for characteristics of events.
    /// Library class for the different characteristics of events.
    /// It is both possible to get the enum from strings, and vice verse.
    /// </summary>
    
    public class DataConversionLibrary
    {
        private static Dictionary<EnemyType, string> enemyLibrary;
        private static Dictionary<PointEventType, string> pointLibrary;
        private static Dictionary<DurationEventType, string> durationLibrary;
        private static Dictionary<Movement, string> movementLibrary;
        
        public DataConversionLibrary()
        {
            enemyLibrary = new Dictionary<EnemyType, string>();
            pointLibrary = new Dictionary<PointEventType, string>();
            durationLibrary = new Dictionary<DurationEventType, string>();
            movementLibrary = new Dictionary<Movement, string>();
            
            //Enemy
            enemyLibrary.Add(EnemyType.none, "0");
            enemyLibrary.Add(EnemyType.turret, "T");
            enemyLibrary.Add(EnemyType.meteor, "S");
            
            enemyLibrary.Add(EnemyType.R_mosquito, "G");
            enemyLibrary.Add(EnemyType.R_thickShooter, "R");
            enemyLibrary.Add(EnemyType.R_smallSniper, "Y");
            enemyLibrary.Add(EnemyType.R_medium, "M");
            enemyLibrary.Add(EnemyType.R_minelayer, "N");
            enemyLibrary.Add(EnemyType.R_smallAttack, "a");
            enemyLibrary.Add(EnemyType.R_burster, "B");
            enemyLibrary.Add(EnemyType.R_missileAttackShip, "m");

            enemyLibrary.Add(EnemyType.A_stealthShip, "t");
            enemyLibrary.Add(EnemyType.A_drone, "d");
            enemyLibrary.Add(EnemyType.A_smallLaserShip, "n");
            enemyLibrary.Add(EnemyType.A_shielder, "h");
            
            enemyLibrary.Add(EnemyType.A_big_I, "L");
            enemyLibrary.Add(EnemyType.A_homingBullet, "H");
            enemyLibrary.Add(EnemyType.A_homingMissile, "O");
            enemyLibrary.Add(EnemyType.A_bigMissile, "I");
            enemyLibrary.Add(EnemyType.A_smallShooter, "s");

            enemyLibrary.Add(EnemyType.fighterAlly, "A");
            enemyLibrary.Add(EnemyType.freighterAlly, "F");

            //Point event
            pointLibrary.Add(PointEventType.point, "0");
            pointLibrary.Add(PointEventType.line, "l");
            pointLibrary.Add(PointEventType.square, "s");
            pointLibrary.Add(PointEventType.vformation, "t");

            //Duration event
            durationLibrary.Add(DurationEventType.even, "E");
            durationLibrary.Add(DurationEventType.gradient, "G");
            
            //Movement
            movementLibrary.Add(Movement.None, "nn");
            movementLibrary.Add(Movement.CrossOver, "c1");
            //movementLibrary.Add(Movement.CrossOver2, "c2");
            movementLibrary.Add(Movement.Following, "fo");
            //movementLibrary.Add(Movement.FollowingAll, "fa");
            //movementLibrary.Add(Movement.FollowingPlayer, "fp");
            movementLibrary.Add(Movement.Line, "ln");
            movementLibrary.Add(Movement.SlantingLine, "sl");
            movementLibrary.Add(Movement.Stopping, "st");
            //movementLibrary.Add(Movement.SweepingShooting, "ss");
            movementLibrary.Add(Movement.Zigzag, "zz");
            movementLibrary.Add(Movement.AI, "ai");
        }

        public static string GetEnemyStringFromEnum(EnemyType type)
        {
            return enemyLibrary[type];
        }

        public static EnemyType GetEnemyEnumFromString(String str)
        {
            ICollection<EnemyType> keys = enemyLibrary.Keys;

            foreach (EnemyType type in keys)
            {
                if (enemyLibrary[type].Equals(str))
                {
                    return type;
                }
            }

            throw new ArgumentException("Input string wasn't found!");
        }

        public static string GetPointStringFromEnum(PointEventType type)
        {
            return pointLibrary[type];
        }

        public static PointEventType GetPointEnumFromString(String str)
        {
            ICollection<PointEventType> keys = pointLibrary.Keys;

            foreach (PointEventType type in keys)
            {
                if (pointLibrary[type].Equals(str))
                {
                    return type;
                }
            }

            throw new ArgumentException("Input string wasn't found!");
        }

        public static string GetDurationStringFromEnum(DurationEventType type)
        {
            return durationLibrary[type];
        }

        public static DurationEventType GetDurationEnumFromString(String str)
        {
            ICollection<DurationEventType> keys = durationLibrary.Keys;

            foreach (DurationEventType type in keys)
            {
                if (durationLibrary[type].Equals(str))
                {
                    return type;
                }
            }

            throw new ArgumentException("Input string wasn't found!");
        }

        public static string GetMovementStringFromEnum(Movement type)
        {
            return movementLibrary[type];
        }

        public static Movement GetMovementEnumFromString(String str)
        {
            ICollection<Movement> keys = movementLibrary.Keys;

            foreach (Movement type in keys)
            {
                if (movementLibrary[type].Equals(str))
                {
                    return type;
                }
            }

            throw new ArgumentException("Input string wasn't found!");
        }

        public static Color GetSquareColor(EnemyType squareState)
        {
            switch (squareState)
            {
                case EnemyType.none:
                    return Color.White;

                case EnemyType.R_mosquito:
                    return Color.Green;

                case EnemyType.R_thickShooter:
                    return Color.Red;

                case EnemyType.R_burster:
                    return Color.Blue;

                case EnemyType.R_smallSniper:
                    return Color.Yellow;

                case EnemyType.turret:
                    return Color.Black;

                case EnemyType.R_medium:
                    return Color.Orange;

                case EnemyType.A_big_I:
                    return Color.Purple;

                case EnemyType.meteor:
                    return Color.Gray;

                case EnemyType.A_homingBullet:
                    return new Color(80, 10, 30);

                case EnemyType.A_homingMissile:
                    return new Color(10, 200, 150);

                case EnemyType.A_bigMissile:
                    return new Color(0, 0, 100);

                case EnemyType.fighterAlly:
                    return new Color(200, 200, 255);

                case EnemyType.freighterAlly:
                    return new Color(100, 100, 255);

                case EnemyType.R_minelayer:
                    return new Color(80, 80, 80);

                case EnemyType.A_stealthShip:
                    return new Color(80, 0, 0);

                case EnemyType.A_smallShooter:
                    return new Color(0, 0, 0);

                case EnemyType.R_smallAttack:
                    return new Color(0, 0, 0);

                case EnemyType.R_missileAttackShip:
                    return new Color(0, 80, 0);

                case EnemyType.A_drone:
                    return new Color(0, 0, 80);

                case EnemyType.A_smallLaserShip:
                    return new Color(0, 80, 80);

                case EnemyType.A_shielder:
                    return new Color(160, 80, 80);

                default:
                    throw new ArgumentException("Not-yet-implemented enemytype entered");
            }
        }
    }
}
