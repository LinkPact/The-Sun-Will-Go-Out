using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class Infiltration1Level : MapCreatorLevel
    {
        private static readonly int Message1Index = 8;
        private static readonly int Message1Time = 8000;

        public Infiltration1Level(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, LevelEntry levelEntry)
            : base(Game, spriteSheet, player1, levelEntry)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message1Time, MissionManager.GetMission(MissionID.Main4_Infiltration).GetEvent(Message1Index).Text));
        }
    }
}
