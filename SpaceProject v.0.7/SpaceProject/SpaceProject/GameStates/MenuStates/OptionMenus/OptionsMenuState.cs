﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Globalization;

namespace SpaceProject
{

    public class OptionsMenuState : GameState
    {
        private int buttonYPosition = 120;
        private const int BUTTON_Y_DISTANCE = 90;

        private SpriteFont buttonsFont;
        private int buttonIndex;
        private int holdTimer;

        private Sprite buttonsSprite;
        private List<MenuDisplayObject> buttons;

        private MenuDisplayObject selectedButton;
        private MenuDisplayObject gameOptionsButton;
        private MenuDisplayObject controlOptionsButton;
        private MenuDisplayObject visualOptionsButton;
        private MenuDisplayObject soundOptionsButton;
        private MenuDisplayObject backButton;

        private List<OptionSubState> subStates;

        private OptionSubState activeOptionState;
        private OptionSubState previousOptionState;

        private GameOptionsSubState gameOptionsSubState;
        private ControlOptionsSubState controlOptionsSubState;
        private SoundOptionsSubState soundOptionsSubState;
        private VisualOptionsSubState visualOptionsSubState;

        private string previousScreen;

        public OptionsMenuState(Game1 Game, string name) :
            base(Game, name)
        {
        }

        public override void Initialize()
        {
            buttonsSprite = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/buttons"), null);
            buttonsFont = Game.fontManager.GetFont(16);

            buttonYPosition = Game.Window.ClientBounds.Height / 5;

            gameOptionsSubState = new GameOptionsSubState(Game, buttonsSprite, this, "Game Options");
            controlOptionsSubState = new ControlOptionsSubState(Game, buttonsSprite, this, "Control Options");
            soundOptionsSubState = new SoundOptionsSubState(Game, buttonsSprite, this, "Sound Options");
            visualOptionsSubState = new VisualOptionsSubState(Game, buttonsSprite, this, "Visual Options");

            gameOptionsSubState.Initialize();
            controlOptionsSubState.Initialize();
            soundOptionsSubState.Initialize();
            visualOptionsSubState.Initialize();

            subStates = new List<OptionSubState>();
            subStates.Add(gameOptionsSubState);
            subStates.Add(controlOptionsSubState);
            subStates.Add(soundOptionsSubState);
            subStates.Add(visualOptionsSubState);

            previousScreen = "";

            gameOptionsButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            gameOptionsButton.name = "Game Options";

            controlOptionsButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition + BUTTON_Y_DISTANCE),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            controlOptionsButton.name = "Control Options";

            visualOptionsButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition + BUTTON_Y_DISTANCE * 2),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            visualOptionsButton.name = "Visual Options";

            soundOptionsButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition + BUTTON_Y_DISTANCE * 3),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            soundOptionsButton.name = "Sound Options";

            backButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition + BUTTON_Y_DISTANCE * 4),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            backButton.name = "Back";


            buttons = new List<MenuDisplayObject>();

            buttons.Add(gameOptionsButton);
            buttons.Add(controlOptionsButton);
            buttons.Add(visualOptionsButton);
            buttons.Add(soundOptionsButton);
            buttons.Add(backButton);

            holdTimer = Game.HoldKeyTreshold;

            buttonIndex = 0;

            base.Initialize();
        }

        public override void OnEnter()
        {
            if (previousScreen == "")
                previousScreen = GameStateManager.previousState;

            activeOptionState = null;
        }

        public override void OnLeave()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            //Game.Window.Title = "Space Project - Options Menu";
            MouseControls();

            if (activeOptionState != null && Game.menuBGController.DisplayButtons)
            {
                activeOptionState.Update(gameTime);
            }
            else if (Game.menuBGController.DisplayButtons)
            {
                ButtonControls(gameTime);

                if ((ControlManager.CheckPress(RebindableKeys.Action2) ||
                    ControlManager.CheckPress(RebindableKeys.Pause)) && Game.menuBGController.backdropSpeed.Equals(0))
                {
                    if (GameStateManager.previousState == "MainMenuState")
                    {
                        Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-101, -703), "MainMenuState");
                    }
                    else
                    {
                        Game.stateManager.ChangeState("OverworldState");
                    }
                }

                else if (ControlManager.CheckPress(RebindableKeys.Action1) ||
                    ControlManager.CheckKeypress(Keys.Enter))
                {
                    ButtonActions();
                }
            }

            selectedButton = buttons[buttonIndex];

            foreach (MenuDisplayObject button in buttons)
                button.isActive = false;

            selectedButton.isActive = true;
        }

        private void ButtonControls(GameTime gameTime)
        {
            if (ControlManager.CheckPress(RebindableKeys.Down))
            {
                buttonIndex++;
                holdTimer = Game.HoldKeyTreshold;
            }

            else if (ControlManager.CheckHold(RebindableKeys.Down))
            {
                holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (holdTimer <= 0)
                {
                    buttonIndex++;
                    holdTimer = Game.ScrollSpeedFast;
                }
            }

            else if (ControlManager.CheckPress(RebindableKeys.Up))
            {
                buttonIndex--;
                holdTimer = Game.HoldKeyTreshold;
            }

            else if (ControlManager.CheckHold(RebindableKeys.Up))
            {
                holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (holdTimer <= 0)
                {
                    buttonIndex--;
                    holdTimer = Game.ScrollSpeedFast;
                }
            }

            if (buttonIndex < 0)
            {
                if (ControlManager.PreviousKeyUp(RebindableKeys.Up))
                    buttonIndex = buttons.Count - 1;
                else
                    buttonIndex = 0;
            }

            else if (buttonIndex > buttons.Count - 1)
            {
                if (ControlManager.PreviousKeyUp(RebindableKeys.Down))
                    buttonIndex = 0;
                else
                    buttonIndex = buttons.Count - 1;
            }
        }

        private void MouseControls()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Rectangle buttonRect = new Rectangle(
                    (int)buttons[i].Position.X - buttons[i].Passive.SourceRectangle.Value.Width / 2,
                    (int)buttons[i].Position.Y - buttons[i].Passive.SourceRectangle.Value.Height / 2,
                    buttons[i].Passive.SourceRectangle.Value.Width,
                    buttons[i].Passive.SourceRectangle.Value.Height);

                if (CollisionDetection.IsPointInsideRectangle(ControlManager.GetMousePosition(), buttonRect))
                {
                    if (ControlManager.GetMousePosition() != ControlManager.GetPreviousMousePosition())
                    {
                        buttonIndex = i;
                    }

                    if (ControlManager.IsLeftMouseButtonClicked())
                    {
                        ButtonActions();
                    }
                }
            }
        }

        private void ButtonActions()
        {
            if (Game.menuBGController.DisplayButtons)
            {
                previousOptionState = activeOptionState;

                switch (buttons[buttonIndex].name.ToLower())
                {
                    case "game options":
                        activeOptionState = gameOptionsSubState;
                        break;

                    case "control options":
                        activeOptionState = controlOptionsSubState;
                        break;

                    case "visual options":
                        activeOptionState = visualOptionsSubState;
                        break;

                    case "sound options":
                        activeOptionState = soundOptionsSubState;
                        break;

                    case "back":
                        if (previousScreen == "MainMenuState")
                        {
                            Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-101, -703), "MainMenuState");
                        }
                        else if (previousScreen == "OverworldState")
                            Game.stateManager.ChangeState("OverworldState");

                        previousScreen = "";
                        buttonIndex = 0;
                        break;
                }

                if (activeOptionState != null)
                {
                    if (previousOptionState != null)
                    {
                        previousOptionState.OnHide();
                        previousOptionState.OnLeave();
                    }

                    activeOptionState.OnDisplay();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game.menuBGController.menuSpriteSheet.Texture,
                             Game.menuBGController.backdropPosition,
                             Game.menuBGController.menuSpriteSheet.SourceRectangle,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width / Game.DefaultResolution.X,
                                         Game.Window.ClientBounds.Height / Game.DefaultResolution.Y),
                             SpriteEffects.None,
                             0.5f);

            if (Game.menuBGController.DisplayButtons)
            {
                foreach (MenuDisplayObject button in buttons)
                {
                    button.Draw(spriteBatch);

                    if (button != selectedButton)
                    {
                        spriteBatch.DrawString(buttonsFont, button.name, button.Position + Game.fontManager.FontOffset, Game.fontManager.FontColor, 0f, buttonsFont.MeasureString(button.name) / 2, 1f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.DrawString(buttonsFont, button.name, button.Position + Game.fontManager.FontOffset, Color.LightSkyBlue, 0f, buttonsFont.MeasureString(button.name) / 2, 1f, SpriteEffects.None, 1f);
                    }
                }
            }

            if (activeOptionState != null && Game.menuBGController.DisplayButtons)
            {
                activeOptionState.Draw(spriteBatch);
            }
        }

        public void DisplaySubState(String subStateName)
        {

        }

        public void EnterSubState(String subStateName)
        {

        }

        public void HideSubState(String subStateName)
        {
            
        }

        public void LeaveSubState()
        {
            activeOptionState = null;
        }

        public void SaveSettings()
        {
            Game.settingsFile.EmptySaveFile("settings.ini");

            SortedDictionary<String, String> keySaveData = new SortedDictionary<string, string>();
            keySaveData.Add("action1", ControlManager.GetKeyName(RebindableKeys.Action1));
            keySaveData.Add("action2", ControlManager.GetKeyName(RebindableKeys.Action2));
            keySaveData.Add("action3", ControlManager.GetKeyName(RebindableKeys.Action3));
            keySaveData.Add("up", ControlManager.GetKeyName(RebindableKeys.Up));
            keySaveData.Add("down", ControlManager.GetKeyName(RebindableKeys.Down));
            keySaveData.Add("left", ControlManager.GetKeyName(RebindableKeys.Left));
            keySaveData.Add("right", ControlManager.GetKeyName(RebindableKeys.Right));
            keySaveData.Add("pause", ControlManager.GetKeyName(RebindableKeys.Pause));

            Game.settingsFile.Save("settings.ini", "keys", keySaveData);

            SortedDictionary<String, String> visualSaveData = new SortedDictionary<string, string>();
            visualSaveData.Add("fullscreen", Game.graphics.IsFullScreen.ToString());
            visualSaveData.Add("showfps", Game.ShowFPS.ToString());
            visualSaveData.Add("resolutionx", ((int)Game.Resolution.X).ToString());
            visualSaveData.Add("resolutiony", ((int)Game.Resolution.Y).ToString());

            Game.settingsFile.Save("settings.ini", "visual", visualSaveData);

            SortedDictionary<String, String> soundSaveData = new SortedDictionary<string, string>();
            soundSaveData.Add("mutemusic", Game.musicManager.IsMusicMuted().ToString());
            soundSaveData.Add("musicvolume", Convert.ToString(Game.musicManager.GetMusicVolume(), CultureInfo.InvariantCulture));
            soundSaveData.Add("muteSound", Game.soundEffectsManager.isSoundMuted().ToString());
            soundSaveData.Add("soundvolume", Convert.ToString(Game.soundEffectsManager.GetSoundVolume(), CultureInfo.InvariantCulture));

            Game.settingsFile.Save("settings.ini", "sound", soundSaveData);

            SortedDictionary<String, String> gameOptionsSaveData = new SortedDictionary<string, string>();
            gameOptionsSaveData.Add("tutorials", Game.tutorialManager.TutorialsUsed.ToString());

            Game.settingsFile.Save("settings.ini", "game options", gameOptionsSaveData);
        }
    }
}
