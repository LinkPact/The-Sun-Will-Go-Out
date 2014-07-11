using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class EliminationBoss : WavesLevelEvent
    {
        //List<PointLevelEvent> waveEvents = new List<PointLevelEvent>();

        public EliminationBoss(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, float startTime) :
            base(Game, player, spriteSheet, level, startTime)
        {
            //waveEvents.Add(new EvenSwarm(Game, player, spriteSheet, "yellow", 0, 5000, 1f));

            waveEvents.Add(new SquareFormation(Game, player, spriteSheet, level, EnemyType.R_mosquito, 1000, 8, 2, 20, 20, 200));
            PointLevelEvent square1 = new SquareFormation(Game, player, spriteSheet, level, EnemyType.R_thickShooter, 1000, 5, 1, 30, 20, 200);
            square1.SetMovement(Movement.Following, 400);
            waveEvents.Add(square1);
            waveEvents.Add(new LineFormation(Game, player, spriteSheet, level, EnemyType.blue, 1500, 2, 20, 200));

            waveEvents.Add(new SquareFormation(Game, player, spriteSheet, level, EnemyType.R_mosquito, 1000, 8, 2, 20, 20, 400));
            PointLevelEvent square2 = new SquareFormation(Game, player, spriteSheet, level, EnemyType.R_thickShooter, 1000, 5, 1, 30, 20, 400);
            square2.SetMovement(Movement.Following, 400);
            waveEvents.Add(square2);
            waveEvents.Add(new LineFormation(Game, player, spriteSheet, level, EnemyType.blue, 1500, 2, 20, 400));

            waveEvents.Add(new SquareFormation(Game, player, spriteSheet, level, EnemyType.R_mosquito, 1000, 8, 2, 20, 20, 600));
            PointLevelEvent square3 = new SquareFormation(Game, player, spriteSheet, level, EnemyType.R_thickShooter, 1000, 5, 1, 30, 20, 600);
            square3.SetMovement(Movement.Following, 400);
            waveEvents.Add(square3);
            waveEvents.Add(new LineFormation(Game, player, spriteSheet, level, EnemyType.blue, 1500, 2, 20, 600));
            
            
            //waveEvents.Add(new SquareFormation(Game, player, spriteSheet, "blue", 2000, 1, 1, 1, 1, 400));
            //waveEvents.Add(new SquareFormation(Game, player, spriteSheet, "yellow", 1500, 2, 1, 20, 1, 400));

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
