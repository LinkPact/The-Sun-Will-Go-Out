using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class EliminationBoss : BossLevelEvent
    {
        public EliminationBoss(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, float startTime) :
            base(Game, player, spriteSheet, level, startTime)
        {
            float x1 = 1 * level.LevelWidth / 6;
            SetupCreature setup1 = new SetupCreature();
            setup1.SetBossMovement(100);
            SingleEnemy event1 = new SingleEnemy(Game, player, spriteSheet, level, EnemyType.turret, 0, x1);
            event1.CreatureSetup(setup1);
            waveEvents.Add(event1);

            float x2 = 2 * level.LevelWidth / 6;
            SetupCreature setup2 = new SetupCreature();
            setup2.SetBossMovement(200);
            SingleEnemy event2 = new SingleEnemy(Game, player, spriteSheet, level, EnemyType.turret, 0, x2);
            event2.CreatureSetup(setup2);
            waveEvents.Add(event2);

            float x3 = 3 * level.LevelWidth / 6;
            SetupCreature setup3 = new SetupCreature();
            setup3.SetBossMovement(300);
            SingleEnemy event3 = new SingleEnemy(Game, player, spriteSheet, level, EnemyType.turret, 0, x3);
            event3.CreatureSetup(setup3);
            waveEvents.Add(event3);

            float x4 = 4 * level.LevelWidth / 6;
            SetupCreature setup4 = new SetupCreature();
            setup4.SetBossMovement(200);
            SingleEnemy event4 = new SingleEnemy(Game, player, spriteSheet, level, EnemyType.turret, 0, x4);
            event4.CreatureSetup(setup4);
            waveEvents.Add(event4);

            float x5 = 5 * level.LevelWidth / 6;
            SetupCreature setup5 = new SetupCreature();
            setup5.SetBossMovement(100);
            SingleEnemy event5 = new SingleEnemy(Game, player, spriteSheet, level, EnemyType.turret, 0, x5);
            event5.CreatureSetup(setup5);
            waveEvents.Add(event5);

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
