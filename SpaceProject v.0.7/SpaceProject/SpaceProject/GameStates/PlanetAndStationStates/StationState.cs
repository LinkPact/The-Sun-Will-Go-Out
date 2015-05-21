using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{

    public class StationState : BaseState
    {
        public static string PreviousStation;
        private StationStateManager subStateManager;

        #region Texture Fields

        private Vector2 stationTexturePosition;
        private Vector2 stationTextureOrigin;

        #endregion

        #region Planet Data Fields

        //planet data variables
        //private Station station;
        private Sprite stationSprite;
        private string stationName;
        private float stationScale;

        #endregion

        #region Properties

        //public Sprite SpriteSheet { get { return spriteSheet; } }

        public Station Station { get { return station; } }

        public StationStateManager SubStateManager { get { return subStateManager; } }

        #region String Properties

        //public SpriteFont Game.fontManager.GetFont(16) { get { return Game.fontManager.GetFont(16); } }
        //public SpriteFont Game.fontManager.GetFont(14) { get { return Game.fontManager.GetFont(14); } }

        //public string Padding { get { return padding; } set { padding = value; } }
        //
        //public string DataHead { get { return dataHead; } set { dataHead = value; } }
        //public string DataBody { get { return dataBody; } set { dataBody = value; } }

        #endregion

        #endregion

        public StationState(Game1 Game, string name) :
            base(Game, name)
        {
        }

        public override void Initialize()
        {
            #region Initailize Strings

            nameStringPosition = new Vector2(Game.Window.ClientBounds.Width / 2, 30);

            iconExpl = "";
            iconExplPos = new Vector2(Game.Window.ClientBounds.Width / 6, Game.Window.ClientBounds.Height / 2 + 10);
            iconExplOrigin = Vector2.Zero;            

            #endregion

            #region Initailize Textures/Sprites

            stationTexturePosition = new Vector2(Game.Window.ClientBounds.Width / 2,
                                         Game.Window.ClientBounds.Height / 4);

            

            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/PlanetOverviewSpritesheet"),null);

            #endregion          

            subStateManager = new StationStateManager(this.Game);
            subStateManager.Initialize();

            PreviousStation = "";

            ActiveSong = Music.SpaceStation;
        }

        //Method for loading data from the planet that the player has entered 
        public void LoadStationData(Station station)
        {
            this.station = station;

            stationSprite = station.sprite;
            stationSprite.GetSubSprite(station.sprite.SourceRectangle);

            stationName = station.name;
            stationScale = station.scale;

            stationTextureOrigin = new Vector2(station.sprite.SourceRectangle.Value.Width / 2,
                                                  station.sprite.SourceRectangle.Value.Height / 2);

            nameStringOrigin = Game.fontManager.GetFont(16).MeasureString(stationName) / 2;

            if (!station.Abandoned)
            {
                dataBody = TextUtils.FormatDataBody(Game.fontManager.GetFont(16),
                   new List<string>() { "Inhabitants:" },
                   new List<string>() { station.StationInhabitants.ToString() },
                   383);
            }
            else
            {
                dataBody = TextUtils.FormatDataBody(Game.fontManager.GetFont(16),
                   new List<string>() { "Inhabitants:" },
                   new List<string>() { "0" },
                   383);
            }
        }

        private float ScaleStation(float diameter, float planetScale)
        {
            int preferredSize = (int)Game.Window.ClientBounds.Width / 4;
            float maxScale = 2f;
            float scale = planetScale;

            while ((diameter * scale) > preferredSize)
                scale -= 0.01f;

            while ((diameter * scale) < preferredSize && scale < maxScale)
                scale += 0.01f;

            return scale;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //Game.musicManager.SetMusicVolume(0);
            subStateManager.OnEnter();

            iconExpl = "Missions";

            if (!Station.Abandoned)
            {
                Game.soundEffectsManager.LoopSoundEffect(SoundEffects.Crowd, 0f, 0f);
            }
        }

        public override void OnLeave()
        {
            PreviousStation = station.name;
            station = null;
            stationSprite = null;
            stationName = "";

            Game.soundEffectsManager.StopSoundEffect(SoundEffects.Crowd);
        }

        public override void Update(GameTime gameTime)
        {
            subStateManager.Update(gameTime);        

            if (subStateManager.ActiveButton != null)
            {
                iconExpl = subStateManager.ActiveButton.name;
                iconExplOrigin = Game.fontManager.GetFont(16).MeasureString(iconExpl) / 2;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(new Color(42, 95, 130, 255));

            #region Textures

            //Backdrop
            spriteBatch.Draw(spriteSheet.Texture,
                             new Vector2(0, 0),
                             new Rectangle(0, 486, 1280, 720),
                             Color.White,
                             0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width / 1280,
                                 Game.Window.ClientBounds.Height / 720),
                             SpriteEffects.None,
                             0.0f);

            //Draw planet texture
            spriteBatch.Draw(stationSprite.Texture,
                             stationTexturePosition,
                             stationSprite.SourceRectangle,
                             Color.White,
                             .0f,
                             stationTextureOrigin,
                             ScaleStation(stationSprite.SourceRectangle.Value.Width, stationScale),
                             SpriteEffects.None,
                             .5f);

            #endregion

            #region strings

            //Draw planet name string
            spriteBatch.DrawString(Game.fontManager.GetFont(16),
                                   stationName,
                                   nameStringPosition + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   nameStringOrigin,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);

            //Draw icon expl string
            spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                   iconExpl,
                                   iconExplPos + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   iconExplOrigin,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);
            #endregion

            subStateManager.Draw(spriteBatch);
        }

    }
}
