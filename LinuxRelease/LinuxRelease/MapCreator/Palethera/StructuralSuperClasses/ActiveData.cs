using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public static class ActiveData
    {
        public static EnemyType enemyState = EnemyType.R_mosquito;
        public static PointEventType pointEventState = PointEventType.point;
        public static DurationEventType durationEventState = DurationEventType.even;
        public static Movement movementState = Movement.None;
        public static LevelObjective levelObjective = LevelObjective.Finish;

        public static Boolean isPointActive = true;
    }
}
