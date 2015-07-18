using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    class OptionSubState
    {
        protected Sprite buttonsSprite;
        
        private Sprite contrastBackDropSprite;
        protected bool drawContrastBackdrop;

        protected Game1 game;
        protected OptionsMenuState optionsMenuState;
        public String name;

        protected int cursorIndex = 0;
        private int holdTimer;

        protected SpriteFont menuOptionFont;
        private Vector2 fontOffset;
        protected String[,] menuOptions;
        protected String[] onEnterMenuOptions;

        protected List<MenuDisplayObject> directionalButtons;

        protected OptionSubState(Game1 game, Sprite buttonsSprite, OptionsMenuState optionsMenuState, String name)
        {
            this.game = game;
            this.buttonsSprite = buttonsSprite;
            this.optionsMenuState = optionsMenuState;
            this.name = name;
        }

        public virtual void Initialize()
        {
            contrastBackDropSprite = buttonsSprite.GetSubSprite(new Rectangle(0, 0, 1, 1));
            drawContrastBackdrop = false;
            holdTimer = game.HoldKeyTreshold;
            menuOptionFont = game.fontManager.GetFont(14);
            fontOffset = game.fontManager.FontOffset;
            directionalButtons = new List<MenuDisplayObject>();
        }

        public virtual void OnDisplay()
        {
            cursorIndex = 0;
            drawContrastBackdrop = true;

            for (var i = 0; i < menuOptions.Length / 2; i++)
            {
                if (!menuOptions[i, 1].Equals(""))
                {
                    onEnterMenuOptions[i] = menuOptions[i, 1];
                }
            }
        }

        public virtual void OnEnter()
        {
            cursorIndex = 0;
        }

        public virtual void OnLeave()
        {
            if (SettingsHasChanged())
            {
                optionsMenuState.SaveSettings();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            ButtonControls(gameTime);
            MouseControls();
        }

        public virtual void ButtonActions() 
        {
            
        }

        public virtual void DirectionalButtonActions(String buttonName) { }

        private void ButtonControls(GameTime gameTime)
        {
            if (!ControlManager.GamepadReady)
            {
                if (ControlManager.CheckKeyPress(Keys.Down))
                {
                    cursorIndex++;
                    holdTimer = game.HoldKeyTreshold;

                    PlayHoverSound();
                }

                else if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.Down))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorIndex++;
                        holdTimer = game.ScrollSpeedFast;

                        PlayHoverSound();
                    }
                }

                else if (ControlManager.CheckKeyPress(Keys.Up))
                {
                    cursorIndex--;
                    holdTimer = game.HoldKeyTreshold;

                    PlayHoverSound();
                }

                else if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.Up))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorIndex--;
                        holdTimer = game.ScrollSpeedFast;

                        PlayHoverSound();
                    }
                }

                if (cursorIndex < 0)
                {
                    if (ControlManager.PreviousKeyboardState.IsKeyUp(Keys.Up))
                        cursorIndex = menuOptions.Length / 2 - 1;
                    else
                        cursorIndex = 0;
                }

                else if (cursorIndex > menuOptions.Length / 2 - 1)
                {
                    if (ControlManager.PreviousKeyboardState.IsKeyUp(Keys.Down))
                        cursorIndex = 0;
                    else
                        cursorIndex = menuOptions.Length / 2 - 1;
                }
            }

            else
            {
                if (ControlManager.CheckButtonPress(Buttons.DPadDown) ||
                    ControlManager.CheckButtonPress(Buttons.LeftThumbstickDown))
                {
                    cursorIndex++;
                    holdTimer = game.HoldKeyTreshold;

                    PlayHoverSound();
                }

                else if (ControlManager.CurrentGamepadState.IsButtonDown(Buttons.DPadDown) ||
                    ControlManager.CurrentGamepadState.IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorIndex++;
                        holdTimer = game.ScrollSpeedFast;

                        PlayHoverSound();
                    }
                }

                else if (ControlManager.CheckButtonPress(Buttons.DPadUp) ||
                    ControlManager.CheckButtonPress(Buttons.LeftThumbstickUp))
                {
                    cursorIndex--;
                    holdTimer = game.HoldKeyTreshold;

                    PlayHoverSound();
                }

                else if (ControlManager.CurrentGamepadState.IsButtonDown(Buttons.DPadUp) ||
                    ControlManager.CurrentGamepadState.IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorIndex--;
                        holdTimer = game.ScrollSpeedFast;

                        PlayHoverSound();
                    }
                }

                if (cursorIndex < 0)
                {
                    if (ControlManager.CurrentGamepadState.IsButtonUp(Buttons.DPadUp))
                        cursorIndex = menuOptions.Length / 2 - 1;
                    else if (ControlManager.CurrentGamepadState.IsButtonUp(Buttons.LeftThumbstickUp))
                        cursorIndex = menuOptions.Length / 2 - 1;
                    else
                        cursorIndex = 0;
                }

                else if (cursorIndex > menuOptions.Length / 2 - 1)
                {
                    if (ControlManager.CurrentGamepadState.IsButtonUp(Buttons.DPadDown))
                        cursorIndex = 0;
                    else if (ControlManager.CurrentGamepadState.IsButtonUp(Buttons.LeftThumbstickUp))
                        cursorIndex = 0;
                    else
                        cursorIndex = menuOptions.Length / 2 - 1;
                }
            }


            if (ControlManager.CheckPress(RebindableKeys.Action2) ||
                ControlManager.CheckPress(RebindableKeys.Pause))
            {
                optionsMenuState.LeaveSubState();
                OnLeave();
            }

            else if (ControlManager.CheckPress(RebindableKeys.Action1) ||
                ControlManager.CheckKeyPress(Keys.Enter))
            {
                ButtonActions();
            }
        }

        private void MouseControls()
        {
            for (int i = 0; i < menuOptions.Length / 2; i++)
            {
                if (ControlManager.IsMouseOverText(menuOptionFont, menuOptions[i, 0],
                    new Vector2((game.Window.ClientBounds.Width / 9) * 4 + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 0]).X / 2,
                        game.Window.ClientBounds.Height / 3 + (i * 23)) + fontOffset))
                {
                    if (cursorIndex != i)
                    {
                        PlayHoverSound();
                    }

                    if (ControlManager.IsLeftMouseButtonClicked())
                    {
                        ButtonActions();
                    }

                    if (ControlManager.IsMouseMoving())
                    {
                        cursorIndex = i;
                    }

                    continue;
                }

                if (!menuOptions[i, 1].Equals(""))
                {
                    if (ControlManager.IsMouseOverText(menuOptionFont, menuOptions[i, 1],
                        new Vector2(game.Window.ClientBounds.Width - 150 + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 1]).X / 2,
                            game.Window.ClientBounds.Height / 3 + (i * 23)) + fontOffset))
                    {
                        if (cursorIndex != i)
                        {
                            PlayHoverSound();
                        }

                        if (ControlManager.IsLeftMouseButtonClicked())
                        {
                            ButtonActions();
                        }
                        if (ControlManager.IsMouseMoving())
                        {
                            cursorIndex = i;
                        }
                    }
                }
            }

            for (int i = 0; i < directionalButtons.Count; i++)
            {
                Rectangle dirButtonRect = new Rectangle(
                    (int)directionalButtons[i].Position.X - directionalButtons[i].Passive.SourceRectangle.Value.Width / 2,
                    (int)directionalButtons[i].Position.Y - directionalButtons[i].Passive.SourceRectangle.Value.Height / 2,
                    directionalButtons[i].Passive.SourceRectangle.Value.Width + 10,
                    directionalButtons[i].Passive.SourceRectangle.Value.Height);

                if (ControlManager.IsMouseOverArea(dirButtonRect) &&
                    ControlManager.IsLeftMouseButtonClicked())
                {
                    DirectionalButtonActions(directionalButtons[i].name);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (drawContrastBackdrop)
            {
                int length = menuOptions.Length / 2;
                if (length % 2 != 0)
                {
                    length++;
                }

                spriteBatch.Draw(contrastBackDropSprite.Texture, new Vector2((game.Window.ClientBounds.Width / 9) * 4 - 10, game.Window.ClientBounds.Height / 3 - 15),
                    contrastBackDropSprite.SourceRectangle, Color.Black, 0.0f, Vector2.Zero,
                    new Vector2(game.Window.ClientBounds.Width - (game.Window.ClientBounds.Width / 9) * 4 - 20, 15 + (20 *length)),
                    SpriteEffects.None, 0.8f);
            }

            for (int i = 0; i < menuOptions.Length / 2; i++)
            {
                if (i == cursorIndex)
                {
                    spriteBatch.DrawString(game.fontManager.GetFont(14), menuOptions[i, 0],
                        new Vector2((game.Window.ClientBounds.Width / 9) * 4  + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 0]).X / 2,
                                    game.Window.ClientBounds.Height / 3 + (i * 23)) + game.fontManager.FontOffset,
                        FontManager.FontSelectColor1, 0f,
                        game.fontManager.GetFont(14).MeasureString(menuOptions[i, 0]) / 2,
                        1f, SpriteEffects.None, 1f);


                    spriteBatch.DrawString(game.fontManager.GetFont(14), menuOptions[i, 1],
                        new Vector2(game.Window.ClientBounds.Width - 150 + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 1]).X / 2,
                                    game.Window.ClientBounds.Height / 3 + (i * 23)) + game.fontManager.FontOffset,
                        FontManager.FontSelectColor1, 0f,
                        game.fontManager.GetFont(14).MeasureString(menuOptions[i, 1]) / 2,
                        1f, SpriteEffects.None, 1f);

                }
                else
                {
                    spriteBatch.DrawString(game.fontManager.GetFont(14), menuOptions[i, 0],
                        new Vector2((game.Window.ClientBounds.Width / 9) * 4 + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 0]).X / 2,
                                    game.Window.ClientBounds.Height / 3 + (i * 23)) + game.fontManager.FontOffset,
                        game.fontManager.FontColor, 0f,
                        game.fontManager.GetFont(14).MeasureString(menuOptions[i, 0]) / 2,
                        1f, SpriteEffects.None, 1f);

                    spriteBatch.DrawString(game.fontManager.GetFont(14), menuOptions[i, 1],
                        new Vector2(game.Window.ClientBounds.Width - 150 + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 1]).X / 2,
                                    game.Window.ClientBounds.Height / 3 + (i * 23)) + game.fontManager.FontOffset,
                        game.fontManager.FontColor, 0f,
                        game.fontManager.GetFont(14).MeasureString(menuOptions[i, 1]) / 2,
                        1f, SpriteEffects.None, 1f);
                }

            }

            for (int i = 0; i < directionalButtons.Count; i++)
            {
                directionalButtons[i].Draw(spriteBatch);
            }
        }

        protected void PlayHoverSound()
        {
            if (game.menuBGController.DisplayButtons)
            {
                game.soundEffectsManager.PlaySoundEffect(SoundEffects.MenuHover);
            }
        }

        protected void PlaySelectSound()
        {
            game.soundEffectsManager.PlaySoundEffect(SoundEffects.MenuSelect);
        }

        protected void PlayLowPitchSelectSound()
        {
            game.soundEffectsManager.PlaySoundEffect(SoundEffects.MenuSelect, 0, -1f);
        }

        protected bool SettingsHasChanged()
        {
            for (int i = 0; i < onEnterMenuOptions.Length; i++)
            {
                if (menuOptions[i, 1] != null && onEnterMenuOptions[i] != null &&
                    !onEnterMenuOptions[i].ToLower().Equals(menuOptions[i, 1].ToLower()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
