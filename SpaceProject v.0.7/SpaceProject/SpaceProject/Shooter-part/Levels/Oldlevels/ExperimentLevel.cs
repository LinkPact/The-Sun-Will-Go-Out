using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class ExperimentLevel : Level
    {
        #region declaration

        private int victory;

        #endregion
        public ExperimentLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, MissionType missionType)
            : base(Game, spriteSheet, player1, missionType)
        {
            this.Name = "ExperimentLevel";
        }
        public override void Initialize()
        {
            base.Initialize();

            LevelWidth = 800;

            //Single enemies
            // -----> Tog mig friheten att komentera ut dessa! //Johan <-----
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "green", 0));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "green", 3000, 200));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "red", 6000, 200));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "blue", 9000, 200));
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "red", 13000, 2, 30, 30, 600));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "big", 0));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "green", 0, 200));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, "blue", 2000, 10, 50, 400));
            //untriggeredEvents.Add(new SquareFormation(Game, player, spriteSheet, "red", 1000, 5, 4, 30, 30, 400));
            //untriggeredEvents.Add(new SquareFormation(Game, player, spriteSheet, "yellow", 1000, 6, 3, 30, 30, 200));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "blue", 200, 220));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "green", 200));
            //
            ////V-formation
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, "red", 300, 2, 30, 30, 600));
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, "yellow", 1300, 6, 30, 50, 400));
            //untriggeredEvents.Add(new SquareFormation(Game, player, spriteSheet, "red", 2000, 10, 8, 40, 40, 300));

            //Swarms
            //untriggeredEvents.Add(new Swarm(Game, player, spriteSheet, "red", 5000, 4f, 4000));
            //untriggeredEvents.Add(new Swarm(Game, player, spriteSheet, "yellow", 10000, 4f, 4000));
            //untriggeredEvents.Add(new Swarm(Game, player, spriteSheet, "blue", 15000, 1f, 4000));
            //untriggeredEvents.Add(new Swarm(Game, player, spriteSheet, "mixedColors", 20000, 10f, 8000));
            
            victory = 35;
            
            SetCustomVictoryCondition(LevelObjective.Time, victory);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsObjectiveCompleted)
            {
                SpawnControlUpdate(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
