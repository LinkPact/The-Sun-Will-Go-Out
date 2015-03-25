using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    class MissionScreenText
    {

        private Game1 Game;

        //private SpriteFont fontRegular;

        //Denotes current active position of cursor.
        int layer;
        int var1;
        int var2;

        private Color txtColor;

        private float edgePadding;

        public MissionScreenText(Game1 Game)
        {
            this.Game = Game;
        }

        public void Initialize()
        {
            edgePadding = Game.Window.ClientBounds.Width / 16;

            txtColor = Game.fontManager.FontColor;
        }

        public void Update(GameTime gameTime, int layer, int var1, int var2)
        {
            this.layer = layer;
            this.var1 = var1;
            this.var2 = var2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (var1 == 0) { DisplayActiveMissions(spriteBatch); }
            else if (var1 == 1) { DisplayCompletedMissions(spriteBatch); }
            else if (var1 == 2) { DisplayFailedMissions(spriteBatch); }
            else if (var1 == 3)
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(16), "Go back to overworld", new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93) + Game.fontManager.FontOffset, Game.fontManager.FontColor);
            }
        }

        public void DisplayActiveMissions(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game.fontManager.GetFont(16), "Active missions", new Vector2(MissionScreenState.GetRightRectangle.X + MissionScreenState.GetRightRectangle.Width / 2, 30) + Game.fontManager.FontOffset, txtColor, 0, Game.fontManager.GetFont(16).MeasureString("Active missions") / 2, 1.0f, SpriteEffects.None, 0.5f);

            if (MissionManager.ReturnActiveMissions().Count > 0)
            {
                int missionCount = 0;
                for (int n = 0; n < MissionManager.ReturnActiveMissions().Count; n++)
                {
                    missionCount++;
                    if (n < MissionManager.ReturnActiveMissions().Count)
                        spriteBatch.DrawString(Game.fontManager.GetFont(16), MissionManager.ReturnActiveMissions()[n].MissionName, new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93 + n * 23) + Game.fontManager.FontOffset, txtColor);
                }

                if (layer == 2)
                {
                    spriteBatch.DrawString(Game.fontManager.GetFont(16), "Back", new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93 + missionCount * 23) + Game.fontManager.FontOffset, txtColor);
                }
            }

            else
                spriteBatch.DrawString(Game.fontManager.GetFont(16), "No active missions", new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93) + Game.fontManager.FontOffset, txtColor);

        }

        public void DisplayCompletedMissions(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game.fontManager.GetFont(16), "Completed missions", new Vector2(MissionScreenState.GetRightRectangle.X + MissionScreenState.GetRightRectangle.Width / 2, 30) + Game.fontManager.FontOffset, txtColor, 0, Game.fontManager.GetFont(16).MeasureString("Completed missions") / 2, 1.0f, SpriteEffects.None, 0.5f);

            if (MissionManager.ReturnCompletedDeadMissions().Count > 0)
            {
                int missionCount = 0;
                for (int n = 0; n < MissionManager.ReturnCompletedDeadMissions().Count; n++)
                {
                    missionCount++;
                    if (n < MissionManager.ReturnCompletedDeadMissions().Count)
                        spriteBatch.DrawString(Game.fontManager.GetFont(16), MissionManager.ReturnCompletedDeadMissions()[n].MissionName, new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93 + n * 23) + Game.fontManager.FontOffset, txtColor);
                }

                if (layer == 2)
                {
                    spriteBatch.DrawString(Game.fontManager.GetFont(16), "Back", new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93 + missionCount * 23) + Game.fontManager.FontOffset, txtColor);
                }
            }

            else
                spriteBatch.DrawString(Game.fontManager.GetFont(16), "No completed missions", new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93) + Game.fontManager.FontOffset, txtColor);
        }

        public void DisplayFailedMissions(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game.fontManager.GetFont(16), "Failed missions", new Vector2(MissionScreenState.GetRightRectangle.X + MissionScreenState.GetRightRectangle.Width / 2, 30) + Game.fontManager.FontOffset,
                txtColor, 0, Game.fontManager.GetFont(16).MeasureString("Failed missions") / 2, 1.0f, SpriteEffects.None, 0.5f);

            if (MissionManager.ReturnFailedDeadMissions().Count > 0)
            {
                int missionCount = 0;

                for (int n = 0; n < MissionManager.ReturnFailedDeadMissions().Count; n++)
                {
                    missionCount++;
                    if (n < MissionManager.ReturnFailedDeadMissions().Count)
                        spriteBatch.DrawString(Game.fontManager.GetFont(16), MissionManager.ReturnFailedDeadMissions()[n].MissionName, new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93 + n * 23) + Game.fontManager.FontOffset, txtColor);
                }

                if (layer == 2)
                {
                    spriteBatch.DrawString(Game.fontManager.GetFont(16), "Back", new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93 + missionCount * 23) + Game.fontManager.FontOffset, txtColor);
                }
            }

            else
                spriteBatch.DrawString(Game.fontManager.GetFont(16), "No failed missions", new Vector2(MissionScreenState.GetRightRectangle.X + edgePadding, 93) + Game.fontManager.FontOffset, txtColor);
        }
    }
}
