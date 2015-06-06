﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class OverviewMenuState: MenuState
    {
        private MenuDisplayObject buttonBack;

        private MenuDisplayObject buttonMission;
        private MenuDisplayObject buttonRumors;

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
            #region CreateButtons

            base.Initialize();

            //Mission
            tempRectPassive = new Rectangle(202, 334, 245, 60);
            tempRectActive = new Rectangle(202, 395, 245, 60);
            tempRectSelected = new Rectangle(202, 395, 245, 60);
            buttonMission = new MenuDisplayObject(this.Game,
                                    SpriteSheet.GetSubSprite(tempRectPassive),
                                    SpriteSheet.GetSubSprite(tempRectActive),
                                    SpriteSheet.GetSubSprite(tempRectSelected),
                                    new Vector2(Game.Window.ClientBounds.Width / 2,
                                        (Game.Window.ClientBounds.Height / 2) + Game.Window.ClientBounds.Height / 7),
                                    new Vector2(tempRectActive.Value.Width / 2f, tempRectActive.Value.Height / 2f));
            buttonMission.name = "Missions";
            buttonMission.isVisible = true;

            //Rumors
            buttonRumors = new MenuDisplayObject(this.Game,
                                            SpriteSheet.GetSubSprite(tempRectPassive),
                                            SpriteSheet.GetSubSprite(tempRectActive),
                                            SpriteSheet.GetSubSprite(tempRectSelected),
                                            new Vector2(Game.Window.ClientBounds.Width / 2,
                                                (Game.Window.ClientBounds.Height / 2) + Game.Window.ClientBounds.Height / 7 * 2),
                                            new Vector2(tempRectActive.Value.Width / 2f, tempRectActive.Value.Height / 2f));
            buttonRumors.name = "Rumors";
            buttonRumors.isVisible = true;

            //Back
            buttonBack = new MenuDisplayObject(this.Game,
                                            SpriteSheet.GetSubSprite(tempRectPassive),
                                            SpriteSheet.GetSubSprite(tempRectActive),
                                            SpriteSheet.GetSubSprite(tempRectSelected),
                                            new Vector2(Game.Window.ClientBounds.Width / 2,
                                                (Game.Window.ClientBounds.Height / 2) + (Game.Window.ClientBounds.Height / 7) * 3),
                                            new Vector2(tempRectActive.Value.Width / 2f, tempRectActive.Value.Height / 2f));
            buttonBack.name = "Back";
            buttonBack.isVisible = true;

            BaseStateManager.FirstButtons.Add(buttonBack);
            BaseStateManager.SecondButtons.Add(buttonMission);
            BaseStateManager.SecondButtons.Add(buttonRumors);

            BaseStateManager.AllButtons.Add(buttonMission);
            BaseStateManager.AllButtons.Add(buttonRumors);
            BaseStateManager.AllButtons.Add(buttonBack);

            #endregion

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
                            if (BaseStateManager.ActiveMenuState.Equals(BaseStateManager.OverviewMenuState))
                            {
                                if (MissionManager.ReturnAvailableMissions(BaseState.GetBase().name).Count > 0 ||
                                    MissionManager.ReturnCompletedMissions(BaseState.GetBase().name).Count > 0)
                                {
                                    BaseStateManager.ChangeMenuSubState("Mission");

                                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16),
                                                      new Rectangle((Game.Window.ClientBounds.Width * 2 / 3),
                                                                   ((Game.Window.ClientBounds.Height / 2) + 40) + 20 * (BaseStateManager.MissionMenuState.AvailableMissions.Count + 1),
                                                                     Game.Window.ClientBounds.Width - 20,
                                                      10),
                                                      true,
                                                      "Back"));

                                    BaseStateManager.MissionMenuState.SelectMission();
                                }
                            }

                            break;
                        }

                    case "Buy/Sell":
                        {
                            if (BaseState.GetBase().HasShop)
                            {
                                BaseStateManager.TextBoxes.Clear();

                                BaseStateManager.ButtonControl = ButtonControl.SelectShop;

                                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16),
                                                                                      shopSelectRectangle1,
                                                                                      true,
                                                                                      "Buy/Sell Items"));

                                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16),
                                                                                      shopSelectRectangle3,
                                                                                      true,
                                                                                      "Back"));
                            }
                            break;
                        }

                    case "Mining":
                        {
                            if (BaseState.GetBase() is Planet)
                            {
                                List<string> tempStrList = new List<string>();
                                List<int> tempIntList = new List<int>();

                                if (((Planet)BaseState.GetBase()).ResourceTypes.Count > 0)
                                {
                                    bool enter = false;

                                    for (int i = 0; i < ((Planet)BaseState.GetBase()).ResourceTypes.Count; i++)
                                    {
                                        if (((Planet)BaseState.GetBase()).ResourceCount[i] > 0)
                                            enter = true;
                                    }

                                    if (enter)
                                    {
                                        int val = Game.random.Next(3);

                                        switch (val)
                                        {
                                            case 0:
                                                throw new NotImplementedException("Outdated recource gather level removed during Level-class cleaning // Jakob 150117");
                                                //((LevelResourceGather)Game.stateManager.shooterState.GetLevel("ResourceGatherLevel")).SetUpLevel(((Planet)BaseState.GetBase()));

                                            case 1:
                                                throw new NotImplementedException("Outdated recource gather level removed during Level-class cleaning // Jakob 150117");
                                                //((LevelResourceGather)Game.stateManager.shooterState.GetLevel("ResourceGatherLevel")).SetUpLevel(((Planet)BaseState.GetBase()),
                                                //    "green", 45);

                                            case 2:
                                                tempStrList.Add("green");
                                                tempIntList.Add(36);
                                                tempStrList.Add("red");
                                                tempIntList.Add(18);
                                                tempStrList.Add("blue");
                                                tempIntList.Add(9);
                                                throw new NotImplementedException("Outdated recource gather level removed during Level-class cleaning // Jakob 150117");
                                                //((LevelResourceGather)Game.stateManager.shooterState.GetLevel("ResourceGatherLevel")).SetUpLevel(((Planet)BaseState.GetBase()),
                                                //    tempStrList, tempIntList);
                                        }

                                        Game.stateManager.shooterState.BeginLevel("ResourceGatherLevel");
                                    }
                                }
                            }
                            break;
                        }

                    case "Back":
                        {
                            //Actions for pressing "BACK" button
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

            else if (BaseStateManager.ButtonControl == ButtonControl.SelectShop)
            {
                switch (shopSelectCursorIndex)
                {
                    case 0:
                        BaseStateManager.ChangeMenuSubState("Shop");

                        BaseStateManager.TextBoxes.Clear();
                        shopSelectCursor.isVisible = false;
                        break;

                    case 1:
                        BaseStateManager.ChangeMenuSubState("Overview");

                        BaseStateManager.TextBoxes.Clear();
                        shopSelectCursor.isVisible = false;

                        CursorActions();
                        break;
                }
            }
        }

        public override void CursorActions()
        {
            base.CursorActions();

            switch (BaseStateManager.ActiveButton.name)
            {
                case "Rumors":
                    {
                        BaseStateManager.RumorsMenuState.DisplayRumors();
                        break;
                    }


                case "Missions":
                    {
                        BaseStateManager.MissionMenuState.DisplayAvailableMissions(MissionManager.ReturnAvailableMissions(BaseState.GetBase().name));

                        if (MissionManager.ReturnAvailableMissions(BaseState.GetBase().name).Count > 0)
                        {
                            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                          new Rectangle(Game.Window.ClientBounds.Width * 2 / 3,
                                                                                        (int)(Game.Window.ClientBounds.Height - (int)BaseState.Game.fontManager.GetFont(14).MeasureString("Press 'Enter' to go to mission selection..").Y) - 20,
                                                                                        (Game.Window.ClientBounds.Width * 2 / 3) - 20,
                                                                                        10),
                                                                          true,
                                                                          "Press 'Enter' to go to mission selection.."));
                        }
                        break;
                    }

                case "Buy/Sell":
                    {
                        if (BaseState.GetBase().HasShop)
                        {
                            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                        new Rectangle(Game.Window.ClientBounds.Width * 2 / 3,
                                                      (int)(Game.Window.ClientBounds.Height - (int)BaseState.Game.fontManager.GetFont(14).MeasureString("Press 'Enter' to access shop..").Y) - 20,
                                                      (Game.Window.ClientBounds.Width * 2 / 3) / 2,
                                                      10),
                                        true,
                                        "Press 'Enter' to access shop.."));
                        }
                        else
                        {
                            if (BaseStateManager.PreviousButton == buttonRumors)
                            {
                                BaseStateManager.ActiveButtonIndexX = 0;
                                CursorActions();
                            }
                            else if (BaseStateManager.PreviousButton == buttonMission)
                            {
                                BaseStateManager.ActiveButtonIndexX = 1;
                                CursorActions();
                            }
                            else if (BaseStateManager.PreviousButton == buttonBack)
                            {
                                BaseStateManager.ActiveButtonIndexX = 2;
                                BaseStateManager.ActiveButtonIndexY = 1;
                                CursorActions();
                            }
                        }
                        break;
                    }

                case "Mining":
                    {
                        if (BaseState.GetBase() is Planet)
                        {
                            string tempString;

                            //AAAAAHHH. I will fix this float-to-int-transition-sickness-symptom later..
                            tempString = "Resources\n\n" + TextUtils.ReturnStringFromList(((Planet)BaseState.GetBase()).ResourceTypes);


                            if (((Planet)BaseState.GetBase()).ResourceTypes.Count > 0)
                            {
                                for (int i = 0; i < ((Planet)BaseState.GetBase()).ResourceCount.Count; i++)
                                {
                                    if (((Planet)BaseState.GetBase()).ResourceCount[i] > 0)
                                    {
                                        tempString += "\n\nPress 'Enter' to start mining resources.";
                                        break;
                                    }
                                }
                            }

                            else
                                tempString += "\n\nNo resources to mine.";

                            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                BaseStateManager.NormalTextRectangle,
                                false,
                                tempString));
                        }
                        break;
                    }

                case "Planet Info":
                    {
                        if (BaseState.GetBase() is Planet)
                        {
                            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBoxAndGetText(BaseState.Game.fontManager.GetFont(14),
                                                                                        BaseStateManager.NormalTextRectangle,
                                                                                        "Data/planetdata.dat",
                                                                                        ((Planet)BaseState.GetBase()).PlanetCodeName,
                                                                                        "Info",
                                                                                        false));

                            //SubStateManager.ButtonControl = ButtonControl.Menu;

                        }
                        break;
                    }

                case "Back":
                    {
                        BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                      new Rectangle(Game.Window.ClientBounds.Width * 2 / 3,
                                                                                    (int)(Game.Window.ClientBounds.Height - (int)BaseState.Game.fontManager.GetFont(14).MeasureString("Press 'Enter' to access shop..").Y) - 20,
                                                                                    (Game.Window.ClientBounds.Width * 2 / 3) / 2,
                                                                                    10),
                                                                      true,
                                                                      "Press 'Enter' to leave.."));
                        break;
                    }

                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            shopSelectCursor.Draw(spriteBatch);
        }
    }
}
