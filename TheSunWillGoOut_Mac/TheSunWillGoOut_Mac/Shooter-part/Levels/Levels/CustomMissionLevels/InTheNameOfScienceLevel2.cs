using System;
using System.Collections.Generic;

namespace SpaceProject_Mac
{
    class InTheNameOfScienceLevel2 : MapCreatorLevel
    {
        private static readonly int MessageIndex = 13;
        private static readonly int MessageTime = 50;

        public InTheNameOfScienceLevel2(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, LevelEntry levelEntry)
            : base(Game, spriteSheet, player1, levelEntry)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, MessageTime, MissionManager.GetMission(MissionID.Main6_InTheNameOfScience).GetEvent(MessageIndex).Text, PortraitID.RebelTroopLeader));
        }
    }
}
