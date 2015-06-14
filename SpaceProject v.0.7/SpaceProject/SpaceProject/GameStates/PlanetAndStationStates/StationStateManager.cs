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
        private StationState stationState;

        public StationStateManager(Game1 Game)
        {
            this.Game = Game;
            this.stationState = Game.stateManager.stationState;
        }

        public override void Initialize()
        {
            base.Initialize();

            overviewMenuState = new OverviewMenuState(this.Game, "Overview", this, stationState);
            overviewMenuState.Initialize();
            menuStates.Add(overviewMenuState);

            missionMenuState = new MissionMenuState(this.Game, "Mission", this, stationState);
            missionMenuState.Initialize();
            menuStates.Add(missionMenuState);

            shopMenuState = new ShopMenuState(this.Game, "Shop", this, stationState);
            shopMenuState.Initialize();
            menuStates.Add(shopMenuState);

            fuelShopMenuState = new FuelShopMenuState(this.Game, "FuelShop", this, stationState);
            fuelShopMenuState.Initialize();
            menuStates.Add(fuelShopMenuState);

            rumorsMenuState = new RumorsMenuState(this.Game, "Rumors", this, stationState);
            rumorsMenuState.Initialize();
            menuStates.Add(rumorsMenuState);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (Game.stateManager.stationState.Station.Abandoned)
            {
                overviewMenuState.ButtonMission.isDeactivated = true;
                overviewMenuState.ButtonRumors.isDeactivated = true;

                activeButton = overviewMenuState.ButtonBack;
                activeButtonIndexY = 1;
            }

            else
            {
                overviewMenuState.ButtonMission.isDeactivated = false;
                overviewMenuState.ButtonRumors.isDeactivated = false;

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
