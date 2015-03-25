using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class JakobsDevelopLevel : Level
    {
        public JakobsDevelopLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, MissionType missionType, String identifier)
            : base(Game, spriteSheet, player, missionType, identifier)
        {
            //Identifier = "JakobDevelop";
            LevelWidth = 500;
        }

        public override void Initialize()
        {
            base.Initialize();

            EliminationBoss boss = new EliminationBoss(Game, player, spriteSheet, this, 0);
            untriggeredEvents.Add(boss);

            LevelEvent swarm = new EvenSwarm(Game, player, spriteSheet, this, EnemyType.meteor, 0, 35000, 30);
            SetupCreature testSetup = new SetupCreature();
            testSetup.SetMovement(Movement.FullStop);
            swarm.CreatureSetup(testSetup);
            untriggeredEvents.Add(swarm);

            LevelEvent trueMessage = new SimpleMessageVertical(Game, player, spriteSheet, this, 2000, "HELLUUU!!");
            untriggeredEvents.Add(trueMessage);

            LevelEvent trueMessage2 = new SimpleMessageVertical(Game, player, spriteSheet, this, 3000, "HELLUUU2!!");
            untriggeredEvents.Add(trueMessage2);

            LevelEvent trueMessage3 = new SimpleMessageVertical(Game, player, spriteSheet, this, 5000, "HELLUUU33!!");
            untriggeredEvents.Add(trueMessage3);

            LevelEvent message = new SingleEnemy(Game, player, spriteSheet, this, EnemyType.R_mosquito, 3000, 100);
            untriggeredEvents.Add(message);

            SetCustomVictoryCondition(LevelObjective.Boss, -1);
        }
    }
}
