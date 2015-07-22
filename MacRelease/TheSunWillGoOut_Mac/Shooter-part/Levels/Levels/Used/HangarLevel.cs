using System;
using System.Collections.Generic;

namespace SpaceProject_Mac
{
    class HangarLevel : MapCreatorLevel
    {
        public HangarLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, String identifier,
            String filePath, MissionType missionType)
            : base(Game, spriteSheet, player1, identifier, filePath, missionType)
        {
        }

        public HangarLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, LevelEntry levelEntry)
            : base(Game, spriteSheet, player1, levelEntry)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            HangarBoss boss = new HangarBoss(Game, player, spriteSheet, this, 0);
            untriggeredEvents.Add(boss);

            SetCustomVictoryCondition(LevelObjective.Boss, -1);
        }
    }
}
