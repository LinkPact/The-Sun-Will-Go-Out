﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class Menu : Popup
    {
        private readonly float MenuOptionXDistance = 140f;

        protected List<string> menuOptions;
        protected List<System.Action> menuActions;
        protected int cursorIndex;
        protected int currentIndexMax;

        private int holdTimer;

        public Menu(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(405, 288, 100, 25));
        }

        public virtual void SetMenuOptions(params string[] menuOptions)
        {
            this.menuOptions.Clear();

            foreach (String str in menuOptions)
            {
                this.menuOptions.Add(str);
            }

            currentIndexMax = menuOptions.Length - 1;
        }

        public virtual void SetMenuActions(params System.Action[] menuActions)
        {
            this.menuActions.Clear();

            foreach (System.Action action in menuActions)
            {
                this.menuActions.Add(action);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            menuOptions = new List<string>();
            menuActions = new List<System.Action>();

            holdTimer = game.HoldKeyTreshold;

            usePause = true;
            useOkayButton = false;

            if (!(this is SelectionMenu))
            {
                canvasScale = new Vector2((float)game.Window.ClientBounds.Width / canvas.SourceRectangle.Value.Width, 1);
                canvasPosition = new Vector2(game.camera.cameraPos.X - game.ScreenCenter.X,
                    game.camera.cameraPos.Y - game.ScreenCenter.Y);
                canvasOrigin = Vector2.Zero;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawMenuOptions(spriteBatch);
        }

        protected override void OnPress(RebindableKeys key)
        {
            base.OnPress(key);

            switch (key)
            {
                case RebindableKeys.Action1:
                    {
                        if (menuActions.Count <= 0)
                        {
                            DefaultOnPressActions();
                        }
                        else
                        {
                            InvokeCustomOnPressActions();
                        }
                        break;
                    }

                case RebindableKeys.Pause:
                    {
                        Hide();
                        break;
                    }

                case RebindableKeys.Right:
                    {
                        cursorIndex++;
                        CheckCursorIndex();
                        break;
                    }

                case RebindableKeys.Left:
                    {
                        cursorIndex--;
                        CheckCursorIndex();
                        break;
                    }
            }
        }

        protected virtual void DefaultOnPressActions()
        {
            switch (menuOptions[cursorIndex])
            {
                case "Ship Inventory":
                    game.stateManager.ChangeState("ShipManagerState");
                    Hide();
                    break;

                case "Mission Screen":
                    game.stateManager.ChangeState("MissionScreenState");
                    Hide();
                    break;

                case "Exit Game":
                    if (GameStateManager.currentState.Equals("OverworldState"))
                    {
                        PopupHandler.DisplaySelectionMenu("What do you want to do?",
                            new List<string> { "Save and exit to menu", "Save and exit to desktop", "Exit to menu without saving",
                        "Exit to desktop without saving", "Cancel"},
                        new List<System.Action>());
                    }

                    else if (GameStateManager.currentState.Equals("ShooterState"))
                    {
                        PopupHandler.DisplaySelectionMenu("What do you want to do? You cannot save during combat.",
                            new List<string> { "Exit to menu without saving", "Exit to desktop without saving", "Cancel" },
                            new List<System.Action>());
                    }
                    break;

                case "Options":
                    game.stateManager.ChangeState("OptionsMenuState");
                    game.menuBGController.SetBackdropPosition(new Vector2(-903, -101));
                    Hide();
                    break;

                case "Save":
                    game.Save();
                    break;

                case "Help":
                    game.stateManager.ChangeState("HelpScreenState");
                    Hide();
                    break;

                case "Return To Game":
                    Hide();
                    break;

                case "Restart Level":
                    game.stateManager.shooterState.CurrentLevel.ResetLevel();
                    game.stateManager.shooterState.Initialize();
                    game.stateManager.shooterState.CurrentLevel.Initialize();
                    Hide();
                    break;

                case "Give Up Level":
                    game.stateManager.shooterState.CurrentLevel.GiveUpLevel();
                    Hide();
                    break;

                case "Exit Level":
                    game.stateManager.shooterState.CurrentLevel.LeaveLevel();
                    Hide();
                    break;
            }
        }

        protected virtual void DrawMenuOptions(SpriteBatch spriteBatch)
        {
            Vector2 pos;
            Color color;

            if (GameStateManager.currentState == "OverworldState")
            {
                pos = new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2,
                                  game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2 + 4);
            }
            else
            {
                pos = new Vector2(0, 4);
            }

            for (int i = 0; i < menuOptions.Count; i++)
            {
                color = FontManager.FontColorStatic;

                if (cursorIndex == i)
                {
                    color = Color.LightBlue;
                }

                spriteBatch.DrawString(FontManager.GetFontStatic(14),
                    menuOptions[i],
                    new Vector2(pos.X + (i * MenuOptionXDistance),
                                pos.Y) + FontManager.FontOffsetStatic,
                    color,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    1f);
            }
        }

        protected void CheckCursorIndex()
        {
            if (cursorIndex < 0)
            {
                cursorIndex = currentIndexMax;
            }

            else if (cursorIndex > currentIndexMax)
            {
                cursorIndex = 0;
            }
        }

        private void InvokeCustomOnPressActions()
        {
            menuActions[cursorIndex].Invoke();
            Hide();
        }
    }
}