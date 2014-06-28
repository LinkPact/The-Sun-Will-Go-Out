using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceProject
{
    public class EliminationLevel : Level
    { 
        #region declaration

        //private int victory;

        private AlliedShip ally;
        private AlliedShip ally2;
        private AlliedShip ally3;

        #endregion
        public EliminationLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, MissionType missionType)
            : base(Game, spriteSheet, player1, missionType)
        {
            this.Name = "EliminationLevel";
            //levelSong = Game.Content.Load<Song>("SpaceProject_Intro");
        }
        public override void Initialize()
        {
            base.Initialize();
            LevelWidth = 800;

            //Allies
            ally = new FighterAlly(this.Game, this.spriteSheet, player, new Rectangle(495, 495, 10, 10));
            ally.CreateAI(AIBehaviour.Standard);
            ally.Initialize();
            ally.PositionX = 500;
            ally.PositionY = 500;
            Game.stateManager.shooterState.gameObjects.Add(ally);

            ally2 = new FighterAlly(this.Game, this.spriteSheet, player, new Rectangle(395, 445, 10, 10));
            ally2.CreateAI(AIBehaviour.Standard);
            ally2.Initialize();
            ally2.PositionX = 400;
            ally2.PositionY = 450;
            Game.stateManager.shooterState.gameObjects.Add(ally2);

            ally3 = new FighterAlly(this.Game, this.spriteSheet, player, new Rectangle(295, 495, 10, 10));
            ally3.CreateAI(AIBehaviour.Standard);
            ally3.Initialize();
            ally3.PositionX = 300;
            ally3.PositionY = 500;
            Game.stateManager.shooterState.gameObjects.Add(ally3);

            //Setup creatures
            SetupCreature longShotDelay = new SetupCreature();
            longShotDelay.SetShootDelayFactor(4f);
            longShotDelay.SetSpeedFactor(0.25f);
            
            //SetupCreature setupCrit = new SetupCreature(Game, spriteSheet, player);
            //setupCrit.SetSpeedFactor(2f);

            //LevelEvent test = new GradientSwarm(Game, player, spriteSheet, "homing", 0, 45000, 45000, 0.5f, 10f);
            //test.SetMovement(Movement.SlantingLine);
            ////test.CreatureSetup(setupCrit);
            //untriggeredEvents.Add(test);

            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "bigmissile", 0, 25000, 1f));

            //LevelEvent missileLine = new LineFormation(Game, player, spriteSheet, this, "bigmissile", 0, 6, 100, 400);
            //missileLine.CreatureSetup(longShotDelay);
            //untriggeredEvents.Add(missileLine);
            //
            ////untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "green", 5000, 25000, 4f));
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, this, "red", 10000, 10000, 0.5f));
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, this,
            //    new VFormation(Game, player, spriteSheet, this, "red", 2, 20, 20), 20000, 10000, 0.5f));
            //
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, this,
            //    new LineFormation(Game, player, spriteSheet, this, "blue", 0, 2, 160, 400), 33000, 15000, 0.3f));
            //
            //untriggeredEvents.Add(new SquareFormation(Game, player, spriteSheet, this, "turret", 37000, 2, 2, 700, 300, 400));
            //
            //untriggeredEvents.Add(new EliminationBoss(Game, player, spriteSheet, this, 60000));

            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "big", 0, 400));

            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "big", 0, 10000, 0.2f));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, "medium", 0, 5, 120, 400));
            
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, "bigyellow", 0, 5, 120, 400));
            

            //First part, enemies easy, spread out
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "bigred", 1000, 10000, 0.5f));
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "green", 3000, 20000, 1.5f));
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "red", 13000, 10000, 1.5f));
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "green", 18000, 10000, 4.0f));
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "red", 23000, 10000, 3.0f));
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "green", 28000, 10000, 8.0f));
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, "green", 38000, 7000, 3.0f));
                                      
            //untriggeredEvents.Add(new GradientSwarm(Game, player, spriteSheet, "smallestmeteor", 50000, 40000, 35000, 2.0f, 20.0f));

            //Single enemies
            // -----> Tog mig friheten att komentera ut dessa! //Johan <-----
            /* untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "green", 0, 200));
            untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, "blue", 2000, 10, 50, 400));
            untriggeredEvents.Add(new SquareFormation(Game, player, spriteSheet, "red", 1000, 5, 4, 30, 30, 400));
            untriggeredEvents.Add(new SquareFormation(Game, player, spriteSheet, "yellow", 1000, 6, 3, 30, 30, 200));
            untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "blue", 200, 220));
            untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "green", 200));*/

            //LevelEvent square = new SquareFormation(Game, player, spriteSheet, "blue", 0, 5, 5, 20f, 20f);
            ////square.SetMovement(Movement.Zigzag);
            //untriggeredEvents.Add(square);
            
            //V-formation
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, "red", 300, 2, 30, 30, 600));
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, "yellow", 1300, 6, 30, 50, 400));
            //untriggeredEvents.Add(new SquareFormation(Game, player, spriteSheet, "red", 2000, 10, 8, 40, 40, 300));

            //Swarms
            //untriggeredEvents.Add(new Swarm(Game, player, spriteSheet, "red", 5000, 4f, 4000));
            //untriggeredEvents.Add(new Swarm(Game, player, spriteSheet, "yellow", 10000, 4f, 4000));
            //untriggeredEvents.Add(new Swarm(Game, player, spriteSheet, "blue", 15000, 1f, 4000));
            //untriggeredEvents.Add(new Swarm(Game, player, spriteSheet, "mixedColors", 20000, 10f, 8000));

            //victory = 180;

            //SetVictoryToTime(victory);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsObjectiveCompleted)
            {
                SpawnControlUpdate(gameTime);
            }

            else
                EndText = "Press 'Enter' to return..";
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
