using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class GradientSwarm : Swarm
    {
        //This class represents a swarm of Creatures of desired type and duration.
        #region declaration
        private float minThickness;
        private float maxThickness;
        private float peakTime;
        #endregion
        public GradientSwarm(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, float duration, float peakPercentage, float minThickness, float maxThickness)
            : base(Game, player, spriteSheet, level, identifier, startTime, duration)
        {
            this.minThickness = minThickness / 10;
            this.maxThickness = maxThickness / 10;
            this.peakTime = peakPercentage / 100 * duration;
        }
        public GradientSwarm(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            PointLevelEvent singleEvent, float startTime, float duration, float peakPercentage, float minThickness, float maxThickness)
            : base(Game, player, spriteSheet, level, singleEvent, startTime, duration)
        {
            this.minThickness = minThickness / 10;
            this.maxThickness = maxThickness / 10;
            this.peakTime = peakPercentage / 100 * duration;
        }
        public override void Run(GameTime gameTime)
        {
            SpawnDelay = 1000 / CalculateCurrentThickness();
            base.Run(gameTime);
        }
        public override List<CreaturePackage> RetrieveCreatures()
        {
            List<CreaturePackage> creatures = new List<CreaturePackage>();

            SpawnDelay = 1000 / CalculateCurrentThickness();
            for (float t = 0; t <= Duration; t += SpawnDelay)
            {
                SpawnDelay = 1000 / CalculateCurrentThickness();
                creatures.Add(new CreaturePackage(Game, spriteSheet, ReturnCreature(), (int)t));
            }

            return creatures;
        }
        private float CalculateCurrentThickness()
        {
            bool peakTimePassed;

            if (timePassed < peakTime) peakTimePassed = false;
            else peakTimePassed = true;

            float currentThickness;

            if (!peakTimePassed)
                currentThickness = minThickness + (maxThickness - minThickness) * timePassed / peakTime;
            else
                currentThickness = minThickness + maxThickness - maxThickness * (timePassed - peakTime) / (Duration - peakTime);

            return currentThickness;
        }
    }
}
