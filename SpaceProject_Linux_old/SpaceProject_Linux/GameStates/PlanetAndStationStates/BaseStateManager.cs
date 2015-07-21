using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    public enum ButtonControl
    {
        Menu,
        Second,
        Mission,
        Response,
        SelectShop,
        Shop,
        TransactionConfirm,
        Counter,
        Confirm,
        Inventory
    }

    public class BaseStateManager
    {
        protected Game1 Game;

        protected List<MenuState> menuStates;

        protected MenuState activeMenuState;

        protected OverviewMenuState overviewMenuState;
        protected InfoMenuState infoMenuState;
        protected MineMenuState miningMenuState;
        protected MissionMenuState missionMenuState;
        protected ShopMenuState shopMenuState;
        protected FuelShopMenuState fuelShopMenuState;
        protected RumorsMenuState rumorsMenuState;

        #region TextBox Fields

        protected List<TextBox> textBoxes;

        protected List<string> tempTextList;
        protected List<string> tempVariableList;

        protected Rectangle portraitTextRectangle;
        protected Rectangle normalTextRectangle;

        protected List<Rectangle> responseRectangles;

        protected Rectangle responseRectangle1;
        protected Rectangle responseRectangle2;
        protected Rectangle responseRectangle3;
        protected Rectangle responseRectangle4;

        #endregion

        #region Button Fields

        protected MenuDisplayObject[,] allButtons;
        protected List<MenuDisplayObject> firstButtons;
        protected List<MenuDisplayObject> secondButtons;
        protected List<MenuDisplayObject> buttonsToRemove;

        protected int activeButtonIndexX;
        protected int activeButtonIndexY;
        protected MenuDisplayObject activeButton;
        protected MenuDisplayObject previousButton;
        protected ButtonControl buttonControl;

        #endregion

        #region Properties

        #region TextBox Properties

        public List<TextBox> TextBoxes { get { return textBoxes; } set { textBoxes = value; } }

        public List<string> TempTextList { get { return tempTextList; } set { tempTextList = value; } }
        public List<string> TempVariableList { get { return tempVariableList; } set { tempVariableList = value; } }

        public Rectangle PortraitTextRectangle { get { return portraitTextRectangle; } set { portraitTextRectangle = value; } }
        public Rectangle NormalTextRectangle { get { return normalTextRectangle; } set { normalTextRectangle = value; } }

        public List<Rectangle> ResponseRectangles { get { return responseRectangles; } set { responseRectangles = value; } }

        public Rectangle ResponseRectangle1 { get { return responseRectangle1; } set { responseRectangle1 = value; } }
        public Rectangle ResponseRectangle2 { get { return responseRectangle2; } set { responseRectangle2 = value; } }
        public Rectangle ResponseRectangle3 { get { return responseRectangle3; } set { responseRectangle3 = value; } }
        public Rectangle ResponseRectangle4 { get { return responseRectangle4; } set { responseRectangle4 = value; } }

        #endregion

        #region Button Properties

        public MenuDisplayObject[,] AllButtons { get { return allButtons; } set { allButtons = value; } }
        public List<MenuDisplayObject> FirstButtons { get { return firstButtons; } set { firstButtons = value; } }
        public List<MenuDisplayObject> SecondButtons { get { return secondButtons; } set { secondButtons = value; } }
        public List<MenuDisplayObject> ButtonsToRemove { get { return buttonsToRemove; } set { buttonsToRemove = value; } }

        public int ActiveButtonIndexX { get { return activeButtonIndexX; } set { activeButtonIndexX = value; } }
        public int ActiveButtonIndexY { get { return activeButtonIndexY; } set { activeButtonIndexY = value; } }
        public MenuDisplayObject ActiveButton { get { return activeButton; } set { activeButton = value; } }
        public MenuDisplayObject PreviousButton { get { return previousButton; } set { previousButton = value; } }
        public ButtonControl ButtonControl { get { return buttonControl; } set { buttonControl = value; } }

        #endregion

        #endregion

        #region MenuState Properties

        public MenuState ActiveMenuState { get { return activeMenuState; } set { activeMenuState = value; } }

        public OverviewMenuState OverviewMenuState { get { return overviewMenuState; } set { overviewMenuState = value; } }
        public InfoMenuState InfoMenuState { get { return infoMenuState; } set { infoMenuState = value; } }
        public MineMenuState MiningMenuState { get { return miningMenuState; } set { miningMenuState = value; } }
        public MissionMenuState MissionMenuState { get { return missionMenuState; } set { missionMenuState = value; } }
        public ShopMenuState ShopMenuState { get { return shopMenuState; } set { shopMenuState = value; } }
        public FuelShopMenuState FuelShopMenuState { get { return fuelShopMenuState; } set { fuelShopMenuState = value; } }
        public RumorsMenuState RumorsMenuState { get { return rumorsMenuState; } set { rumorsMenuState = value; } }

        #endregion

        public void ChangeMenuSubState(String state)
        {
            for (int i = 0; i < menuStates.Count; i++)
            {
                if (state.ToLower().Equals(menuStates[i].Name.ToLower()))
                {
                    if (activeMenuState != null)
                        activeMenuState.OnLeave();

                    activeMenuState = menuStates[i];

                    activeMenuState.OnEnter();
                }

            }
        }
    }
}
