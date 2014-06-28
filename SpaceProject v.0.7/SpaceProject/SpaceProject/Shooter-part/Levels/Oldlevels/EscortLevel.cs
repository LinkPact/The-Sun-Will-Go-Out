using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class EscortLevel: Level
    {
        private FreighterAlly freighter;
        private Bar freighterHpBar;

        private FighterAlly fighter;

        public EscortLevel(Game1 Game, Sprite SpriteSheet, PlayerVerticalShooter Player, MissionType missionType) :
            base(Game, SpriteSheet, Player, missionType)
        {
            this.Name = "EscortLevel";
        }

        public override void Initialize()
        {
            base.Initialize();
            SetCustomVictoryCondition(LevelObjective.Finish, 0);
            LevelWidth = 800;

            freighter = new FreighterAlly(this.Game, this.spriteSheet, player, new Rectangle(390, 490, 20, 20));
            freighter.CreateAI(AIBehaviour.NoWeapon);
            freighter.Initialize();
            freighter.PositionX = 400;
            freighter.PositionY = 500;
            Game.stateManager.shooterState.gameObjects.Add(freighter);

            freighterHpBar = new Bar(Game, spriteSheet, Color.Red, true, new Rectangle(4, 53, 139, 7), new Rectangle(2, 374, 137, 5));

            fighter = new FighterAlly(this.Game, this.spriteSheet, player, new Rectangle(145, 445, 10, 10));
            fighter.CreateAI(AIBehaviour.Standard);
            fighter.Initialize();
            fighter.PositionX = 150;
            fighter.PositionY = 450;
            Game.stateManager.shooterState.gameObjects.Add(fighter);

            player.PositionX = 650;
            player.PositionY = 450;
            player.HPmax = 100000;
            player.HP = player.HPmax;

            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 5000, 4, 50, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "yellow", 8000, 2, 50, 50, Game.ScreenCenter.X / 2));
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "yellow", 8000, 2, 50, 50, Game.Window.ClientBounds.Width - Game.ScreenCenter.X / 2));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "green", 10500, 2, 50, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new EvenSwarm(Game, player, spriteSheet, this, "green", 14000, 10000, 4.0f));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "bigyellow", 22000, 2, 50, Game.ScreenCenter.X / 2));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "bigyellow", 22000, 2, 50, Game.Window.ClientBounds.Width - Game.ScreenCenter.X / 2));
            //untriggeredEvents.Add(new SquareFormation(Game, player, spriteSheet, this, "biggreen", 24000, 3, 2, 50, 50, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "medium", 27500, 6, 50, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new GradientSwarm(Game, player, spriteSheet, this, "mixedcolors", 29000, 20000, 10000, 1.5f, 2.5f));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            freighterHpBar.Update(gameTime, freighter.HP, freighter.HPmax, new Vector2(10, 30));

            if (!IsObjectiveCompleted)
            {
                SpawnControlUpdate(gameTime);
            }

            else
                EndText = "Press 'Enter' to return..";

            if (freighter.HP <= 0)
                player.HP = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Game.fontManager.GetFont(12),
                                   "Freighter HP:",
                                   new Vector2(freighterHpBar.Position.X,
                                               10) + Game.fontManager.FontOffset,
                                   Color.White,
                                   0.0f,
                                   Vector2.Zero,
                                   1f,
                                   SpriteEffects.None,
                                   0.95f);

            freighterHpBar.Draw(spriteBatch);
        }
    }
}
