using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{

    public class StationStateManager : BaseStateManager
    {
        public StationStateManager(Game1 Game)
        {
            this.Game = Game;
            this.baseState = Game.stateManager.stationState;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (Game.stateManager.stationState.Station.Abandoned)
            {
                activeButton = overviewMenuState.ButtonBack;
                activeButtonIndexY = 1;
            }

            else
            {
                activeButton = overviewMenuState.ButtonMission;
                activeButtonIndexY = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Game.stateManager.stationState.Station != null &&
                Game.stateManager.stationState.Station.Abandoned)
            {
                overviewMenuState.ButtonMission.isDeactivated = true;
                overviewMenuState.ButtonRumors.isDeactivated = true;

                activeButton = overviewMenuState.ButtonBack;
                activeButtonIndexX = 0;
                activeButtonIndexY = 1;
            }
        }
    }
}
