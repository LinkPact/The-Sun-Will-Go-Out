using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceProject
{
    public enum LoadedOrNewGame
    {
        New,
        Loaded
    }

    public class MainMenuState: GameState
    {
        private static LoadedOrNewGame loadedOrNewGame;
        public static LoadedOrNewGame LoadedOrNewGame { get { return loadedOrNewGame; } set { loadedOrNewGame = value; } }

        public Sprite spriteSheet;
        private SpriteFont fontButtons;
        private SpriteFont fontHeading;

        private Sprite buttonSprite;

        private int buttonIndex;

        private MenuDisplayObject activeButton;

        private List<MenuDisplayObject> buttons;
        private MenuDisplayObject newGameButton;
        private MenuDisplayObject casualNewGameButton;
        private MenuDisplayObject optionsButton;
        private MenuDisplayObject mapCreatorButton;
        private MenuDisplayObject exitGameButton;

        private Sprite backdrop;

        private int holdTimer;

        public MainMenuState(Game1 Game, string name) :
            base(Game, name)
        {
        }

        public override void Initialize()
        {
            fontButtons = Game.fontManager.GetFont(16);
            fontHeading = Game.fontManager.GetFont(36);

            buttonSprite = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/buttons"), null);

            CreateButtons();

            backdrop = Game.menuBGController.menuSpriteSheet.GetSubSprite(null);

            holdTimer = Game.HoldKeyTreshold;

            ActiveSong = Music.MainMenu;

            base.Initialize();
        }

        private void CreateButtons()
        {
            buttons = new List<MenuDisplayObject>();

            newGameButton = new MenuDisplayObject(this.Game,
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                                                  new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 3),
                                                  new Vector2(buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));
            newGameButton.name = "New Game";

            buttons.Add(newGameButton);

            casualNewGameButton = new MenuDisplayObject(this.Game,
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                                                  new Vector2(Game.Window.ClientBounds.Width / 2,
                                                      Game.Window.ClientBounds.Height / 3 + 80),
                                                  new Vector2(buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));

            casualNewGameButton.name = "Continue";

            buttons.Add(casualNewGameButton);

            optionsButton = new MenuDisplayObject(this.Game,
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                                                  new Vector2(Game.Window.ClientBounds.Width / 2,
                                                      Game.Window.ClientBounds.Height / 3 + 160),
                                                  new Vector2(buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));

            optionsButton.name = "Options";

            buttons.Add(optionsButton);

            mapCreatorButton = new MenuDisplayObject(this.Game,
                                      buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                                      buttonSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                                      new Vector2(Game.Window.ClientBounds.Width / 2,
                                          Game.Window.ClientBounds.Height / 3 + 240),
                                      new Vector2(buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                                      buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));

           //mapCreatorButton.name = "Create Level";
           mapCreatorButton.name = "Credits";

            buttons.Add(mapCreatorButton);

            exitGameButton = new MenuDisplayObject(this.Game,
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)),
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 65, 256, 65)),
                                                  new Vector2(Game.Window.ClientBounds.Width / 2,
                                                      Game.Window.ClientBounds.Height / 3 + 320),
                                                  new Vector2(buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Width / 2,
                                                  buttonSprite.GetSubSprite(new Rectangle(0, 0, 256, 65)).Height / 2));

            exitGameButton.name = "Exit Game";

            buttons.Add(exitGameButton);
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnLeave()
        {
        }

        public override void Update(GameTime gameTime)
        {
            ButtonControls(gameTime);
            MouseControls();
        }

        private void ButtonControls(GameTime gameTime)
        {
            if (ControlManager.CheckPress(RebindableKeys.Up))
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
                    holdTimer = Game.ScrollSpeedSlow;

                    PlayHoverSound();
                }
            }

            else if (ControlManager.CheckPress(RebindableKeys.Down))
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
                    holdTimer = Game.ScrollSpeedSlow;

                    PlayHoverSound();
                }
            }

            if (buttonIndex > buttons.Count - 1)
            {
                if (ControlManager.PreviousKeyUp(RebindableKeys.Down))
                    buttonIndex = 0;
                else
                    buttonIndex = buttons.Count - 1;
            }

            else if (buttonIndex < 0)
            {
                if (ControlManager.PreviousKeyUp(RebindableKeys.Up))
                    buttonIndex = buttons.Count - 1;
                else
                    buttonIndex = 0;
            }

            activeButton = buttons[buttonIndex];

            foreach (MenuDisplayObject button in buttons)
                button.isActive = false;

            activeButton.isActive = true;

            if (ControlManager.CheckPress(RebindableKeys.Action1) ||
                ControlManager.CheckKeyPress(Keys.Enter))
            {
                ButtonActions();
            }

            // TODO: REMOVE FOR RELEASE VERSION
            if (ControlManager.CheckKeyPress(Keys.M))
            {
                Game.stateManager.ChangeState("LevelTesterState");
            }
            
            if (ControlManager.CheckKeyPress(Keys.C))
            {
                Game.stateManager.ChangeState("CampaignState");
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
                switch (buttonIndex)
                {
                    case 0:
                        if (SaveFile.CheckIfFileExists(Game1.SaveFilePath, "save.ini"))
                        {
                            PopupHandler.DisplaySelectionMenu("If you start a new game, your previously saved game will be lost. Is this okay?",
                                new List<string>() { "Yes", "No"}, 
                                new List<System.Action>()
                                {
                                    delegate 
                                    {
                                        Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-201, -101), "StartGameState");
                                        PlaySelectSound();
                                    },
                                    delegate { }
                                });
                        }
                        else
                        {
                            Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-201, -101), "StartGameState");

                            PlaySelectSound();
                        }
                        break;

                    // LOADGAMELOGIC
                    case 1:
                        if (SaveFile.CheckIfFileExists(Game1.SaveFilePath, "save.ini"))
                        {
                            Game.stateManager.StartGame("OverworldState");
                            Game.Load();
                            Game.GameStarted = true;
                            loadedOrNewGame = LoadedOrNewGame.Loaded;

                            PlaySelectSound();
                        }
                        else
                            PopupHandler.DisplayMessage("No save-file!");
                        break;

                    case 2:
                        Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-903, -101), "OptionsMenuState");

                        PlaySelectSound();
                        break;

                    case 3:
                        //Game.stateManager.ChangeState("MapCreatorState");

                        PlaySelectSound();
                        Game.stateManager.ChangeState("CreditState");
                        break;

                    case 4:
                        MediaPlayer.Stop();
                        Game.Exit();
                        break;

                }
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Draw(Game.menuBGController.menuSpriteSheet.Texture,
                             Game.menuBGController.backdropPosition,
                             Game.menuBGController.menuSpriteSheet.SourceRectangle,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width / Game.DefaultResolution.X,
                                         Game.Window.ClientBounds.Height / Game.DefaultResolution.Y),
                             SpriteEffects.None,
                             0.1f);

            if (Game.menuBGController.DisplayButtons)
            {
                spriteBatch.DrawString(fontHeading, "The sun will go out", new Vector2(Game.Window.ClientBounds.Width / 2,
                    Game.Window.ClientBounds.Height / 5) + Game.fontManager.FontOffset,
                    Game.fontManager.FontColor, 0f, fontHeading.MeasureString("The sun will go out") / 2,
                    1f, SpriteEffects.None, 1f);

                spriteBatch.DrawString(fontButtons, "Version: 1.0", new Vector2(10,
                    5) + Game.fontManager.FontOffset,
                    Game.fontManager.FontColor, 0f, Vector2.Zero,
                    1f, SpriteEffects.None, 1f);

                
                foreach (MenuDisplayObject button in buttons)
                {
                    button.Draw(spriteBatch);

                    if (button != activeButton)
                    {
                        spriteBatch.DrawString(fontButtons, button.name, button.Position + Game.fontManager.FontOffset, Game.fontManager.FontColor, 0f, fontButtons.MeasureString(button.name) / 2, 1f, SpriteEffects.None, 0.95f);
                    }
                    else
                    {
                        spriteBatch.DrawString(fontButtons, button.name, button.Position + Game.fontManager.FontOffset, FontManager.FontSelectColor1, 0f, fontButtons.MeasureString(button.name) / 2, 1f, SpriteEffects.None, 0.95f);
                    }
                }
            }
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
    }
}
