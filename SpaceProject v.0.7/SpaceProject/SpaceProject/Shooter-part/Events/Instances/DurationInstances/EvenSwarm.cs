using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class EvenSwarm : Swarm
    {
        //This class represents a swarm of Creatures of desired type and duration.
        #region declaration
        private float thickness;
        #endregion
        public EvenSwarm(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, float duration, float thickness)
            : base(Game, player, spriteSheet, level, identifier, startTime, duration)
        {
            this.thickness = thickness;
            SpawnDelay = 1000 / (thickness / 10);
        }
        public EvenSwarm(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            PointLevelEvent singleEvent, float startTime, float duration, float thickness)
            : base(Game, player, spriteSheet, level, singleEvent, startTime, duration)
        {
            this.thickness = thickness;
            SpawnDelay = 1000 / (thickness / 10);
        }
        public override void Run(GameTime gameTime)
        {
            base.Run(gameTime);
        }
        public override List<CreaturePackage> RetrieveCreatures()
        {
            List<CreaturePackage> creatures = new List<CreaturePackage>();

            for (float t = 0; t < Duration; t += SpawnDelay)
            {
                Creature crit = ReturnCreature();
                creatures.Add(new CreaturePackage(Game, spriteSheet, crit, (int)t));
            }

            return creatures;
        }
    }
}
