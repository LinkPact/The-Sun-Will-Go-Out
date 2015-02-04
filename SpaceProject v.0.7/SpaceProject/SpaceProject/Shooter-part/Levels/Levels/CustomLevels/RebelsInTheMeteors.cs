using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class RebelsInTheMeteors : MapCreatorLevel
    {
        private static readonly int Message1 = 4;
        private static readonly int Message2 = 5;
        private static readonly int Message1Time = 1000;
        private static readonly int Message2Time = 15000;

        public RebelsInTheMeteors(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, String identifier,
            String filePath, MissionType missionType)
            : base(Game, spriteSheet, player1, identifier, filePath, missionType)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message1Time, MissionManager.GetMission("Main - New First Mission").GetEvent(Message1).Text));
            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message2Time, MissionManager.GetMission("Main - New First Mission").GetEvent(Message2).Text));
        }
    }
}
