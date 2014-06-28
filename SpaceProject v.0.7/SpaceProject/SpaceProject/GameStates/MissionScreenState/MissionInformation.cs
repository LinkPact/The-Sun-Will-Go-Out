using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class MissionInformation
    {
        private Game1 Game;

        //Denotes current active position of cursor.
        int layer;
        int var1;
        int var2;
        String state;

        public MissionInformation(Game1 Game)
        {
            this.Game = Game;
        }

        public void Initialize()
        {
        }

        public void Update(GameTime gameTime, int layer, int var1, int var2, String state)
        {
            this.layer = layer;
            this.var1 = var1;
            this.var2 = var2;

            this.state = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (state == "MissionScreenState")
            {
                if (var1 == 0 && layer >= 2)
                {
                    DisplayActiveMissionInfo(spriteBatch);
                }

                else if (var1 == 1 && layer >= 2)
                {
                    DisplayCompletedMissionInfo(spriteBatch);
                }

                else if (var1 == 2 && layer >= 2)
                {
                    DisplayFailedMissionInfo(spriteBatch);
                }
            }
        }

        public void DisplayActiveMissionInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (MissionManager.ReturnActiveMissions().Count > 0)
                {
                    if (var2 <= MissionManager.ReturnActiveMissions().Count - 1)
                    {
                        MissionManager.ReturnActiveMissions()[var2].DisplayMissionInfo(spriteBatch, Game.fontManager.GetFont(16));
                    }
                }
            }
        }

        public void DisplayCompletedMissionInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (MissionManager.ReturnCompletedDeadMissions().Count > 0)
                {
                    if (var2 <= MissionManager.ReturnCompletedDeadMissions().Count - 1)
                    {
                        MissionManager.ReturnCompletedDeadMissions()[var2].DisplayMissionInfo(spriteBatch, Game.fontManager.GetFont(16));
                    }
                }
            }
        }

        public void DisplayFailedMissionInfo(SpriteBatch spriteBatch)
        {
            if (layer == 2)
            {
                if (MissionManager.ReturnFailedDeadMissions().Count > 0)
                {
                    if (var2 <= MissionManager.ReturnFailedDeadMissions().Count - 1)
                    {
                        MissionManager.ReturnFailedDeadMissions()[var2].DisplayMissionInfo(spriteBatch, Game.fontManager.GetFont(16));
                    }
                }
            }
        }
    }
}
