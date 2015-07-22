using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class EliminationBossDanne : WavesLevelEvent
    {
        //List<PointLevelEvent> waveEvents = new List<PointLevelEvent>();

        public EliminationBossDanne(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, float startTime) :
            base(Game, player, spriteSheet, level, startTime)
        {
            //waveEvents.Add(new EvenSwarm(Game, player, spriteSheet, "yellow", 0, 5000, 1f));


            //LineFormation greenLine = new LineFormation(Game, player, spriteSheet, level, "green", 0, 4, 50, Game.ScreenCenter.X);
            //greenLine.SetMovement(Movement.CrossOver);
            //waveEvents.Add(greenLine);
            //
            //waveEvents.Add(new VFormation(Game, player, spriteSheet, level, "big", 1000, 2, 150, 50, Game.ScreenCenter.X));
            //
            //waveEvents.Add(new LineFormation(Game, player, spriteSheet, level, "blue", 2000, 2, 50, Game.ScreenCenter.X / 3));
            //waveEvents.Add(new LineFormation(Game, player, spriteSheet, level, "blue", 2000, 2, 50, Game1.ScreenSize.X - Game.ScreenCenter.X / 3));
            
            //waveEvents.Add(new SquareFormation(Game, player, spriteSheet, "green", 1000, 8, 2, 20, 20, 400));
            //PointLevelEvent square2 = new SquareFormation(Game, player, spriteSheet, "red", 1000, 5, 1, 30, 20, 400);
            //square2.SetMovement(Movement.Following, 400);
            //waveEvents.Add(square2);
            //waveEvents.Add(new LineFormation(Game, player, spriteSheet, "blue", 1500, 2, 20, 400));
            //
            //waveEvents.Add(new SquareFormation(Game, player, spriteSheet, "green", 1000, 8, 2, 20, 20, 600));
            //PointLevelEvent square3 = new SquareFormation(Game, player, spriteSheet, "red", 1000, 5, 1, 30, 20, 600);
            //square3.SetMovement(Movement.Following, 400);
            //waveEvents.Add(square3);
            //waveEvents.Add(new LineFormation(Game, player, spriteSheet, "blue", 1500, 2, 20, 600));
                        
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
