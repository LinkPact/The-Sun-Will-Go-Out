using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
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
        private readonly Vector2 PortraitTextOffset = new Vector2(200, 65);
        private readonly Vector2 NormalTextOffset = new Vector2(25, 25);
        private readonly Vector2 ResponseTextOffset = new Vector2(200, 25);

        private readonly Vector2 PortraitOverlaySize = new Vector2(567, 234);
        private readonly Vector2 TextOverlaySize = new Vector2(400, 183);
        private readonly Vector2 ResponseOverlaySize = new Vector2(571, 309);

        protected Game1 Game;
        protected BaseState baseState;

        protected List<MenuState> menuStates;

        protected MenuState activeMenuState;

        protected OverviewMenuState overviewMenuState;
        protected MissionMenuState missionMenuState;
        protected ShopMenuState shopMenuState;
        protected RumorsMenuState rumorsMenuState;

        #region TextBox Fields

        protected List<TextBox> textBoxes;

        protected List<string> tempTextList;
        protected List<string> tempVariableList;

        protected Rectangle portraitTextRectangle;
        protected Rectangle normalTextRectangle;
        protected Rectangle responseTextRectangle;

        protected List<Rectangle> responseRectangles;

        protected Rectangle responseRectangle1;
        protected Rectangle responseRectangle2;
        protected Rectangle responseRectangle3;
        protected Rectangle responseRectangle4;

        #endregion

        #region Button Fields

        protected List<MenuDisplayObject> allButtons;
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

        public Rectangle PortraitTextRectangle { get { return portraitTextRectangle; } }
        public Rectangle NormalTextRectangle { get { return normalTextRectangle; } }
        public Rectangle ResponseTextRectangle { get { return responseTextRectangle; } }

        public List<Rectangle> ResponseRectangles { get { return responseRectangles; } set { responseRectangles = value; } }

        public Rectangle ResponseRectangle1 { get { return responseRectangle1; } set { responseRectangle1 = value; } }
        public Rectangle ResponseRectangle2 { get { return responseRectangle2; } set { responseRectangle2 = value; } }
        public Rectangle ResponseRectangle3 { get { return responseRectangle3; } set { responseRectangle3 = value; } }
        public Rectangle ResponseRectangle4 { get { return responseRectangle4; } set { responseRectangle4 = value; } }

        #endregion

        #region Button Properties

        public List<MenuDisplayObject> AllButtons { get { return allButtons; } set { allButtons = value; } }
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
        public MissionMenuState MissionMenuState { get { return missionMenuState; } set { missionMenuState = value; } }
        public ShopMenuState ShopMenuState { get { return shopMenuState; } set { shopMenuState = value; } }
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

        public virtual void Initialize()
        {
            textBoxes = new List<TextBox>();
            tempTextList = new List<string>();
            tempVariableList = new List<string>();

            portraitTextRectangle = new Rectangle((int)(Game.Window.ClientBounds.Width / 2 - PortraitOverlaySize.X / 2 + PortraitTextOffset.X),
                                                  (int)(Game.Window.ClientBounds.Height / 2 - PortraitOverlaySize.Y / 2 + PortraitTextOffset.Y),
                                                  (int)(PortraitOverlaySize.X - PortraitTextOffset.X - 20),
                                                  (int)(PortraitOverlaySize.Y / 2 - PortraitTextOffset.Y - 20));

            normalTextRectangle = new Rectangle((int)(Game.Window.ClientBounds.Width / 2 - TextOverlaySize.X / 2 + NormalTextOffset.X),
                                                (int)(Game.Window.ClientBounds.Height / 2 - TextOverlaySize.Y / 2 + NormalTextOffset.Y),
                                                (int)(TextOverlaySize.X - NormalTextOffset.X - 20),
                                                (int)(TextOverlaySize.Y / 2 - NormalTextOffset.Y - 20));

            responseTextRectangle = new Rectangle((int)(Game.Window.ClientBounds.Width / 2 - ResponseOverlaySize.X / 2 + ResponseTextOffset.X),
                                      (int)(Game.Window.ClientBounds.Height / 2 - ResponseOverlaySize.Y / 2 + ResponseTextOffset.Y),
                                      (int)(ResponseOverlaySize.X - ResponseTextOffset.X - 20),
                                      (int)(ResponseOverlaySize.Y / 2 - ResponseTextOffset.Y - 20));

            responseRectangles = new List<Rectangle>();

            responseRectangle1 = new Rectangle((Game.Window.ClientBounds.Width / 2 + (int)ResponseOverlaySize.X / 6) - 10,
                                               Game.Window.ClientBounds.Height / 2 + 30,
                                               (int)ResponseOverlaySize.X - 20,
                                               10);

            responseRectangles.Add(responseRectangle1);

            responseRectangle2 = new Rectangle((Game.Window.ClientBounds.Width / 2 + (int)ResponseOverlaySize.X / 6) - 10,
                                               Game.Window.ClientBounds.Height / 2 + 30 + 24,
                                               (int)ResponseOverlaySize.X - 20,
                                               10);

            responseRectangles.Add(responseRectangle2);

            responseRectangle3 = new Rectangle((Game.Window.ClientBounds.Width / 2 + (int)ResponseOverlaySize.X / 6) - 10,
                                               Game.Window.ClientBounds.Height / 2 + 30 + 48,
                                               (int)ResponseOverlaySize.X - 20,
                                               10);

            responseRectangles.Add(responseRectangle3);

            responseRectangle4 = new Rectangle((Game.Window.ClientBounds.Width / 2 + (int)ResponseOverlaySize.X / 6) - 10,
                                               Game.Window.ClientBounds.Height / 2 + 30 + 72,
                                               (int)ResponseOverlaySize.X - 20,
                                               10);

            responseRectangles.Add(responseRectangle4);

            allButtons = new List<MenuDisplayObject>();
            buttonsToRemove = new List<MenuDisplayObject>();

            menuStates = new List<MenuState>();

            rumorsMenuState = new RumorsMenuState(this.Game, "Rumors", this, baseState);
            rumorsMenuState.Initialize();

            overviewMenuState = new OverviewMenuState(this.Game, "Overview", this, baseState);
            overviewMenuState.Initialize();

            missionMenuState = new MissionMenuState(this.Game, "Mission", this, baseState);
            missionMenuState.Initialize();

            shopMenuState = new ShopMenuState(this.Game, "Shop", this, baseState);
            shopMenuState.Initialize();

            menuStates.Add(overviewMenuState);
            menuStates.Add(missionMenuState);
            menuStates.Add(shopMenuState);
            menuStates.Add(rumorsMenuState);
        }

        public virtual void OnEnter()
        {
            rumorsMenuState.OnEnter();
            textBoxes.Clear();
            ChangeMenuSubState("Overview");
            activeMenuState.CursorActions();

            missionMenuState.MissionCursorIndex = 0;
            missionMenuState.ResponseCursorIndex = 0;

        }

        public virtual void Update(GameTime gameTime)
        {
            activeMenuState.Update(gameTime);

            MouseControls();

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

                previousButton = activeButton;
                activeButton = allButtons[activeButtonIndexY];
            }

            else if (buttonControl.Equals(ButtonControl.Mission) ||
                buttonControl.Equals(ButtonControl.Response) ||
                buttonControl.Equals(ButtonControl.Confirm))
            {
                activeButton = null;
            }

            if (ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter))
            {
                ActiveMenuState.ButtonActions();
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

        protected void WrapActiveButton()
        {
            if (buttonControl.Equals(ButtonControl.Menu))
            {
                if (activeButtonIndexY >= allButtons.Count)
                {
                    activeButtonIndexY = 0;
                }

                else if (activeButtonIndexY < 0)
                {
                    activeButtonIndexY = allButtons.Count - 1;
                }

                activeButton = allButtons[activeButtonIndexY];
            }
        }

        protected void MouseControls()
        {
            if (baseState.IsOverlayDisplayed)
            {
                if (ControlManager.IsLeftMouseButtonClicked()
                    && Game.IsActive)
                {
                    ActiveMenuState.ButtonActions();
                }
            }
            else
            {
                for (int i = 0; i < allButtons.Count; i++)
                {
                    if (ControlManager.IsMouseOverArea(allButtons[i].Bounds))
                    {
                        if (ControlManager.GetMousePosition() != ControlManager.GetPreviousMousePosition())
                        {
                            activeButtonIndexY = i;
                        }

                        if (ControlManager.IsLeftMouseButtonClicked())
                        {
                            ActiveMenuState.ButtonActions();
                        }
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < allButtons.Count; i++)
            {
                Color fontColor = FontManager.FontColorStatic;

                allButtons[i].Draw(spriteBatch);

                if (i == activeButtonIndexY)
                    fontColor = Color.LightSkyBlue;

                if (allButtons[i].isVisible)
                {
                    spriteBatch.DrawString(FontManager.GetFontStatic(16), allButtons[i].name, allButtons[i].Position, fontColor, 0f,
                        FontManager.GetFontStatic(16).MeasureString(allButtons[i].name) / 2, 1f, SpriteEffects.None, 0.9f);
                }
            }

            activeMenuState.Draw(spriteBatch);

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
