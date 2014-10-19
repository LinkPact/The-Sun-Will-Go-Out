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
            fontHeading = Game.fontManager.GetFont(28);

            buttonSprite = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/buttons"), null);

            CreateButtons();

            backdrop = Game.menuBGController.menuSpriteSheet.GetSubSprite(null);

            holdTimer = Game.HoldKeyTreshold;

            ActiveSong = Music.MainMenu2;

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

           mapCreatorButton.name = "Create Level";
           //mapCreatorButton.name = "Credits";

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
            //Game.Window.Title = ("The Sun Will Go Out - " + "Main Menu") + ControlManager.IsGamepadConnected.ToString();

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
                ControlManager.CheckKeypress(Keys.Enter))
            {
                ButtonActions();
            }

            // TODO: REMOVE FOR RELEASE VERSION
            if (ControlManager.CheckKeypress(Keys.M))
            {
                Game.stateManager.ChangeState("LevelTesterState");
            }
            //
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
                        /*MediaPlayer.Stop();
                        loadedOrNewGame = LoadedOrNewGame.New;

                        KeyboardState current = Keyboard.GetState();

                        //TODO Remove!!!
                        if (current.IsKeyDown(Keys.H))
                        {
                            Game.statsManager.SetHardcoreStats();
                        }

                        Game.stateManager.ChangeState("IntroFirstState");*/
                        Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-201, -101), "StartGameState");

                        PlaySelectSound();
                        break;
                    
                    //case 1:
                    //    MediaPlayer.Stop();
                    //    loadedOrNewGame = LoadedOrNewGame.New;
                    //    Game.statsManager.SetCasualStats();
                    //    Game.stateManager.ChangeState("IntroFirstState");
                    //    break;

                    // LOADGAMELOGIC
                    case 1:
                        if (SaveFile.CheckIfFileExists("save.ini"))
                        {
                            Game.stateManager.StartGame("OverworldState");
                            Game.Load();
                            Game.GameStarted = true;
                            MediaPlayer.Stop();
                            loadedOrNewGame = LoadedOrNewGame.Loaded;

                            PlaySelectSound();
                        }
                        else
                            Game.messageBox.DisplayMessage("No save-file!", false);
                        break;

                    case 2:
                        Game.menuBGController.SetPreferredBackdropPosition(new Vector2(-903, -101), "OptionsMenuState");

                        PlaySelectSound();
                        break;

                    case 3:
                        Game.stateManager.ChangeState("MapCreatorState");

                        PlaySelectSound();
                        //Game.stateManager.ChangeState("CreditState");
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
                             0.5f);

            if (Game.menuBGController.DisplayButtons)
            {
                spriteBatch.DrawString(fontHeading, "The sun will go out", new Vector2(Game.Window.ClientBounds.Width / 2,
                    Game.Window.ClientBounds.Height / 5) + Game.fontManager.FontOffset,
                    Game.fontManager.FontColor, 0f, fontHeading.MeasureString("The sun will go out") / 2,
                    1f, SpriteEffects.None, 1f);

                spriteBatch.DrawString(fontButtons, "Alpha Release 2", new Vector2(10,
                    5) + Game.fontManager.FontOffset,
                    Game.fontManager.FontColor, 0f, Vector2.Zero,
                    1f, SpriteEffects.None, 1f);

                
                foreach (MenuDisplayObject button in buttons)
                {
                    button.Draw(spriteBatch);

                    if (button != activeButton)
                    {
                        spriteBatch.DrawString(fontButtons, button.name, button.Position + Game.fontManager.FontOffset, Game.fontManager.FontColor, 0f, fontButtons.MeasureString(button.name) / 2, 1f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.DrawString(fontButtons, button.name, button.Position + Game.fontManager.FontOffset, Color.LightSkyBlue, 0f, fontButtons.MeasureString(button.name) / 2, 1f, SpriteEffects.None, 1f);
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
