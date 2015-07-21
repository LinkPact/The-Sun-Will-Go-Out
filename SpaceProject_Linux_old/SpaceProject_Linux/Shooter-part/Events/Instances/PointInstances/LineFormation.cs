using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{

    //Class used to create a single instance of an enemy
    class LineFormation : PointLevelEvent
    {
        private int xNumber;
        private float xSpacing;

        public LineFormation(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, int xNumber, float xSpacing)
            : base(Game, player, spriteSheet, level, identifier, startTime)
        {
            xPos = -1;

            this.xNumber = xNumber;
            this.xSpacing = xSpacing;
        }

        public LineFormation(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, int xNumber, float xSpacing, float startPos)
            : base(Game, player, spriteSheet, level, identifier, startTime, startPos)
        {
            xPos = startPos;

            this.xNumber = xNumber;
            this.xSpacing = xSpacing;
        }

        public override void Run(GameTime gameTime)
        {
            base.Run(gameTime);

            if (xPos == -1f)
                SetRandomXPosition();

            float xLeftEdge = xPos - (float)xSpacing * ((float)xNumber - 1) / 2;

            for (int x = 0; x < xNumber; x++)
            {
                CreateCreature((float)(xLeftEdge + x * xSpacing));
            }
        }

        public override List<CreaturePackage> RetrieveCreatures()
        {
            List<CreaturePackage> creatures = new List<CreaturePackage>();

            if (xPos == -1f)
                SetRandomXPosition();

            float xLeftEdge = xPos - (float)xSpacing * ((float)xNumber - 1) / 2;

            for (int x = 0; x < xNumber; x++)
            {
                creatures.Add(new CreaturePackage(Game, spriteSheet, ReturnCreature((float)(xLeftEdge + x * xSpacing)), startTime));
            }
            return creatures;
        }
    }
}
