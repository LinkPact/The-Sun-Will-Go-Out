using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class HangarBoss : BossLevelEvent
    {
        public HangarBoss(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, float startTime) :
            base(Game, player, spriteSheet, level, startTime)
        {
            float x1 = level.LevelWidth / 2;
            SetupCreature setup1 = new SetupCreature();
            setup1.SetBossMovement(100);
            SingleEnemy event1 = new SingleEnemy(Game, player, spriteSheet, level, EnemyType.A_hangar, 0, x1);
            event1.CreatureSetup(setup1);
            waveEvents.Add(event1);

            CompileEvents();
        }

        private void BuildCreatureList()
        { 

        }

        public override void Run(GameTime gameTime)
        {
            base.Run(gameTime);
        }
    }
}
