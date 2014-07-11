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
        }

        public override void Initialize()
        {
            base.Initialize();
            LevelWidth = 500;

            LevelEvent swarm = new EvenSwarm(Game, player, spriteSheet, this, EnemyType.R_mosquito, 0, 3500, 10);
            untriggeredEvents.Add(swarm);

            LevelEvent point = new SingleEnemy(Game, player, spriteSheet, this, EnemyType.R_thickShooter, 200);
            untriggeredEvents.Add(point);

            SetCustomVictoryCondition(LevelObjective.Time, 3);

        }
    }
}
