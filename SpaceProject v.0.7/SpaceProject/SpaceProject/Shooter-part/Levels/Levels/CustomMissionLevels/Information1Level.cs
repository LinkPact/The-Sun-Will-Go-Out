using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class Information1Level : MapCreatorLevel
    {
        private static readonly int Message1Index = 3;
        private static readonly int Message1Time = 500;

        public Information1Level(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, LevelEntry levelEntry)
            : base(Game, spriteSheet, player1, levelEntry)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message1Time, MissionManager.GetMission("Main - Information").GetEvent(Message1Index).Text));
        }
    }
}
