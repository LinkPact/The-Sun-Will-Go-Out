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
            #region Initailize TextBox Fields

            textBoxes = new List<TextBox>();
            tempTextList = new List<string>();
            tempVariableList = new List<string>();

            portraitTextRectangle = new Rectangle(Game.Window.ClientBounds.Width / 2 + 10,
                                                (Game.Window.ClientBounds.Height / 2) + 10,
                                                 (Game.Window.ClientBounds.Width / 2) - 20,
                                                (Game.Window.ClientBounds.Height / 2) - 20);

            normalTextRectangle = new Rectangle(Game.Window.ClientBounds.Width / 3 + 10,
                                    (Game.Window.ClientBounds.Height / 2) + 10,
                                     (Game.Window.ClientBounds.Width * 2 / 3) - 20,
                                    (Game.Window.ClientBounds.Height / 2) - 20);

            responseRectangles = new List<Rectangle>();

            responseRectangle1 = new Rectangle((Game.Window.ClientBounds.Width * 2 / 3),
                                               Game.Window.ClientBounds.Height * 10 / 12,
                                               Game.Window.ClientBounds.Width - 20,
                                               10);

            responseRectangles.Add(responseRectangle1);

            responseRectangle2 = new Rectangle((Game.Window.ClientBounds.Width * 2 / 3),
                                               Game.Window.ClientBounds.Height * 10 / 12 + 24,
                                               Game.Window.ClientBounds.Width - 20,
                                               10);

            responseRectangles.Add(responseRectangle2);

            responseRectangle3 = new Rectangle((Game.Window.ClientBounds.Width * 2 / 3),
                                               Game.Window.ClientBounds.Height * 10 / 12 + 48,
                                               Game.Window.ClientBounds.Width - 20,
                                               10);

            responseRectangles.Add(responseRectangle3);

            responseRectangle4 = new Rectangle((Game.Window.ClientBounds.Width * 2 / 3),
                                               Game.Window.ClientBounds.Height * 10 / 12 + 72,
                                               Game.Window.ClientBounds.Width - 20,
                                               10);

            responseRectangles.Add(responseRectangle4);

            #endregion

            #region Initailize Button Fields

            allButtons = new List<MenuDisplayObject>();
            firstButtons = new List<MenuDisplayObject>();
            secondButtons = new List<MenuDisplayObject>();
            buttonsToRemove = new List<MenuDisplayObject>();

            #endregion

            menuStates = new List<MenuState>();

            overviewMenuState = new OverviewMenuState(this.Game, "Overview", this, planetState);
            overviewMenuState.Initialize();
            menuStates.Add(overviewMenuState);

            infoMenuState = new InfoMenuState(this.Game, "Info", this, planetState);
            infoMenuState.Initialize();
            menuStates.Add(infoMenuState);

            miningMenuState = new MineMenuState(this.Game, "Mining", this, planetState);
            miningMenuState.Initialize();
            menuStates.Add(miningMenuState);

            missionMenuState = new MissionMenuState(this.Game, "Mission", this, planetState);
            missionMenuState.Initialize();
            menuStates.Add(missionMenuState);

            shopMenuState = new ShopMenuState(this.Game, "Shop", this, planetState);
            shopMenuState.Initialize();
            menuStates.Add(shopMenuState);

            fuelShopMenuState = new FuelShopMenuState(this.Game, "FuelShop", this, planetState);
            fuelShopMenuState.Initialize();
            menuStates.Add(fuelShopMenuState);

            rumorsMenuState = new RumorsMenuState(this.Game, "Rumors", this, planetState);
            rumorsMenuState.Initialize();
            menuStates.Add(rumorsMenuState);
        }

        public override void OnEnter()
        {
            rumorsMenuState.OnEnter();

            //activeButton = overviewMenuState.ButtonColony;

            if (planetState.Planet.HasColony)
                activeButtonIndexY = 0;
            else
                activeButtonIndexY = 1;

            activeButtonIndexX = 0;

            ChangeMenuSubState("Overview");
            activeMenuState.CursorActions();

            missionMenuState.MissionCursorIndex = 0;
            missionMenuState.ResponseCursorIndex = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void MouseControls(int activeButtonIndex)
        {
            if (activeButtonIndex == 1)
            {
                for (int i = 0; i < firstButtons.Count; i++)
                {
                    Rectangle buttonRect = new Rectangle(
                        (int)firstButtons[i].Position.X,
                        (int)firstButtons[i].Position.Y,
                        firstButtons[i].Passive.SourceRectangle.Value.Width,
                        firstButtons[i].Passive.SourceRectangle.Value.Height);

                    if (CollisionDetection.IsPointInsideRectangle(ControlManager.GetMousePosition(), buttonRect))
                    {
                        if (ControlManager.GetMousePosition() != ControlManager.GetPreviousMousePosition())
                        {
                            activeButtonIndexX = i;
                        }
                        ActiveMenuState.CursorActions();
                    }
                }
            }
            else
            {
                for (int i = 0; i < secondButtons.Count; i++)
                {
                    Rectangle buttonRect = new Rectangle(
                        (int)secondButtons[i].Position.X,
                        (int)secondButtons[i].Position.Y,
                        secondButtons[i].Passive.SourceRectangle.Value.Width,
                        secondButtons[i].Passive.SourceRectangle.Value.Height);

                    if (CollisionDetection.IsPointInsideRectangle(ControlManager.GetMousePosition(), buttonRect))
                    {
                        if (ControlManager.GetMousePosition() != ControlManager.GetPreviousMousePosition())
                        {
                            activeButtonIndexY = i;
                        }
                        ActiveMenuState.CursorActions();
                    }
                }
            }
        }
    }
}
