using Microsoft.Xna.Framework;
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
        private readonly float TextLayerDepth = 1f;

        protected List<string> menuOptions;
        protected List<System.Action> menuActions;
        protected int cursorIndex;
        protected int currentIndexMax;

        public Menu(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(0, 56, 269, 184));
        }

        public override void Initialize()
        {
            base.Initialize();

            menuOptions = new List<string>();
            menuActions = new List<System.Action>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public virtual void SetMenuOptions(params string[] menuOptions)
        {
            this.menuOptions.Clear();

            foreach (String str in menuOptions)
            {
                this.menuOptions.Add(str);
            }

            currentIndexMax = menuOptions.Length;
        }

        public virtual void SetMenuActions(params System.Action[] menuActions)
        {
            this.menuActions.Clear();

            foreach (System.Action action in menuActions)
            {
                this.menuActions.Add(action);
            }
        }

        public override void OnPress()
        {
            base.OnPress();

            if (menuActions.Count <= 0)
            {
                DefaultOnPressActions();
            }
            else
            {
                InvokeCustomOnPressActions();
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

                case "Missions Screen":
                    game.stateManager.ChangeState("MissionScreenState");
                    Hide();
                    break;

                case "Exit Game":
                    if (GameStateManager.currentState.Equals("OverworldState"))
                    {
                        game.messageBox.DisplaySelectionMenu("What do you want to do?",
                            new List<string> { "Save and exit to menu", "Save and exit to desktop", "Exit to menu without saving",
                        "Exit to desktop without saving", "Cancel"});
                    }

                    else if (GameStateManager.currentState.Equals("ShooterState"))
                    {
                        game.messageBox.DisplaySelectionMenu("What do you want to do? You cannot save during combat.",
                            new List<string> { "Exit to menu without saving", "Exit to desktop without saving", "Cancel" });
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

        private void InvokeCustomOnPressActions()
        {
            menuActions[cursorIndex].Invoke();
            textBuffer.Remove(textBuffer[0]);
            Hide();
            menuActions.Clear();
        }
    }
}
