﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class SecondMissionLevel : MapCreatorLevel
    {
        FreighterAlly freighter;
        FighterAlly fighter;

        private Bar freighterHpBar;

        public SecondMissionLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, String identifier, 
            String filePath, MissionType missionType)
            : base(Game, spriteSheet, player1, identifier, filePath, missionType)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            freighter = new FreighterAlly(Game, spriteSheet, player,
                new Rectangle(Game.Window.ClientBounds.Width / 2 - 100, (Game.Window.ClientBounds.Height - 600) / 2 + 425, 200, 50),
            "none", 200, 2000, 300, 0, 4);

            freighter.Initialize();
            freighter.PositionX = Game.Window.ClientBounds.Width / 2;
            freighter.PositionY = (Game.Window.ClientBounds.Height - 600) / 2 + 450;
            freighter.CreateAI(AIBehaviour.NoWeapon);
            Game.stateManager.shooterState.gameObjects.Add(freighter);

            fighter = new FighterAlly(Game, spriteSheet, player,
                new Rectangle(Game.Window.ClientBounds.Width / 2, (Game.Window.ClientBounds.Height - 600) / 2 + 375, 100, 50),
                "basiclaser", 50, 5000, 500, 500, 5);

            fighter.Initialize();
            fighter.PositionX = Game.Window.ClientBounds.Width / 2 + 50;
            fighter.PositionY = (Game.Window.ClientBounds.Height - 600) / 2 + 400;
            fighter.CreateAI(AIBehaviour.Standard);
            Game.stateManager.shooterState.gameObjects.Add(fighter);

            freighterHpBar = new Bar(Game, spriteSheet, Color.Red, true, new Rectangle(4, 53, 139, 7), new Rectangle(2, 374, 137, 5));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            freighterHpBar.Update(gameTime, freighter.HP, freighter.HPmax, new Vector2(10, 30));

            if (player.HP <= 0 || IsGameOver)
            {
                freighter.HP = 0;
            }

            if (ControlManager.CheckKeypress(Microsoft.Xna.Framework.Input.Keys.PageDown))
            {
                freighter.HP = 0;
            }
        }

        public void SetFreighterHP(float hp)
        {
            freighter.HP = hp;
        }

        public float GetFreighterHP()
        {
            return freighter.HP;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Game.fontManager.GetFont(14),
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