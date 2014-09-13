using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceProject.MapCreator;

namespace SpaceProject
{

    //Class used to create a single instance of an enemy
    class SingleEnemy : PointLevelEvent
    {
        public SingleEnemy(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startTime, float startPos)
            : base(Game, player, spriteSheet, level, identifier, startTime)
        {
            xPos = startPos;
        }

        public SingleEnemy(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level,
            EnemyType identifier, float startPos)
            : base(Game, player, spriteSheet, level, identifier)
        {
            xPos = startPos;
        }

        //The function called when the class is activated.
        //For events inheriting from the "PointLevelEvent"-class
        //this is all that is executed before the event is removed.
        //Check "Swarm"-class to see how lasting events can look.
        public override void Run(GameTime gameTime)
        {
            base.Run(gameTime);

            if (xPos == -1f) xPos = (float)(random.NextDouble() * 800);

            CreateCreature(xPos);
        }

        public override List<CreaturePackage> RetrieveCreatures()
        {
            List<CreaturePackage> creatures = new List<CreaturePackage>();

            if (xPos == -1f) xPos = (float)(random.NextDouble() * 800);

            creatures.Add(new CreaturePackage(Game, spriteSheet, ReturnCreature(new Vector2(xPos, 0)), startTime));

            return creatures;
        }
    }
}
