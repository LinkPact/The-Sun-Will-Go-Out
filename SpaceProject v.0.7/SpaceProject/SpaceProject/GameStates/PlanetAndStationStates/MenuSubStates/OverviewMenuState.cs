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

        public OverviewMenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState) :
            base(game, name, manager, baseState)
        { }

        public override void Initialize()
        {
            base.Initialize();

            SetButtons();
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

        public override void Draw(SpriteBatch spriteBatch) { }

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
