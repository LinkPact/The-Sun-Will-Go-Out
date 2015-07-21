using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class MenuState
    {
        protected Game1 Game;
        protected string name;
        public string Name { get { return name; } private set { ;} }

        protected BaseState BaseState;
        protected BaseStateManager BaseStateManager;
        protected Sprite SpriteSheet;

        protected Nullable<Rectangle> tempRectPassive;
        protected Nullable<Rectangle> tempRectActive;
        protected Nullable<Rectangle> tempRectSelected;
        protected Nullable<Rectangle> tempRectDisabled;

        protected string confirmString;
        protected Vector2 confirmStringPos;
        protected Vector2 confirmStringOrigin;

        protected MenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState)
        {
            this.Game = game;
            this.name = name;
            this.BaseState = baseState;
            this.BaseStateManager = manager;

            this.SpriteSheet = game.stateManager.planetState.SpriteSheet;
        }

        public virtual void Initialize()
        {            
        }

        public virtual void OnEnter()
        {            
        }

        public virtual void OnLeave()
        { }

        public virtual void Update(GameTime gameTime)
        {
            if (ControlManager.CheckPress(RebindableKeys.Pause) ||
                ControlManager.CheckPress(RebindableKeys.Action2))
            {
                if (BaseStateManager.ButtonControl != ButtonControl.Confirm &&
                    BaseStateManager.ButtonControl != ButtonControl.Response)
                {
                    if (BaseStateManager.ActiveMenuState.Equals(BaseStateManager.OverviewMenuState))
                    {
                        if (StatsManager.gameMode != GameMode.campaign)
                            Game.stateManager.ChangeState("OverworldState");
                        else
                            Game.stateManager.ChangeState("CampaignState");
                    }

                    else if (BaseStateManager.ActiveMenuState.Equals(BaseStateManager.MissionMenuState) &&
                        BaseStateManager.ButtonControl.Equals(ButtonControl.Mission))
                    {
                        BaseStateManager.ChangeMenuSubState("Overview");
                    }

                    else if (BaseStateManager.ActiveMenuState.Equals(BaseStateManager.MissionMenuState) &&
                        BaseStateManager.ButtonControl.Equals(ButtonControl.Response))
                    {
                        BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                        BaseStateManager.MissionMenuState.SelectMission();
                    }

                    else if (BaseStateManager.ActiveMenuState.Equals(BaseStateManager.ShopMenuState))
                    {
                        BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                        BaseStateManager.ButtonControl = ButtonControl.Second;
                        BaseStateManager.ChangeMenuSubState("Overview");
                    }
                }
            }
        }

        public virtual void ButtonActions()
        { }

        public virtual void CursorActions()
        {
            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Menu))
                BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];

            BaseStateManager.TextBoxes.Clear();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        { }
    }
}
