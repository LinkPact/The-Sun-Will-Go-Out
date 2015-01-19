using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceProject
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region declaration
        //public static bool OkayToExit = false;
        public SaveFile settingsFile;

        public Sprite spriteSheetVerticalShooter;
        public Sprite spriteSheetOverworld;
        private Sprite messageBoxSpriteSheet;

        public GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public PlayerOverWorld player;

        public MenuBackdropController menuBGController;

        public MusicManager musicManager;
        public SoundEffectsManager soundEffectsManager;
        public FontManager fontManager;
        public GameStateManager stateManager;
        public StatsManager statsManager;
        public ShipInventoryManager shipInventoryManager;
        public MissionManager missionManager;
        public TutorialManager tutorialManager;
        public ShopManager shopManager;

        public MessageBox messageBox;
        public HelperBox helper;
        private BeaconMenu beaconMenu;
        public BeaconMenu GetBeaconMenu { get { return beaconMenu; } private set { ; } }

        private bool saveOnEnterOverworld;
        public bool SaveOnEnterOverworld { get { return saveOnEnterOverworld; } set { saveOnEnterOverworld = value; } }

        public SaveFile saveFile;

        public static bool Paused = false;

        public Vector2 ScreenCenter { get { return new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2); } }

        //Cursor
        private const int HOLD_KEY_TRESHOLD = 200;
        public int HoldKeyTreshold { get { return HOLD_KEY_TRESHOLD; } }
        private const int SCROLL_SPEED_FAST = 60; //Lower = faster
        private const int SCROLL_SPEED_SLOW = 90;
        public int ScrollSpeedFast { get { return SCROLL_SPEED_FAST; } }
        public int ScrollSpeedSlow { get { return SCROLL_SPEED_SLOW; } }

        public Camera camera;

        //fps
        private float fps;
        private float fpsTimer;
        private bool showFPS;
        public bool ShowFPS { get { return showFPS; } set { showFPS = value; } }

        public static List<Vector2> ResolutionOptions;

        private Vector2 resolution;
 
        public Vector2 Resolution { get { return resolution; } }
        public Vector2 DefaultResolution { get { return new Vector2(800, 600); } }

        public bool GameStarted;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            SetAvailableResolutions();

            GameStarted = false;

            settingsFile = new SaveFile(this);
            settingsFile.Load("settings.ini");

            TextToSpeech.TTSMode = (TextToSpeechMode)settingsFile.GetPropertyAsInt("sound", "text-to-speech", 2);

            resolution = new Vector2(settingsFile.GetPropertyAsFloat("visual", "resolutionx", 800),
                                     settingsFile.GetPropertyAsFloat("visual", "resolutiony", 600));

            showFPS = settingsFile.GetPropertyAsBool("visual", "showfps", false);

            graphics.PreferredBackBufferWidth = (int)resolution.X;
            graphics.PreferredBackBufferHeight = (int)resolution.Y;
            graphics.IsFullScreen = settingsFile.GetPropertyAsBool("visual", "fullscreen", false);
            graphics.ApplyChanges();

            IsMouseVisible = true;
            IsFixedTimeStep = false;

            menuBGController = new MenuBackdropController(this);
            menuBGController.Initialize();

            musicManager = new MusicManager(this);
            musicManager.Initialize();

            soundEffectsManager = new SoundEffectsManager(this);
            soundEffectsManager.Initialize();

            fontManager = new FontManager(this);
            fontManager.Initialize();

            ControlManager.LoadControls(settingsFile);

            spriteSheetOverworld = new Sprite(Content.Load<Texture2D>("Overworld-Sprites/smallObjectSpriteSheet"));
            spriteSheetVerticalShooter = new Sprite(Content.Load<Texture2D>("Vertical-Sprites/shooterSheet"));
            messageBoxSpriteSheet = new Sprite(Content.Load<Texture2D>("Overworld-Sprites/messageBoxSpriteSheet"));

            shipInventoryManager = new ShipInventoryManager(this);
            shipInventoryManager.Initialize();

            statsManager = new StatsManager(this);
            statsManager.Initialize();

            player = new PlayerOverWorld(this, spriteSheetOverworld);
            player.Initialize();

            beaconMenu = new BeaconMenu(this, spriteSheetOverworld);
            beaconMenu.Initialize();

            stateManager = new GameStateManager(this);
            stateManager.Initialize();

            missionManager = new MissionManager(this);
            missionManager.Initialize();

            tutorialManager = new TutorialManager(this);
            tutorialManager.Initialize();
            tutorialManager.TutorialsUsed = settingsFile.GetPropertyAsBool("game options", "tutorials", true);

            shopManager = new ShopManager();

            saveFile = new SaveFile(this);

            messageBox = new MessageBox(this, messageBoxSpriteSheet);
            messageBox.Initialize();

            helper = new HelperBox(this);

            ShopManager.SetShopUpdateTime(ShopManager.PRESET_SHOPTIME);

            TextToSpeech.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime) 
        {
            Window.Title = "The Sun Will Go Out";

            if (IsActive)
            {
                ControlManager.Update(gameTime);

                //Toggles fullscreen on/off
                if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.LeftAlt) && ControlManager.CheckKeypress(Keys.F))
                {
                    graphics.ToggleFullScreen();
                    graphics.ApplyChanges();
                }

                //Checks if the player should be used
                if (GameStateManager.currentState == "OverworldState" || GameStateManager.currentState == "System1State"
                    || GameStateManager.currentState == "System2State" || GameStateManager.currentState == "System3State")
                    player.IsUsed = true;
                else
                    player.IsUsed = false;

                if (!Paused)
                {
                    if (player.IsUsed)
                        player.Update(gameTime);
                    
                    stateManager.Update(gameTime);
                    missionManager.Update(gameTime);
                    tutorialManager.Update(gameTime);
                    shopManager.Update(gameTime);
                }

                soundEffectsManager.Update(gameTime);

                messageBox.Update(gameTime);
                helper.Update(gameTime);
                beaconMenu.Update(gameTime);

                fpsTimer -= gameTime.ElapsedGameTime.Milliseconds;
                if (fpsTimer <= 0)
                {
                    fps = (float)Math.Round((1 / gameTime.ElapsedGameTime.TotalSeconds), 0);
                    fpsTimer = 250;
                }

                if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.LeftAlt) &&
                    ControlManager.CheckKeypress(Keys.M))
                {
                    musicManager.SwitchMusicMuted();
                }

                menuBGController.Update(gameTime);

                if (saveOnEnterOverworld)
                {
                    saveOnEnterOverworld = false;
                    Save();
                }

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (GameStateManager.currentState == "OverworldState")
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.GetTransformation());
            }
            else if (GameStateManager.currentState == "ShooterState")
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            }
            else if (GameStateManager.currentState == "IntroSecondState")
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            }
            else
            {
                spriteBatch.Begin();
            }

            stateManager.Draw(spriteBatch);
            missionManager.Draw(spriteBatch);
            tutorialManager.Draw(spriteBatch);

            if (messageBox.MessageState != MessageState.Invisible)
                messageBox.Draw(spriteBatch);

            helper.Draw(spriteBatch);
            beaconMenu.Draw(spriteBatch);

            if (showFPS)
            {
                if (GameStateManager.currentState == "OverworldState")
                {
                    spriteBatch.DrawString(fontManager.GetFont(14), "Fps: " + fps.ToString(),
                        new Vector2((camera.cameraPos.X + ScreenCenter.X) - fontManager.GetFont(14).MeasureString("Fps: " + fps.ToString()).X,
                                     camera.cameraPos.Y - ScreenCenter.Y) + fontManager.FontOffset,
                        fontManager.FontColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }

                else
                {
                    spriteBatch.DrawString(fontManager.GetFont(14), "Fps: " + fps.ToString(),
                        new Vector2(Window.ClientBounds.Width - fontManager.GetFont(14).MeasureString("Fps: " + fps.ToString()).X,
                                    0) + fontManager.FontOffset,
                        fontManager.FontColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Save()
        {
            helper.DisplayText("The game has been saved!", 2);

            saveFile.EmptySaveFile("save.ini");
            statsManager.Save();
            player.Save();
            //baseInventoryManager.Save();
            missionManager.Save();
            tutorialManager.Save();
            shipInventoryManager.Save();
            stateManager.overworldState.Save();
        }

        public void Load()
        {
            saveFile.Load("save.ini");
            statsManager.Load();
            player.Load();
            //baseInventoryManager.Load();
            missionManager.Load();
            tutorialManager.Load();
            shipInventoryManager.Load();
            stateManager.overworldState.Load();
        }

        public void Restart()
        {
            settingsFile = new SaveFile(this);
            settingsFile.Load("settings.ini");

            graphics.PreferredBackBufferWidth = (int)resolution.X;
            graphics.PreferredBackBufferHeight = (int)resolution.Y;
            graphics.ApplyChanges();

            menuBGController.Initialize();
            musicManager.Initialize();
            soundEffectsManager.Initialize();
            fontManager.Initialize();
            shipInventoryManager.Initialize();
            statsManager.Initialize();
            player.Initialize();
            missionManager.Initialize();
            tutorialManager.Initialize();
            beaconMenu.Initialize();
            stateManager.Initialize();
            messageBox.Initialize();
        }

        public void ChangeResolution(Vector2 newResolution)
        {
            resolution = newResolution;
        }

        #region uglyHelpFunctions
        public void AddGameObjToShooter(GameObjectVertical obj)
        {
            stateManager.shooterState.gameObjects.Add(obj);
        }

        public void AddGameObjToShooter(BackgroundObject obj)
        {
            stateManager.shooterState.backgroundObjects.Add(obj);
        }
        #endregion

        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            soundEffectsManager.DisposeSoundEffect();
        }

        private void SetAvailableResolutions()
        {
            ResolutionOptions = new List<Vector2>();

            ResolutionOptions.Add(new Vector2(800, 600));
            ResolutionOptions.Add(new Vector2(1024, 576));
            ResolutionOptions.Add(new Vector2(1024, 768));
            ResolutionOptions.Add(new Vector2(1280, 720));
        }
    }
}
