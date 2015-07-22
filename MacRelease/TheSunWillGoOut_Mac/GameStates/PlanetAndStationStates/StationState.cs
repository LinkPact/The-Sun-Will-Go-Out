using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{

    public class StationState : BaseState
    {
        public static string PreviousStation;

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

        public BaseStateManager SubStateManager { get { return subStateManager; } }

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
            base.Initialize();

            stationTexturePosition = new Vector2(Game1.ScreenSize.X / 2,
                                         Game1.ScreenSize.Y / 4);    

            subStateManager = new StationStateManager(this.Game);
            subStateManager.Initialize();

            PreviousStation = "";

            ActiveSong = Music.none;
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

            nameStringOrigin = Game.fontManager.GetFont(18).MeasureString(stationName) / 2;

            subStateManager.RumorsMenuState.LoadRumors(station);
            subStateManager.OverviewMenuState.SetButtons();
        }

        private float ScaleStation(float diameter, float planetScale)
        {
            float preferredSize = Game1.ScreenSize.X / 5f;
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
            SetActiveSong();
            base.OnEnter();

            subStateManager.ActiveButtonIndexX = 0;
            subStateManager.ActiveButtonIndexY = 0;

            subStateManager.OnEnter();
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
            base.Update(gameTime);

            subStateManager.Update(gameTime);        
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(stationSprite.Texture,
                             stationTexturePosition,
                             stationSprite.SourceRectangle,
                             Color.White,
                             .0f,
                             stationTextureOrigin,
                             ScaleStation(stationSprite.SourceRectangle.Value.Width, stationScale),
                             SpriteEffects.None,
                             .5f);

            spriteBatch.DrawString(Game.fontManager.GetFont(18),
                                   stationName,
                                   nameStringPosition + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   nameStringOrigin,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);

            subStateManager.Draw(spriteBatch);
        }

        private void SetActiveSong()
        {
            if (station.name.Equals("Alliance Fleet")
                || station.name.Equals("Rebel Fleet"))
            {
                ActiveSong = Music.none;
                Game.musicManager.StopMusic();
            }
            else
            {
                ActiveSong = Music.SpaceStation;
            }
        }
    }
}
