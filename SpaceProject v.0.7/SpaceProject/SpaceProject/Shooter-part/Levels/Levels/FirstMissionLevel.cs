using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class FirstMissionLevel : MapCreatorLevel
    {
        private int suppliesCount;

        public int SuppliesCount { get { return suppliesCount; } set { suppliesCount = value; } }

        private int suppliesDelay = 7500;
        private List<Supplies> supplies;

        public FirstMissionLevel(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, String identifier,
            String filePath, MissionType missionType)
            : base(Game, spriteSheet, player1, identifier, filePath, missionType)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            SetCustomVictoryCondition(LevelObjective.Finish, 0);

            supplies = new List<Supplies>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Supplies s in supplies)
            {
                // Player picks up supplies
                if (CollisionDetection.IsRectInRect(player.Bounding, s.Bounding) && s.HP > 0)
                {
                    s.HP = -1;
                    suppliesCount++;
                }
            }

            suppliesDelay -= gameTime.ElapsedGameTime.Milliseconds;
            if (suppliesDelay <= 0)
            {
                AddSupplies();
                suppliesDelay = 7500;
            }
        }

        private void AddSupplies()
        {
            Supplies s = new Supplies(Game, spriteSheet, player);
            s.Initialize();
            s.HP = 1;
            Game.stateManager.shooterState.gameObjects.Add(s);

            supplies.Add(s);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            String text = "Supplies collected: " + suppliesCount;
            spriteBatch.DrawString(Game.fontManager.GetFont(14), text, new Vector2(10, 30), Color.White, 0.0f, Vector2.Zero, 1f,
                SpriteEffects.None, 0.95f);
        }
    }
}
