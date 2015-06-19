using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class OverviewMenuState: MenuState
    {
        private MenuDisplayObject buttonShop;
        private MenuDisplayObject buttonMission;
        private MenuDisplayObject buttonRumors;
        private MenuDisplayObject buttonBack;

        public MenuDisplayObject ButtonMission { get { return buttonMission; } set { buttonMission = value; } }
        public MenuDisplayObject ButtonRumors { get { return buttonRumors; } set { buttonRumors = value; } }
        public MenuDisplayObject ButtonBack { get { return buttonBack; } set { buttonBack = value; } } 

        private Cursor shopSelectCursor;
        private int shopSelectCursorIndex;
        private Rectangle shopSelectRectangle1;
        private Rectangle shopSelectRectangle2;
        private Rectangle shopSelectRectangle3;

        public OverviewMenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState) :
            base(game, name, manager, baseState)
        { }

        public override void Initialize()
        {
            base.Initialize();

            SetButtons();

            shopSelectCursor = new Cursor(this.Game, this.SpriteSheet, new Rectangle(201, 121, 14, 14), new Rectangle(201, 134, 14, 14));

            shopSelectRectangle1 = new Rectangle(Game.Window.ClientBounds.Width * 2 / 3,
                                                Game.Window.ClientBounds.Height / 2 + 100,
                                                Game.Window.ClientBounds.Width * 2 / 3 - 20,
                                                10);

            shopSelectRectangle2 = new Rectangle(Game.Window.ClientBounds.Width * 2 / 3,
                                                Game.Window.ClientBounds.Height / 2 + 120,
                                                Game.Window.ClientBounds.Width * 2 / 3 - 20,
                                                10);

            shopSelectRectangle3 = new Rectangle(Game.Window.ClientBounds.Width * 2 / 3,
                                                Game.Window.ClientBounds.Height / 2 + 140,
                                                Game.Window.ClientBounds.Width * 2 / 3 - 20,
                                                10);
        }

        public override void OnEnter()
        {
            foreach (MenuDisplayObject button in BaseStateManager.AllButtons)
            {
                button.isVisible = true;
            }

            if (BaseState.GetBase() != null)
            {
                if (BaseState.GetBase() is Planet)
                {
                    buttonMission.isDeactivated = !((Planet)BaseState.GetBase()).HasColony;
                    buttonRumors.isDeactivated = !((Planet)BaseState.GetBase()).HasColony;
                }
            }

            BaseStateManager.ButtonControl = ButtonControl.Menu;

            CursorActions();

            MissionManager.CheckMissionLogic(Game);

            if (MissionManager.MissionStartBuffer.Count > 0)
            {
                BaseStateManager.ChangeMenuSubState("Mission");
                BaseStateManager.MissionMenuState.DisplayMissionStartBufferText();
                return;
            }

            if (MissionManager.MissionEventBuffer.Count > 0)
            {
                BaseStateManager.ChangeMenuSubState("Mission");
                BaseStateManager.MissionMenuState.MissionEvent();
                return;
            }

            if (BaseState.GetBase() != null)
            {
                if (MissionManager.ReturnCompletedMissions(BaseState.GetBase().name).Count <= 0 &&
                    MissionManager.ReturnFailedMissions(BaseState.GetBase().name).Count <= 0)
                {
                    shopSelectCursorIndex = 0;

                    CursorActions();

                    if (StatsManager.EmergencyFusionCell < 1)
                        StatsManager.EmergencyFusionCell = 1;
                }

                else if (MissionManager.ReturnCompletedMissions(BaseState.GetBase().name).Count > 0)
                {
                    BaseStateManager.ChangeMenuSubState("Mission");
                    BaseStateManager.MissionMenuState.DisplayMissionCompletedText();
                }

                else if (MissionManager.ReturnFailedMissions(BaseState.GetBase().name).Count > 0)
                {
                    BaseStateManager.ChangeMenuSubState("Mission");
                    BaseStateManager.MissionMenuState.DisplayMissionFailedText();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (ControlManager.CheckKeyPress(Microsoft.Xna.Framework.Input.Keys.S))
            {
                BaseStateManager.ChangeMenuSubState("Shop");

                BaseStateManager.TextBoxes.Clear();
                shopSelectCursor.isVisible = false;
            }

            if (BaseStateManager.ButtonControl == ButtonControl.SelectShop)
            {
                shopSelectCursor.isActive = true;
                shopSelectCursor.isVisible = true;

                BaseStateManager.ActiveButton = null;

                //Moves button cursor right when pressing up. 
                if (ControlManager.CheckPress(RebindableKeys.Up))
                    shopSelectCursorIndex--;

                //Moves button cursor left when pressing down
                else if (ControlManager.CheckPress(RebindableKeys.Down))
                    shopSelectCursorIndex++;

                if (shopSelectCursorIndex > 1)
                    shopSelectCursorIndex = 0;

                else if (shopSelectCursorIndex < 0)
                    shopSelectCursorIndex = 1;

                switch (shopSelectCursorIndex)
                {
                    case 0:
                        {
                            shopSelectCursor.position.X = shopSelectRectangle1.X - Game.fontManager.GetFont(16).MeasureString("Buy/Sell Items").X / 2 - 10;
                            shopSelectCursor.position.Y = shopSelectRectangle1.Y + 13;
                            break;
                        }

                    case 1:
                        {
                            shopSelectCursor.position.X = shopSelectRectangle3.X - Game.fontManager.GetFont(16).MeasureString("Back").X / 2 - 10;
                            shopSelectCursor.position.Y = shopSelectRectangle3.Y + 13;
                            break;
                        }
                }

            }

            else
            {
                shopSelectCursor.isActive = false;
                shopSelectCursor.isVisible = false;
            }

            if (ControlManager.CheckPress(RebindableKeys.Action2) ||
                ControlManager.CheckPress(RebindableKeys.Pause))
            {
                if (BaseStateManager.ButtonControl.Equals(ButtonControl.SelectShop))
                {
                    OnEnter();
                }
            }

            base.Update(gameTime);
        }

        public override void ButtonActions()
        {
            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Menu) ||
                BaseStateManager.ButtonControl.Equals(ButtonControl.Second))
            {
                switch (BaseStateManager.ActiveButton.name)
                {
                    case "Missions":
                        {
                            BaseStateManager.MissionMenuState.DisplayAvailableMissions(MissionManager.ReturnAvailableMissions(BaseState.GetBase().name));

                            BaseStateManager.ChangeMenuSubState("Mission");

                            BaseStateManager.MissionMenuState.SelectMission();

                            break;
                        }

                    case "Rumors":
                        {
                            BaseStateManager.RumorsMenuState.DisplayRumors();
                            break;
                        }

                    case "Buy/Sell":
                        {
                            BaseStateManager.ChangeMenuSubState("Shop");

                            BaseStateManager.TextBoxes.Clear();
                            shopSelectCursor.isVisible = false;
                            buttonBack.isVisible = false;
                            buttonShop.isVisible = false;
                            break;
                        }

                    case "Back":
                        {
                            if (BaseStateManager.ActiveMenuState.Equals(BaseStateManager.OverviewMenuState))
                            {
                                if (StatsManager.gameMode != GameMode.campaign)
                                    Game.stateManager.ChangeState("OverworldState");
                                else
                                    Game.stateManager.ChangeState("CampaignState");
                            }

                            break;
                        }

                    default:
                        break;
                }
            }
        }

        public override void CursorActions()
        {
            base.CursorActions();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            shopSelectCursor.Draw(spriteBatch);
        }

        public void SetButtons()
        {
            BaseStateManager.AllButtons.Clear();

            if (BaseState.GetBase() != null && BaseState.GetBase().HasShop)
            {
                //Shop
                tempRectPassive = new Rectangle(202, 334, 245, 60);
                tempRectActive = new Rectangle(202, 395, 245, 60);
                tempRectSelected = new Rectangle(202, 395, 245, 60);
                buttonShop = new MenuDisplayObject(this.Game,
                                        SpriteSheet.GetSubSprite(tempRectPassive),
                                        SpriteSheet.GetSubSprite(tempRectActive),
                                        SpriteSheet.GetSubSprite(tempRectSelected),
                                        new Vector2(Game.Window.ClientBounds.Width / 2,
                                            (Game.Window.ClientBounds.Height / 2) + Game.Window.ClientBounds.Height / 8),
                                        new Vector2(tempRectActive.Value.Width / 2f, tempRectActive.Value.Height / 2f));
                buttonShop.name = "Buy/Sell";
                buttonShop.isVisible = true;

                //Back
                buttonBack = new MenuDisplayObject(this.Game,
                                                SpriteSheet.GetSubSprite(tempRectPassive),
                                                SpriteSheet.GetSubSprite(tempRectActive),
                                                SpriteSheet.GetSubSprite(tempRectSelected),
                                                new Vector2(Game.Window.ClientBounds.Width / 2,
                                                    (Game.Window.ClientBounds.Height / 2) + (Game.Window.ClientBounds.Height / 8) * 2),
                                                new Vector2(tempRectActive.Value.Width / 2f, tempRectActive.Value.Height / 2f));
                buttonBack.name = "Back";
                buttonBack.isVisible = true;

                BaseStateManager.AllButtons.Add(buttonShop);
                BaseStateManager.AllButtons.Add(buttonBack);
            }

            else
            {

                //Mission
                tempRectPassive = new Rectangle(202, 334, 245, 60);
                tempRectActive = new Rectangle(202, 395, 245, 60);
                tempRectSelected = new Rectangle(202, 395, 245, 60);
                buttonMission = new MenuDisplayObject(this.Game,
                                        SpriteSheet.GetSubSprite(tempRectPassive),
                                        SpriteSheet.GetSubSprite(tempRectActive),
                                        SpriteSheet.GetSubSprite(tempRectSelected),
                                        new Vector2(Game.Window.ClientBounds.Width / 2,
                                            (Game.Window.ClientBounds.Height / 2) + Game.Window.ClientBounds.Height / 8),
                                        new Vector2(tempRectActive.Value.Width / 2f, tempRectActive.Value.Height / 2f));
                buttonMission.name = "Missions";
                buttonMission.isVisible = true;

                //Rumors
                buttonRumors = new MenuDisplayObject(this.Game,
                                                SpriteSheet.GetSubSprite(tempRectPassive),
                                                SpriteSheet.GetSubSprite(tempRectActive),
                                                SpriteSheet.GetSubSprite(tempRectSelected),
                                                new Vector2(Game.Window.ClientBounds.Width / 2,
                                                    (Game.Window.ClientBounds.Height / 2) + Game.Window.ClientBounds.Height / 8 * 2),
                                                new Vector2(tempRectActive.Value.Width / 2f, tempRectActive.Value.Height / 2f));
                buttonRumors.name = "Rumors";
                buttonRumors.isVisible = true;

                //Back
                buttonBack = new MenuDisplayObject(this.Game,
                                                SpriteSheet.GetSubSprite(tempRectPassive),
                                                SpriteSheet.GetSubSprite(tempRectActive),
                                                SpriteSheet.GetSubSprite(tempRectSelected),
                                                new Vector2(Game.Window.ClientBounds.Width / 2,
                                                    (Game.Window.ClientBounds.Height / 2) + (Game.Window.ClientBounds.Height / 8) * 3),
                                                new Vector2(tempRectActive.Value.Width / 2f, tempRectActive.Value.Height / 2f));
                buttonBack.name = "Back";
                buttonBack.isVisible = true;

                BaseStateManager.AllButtons.Add(buttonMission);
                BaseStateManager.AllButtons.Add(buttonRumors);
                BaseStateManager.AllButtons.Add(buttonBack);
            }
        }
    }
}
