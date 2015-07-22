using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class TimeVector
    {
        private Game1 game;

        private float[] timings;
        private static IntervalButton[] timeVector;

        SpriteFont font;

        public TimeVector(Game1 game, Vector2 position, float[] timings)
        {
            this.game = game;
            this.timings = timings;
            SetTimeVector(timings);

            font = game.Content.Load<SpriteFont>("Fonts/Iceland_12");
        }

        public void SetTimeVector(float[] timings)
        {
            this.timings = timings;
            timeVector = new IntervalButton[timings.GetLength(0)];

            for (int n = 0; n < timings.Length; n++)
            {
                if (n == 0)
                {
                    timeVector[n] = new IntervalButton(new Vector2(25, LevelMechanics.CalculateYPos(n)), null, null, 0);
                    timeVector[n].Initialize();
                }
                else
                {
                    timeVector[n] = new IntervalButton(new Vector2(25, LevelMechanics.CalculateYPos(n)), timeVector[n - 1], null, timings[n]);
                    timeVector[n].Initialize();
                    timeVector[n - 1].SetNext(timeVector[n]);
                }
            }
        }

        public IntervalButton[] GetTimeVector()
        {
            return timeVector;
        }

        public void Update(GameTime gameTime)
        {
            for (int m = LevelMechanics.GetLowerPosition(); m < LevelMechanics.GetHigherPosition(); m++)
            {
                if (m >= timings.GetLength(0))
                    break;

                timeVector[m].Update(gameTime);
                timeVector[m].SetOffset(new Vector2(0, LevelMechanics.GetCurrentOffset()));
                    
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int lowerPos = LevelMechanics.GetLowerPosition();
            int higherPos = LevelMechanics.GetHigherPosition();

            for (int m = lowerPos; m < higherPos; m++)
            {
                //Breaks loop if index going out of bounds.
                if (m >= timings.GetLength(0))
                    break;

                timeVector[m].Draw(spriteBatch, font);
            }
        }

        public static float GetTimeFromIndex(int index)
        {
            return timeVector[index].GetTime();
        }
    }
}
