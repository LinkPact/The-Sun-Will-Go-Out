using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class EliminationBoss : BossLevelEvent
    {
        //List<PointLevelEvent> waveEvents = new List<PointLevelEvent>();

        public EliminationBoss(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, float startTime) :
            base(Game, player, spriteSheet, level, startTime)
        {
            SetupCreature setupCreature_fullStop = new SetupCreature();
            setupCreature_fullStop.SetMovement(Movement.FullStop);

            for (float xPos = 400; xPos < 600; xPos += 20)
            {
                SingleEnemy event1 = new SingleEnemy(Game, player, spriteSheet, level, EnemyType.R_smallSniper, 0, xPos);
                event1.CreatureSetup(setupCreature_fullStop);
                waveEvents.Add(event1);
            }
            
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
