using System;
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
        private Bar freighterHpBar;
        private float HP;

        public SecondMissionLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, String identifier, 
            String filePath, MissionType missionType)
            : base(Game, spriteSheet, player1, identifier, filePath, missionType)
        {
        }

        public SecondMissionLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, LevelEntry levelEntry)
            : base(Game, spriteSheet, player1, levelEntry)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            SetHP();

            freighter = new FreighterAlly(Game, spriteSheet, player,
                new Rectangle(Game1.ScreenSize.X / 2 - 100, (Game1.ScreenSize.Y - 600) / 2 + 425, 200, 50),
            "none", 200, HP, 300, 0, 4);

            freighter.Initialize();
            freighter.PositionX = Game1.ScreenSize.X / 2;
            freighter.PositionY = (Game1.ScreenSize.Y - 600) / 2 + 450;
            freighter.CreateAI(AIBehaviour.NoWeapon);
            Game.stateManager.shooterState.gameObjects.Add(freighter);

            freighterHpBar = new Bar(Game, spriteSheet, Color.Red, true, new Rectangle(4, 53, 139, 7), new Rectangle(2, 374, 137, 5));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            freighterHpBar.Update(gameTime, freighter.HP, freighter.HPmax, new Vector2(10, 30));

            if (ControlManager.CheckKeyPress(Microsoft.Xna.Framework.Input.Keys.Insert))
            {
                freighter.HP = 0;
            }

            if (freighter.HP <= 0)
            {
                player.HP = 0;
            }
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

        private void SetHP()
        {
            if (StatsManager.gameMode == GameMode.Easy)
            {
                HP = 3000;
            }
            else
            {
                HP = 2000;
            }
        }
    }
}
