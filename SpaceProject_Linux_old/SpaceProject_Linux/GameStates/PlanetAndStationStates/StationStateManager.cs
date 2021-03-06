﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Linux
{

    public class StationStateManager : BaseStateManager
    {
        private StationState stationState;

        public StationStateManager(Game1 Game)
        {
            this.Game = Game;
            this.stationState = Game.stateManager.stationState;
        }

        public void Initialize()
        {

            #region Initailize TextBox Fields

            textBoxes = new List<TextBox>();
            tempTextList = new List<string>();
            tempVariableList = new List<string>();

            portraitTextRectangle = new Rectangle(Game.ScreenSize.X / 2 + 10,
                                                (Game.ScreenSize.Y / 2) + 10,
                                                 (Game.ScreenSize.X / 2) - 20,
                                                (Game.ScreenSize.Y / 2) - 20);

            normalTextRectangle = new Rectangle(Game.ScreenSize.X / 3 + 10,
                        (Game.ScreenSize.Y / 2) + 10,
                         (Game.ScreenSize.X * 2 / 3) - 20,
                        (Game.ScreenSize.Y / 2) - 20);

            responseRectangles = new List<Rectangle>();

            responseRectangle1 = new Rectangle((Game.ScreenSize.X * 2 / 3),
                                               Game.ScreenSize.Y * 10 / 12,
                                               Game.ScreenSize.X - 20,
                                               10);

            responseRectangles.Add(responseRectangle1);

            responseRectangle2 = new Rectangle((Game.ScreenSize.X * 2 / 3),
                                               Game.ScreenSize.Y * 10 / 12 + 24,
                                               Game.ScreenSize.X - 20,
                                               10);

            responseRectangles.Add(responseRectangle2);

            responseRectangle3 = new Rectangle((Game.ScreenSize.X * 2 / 3),
                                               Game.ScreenSize.Y * 10 / 12 + 48,
                                               Game.ScreenSize.X - 20,
                                               10);

            responseRectangles.Add(responseRectangle3);

            responseRectangle4 = new Rectangle((Game.ScreenSize.X * 2 / 3),
                                               Game.ScreenSize.Y * 10 / 12 + 72,
                                               Game.ScreenSize.X - 20,
                                               10);

            responseRectangles.Add(responseRectangle4);

            #endregion

            #region Initailize Button Fields

            allButtons = new MenuDisplayObject[4, 2];
            firstButtons = new List<MenuDisplayObject>();
            secondButtons = new List<MenuDisplayObject>();
            buttonsToRemove = new List<MenuDisplayObject>();

            #endregion

            menuStates = new List<MenuState>();

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

        public void OnEnter()
        {
            rumorsMenuState.OnEnter();
            textBoxes.Clear();

            missionMenuState.MissionCursorIndex = 0;
            missionMenuState.ResponseCursorIndex = 0;

            if (Game.stateManager.stationState.Station.Abandoned)
            {
                overviewMenuState.ButtonMission.isDeactivated = true;
                overviewMenuState.ButtonRumors.isDeactivated = true;
                overviewMenuState.ButtonShop.isDeactivated = true;

                activeButton = overviewMenuState.ButtonBack;
                activeButtonIndexX = 0;
                activeButtonIndexY = 1;
            }

            else
            {
                overviewMenuState.ButtonMission.isDeactivated = false;
                overviewMenuState.ButtonRumors.isDeactivated = false;
                overviewMenuState.ButtonShop.isDeactivated = false;

                activeButton = overviewMenuState.ButtonMission;
                activeButtonIndexY = 0;
            }

            ChangeMenuSubState("Overview");
        }

        public void Update(GameTime gameTime)
        {
            activeMenuState.Update(gameTime);

            if (Game.stateManager.stationState.Station != null &&
                Game.stateManager.stationState.Station.Abandoned)
            {
                overviewMenuState.ButtonMission.isDeactivated = true;
                overviewMenuState.ButtonRumors.isDeactivated = true;
                overviewMenuState.ButtonShop.isDeactivated = true;

                activeButton = overviewMenuState.ButtonBack;
                activeButtonIndexX = 0;
                activeButtonIndexY = 1;
            }

            if (buttonControl == ButtonControl.Menu)
            {
                if (ControlManager.CheckPress(RebindableKeys.Down))
                {
                    activeButtonIndexY++;
                    WrapActiveButton();
                    ActiveMenuState.CursorActions();
                }

                else if (ControlManager.CheckPress(RebindableKeys.Up))
                {
                    activeButtonIndexY--;
                    WrapActiveButton();
                    ActiveMenuState.CursorActions();
                }

                if (ControlManager.CheckPress(RebindableKeys.Right))
                {
                    activeButtonIndexX++;
                    WrapActiveButton();
                    ActiveMenuState.CursorActions();
                }

                else if (ControlManager.CheckPress(RebindableKeys.Left))
                {
                    activeButtonIndexX--;
                    WrapActiveButton();
                    ActiveMenuState.CursorActions();
                }

                //MouseControls();
            }

            if (buttonControl.Equals(ButtonControl.Menu))
            {
                previousButton = activeButton;
                activeButton = allButtons[activeButtonIndexX, activeButtonIndexY];
            }

            if (ControlManager.CheckPress(RebindableKeys.Action1) ||
                ControlManager.CheckKeyPress(Keys.Enter))
            {
                ActiveMenuState.ButtonActions();
            }

            if (buttonControl.Equals(ButtonControl.Mission) ||
                buttonControl.Equals(ButtonControl.Response) ||
                buttonControl.Equals(ButtonControl.Confirm))
            {
                activeButton = null;
            }

            foreach (MenuDisplayObject button in allButtons)
            {
                if (button != null)
                {
                    button.isActive = false;
                }
            }

            if (activeButton != null)
                activeButton.isActive = true;

            missionMenuState.UpdateTextCursorPos();

            foreach (TextBox txtBox in textBoxes)
                txtBox.Update(gameTime);  
        }

        private void MouseControls()
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

        private void WrapActiveButton()
        {
            if (buttonControl.Equals(ButtonControl.Menu))
            {
                if (activeButtonIndexX < 0)
                {
                    activeButtonIndexX = secondButtons.Count - 1;
                }

                else if (activeButtonIndexX > secondButtons.Count - 1)
                {
                    activeButtonIndexX = 0;
                }

                if (activeButtonIndexY > 1)
                {
                    activeButtonIndexY = 0;
                }

                else if (activeButtonIndexY < 0)
                {
                    activeButtonIndexY = 1;
                }

                if (activeButtonIndexY == 1)
                {
                    activeButtonIndexX = 0;
                }

                //activeButton = allButtons[activeButtonIndexX, activeButtonIndexY];
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            activeMenuState.Draw(spriteBatch);

            //Draw buttons
            foreach (MenuDisplayObject button in allButtons)
            {
                if (button != null && button.isVisible)
                    button.Draw(spriteBatch);
            }

            foreach (TextBox txtBox in textBoxes)
            {
                Color color = Game.fontManager.FontColor;

                if (txtBox.GetText().Contains("Main - "))
                {
                    color = MissionManager.MainMissionColor;
                }

                else if (txtBox.GetText().Contains("Secondary - "))
                {
                    color = MissionManager.SideMissionColor;
                }

                txtBox.Draw(spriteBatch, color, Game.fontManager.FontOffset);
            }
        }
    }
}
