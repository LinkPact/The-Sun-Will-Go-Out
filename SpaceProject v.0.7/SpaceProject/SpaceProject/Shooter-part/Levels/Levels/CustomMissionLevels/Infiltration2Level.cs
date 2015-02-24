using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class Infiltration2Level : MapCreatorLevel
    {
        private static readonly int MessageIndex = 10;
        private static readonly int Message1Time = 4000;

        public Infiltration2Level(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, LevelEntry levelEntry)
            : base(Game, spriteSheet, player1, levelEntry)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message1Time, MissionManager.GetMission("Main - Infiltration").GetEvent(MessageIndex).Text));
        }
    }
}
