using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace SpaceProject_Linux
{
    public class Game1 : Game
    {
        #region declaration
        // Constants
        public static readonly String SaveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/.LinkPact Games/The Sun Will Go Out/";
        private static readonly int FPSRefreshRate = 250;       // (in milliseconds)

        // Flags
        public static bool Paused = false;
        public bool GameStarted;

        // Graphics
        private SpriteBatch spriteBatch;
        public GraphicsDeviceManager graphics;

        // Files
        public SaveFile settingsFile;
        public SaveFile saveFile;

        // Spritesheets
        public Sprite spriteSheetVerticalShooter;
        public Sprite spriteSheetOverworld;
        private Sprite messageBoxSpriteSheet;
        public Sprite spriteSheetItemDisplay;

        // Managers
        public MusicManager musicManager;
        public SoundEffectsManager soundEffectsManager;
        public FontManager fontManager;
        public GameStateManager stateManager;
        public StatsManager statsManager;
        public ShipInventoryManager shipInventoryManager;
        public MissionManager missionManager;
        public TutorialManager tutorialManager;
        public ShopManager shopManager;
        public MenuBackdropController menuBGController;
        private PopupHandler popupHandler;
        public HelperBox helper;

        // Screen and resolution
        public Vector2 ScreenCenter { get { return new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2); } }
        private Point screenSize;
        public Point ScreenSize { get { return screenSize; } }
        private Vector2 resolution;
        public static List<Vector2> ResolutionOptions;
        public Vector2 Resolution { get { return resolution; } }
        public Vector2 DefaultResolution { get { return new Vector2(800, 600); } }

        // TODO: These shouldn't be here
        public PlayerOverworld player;
        public Camera camera;
        private BeaconMenu beaconMenu;
        public BeaconMenu GetBeaconMenu { get { return beaconMenu; } private set { ; } }
        private float fps;
        private float fpsTimer;
        private bool showFPS;
        public bool ShowFPS { get { return showFPS; } set { showFPS = value; } }
        private const int HOLD_KEY_TRESHOLD = 200;
        public int HoldKeyTreshold { get { return HOLD_KEY_TRESHOLD; } }
        private const int SCROLL_SPEED_FAST = 60; //Lower = faster
        private const int SCROLL_SPEED_SLOW = 90;
        public int ScrollSpeedFast { get { return SCROLL_SPEED_FAST; } }
        public int ScrollSpeedSlow { get { return SCROLL_SPEED_SLOW; } }
        private bool saveOnEnterOverworld;
        public bool SaveOnEnterOverworld { get { return saveOnEnterOverworld; } set { saveOnEnterOverworld = value; } }
        public Random random;
        #endregion

        public Game1() :
            base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            CreateDirectories();
            SetAvailableResolutions();
            GameStarted = false;

            settingsFile = new SaveFile(this);
            settingsFile.Load(SaveFilePath, "settings.ini");

            TextToSpeech.TTSMode = (TextToSpeechMode)settingsFile.GetPropertyAsInt("sound", "text-to-speech", 2);

            resolution = new Vector2(settingsFile.GetPropertyAsFloat("visual", "resolutionx", 1024),
                                     settingsFile.GetPropertyAsFloat("visual", "resolutiony", 768));
            screenSize = new Point((int)resolution.X, (int)resolution.Y);

            random = new Random();

            showFPS = settingsFile.GetPropertyAsBool("visual", "showfps", false);

            graphics.PreferredBackBufferWidth = (int)resolution.X;
            graphics.PreferredBackBufferHeight = (int)resolution.Y;
            graphics.IsFullScreen = settingsFile.GetPropertyAsBool("visual", "fullscreen", false);
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();

            CenterScreenWindow();

            IsMouseVisible = true;

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
            spriteSheetVerticalShooter = new Sprite(Content.Load<Texture2D>("Vertical-Sprites/ShooterSheet"));
            messageBoxSpriteSheet = new Sprite(Content.Load<Texture2D>("Overworld-Sprites/messageBoxSpriteSheet"));
            spriteSheetItemDisplay = new Sprite(Content.Load<Texture2D>("itemVisualSheet"));
            CollisionHandlingOverWorld.LoadLineTexture(this);

            shipInventoryManager = new ShipInventoryManager(this);
            shipInventoryManager.Initialize();

            statsManager = new StatsManager(this);
            statsManager.Initialize();

            player = new PlayerOverworld(this, spriteSheetOverworld);
            player.Initialize();

            beaconMenu = new BeaconMenu(this, spriteSheetOverworld);
            beaconMenu.Initialize();

            stateManager = new GameStateManager(this);
            stateManager.Initialize();

            missionManager = new MissionManager(this);
            missionManager.Initialize();

            tutorialManager = new TutorialManager(this);
            tutorialManager.Initialize();
            tutorialManager.TutorialsUsed = settingsFile.GetPropertyAsBool("Game options", "tutorials", true);

            shopManager = new ShopManager();

            saveFile = new SaveFile(this);

            Portrait.InitializePortraitSpriteSheet(this);
            popupHandler = new PopupHandler(this, messageBoxSpriteSheet);
            popupHandler.Initialize();

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

        protected override void Update(GameTime GameTime)
        {
            Window.Title = "The Sun Will Go Out";

            if (IsActive)
            {
                ControlManager.Update(GameTime);

                //Toggles fullscreen on/off
                if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.LeftAlt) && ControlManager.CheckKeyPress(Keys.F))
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
                        player.Update(GameTime);

                    stateManager.Update(GameTime);
                    missionManager.Update(GameTime);
                    tutorialManager.Update(GameTime);
                    shopManager.Update(GameTime);
                }

                else if (ZoomMap.IsMapOn)
                {
                    camera.CameraUpdate(GameTime, player);
                }

                soundEffectsManager.Update(GameTime);

                if (ControlManager.CheckKeyPress(Keys.N)
                    && GameStateManager.currentState == "OverworldState")
                {
                    ZoomMap.ToggleMap();
                    soundEffectsManager.StopSoundEffect(SoundEffects.OverworldEngine);
                }

                if (ZoomMap.IsMapOn)
                {
                    ZoomMap.Update(GameTime, stateManager.overworldState.GetZoomObjects, camera);
                }

                popupHandler.Update(GameTime);
                helper.Update(GameTime);
                beaconMenu.Update(GameTime);

                fpsTimer -= GameTime.ElapsedGameTime.Milliseconds;
                if (fpsTimer <= 0)
                {
                    fps = (float)Math.Round((1 / GameTime.ElapsedGameTime.TotalSeconds), 0);
                    fpsTimer = FPSRefreshRate;
                }

                if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.LeftAlt) &&
                    ControlManager.CheckKeyPress(Keys.M))
                {
                    musicManager.SwitchMusicMuted();
                }

                menuBGController.Update(GameTime);

                if (saveOnEnterOverworld)
                {
                    saveOnEnterOverworld = false;
                    Save();
                }

                base.Update(GameTime);
            }
        }

        protected override void Draw(GameTime GameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (GameStateManager.currentState.ToLower())
            {
                case "overworldstate":
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.GetTransformation());
                    break;

                case "shooterstate":
                case "introsecondstate":
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    break;

                default:
                    spriteBatch.Begin();
                    break;
            }

            stateManager.Draw(spriteBatch);
            missionManager.Draw(spriteBatch);
            tutorialManager.Draw(spriteBatch);

            if (!PopupHandler.TextBufferEmpty
                && !ZoomMap.IsMapOn)
            {
                popupHandler.Draw(spriteBatch);
            }
            else if (ZoomMap.IsMapOn)
            {
                ZoomMap.DrawOverlay(spriteBatch, stateManager.overworldState.GetZoomObjects);
            }

            helper.Draw(spriteBatch);
            beaconMenu.Draw(spriteBatch);

            if (showFPS && !ZoomMap.IsMapOn)
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
                        new Vector2(ScreenSize.X - fontManager.GetFont(14).MeasureString("Fps: " + fps.ToString()).X,
                                    0) + fontManager.FontOffset,
                        fontManager.FontColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }

            spriteBatch.End();

            base.Draw(GameTime);
        }

        public void Save()
        {
            helper.DisplayText("The Game has been saved!", 2);

            saveFile.EmptySaveFile(Game1.SaveFilePath, "save.ini");
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
            saveFile.Load(SaveFilePath, "save.ini");
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
            settingsFile.Load(SaveFilePath, "settings.ini");

            graphics.PreferredBackBufferWidth = (int)resolution.X;
            graphics.PreferredBackBufferHeight = (int)resolution.Y;
            graphics.ApplyChanges();

            screenSize = new Point((int)resolution.X, (int)resolution.Y);
            CenterScreenWindow();

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
            popupHandler.Initialize();
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

            ResolutionOptions.Add(new Vector2(1024, 768));
            ResolutionOptions.Add(new Vector2(1280, 720));
        }

        private void CreateDirectories()
        {
            Directory.CreateDirectory(SaveFilePath);
            Directory.CreateDirectory(LevelLogger.writeDir);
        }

        private void CenterScreenWindow()
        {
            if (!graphics.IsFullScreen)
            {
                Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - ScreenSize.X) / 2,
                        (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - ScreenSize.Y) / 2);
            }
        }
    }
}
