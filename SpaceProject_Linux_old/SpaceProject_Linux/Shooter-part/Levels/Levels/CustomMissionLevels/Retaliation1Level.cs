using System;
using System.Collections.Generic;

namespace SpaceProject_Linux
{
    class Retaliation1Level : MapCreatorLevel
    {
        private static readonly int Message1Index = 7;
        private static readonly int Message2Index = 8;
        private static readonly int Message1Time = 500;

        public Retaliation1Level(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, LevelEntry levelEntry)
            : base(Game, spriteSheet, player1, levelEntry)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message1Time, MissionManager.GetMission(MissionID.Main5_Retribution).GetEvent(Message1Index).Text, PortraitID.RebelPilot));
            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message1Time, MissionManager.GetMission(MissionID.Main5_Retribution).GetEvent(Message2Index).Text, PortraitID.AlliancePilot));
        }
    }
}
