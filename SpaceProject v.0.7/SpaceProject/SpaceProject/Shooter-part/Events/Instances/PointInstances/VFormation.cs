using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceProject.MapCreator;

namespace SpaceProject
{
    //Class used to create a single instance of an enemy
    class VFormation : PointLevelEvent
    {
        //Variables related to this specific event.
        private float xSpacing;
        private float ySpacing;
        private float depth;

        //Different constructors. In this case this lets the player choose if he wants a decided
        //position or not.
        public VFormation(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, int depth, float xSpacing, float ySpacing)
            : base(Game, player, spriteSheet, level, identifier)
        {
            xPos = -1;

            this.depth = depth;
            this.xSpacing = xSpacing;
            this.ySpacing = ySpacing;
        }
        public VFormation(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, int depth, float xSpacing, float ySpacing, float startPos)
            : base(Game, player, spriteSheet, level, identifier, startTime)
        {
            xPos = startPos;
        
            this.depth = depth;
            this.xSpacing = xSpacing;
            this.ySpacing = ySpacing;
        }

        public override void Run(GameTime gameTime)
        {
            base.Run(gameTime);

            // I am sceptical to that this code is ever needed,
            // but not so sure that I dare to remove it right away.
            // Jakob, 140608

            if (xPos == -1f)
                SetRandomXPosition();

            float markedXPos = xPos;

            CreateCreature(new Vector2(xPos, 0));

            for (int x = 0; x < depth; x++)
            {
                CreateCreature(new Vector2(markedXPos + (x * xSpacing), 0 - (x * ySpacing)));
                CreateCreature(new Vector2(markedXPos - (x * xSpacing), 0 - (x * ySpacing)));
            }
        }

        public override List<CreaturePackage> RetrieveCreatures()
        {
            List<CreaturePackage> creatures = new List<CreaturePackage>();

            if (xPos == -1f)
                SetRandomXPosition();

            creatures.Add(new CreaturePackage(Game, spriteSheet, ReturnCreature(new Vector2(xPos, 0)), startTime));

            for (int x = 0; x < depth; x++)
            {
                creatures.Add(new CreaturePackage(Game, spriteSheet, ReturnCreature(new Vector2(xPos + (x * xSpacing), 0 - (x * ySpacing))), startTime));
                creatures.Add(new CreaturePackage(Game, spriteSheet, ReturnCreature(new Vector2(xPos - (x * xSpacing), 0 - (x * ySpacing))), startTime));
            }

            return creatures;
        }
    }
}
