using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//using MonoGame.Framework;
//using MonoGame.Framework.Content.Pipeline;

namespace MonoContentTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D testTexture;
        private SpriteFont testFont;
        private Song testSong;
        private SoundEffect testSound;

        private KeyboardState current;
        private KeyboardState previous;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            testSong = this.Content.Load<Song>("windows/Draft");
            MediaPlayer.Play(testSong);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            testTexture = this.Content.Load<Texture2D>("windows/plottedLevels");
            testFont = this.Content.Load<SpriteFont>("windows/Iceland_10");

            testSound = this.Content.Load<SoundEffect>("windows/distorted_laser");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (current != null) {
                previous = current;
            }
            current = Keyboard.GetState();

            if (IsSpaceClicked()) {
                testSound.Play();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private bool IsSpaceClicked()
        {
            return current.IsKeyUp(Keys.Space) && previous.IsKeyDown(Keys.Space);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(testTexture, new Vector2(100, 100), Color.Green);

            spriteBatch.DrawString(testFont, "Hello! Teststring!", new Vector2(0, 0), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
