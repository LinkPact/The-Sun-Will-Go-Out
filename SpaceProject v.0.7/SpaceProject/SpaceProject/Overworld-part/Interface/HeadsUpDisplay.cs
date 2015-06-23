using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class HeadsUpDisplay
    {
        private Game1 game;

        public Radar radar;
        private List<Bar> fusionCellBars;
        private Bar emergencyFusionCellBar;
        private Bar lifeBar;

        public HeadsUpDisplay(Game1 game)
        {
            this.game = game;
        }

        public void Initialize(Sprite spriteSheet, Vector2 mapCenter, Vector2 mapScale)
        {
            radar = new Radar(game, spriteSheet);
            radar.Initialize(mapCenter, 10000);

            fusionCellBars = new List<Bar>();
            for (int i = 0; i < StatsManager.MaxFusionCells; i++)
                fusionCellBars.Add(new Bar(game, spriteSheet, Color.Green, true, new Rectangle(0, 25, 41, 7), new Rectangle(1, 34, 39, 5)));

            emergencyFusionCellBar = new Bar(game, spriteSheet, Color.Red, true, new Rectangle(0, 25, 41, 7), new Rectangle(1, 34, 39, 5));

            lifeBar = new Bar(game, spriteSheet, Color.Green, true);
            lifeBar.Initialize();
        }

        public void Update(GameTime gameTime, List<GameObjectOverworld> visibleGameObjects)
        {
            UpdateZoomMap(gameTime);

            radar.Update(gameTime, visibleGameObjects, game.camera.Position);

            lifeBar.Update(gameTime, StatsManager.GetShipLife(), StatsManager.Armor(),
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 10,
                            game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 2 - 15));

            int[] fusionCells = StatsManager.FusionCells;
            for (int i = 0; i < StatsManager.MaxFusionCells; i++)
            {
                fusionCellBars[i].Update(gameTime, fusionCells[i], 1,
                    new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                                game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 2 - 60 -
                                (i * 10)));
            }

            emergencyFusionCellBar.Update(gameTime, StatsManager.EmergencyFusionCell, 1,
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                            game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 2 - 45));
        }

        private void UpdateZoomMap(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            radar.Draw(spriteBatch);
            //DrawLife(spriteBatch);
            DrawPosition(spriteBatch);
            // DON'T DELETE
            //DrawFusionCells(spriteBatch);
            //DrawRepAndProgress(spriteBatch);
            DrawMenuInfo(spriteBatch);
        }

        private void DrawLife(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.fontManager.GetFont(14),
                       "Life: " + ((int)StatsManager.GetShipLife()).ToString(),
                       new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 10,
                                   game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 2 - 31) + game.fontManager.FontOffset,
                       game.fontManager.FontColor,
                       .0f,
                       Vector2.Zero,
                       1f,
                       SpriteEffects.None,
                       0.92f);

            lifeBar.Draw(spriteBatch);
        }

        private void DrawPosition(SpriteBatch spriteBatch)
        {
            Vector2 origo = new Vector2(OverworldState.OVERWORLD_WIDTH / 2, OverworldState.OVERWORLD_HEIGHT / 2);

            spriteBatch.DrawString(game.fontManager.GetFont(14),
               "(X: " + ((int)(game.player.position.X / 10) - origo.X / 10) + ",Y: " +
                       (-((int)(game.player.position.Y / 10) - origo.Y / 10)) + ")",
                new Vector2(game.camera.cameraPos.X + game.Window.ClientBounds.Width / 2 - 101,
                           game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 2 - 221) + game.fontManager.FontOffset,
                Color.Red, 0f,
                new Vector2(game.fontManager.GetFont(14).MeasureString("(X:" +
                    (int)(game.player.position.X / 10) + ",Y:" +
                    (int)(game.player.position.Y / 10) + ")").X / 2, 0),
                    1f, SpriteEffects.None, 0.92f);
        }

        private void DrawFusionCells(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.fontManager.GetFont(14), "Fusion Cells: ",
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 2 - 97) +
                game.fontManager.FontOffset,
                game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

            for (int i = 0; i < StatsManager.MaxFusionCells; i++)
                fusionCellBars[i].Draw(spriteBatch);
            emergencyFusionCellBar.Draw(spriteBatch);
        }

        private void DrawRepAndProgress(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.fontManager.GetFont(14), "Reputation: " + StatsManager.reputation,
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                    game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2 + 8) +
                    game.fontManager.FontOffset,
                game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

            spriteBatch.DrawString(game.fontManager.GetFont(14), "Progress: " + StatsManager.progress,
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                    game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2 + 24) +
                    game.fontManager.FontOffset,
                game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);
        }

        private void DrawMenuInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.fontManager.GetFont(12), "Game mode: " + StatsManager.gameMode.ToString(),
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                    game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2 + 15) +
                    game.fontManager.FontOffset,
                game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

            spriteBatch.DrawString(game.fontManager.GetFont(12), "Press 'ESC' to open menu",
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                    game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2 + 40) +
                    game.fontManager.FontOffset,
                game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

            spriteBatch.DrawString(game.fontManager.GetFont(12), "Press 'I' to open inventory",
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                    game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2 + 60) +
                    game.fontManager.FontOffset,
                game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

            spriteBatch.DrawString(game.fontManager.GetFont(12), "Press 'M' to open mission screen",
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                    game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2 + 80) +
                    game.fontManager.FontOffset,
                game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

            spriteBatch.DrawString(game.fontManager.GetFont(12), "Press 'N' to show map",
                new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2 + 8,
                    game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2 + 100) +
                    game.fontManager.FontOffset,
                game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);
        }
    }
}
