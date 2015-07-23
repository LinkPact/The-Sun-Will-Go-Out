using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    class VisualOptionsSubState : OptionSubState
    {
        private int holdTimer;
        private int resIndex;

        private MenuDisplayObject resLeftButton;
        private MenuDisplayObject resRightButton;

        public VisualOptionsSubState(Game1 game, Sprite buttonsSprite, OptionsMenuState optionsMenuState, String name) :
            base(game, buttonsSprite, optionsMenuState, name)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            resLeftButton = new MenuDisplayObject(game, buttonsSprite.GetSubSprite(new Rectangle(20, 135, 12, 11)), buttonsSprite.GetSubSprite(new Rectangle(4, 135, 12, 11)),
                new Vector2(Game1.ScreenSize.X - 168, Game1.ScreenSize.Y / 3 - 1 + (2 * 22)),
                new Vector2(6, 5));
            resLeftButton.Initialize();

            resRightButton = new MenuDisplayObject(game, buttonsSprite.GetSubSprite(new Rectangle(4, 135, 12, 11)), buttonsSprite.GetSubSprite(new Rectangle(4, 135, 12, 11)),
                new Vector2(Game1.ScreenSize.X - 45, Game1.ScreenSize.Y / 3 - 2 + (2 * 22)),
                new Vector2(6, 5));
            resRightButton.Initialize();

            resLeftButton.name = "left res";
            resRightButton.name = "right res";

            directionalButtons.Add(resLeftButton);
            directionalButtons.Add(resRightButton);

            holdTimer = game.HoldKeyTreshold;
            cursorIndex = 0;

            menuOptions = new String[5, 2];
            onEnterMenuOptions = new String[5];
        }

        public override void OnDisplay()
        {
            resLeftButton.isVisible = true;
            resRightButton.isVisible = true;

            resIndex = Game1.ResolutionOptions.IndexOf(game.Resolution);

            menuOptions[0, 0] = "Fullscreen";
            if (game.graphics.IsFullScreen)
            {
                menuOptions[0, 1] = "On";
            }
            else
            {
                menuOptions[0, 1] = "Off";
            }

            menuOptions[1, 0] = "Show FPS";
            if (game.ShowFPS)
            {
                menuOptions[1, 1] = "On";
            }
            else
            {
                menuOptions[1, 1] = "Off";
            }

            menuOptions[2, 0] = "Resolution";
            menuOptions[2, 1] = game.Resolution.X.ToString() + " x " + game.Resolution.Y.ToString();

            menuOptions[3, 0] = "Apply Changes";
            menuOptions[3, 1] = "";

            menuOptions[4, 0] = "Back";
            menuOptions[4, 1] = "";

            base.OnDisplay();
        }

        public override void OnEnter()
        {
            cursorIndex = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (cursorIndex == 2)
            {
                if (ControlManager.CheckPress(RebindableKeys.Left))
                {
                    resIndex--;
                }

                else if (ControlManager.CheckPress(RebindableKeys.Right))
                {
                    resIndex++;
                }

            }

            if (resIndex > Game1.ResolutionOptions.Count - 1)
            {
                resIndex = 0;
            }

            else if (resIndex < 0)
            {
                resIndex = Game1.ResolutionOptions.Count - 1;
            }

            menuOptions[2, 1] = Game1.ResolutionOptions[resIndex].X + " x " + Game1.ResolutionOptions[resIndex].Y;

            base.Update(gameTime);
        }

        public override void ButtonActions()
        {
            switch (menuOptions[cursorIndex, 0].ToLower())
            {
			// MAC CHANGE - Restart on fullscreen
				case "fullscreen":
					game.graphics.ToggleFullScreen ();
					optionsMenuState.SaveSettings ();
                    UpdateText();

                    PlaySelectSound();

					if (game.GameStarted)
					{
						PopupHandler.DisplaySelectionMenu("The game needs to be restarted for this to take effect. Do you want to save your game and restart now?", 
							new List<string> { "Save and restart", "Cancel"},
							new List<System.Action>());
					}

					else
					{
						PopupHandler.DisplaySelectionMenu("The game needs to be restarted for this to take effect. Do you want to restart now?",
							new List<string> { "Restart", "Cancel" },
							new List<System.Action>());
					}
                    break;
			//
            
                case "show fps":
                    game.ShowFPS = !game.ShowFPS;
                    UpdateText();

                    PlaySelectSound();
                    break;

                case "resolution":
                    game.ChangeResolution(Game1.ResolutionOptions[resIndex]);
                    optionsMenuState.SaveSettings();
                    if (!Game1.ResolutionOptions[resIndex].Equals(new Vector2(
                        game.settingsFile.GetPropertyAsFloat("visual", "resolutionx", 800),
                        game.settingsFile.GetPropertyAsFloat("visual", "resolutiony", 600))))
                    {
                        if (game.GameStarted)
                        {
                            PopupHandler.DisplaySelectionMenu("The game needs to be restarted for this to take effect. Do you want to save your game and restart now?", 
                                new List<string> { "Save and restart", "Cancel"},
                                new List<System.Action>());
                        }

                        else
                        {
                            PopupHandler.DisplaySelectionMenu("The game needs to be restarted for this to take effect. Do you want to restart now?",
                                new List<string> { "Restart", "Cancel" },
                                new List<System.Action>());
                        }

                        UpdateText();
                    }

                    PlaySelectSound();
                    break;
            
                case "apply changes":
                    if (SettingsHasChanged())
                    {
                        game.ChangeResolution(Game1.ResolutionOptions[resIndex]);
                        optionsMenuState.SaveSettings();

                        if (game.GameStarted)
                        {
                            PopupHandler.DisplaySelectionMenu("The game needs to be restarted for this to take effect. Do you want to save your game and restart now?",
                                new List<string> { "Save and restart", "Cancel" },
                                new List<System.Action>());
                        }

                        else
                        {
                            PopupHandler.DisplaySelectionMenu("The game needs to be restarted for this to take effect. Do you want to restart now?",
                                new List<string> { "Restart", "Cancel" },
                                new List<System.Action>());
                        }

                        UpdateText();
                    }

                    PlaySelectSound();
                    break;

                case "back":
                    optionsMenuState.LeaveSubState();
                    OnLeave();

                    PlayLowPitchSelectSound();
                    break;
            }
        }

        public override void DirectionalButtonActions(String buttonName) 
        {
            switch (buttonName.ToLower())
            {
                case "left res":
                    cursorIndex = 2;
                    resIndex--;
                    break;

                case "right res":
                    cursorIndex = 2;
                    resIndex++;
                    break;

                default:
                    throw new ArgumentException(String.Format("'%s' is not a valid identifier."));
            }

            PlayHoverSound();
        }

        private void UpdateText()
        {
            if (game.graphics.IsFullScreen)
            {
                menuOptions[0, 1] = "On";
            }
            else
            {
                menuOptions[0, 1] = "Off";
            }

            if (game.ShowFPS)
            {
                menuOptions[1, 1] = "On";
            }
            else
            {
                menuOptions[1, 1] = "Off";
            }

            menuOptions[2, 1] = game.Resolution.X.ToString() + " x " + game.Resolution.Y.ToString();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
