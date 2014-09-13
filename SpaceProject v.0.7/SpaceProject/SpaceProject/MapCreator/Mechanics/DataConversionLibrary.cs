using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject.MapCreator
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
            
            // Used letters for enemies:
            // 0
            // ABCFGHILMNORSTY
            // abcdefhijklmnopst

            // Neutral
            enemyLibrary.Add(EnemyType.none, "0");
            enemyLibrary.Add(EnemyType.turret, "T");
            enemyLibrary.Add(EnemyType.meteor, "S");
            enemyLibrary.Add(EnemyType.big_R, "L");
            enemyLibrary.Add(EnemyType.homingBullet_R, "H");
            enemyLibrary.Add(EnemyType.homingMissile_R, "O");
            enemyLibrary.Add(EnemyType.bigMissile_R, "I");
            enemyLibrary.Add(EnemyType.smallShooter_R, "s");
            enemyLibrary.Add(EnemyType.medium, "M");

            // Allies
            enemyLibrary.Add(EnemyType.fighterAlly, "A");
            enemyLibrary.Add(EnemyType.freighterAlly, "F");

            // Rebels
            enemyLibrary.Add(EnemyType.R_mosquito, "G");
            enemyLibrary.Add(EnemyType.R_smallAttack, "a");
            enemyLibrary.Add(EnemyType.R_thickShooter, "R");
            enemyLibrary.Add(EnemyType.R_smallSniper, "Y");
            enemyLibrary.Add(EnemyType.R_burster, "B");
            enemyLibrary.Add(EnemyType.R_minelayer, "N");
            enemyLibrary.Add(EnemyType.R_missileAttackShip, "m");

            enemyLibrary.Add(EnemyType.R_lightMinelayer, "b");
            enemyLibrary.Add(EnemyType.R_homingMissile, "c");
            enemyLibrary.Add(EnemyType.R_bomber, "e");
            enemyLibrary.Add(EnemyType.R_fatzo, "f");

            // Alliance
            enemyLibrary.Add(EnemyType.A_drone, "d");
            enemyLibrary.Add(EnemyType.A_smallLaserShip, "n");
            enemyLibrary.Add(EnemyType.A_stealthShip, "t");
            enemyLibrary.Add(EnemyType.A_shielder, "h");
            enemyLibrary.Add(EnemyType.A_attackStealth, "C");

            enemyLibrary.Add(EnemyType.A_singleHoming, "i");
            enemyLibrary.Add(EnemyType.A_lightBeamer, "j");
            enemyLibrary.Add(EnemyType.A_multipleShot, "k");
            enemyLibrary.Add(EnemyType.A_heavyBeamer, "l");
            enemyLibrary.Add(EnemyType.A_ballistic, "o");
            enemyLibrary.Add(EnemyType.A_hangar, "p");

            //Point event
            pointLibrary.Add(PointEventType.point, "0");
            pointLibrary.Add(PointEventType.horizontal, "h");
            pointLibrary.Add(PointEventType.line, "l");
            pointLibrary.Add(PointEventType.square, "s");
            pointLibrary.Add(PointEventType.vformation, "t");

            //Duration event
            durationLibrary.Add(DurationEventType.even, "E");
            durationLibrary.Add(DurationEventType.gradient, "G");
            
            //Movement
            movementLibrary.Add(Movement.None, "nn");
            movementLibrary.Add(Movement.CrossOver, "c1");
            movementLibrary.Add(Movement.Following, "fo");
            movementLibrary.Add(Movement.Line, "ln");
            movementLibrary.Add(Movement.SlantingLine, "sl");
            movementLibrary.Add(Movement.Stopping, "st");
            movementLibrary.Add(Movement.SmallZigzag, "zz");
            movementLibrary.Add(Movement.MediumZigzag, "mz");
            movementLibrary.Add(Movement.BigZigzag, "bz");
            movementLibrary.Add(Movement.FullStop, "fs");
            movementLibrary.Add(Movement.RightHorizontal, "ho");
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
                // Neutral (Shades of Gray?)
                case EnemyType.none:                return new Color(255, 255, 255);
                case EnemyType.turret:              return new Color(150, 150, 150);
                case EnemyType.meteor:              return new Color(0, 0, 0);
                case EnemyType.medium:              return new Color(75, 75, 75);

                case EnemyType.big_R:               return Color.Black;
                case EnemyType.homingBullet_R:      return Color.Black;
                case EnemyType.homingMissile_R:     return Color.Black;
                case EnemyType.bigMissile_R:        return Color.Black;
                case EnemyType.smallShooter_R:      return Color.Black;

                // Allies
                case EnemyType.fighterAlly:         return new Color(200, 200, 255);
                case EnemyType.freighterAlly:       return new Color(100, 100, 255);

                // Rebels
                case EnemyType.R_mosquito:          return new Color(255, 255, 102);
                case EnemyType.R_thickShooter:      return new Color(204, 204, 0);
                case EnemyType.R_smallAttack:       return new Color(102, 102, 0);
                case EnemyType.R_smallSniper:       return new Color(51, 51, 0);

                case EnemyType.R_burster:           return new Color(255, 229, 204);                
                case EnemyType.R_minelayer:         return new Color(255, 178, 102);
                case EnemyType.R_lightMinelayer:    return new Color(204, 102, 0);
                case EnemyType.R_homingMissile:     return new Color(102, 51, 0);
                case EnemyType.R_bomber:            return new Color(255, 153, 153);

                case EnemyType.R_fatzo:             return new Color(255, 0, 0);
                case EnemyType.R_missileAttackShip: return new Color(153, 0, 0);

                // Alliance
                case EnemyType.A_drone:             return new Color(204, 255, 153);
                case EnemyType.A_smallLaserShip:    return new Color(128, 255, 0);
                case EnemyType.A_singleHoming:      return new Color(51, 102, 0);

                case EnemyType.A_shielder:          return new Color(102, 255, 102);
                case EnemyType.A_attackStealth:     return new Color(0, 255, 0);
                case EnemyType.A_lightBeamer:       return new Color(0, 153, 0);
                case EnemyType.A_multipleShot:     return new Color(0, 51, 0);
                case EnemyType.A_ballistic:         return new Color(102, 255, 255);

                case EnemyType.A_stealthShip:       return new Color(0, 204, 204);
                case EnemyType.A_heavyBeamer:       return new Color(102, 102, 255);
                case EnemyType.A_hangar:            return new Color(0, 0, 153);

                default:
                    throw new ArgumentException("Not-yet-implemented enemytype entered");
            }
        }
    }
}
