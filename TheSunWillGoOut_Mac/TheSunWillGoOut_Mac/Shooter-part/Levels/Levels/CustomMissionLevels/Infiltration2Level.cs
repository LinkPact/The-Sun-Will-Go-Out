using System;
using System.Collections.Generic;

namespace SpaceProject_Mac
{
    class Infiltration2Level : MapCreatorLevel
    {
        private static readonly int Message1Index = 10;
        private static readonly int Message2Index = 11;
        private static readonly int Message3Index = 12;
        private static readonly int Message1Time = 4000;

        public Infiltration2Level(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, LevelEntry levelEntry)
            : base(Game, spriteSheet, player1, levelEntry)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message1Time, MissionManager.GetMission(MissionID.Main4_Infiltration).GetEvent(Message1Index).Text, PortraitID.AllianceCaptain));
            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message1Time, MissionManager.GetMission(MissionID.Main4_Infiltration).GetEvent(Message2Index).Text));
            untriggeredEvents.Add(new SimpleMessageVertical(Game, player, spriteSheet, this, Message1Time, MissionManager.GetMission(MissionID.Main4_Infiltration).GetEvent(Message3Index).Text, PortraitID.Rok));
        }
    }
}
