using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceProject.MapCreator;

namespace SpaceProject
{
    class SingleHorizontalEnemy : PointLevelEvent
    {
        private Boolean leftToRightDirection;
        private float yPos;

        public SingleHorizontalEnemy(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, int direction, float yPos)
            : base(Game, player, spriteSheet, level, identifier, startTime)
        {
            Setup(direction, yPos);
        }

        public SingleHorizontalEnemy(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, int direction, float yPos)
            : base(Game, player, spriteSheet, level, identifier)
        {
            Setup(direction, yPos);
        }

        private void Setup(int direction, float yPos)
        {
            this.leftToRightDirection = (direction == 1);
            this.yPos = yPos;
        }

        public override void Run(GameTime gameTime)
        {
            base.Run(gameTime);
            Vector2 pos = GetCreaturePosition();
            CreateCreature(pos);
        }

        public override List<CreaturePackage> RetrieveCreatures()
        {
            List<CreaturePackage> creatures = new List<CreaturePackage>();
            Vector2 pos = GetCreaturePosition();
            creatures.Add(new CreaturePackage(Game, spriteSheet, ReturnCreature(pos), startTime));
            return creatures;
        }

        private Vector2 GetCreaturePosition()
        {
            Vector2 testPos;

            float levelHeight = level.LevelHeight;
            float actualYPos = levelHeight * (yPos / 100);

            if (leftToRightDirection)
                testPos = new Vector2(0, actualYPos);
            else
                testPos = new Vector2(level.LevelWidth, actualYPos);
            return testPos;
        }
    }
}
