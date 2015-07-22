using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceProject_Mac
{
    public class StartGameState : GameState
    {
        private int buttonYPosition = 150;
        private const int BUTTON_Y_DISTANCE = 90;

        private SpriteFont buttonsFont;
        private int buttonIndex;
        private int holdTimer;
        private Sprite buttonsSprite;
        private List<MenuDisplayObject> buttons;
        private Sprite contrastBackDropSprite;

        private MenuDisplayObject selectedButton;
        //private MenuDisplayObject devButton;
        private MenuDisplayObject easyButton;
        private MenuDisplayObject normalButton;
        private MenuDisplayObject hardButton;
        //private MenuDisplayObject hardCoreButton;
        private MenuDisplayObject backButton;

        private List<string> descriptions;

        public StartGameState(Game1 Game, string name) :
            base(Game, name)
        {
        }

        public override void Initialize()
        {
            buttonYPosition = Game1.ScreenSize.Y / 5;

            buttonsSprite = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/buttons"), null);
            buttonsFont = Game.fontManager.GetFont(16);

            contrastBackDropSprite = buttonsSprite.GetSubSprite(new Rectangle(0, 0, 1, 1));

            //devButton = new MenuDisplayObject(Game,
            //        buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
            //        buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
            //        new Vector2(Game1.ScreenSize.X / 4, buttonYPosition),
            //        new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
            //                buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            //devButton.name = "Develop";

            easyButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game1.ScreenSize.X / 4, buttonYPosition + BUTTON_Y_DISTANCE),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            easyButton.name = "Easy";

            normalButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game1.ScreenSize.X / 4, buttonYPosition + BUTTON_Y_DISTANCE * 2),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            normalButton.name = "Normal";

            hardButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game1.ScreenSize.X / 4, buttonYPosition + BUTTON_Y_DISTANCE * 3),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                        buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            hardButton.name = "Hard";

            //hardCoreButton = new MenuDisplayObject(Game,
            //        buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
            //        buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
            //        new Vector2(Game1.ScreenSize.X / 4, buttonYPosition + BUTTON_Y_DISTANCE * 4),
            //        new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
            //                buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            //hardCoreButton.name = "Hardcore";

            backButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game1.ScreenSize.X / 4, buttonYPosition + BUTTON_Y_DISTANCE * 4),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            backButton.name = "Back";

            buttons = new List<MenuDisplayObject>();

            //buttons.Add(devButton);
            buttons.Add(easyButton);
            buttons.Add(normalButton);
            buttons.Add(hardButton);
            //buttons.Add(hardCoreButton);
            buttons.Add(backButton);

            holdTimer = Game.HoldKeyTreshold;

            buttonIndex = 0;

            descriptions = new List<string>();
            //descriptions.Add("Mode for development and testing.");
            descriptions.Add("Easy mode for inexperienced players.\nYou take less damage from enemy fire and you receive more money.");
            descriptions.Add("Regular difficulty.\n The way the game is intended to be played.");
            descriptions.Add("An extra difficulty for players looking for a real challenge.\nYou take more damage from enemy fire and you recive less money.");
            //descriptions.Add("An extra difficulty for players looking for a real \nchallenge.\nAs hard as Hard-difficulty but your life remain constant in both Shooter and Space.\nIf you run out of life you die... ");
            descriptions.Add("Return to main menu");

            base.Initialize();
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ButtonControls(gameTime);
            MouseControls();

            if ((ControlManager.CheckPress(RebindableKeys.Action2) ||
                ControlManager.CheckPress(RebindableKeys.Pause)) && Game.menuBGController.backdropSpeed.Equals(0))
            {
                Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-101, -703), "MainMenuState");
            }
            else if (ControlManager.CheckPress(RebindableKeys.Action1) ||
                ControlManager.CheckKeyPress(Keys.Enter))
            {
                ButtonActions();
            }

            selectedButton = buttons[buttonIndex];

            foreach (MenuDisplayObject button in buttons)
                button.isActive = false;

            selectedButton.isActive = true;

            base.Update(gameTime);
        }

        private void ButtonControls(GameTime gameTime)
        {
            if (ControlManager.CheckPress(RebindableKeys.Down))
            {
                buttonIndex++;
                holdTimer = Game.HoldKeyTreshold;

                PlayHoverSound();
            }

            else if (ControlManager.CheckHold(RebindableKeys.Down))
            {
                holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (holdTimer <= 0)
                {
                    buttonIndex++;
                    holdTimer = Game.ScrollSpeedFast;

                    PlayHoverSound();
                }
            }

            else if (ControlManager.CheckPress(RebindableKeys.Up))
            {
                buttonIndex--;
                holdTimer = Game.HoldKeyTreshold;

                PlayHoverSound();
            }

            else if (ControlManager.CheckHold(RebindableKeys.Up))
            {
                holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (holdTimer <= 0)
                {
                    buttonIndex--;
                    holdTimer = Game.ScrollSpeedFast;

                    PlayHoverSound();
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

                if (ControlManager.IsMouseOverArea(buttonRect))
                {
                    if (ControlManager.IsMouseMoving())
                    {
                        if (buttonIndex != i)
                        {
                            PlayHoverSound();
                        }

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
                switch (buttons[buttonIndex].name.ToLower())
                {
                    case "develop":
                        StatsManager.gameMode = GameMode.Develop;
                        MediaPlayer.Stop();
                        MainMenuState.LoadedOrNewGame = LoadedOrNewGame.New;
                        Game.shipInventoryManager.Initialize();
                        Game.stateManager.StartGame("OverworldState");
                        StatsManager.SetDevelopStats();
                        Game.player.UnlockDevelopHyperSpeed();
                        Game.GameStarted = true;

                        PlaySelectSound();
                        break;

                    case "easy":
                        StatsManager.gameMode = GameMode.Easy;
                        MediaPlayer.Stop();
                        MainMenuState.LoadedOrNewGame = LoadedOrNewGame.New;
                        Game.shipInventoryManager.Initialize();
                        StatsManager.SetEasyStats();
                        Game.stateManager.StartGame("IntroFirstState");
                        Game.GameStarted = true;

                        PlaySelectSound();
                        break;

                    case "normal":
                        StatsManager.gameMode = GameMode.Normal;
                        MediaPlayer.Stop();
                        MainMenuState.LoadedOrNewGame = LoadedOrNewGame.New;
                        KeyboardState current = Keyboard.GetState();
                        Game.shipInventoryManager.Initialize();
                        StatsManager.SetNormalStats();
                        Game.stateManager.StartGame("IntroFirstState");
                        Game.GameStarted = true;

                        PlaySelectSound();
                        break;

                    case "hard":
                        StatsManager.gameMode = GameMode.Hard;
                        MediaPlayer.Stop();
                        MainMenuState.LoadedOrNewGame = LoadedOrNewGame.New;
                        Game.shipInventoryManager.Initialize();
                        StatsManager.SetHardStats();
                        Game.stateManager.StartGame("IntroFirstState");
                        Game.GameStarted = true;

                        PlaySelectSound();
                        break;

                    case "hardcore":
                        StatsManager.gameMode = GameMode.Hardcore;                        
                        MediaPlayer.Stop();
                        MainMenuState.LoadedOrNewGame = LoadedOrNewGame.New;
                        Game.shipInventoryManager.Initialize();
                        StatsManager.SetHardcoreStats();
                        Game.stateManager.StartGame("IntroFirstState");
                        Game.GameStarted = true;

                        PlaySelectSound();
                        break;

                    case "back":
                        Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-101, -703), "MainMenuState");
                        buttonIndex = 0;

                        PlayLowPitchSelectSound();
                        break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float xOffset = -50;
            float yOffset = -50;

            spriteBatch.Draw(Game.menuBGController.menuSpriteSheet.Texture,
                 Game.menuBGController.backdropPosition,
                 Game.menuBGController.menuSpriteSheet.SourceRectangle,
                 Color.White,
                 0.0f,
                 Vector2.Zero,
                 new Vector2(Game1.ScreenSize.X / Game.DefaultResolution.X,
                             Game1.ScreenSize.Y / Game.DefaultResolution.Y),
                 SpriteEffects.None,
                 0.5f);

            if (Game.menuBGController.DisplayButtons)
            {
                spriteBatch.Draw(contrastBackDropSprite.Texture,
                    new Vector2(Game1.ScreenSize.X / 2 - 60, Game1.ScreenSize.Y / 2 - 85),
                    contrastBackDropSprite.SourceRectangle, Color.Black, 0.0f, Vector2.Zero,
                    new Vector2(Game1.ScreenSize.X / 2 + xOffset - 10, 75),
                    SpriteEffects.None, 0.8f);

                foreach (MenuDisplayObject button in buttons)
                {
                    button.Draw(spriteBatch);

                    if (button != selectedButton)
                    {
                        spriteBatch.DrawString(buttonsFont, button.name, button.Position + Game.fontManager.FontOffset, Game.fontManager.FontColor, 0f, buttonsFont.MeasureString(button.name) / 2, 1f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.DrawString(buttonsFont, button.name, button.Position + Game.fontManager.FontOffset, FontManager.FontSelectColor1, 0f, buttonsFont.MeasureString(button.name) / 2, 1f, SpriteEffects.None, 1f);
                    }
                }
            }

            if (Game.menuBGController.DisplayButtons && buttonIndex < descriptions.Count)
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(14), descriptions[buttonIndex],
                    new Vector2(Game1.ScreenSize.X / 2 + xOffset, Game1.ScreenSize.Y / 2 + yOffset),
                    Color.White, 0f,
                    new Vector2(0, ((buttonsFont.MeasureString(descriptions[buttonIndex])).Y) / 2),
                    1f, SpriteEffects.None, 1f);

            }

            base.Draw(spriteBatch);
        }

        private void PlayHoverSound()
        {
            if (Game.menuBGController.DisplayButtons)
            {
                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.MenuHover);
            }
        }

        private void PlaySelectSound()
        {
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.MenuSelect);
        }

        private void PlayLowPitchSelectSound()
        {
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.MenuSelect, 0, -1f);
        }
    }
}
