﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceProject
{
    public class StartGameState : GameState
    {
        private int buttonYPosition = 120;
        private const int BUTTON_Y_DISTANCE = 90;

        private SpriteFont buttonsFont;
        private int buttonIndex;
        private int holdTimer;
        private Sprite buttonsSprite;
        private List<MenuDisplayObject> buttons;

        private MenuDisplayObject selectedButton;
        private MenuDisplayObject devButton;
        private MenuDisplayObject easyButton;
        private MenuDisplayObject normalButton;
        private MenuDisplayObject hardCoreButton;
        private MenuDisplayObject backButton;

        private List<string> descriptions;


        public StartGameState(Game1 Game, string name) :
            base(Game, name)
        {
        }

        public override void Initialize()
        {
            buttonYPosition = Game.Window.ClientBounds.Height / 5;

            buttonsSprite = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/buttons"), null);
            buttonsFont = Game.fontManager.GetFont(16);

            devButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            devButton.name = "Develop";

            easyButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition + BUTTON_Y_DISTANCE),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            easyButton.name = "Easy";

            normalButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition + BUTTON_Y_DISTANCE * 2),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            normalButton.name = "Normal";

            hardCoreButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition + BUTTON_Y_DISTANCE * 3),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            hardCoreButton.name = "Hardcore";

            backButton = new MenuDisplayObject(Game,
                    buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                    buttonsSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                    new Vector2(Game.Window.ClientBounds.Width / 4, buttonYPosition + BUTTON_Y_DISTANCE * 4),
                    new Vector2(buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                            buttonsSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            backButton.name = "Back";

            buttons = new List<MenuDisplayObject>();

            buttons.Add(devButton);
            buttons.Add(easyButton);
            buttons.Add(normalButton);
            buttons.Add(hardCoreButton);
            buttons.Add(backButton);

            holdTimer = Game.HoldKeyTreshold;

            buttonIndex = 0;

            descriptions = new List<string>();
            descriptions.Add("Mode for development and testing.");
            descriptions.Add("Easy mode for inexperienced players.");
            descriptions.Add("Regular difficulty.");
            descriptions.Add("An extra difficulty for players looking for a real \nchallenge.");
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
                ControlManager.CheckKeypress(Keys.Enter))
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
                switch (buttons[buttonIndex].name.ToLower())
                {
                    case "develop":
                        StatsManager.gameMode = GameMode.develop;
                        MediaPlayer.Stop();
                        MainMenuState.LoadedOrNewGame = LoadedOrNewGame.New;
                        Game.shipInventoryManager.Initialize();
                        Game.stateManager.StartGame("OverworldState");
                        StatsManager.SetDevelopStats();
                        Game.player.UnlockDevelopHyperSpeed();
                        Game.GameStarted = true;
                        break;

                    case "easy":
                        StatsManager.gameMode = GameMode.easy;
                        MediaPlayer.Stop();
                        MainMenuState.LoadedOrNewGame = LoadedOrNewGame.New;
                        Game.shipInventoryManager.Initialize();
                        StatsManager.SetEasyStats();
                        Game.stateManager.StartGame("IntroFirstState");
                        Game.GameStarted = true;
                        break;

                    case "normal":
                        StatsManager.gameMode = GameMode.normal;
                        MediaPlayer.Stop();
                        MainMenuState.LoadedOrNewGame = LoadedOrNewGame.New;
                        KeyboardState current = Keyboard.GetState();
                        Game.shipInventoryManager.Initialize();
                        StatsManager.SetNormalStats();
                        Game.stateManager.StartGame("IntroFirstState");
                        Game.GameStarted = true;
                        break;

                    case "hardcore":
                        StatsManager.gameMode = GameMode.hardcore;                        
                        MediaPlayer.Stop();
                        MainMenuState.LoadedOrNewGame = LoadedOrNewGame.New;
                        Game.shipInventoryManager.Initialize();
                        StatsManager.SetHardcoreStats();
                        Game.stateManager.StartGame("IntroFirstState");
                        Game.GameStarted = true;
                        break;

                    case "back":
                        Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-101, -703), "MainMenuState");
                        buttonIndex = 0;
                        break;
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
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

            if (Game.menuBGController.DisplayButtons && buttonIndex < descriptions.Count)
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(14), descriptions[buttonIndex],
                    buttons[buttonIndex].Position + Game.fontManager.FontOffset + new Vector2(Game.Window.ClientBounds.Width / 2, 0),
                    Color.White, 0f,
                    Game.fontManager.GetFont(14).MeasureString(descriptions[buttonIndex]) / 2,
                    1f, SpriteEffects.None, 1f);
            }

            base.Draw(spriteBatch);
        }
    }
}