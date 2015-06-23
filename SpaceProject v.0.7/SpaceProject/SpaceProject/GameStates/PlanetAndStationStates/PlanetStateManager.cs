using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{

    public class PlanetStateManager : BaseStateManager
    {
        private PlanetState planetState;

        //MenuStates

        public PlanetStateManager(Game1 Game)
        {
            this.Game = Game;
            this.planetState = Game.stateManager.planetState;
        }

        public override void Initialize()
        {
            base.Initialize();

            overviewMenuState = new OverviewMenuState(this.Game, "Overview", this, planetState);
            overviewMenuState.Initialize();
            menuStates.Add(overviewMenuState);

            missionMenuState = new MissionMenuState(this.Game, "Mission", this, planetState);
            missionMenuState.Initialize();
            menuStates.Add(missionMenuState);

            shopMenuState = new ShopMenuState(this.Game, "Shop", this, planetState);
            shopMenuState.Initialize();
            menuStates.Add(shopMenuState);

            rumorsMenuState = new RumorsMenuState(this.Game, "Rumors", this, planetState);
            rumorsMenuState.Initialize();
            menuStates.Add(rumorsMenuState);
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
