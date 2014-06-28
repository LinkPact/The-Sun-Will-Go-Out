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
    public class DanneLevel : Level
    {
        #region declaration

        //private int victory;
        private AlliedShip ally;
        private AlliedShip ally2;
        private AlliedShip ally3;
        private FreighterAlly ally4;

        #endregion
        public DanneLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, MissionType missionType)
            : base(Game, spriteSheet, player1, missionType)
        {
            this.Name = "DanneLevel";
            //levelSong = Game.Content.Load<Song>("SpaceProject_Intro");
        }
        public override void Initialize()
        {
            base.Initialize();

            LevelWidth = 640;

            ally = new FighterAlly(this.Game, this.spriteSheet, player, new Rectangle(395, 395, 10, 10));
            ally.CreateAI(AIBehaviour.Standard);
            ally.Initialize();
            ally.SetLevelWidth(LevelWidth);
            ally.SetStartPos(new Vector2(100, 400));
            Game.stateManager.shooterState.gameObjects.Add(ally);

            ally2 = new FighterAlly(this.Game, this.spriteSheet, player, new Rectangle(145, 445, 10, 10));
            ally2.CreateAI(AIBehaviour.Standard);
            ally2.Initialize();
            ally2.SetLevelWidth(LevelWidth);
            ally2.SetStartPos(new Vector2(150, 450));
            Game.stateManager.shooterState.gameObjects.Add(ally2);

            ally3 = new FighterAlly(this.Game, this.spriteSheet, player, new Rectangle(645, 445, 10, 10));
            ally3.CreateAI(AIBehaviour.Standard);
            ally3.Initialize();
            ally3.SetLevelWidth(LevelWidth);
            ally3.SetStartPos(new Vector2(350, 450));
            Game.stateManager.shooterState.gameObjects.Add(ally3);

            ally4 = new FreighterAlly(this.Game, this.spriteSheet, player, new Rectangle(390, 490, 20, 20));
            ally4.CreateAI(AIBehaviour.NoWeapon);
            ally4.Initialize();
            ally4.SetLevelWidth(LevelWidth);
            ally4.SetStartPos(new Vector2(200, 500));
            Game.stateManager.shooterState.gameObjects.Add(ally4);

            player.PositionX = 300;
            player.PositionY = 450;

            //TEST
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "turret", 0));

            //LEVEL            
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "red", 2500, 2, 25, 25, Game.Window.ClientBounds.Width / 8));
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "red", 3000, 2, 25, 25, Game.Window.ClientBounds.Width / 2));
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "red", 2500, 2, 25, 25, Game.Window.ClientBounds.Width * 7 / 8));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "green", 8000, 8, 100, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "blue", 12000, Game.Window.ClientBounds.Width / 8));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "blue", 12000, Game.Window.ClientBounds.Width * 7 / 8));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "green", 14000, 4, 100, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "medium", 16000, Game.Window.ClientBounds.Width / 8));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "medium", 16000, Game.Window.ClientBounds.Width * 7 / 8));
            //
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, this, "green", 18000, 10000, 5f));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "yellow", 27000, 6, 100, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "blue", 31000, 3, 100, Game.ScreenCenter.X));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "medium", 32000, Game.Window.ClientBounds.Width / 8));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "medium", 32000, Game.Window.ClientBounds.Width * 7 / 8));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 35000, 7, 100, Game.ScreenCenter.X + 25));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 35500, 8, 100, Game.ScreenCenter.X));

            EliminationBossDanne boss = new EliminationBossDanne(Game, player, spriteSheet, this, 41000);
            untriggeredEvents.Add(boss);

            //untriggeredEvents.Add(new EliminationBossDanne(Game, player, spriteSheet, 41000));

            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "blue", 41000, Game.Window.ClientBounds.Width / 8));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "blue", 41000, Game.Window.ClientBounds.Width * 2 / 8));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "blue", 41000, Game.Window.ClientBounds.Width * 6 / 8));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "blue", 41000, Game.Window.ClientBounds.Width * 7 / 8));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "medium", 41000, 350));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "medium", 41000, 450));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "big", 41000, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, "green", 42000, 6, 100, Game.ScreenCenter.X));
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