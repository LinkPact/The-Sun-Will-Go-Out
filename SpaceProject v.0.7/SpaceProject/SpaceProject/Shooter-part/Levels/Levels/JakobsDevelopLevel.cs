using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class JakobsDevelopLevel : Level
    {
        public JakobsDevelopLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, MissionType missionType)
            : base(Game, spriteSheet, player, missionType)
        {
            Name = "JakobDevelop";
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

            SetCustomVictoryCondition(LevelObjective.Boss, -1);
        }
    }
}
