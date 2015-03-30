using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class SquareFormation : PointLevelEvent
    {
        private int xNumber;
        private int yNumber;
        private float xSpacing;
        private float ySpacing;

        public SquareFormation(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, int xNumber, int yNumber, float xSpacing, float ySpacing)
            : base(Game, player, spriteSheet, level, identifier, startTime)
        {
            xPos = -1;

            this.xNumber = xNumber;
            this.yNumber = yNumber;
            this.xSpacing = xSpacing;
            this.ySpacing = ySpacing;
        }
        
        public SquareFormation(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, int xNumber, int yNumber, float xSpacing, float ySpacing, float startPos)
            : base(Game, player, spriteSheet, level, identifier, startTime)
        {
            xPos = startPos;

            this.xNumber = xNumber;
            this.yNumber = yNumber;
            this.xSpacing = xSpacing;
            this.ySpacing = ySpacing;
        }
 
        //Only used for manually triggered events. Otherwise, it won't trigger.
        public SquareFormation(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, int xNumber, int yNumber, float xSpacing, float ySpacing)
            : base(Game, player, spriteSheet, level, identifier)
        {
            xPos = -1;

            this.xNumber = xNumber;
            this.yNumber = yNumber;
            this.xSpacing = xSpacing;
            this.ySpacing = ySpacing;
        }
        
        public override void Run(GameTime gameTime)
        {
            base.Run(gameTime);

            if (xPos == -1f)
                SetRandomXPosition();

            float xLeftEdge = xPos - (float)xSpacing * ((float)xNumber - 1) / 2;

            for (int x = 0; x < xNumber; x++)
            {
                for (int y = 0; y < yNumber; y++)
                {
                    CreateCreature(new Vector2(xLeftEdge + x * xSpacing, -y * ySpacing));
                }
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
                for (int y = 0; y < yNumber; y++)
                {
                    creatures.Add(new CreaturePackage(Game, spriteSheet, ReturnCreature(new Vector2(xLeftEdge + x * xSpacing, -y * ySpacing)), startTime));
                }
            }

            return creatures;
        }
    }
}
