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

        private Sprite lineTexture;

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

            //fontBig = Game.Content.Load<SpriteFont>("Overworld-Sprites/bigFont");
            //Game.fontManager.GetFont(14) = Game.Content.Load<SpriteFont>("Overworld-Sprites/smallFont");
            //Game.fontManager.GetFont(14) = Game.fontManager.GetFont(14);
            //Game.fontManager.GetFont(16) = Game.fontManager.GetFont(16);

            nameStringPosition = new Vector2(Game.Window.ClientBounds.Width * 3 / 4, 15);

            dataHeadStringPosition = new Vector2(Game.Window.ClientBounds.Width / 4, 30);

            dataBodyStringPosition = new Vector2(5, 60);

            padding = "";

            dataHead = "Planet Data:";
            dataBody = "";

            iconExpl = "";
            iconExplPos = new Vector2(Game.Window.ClientBounds.Width / 6, Game.Window.ClientBounds.Height / 2 + 10);
            iconExplOrigin = Vector2.Zero;            

            #endregion

            #region Initailize Textures/Sprites

            stationTexturePosition = new Vector2(Game.Window.ClientBounds.Width / 2 + Game.Window.ClientBounds.Width / 4,
                                         Game.Window.ClientBounds.Height / 4);

            

            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/PlanetOverviewSpritesheet"),null);

            lineTexture = spriteSheet.GetSubSprite(new Rectangle(240, 0, 1, 1));

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
            //Game.Window.Title = ("SpaceExplorationGame - " + "StationOverview" + " " + subStateManager.ButtonControl.ToString() + " " + subStateManager.OverviewMenuState.ButtonShop.isSelected + subStateManager.OverviewMenuState.ButtonShop.isActive);

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

            Color lineColor = new Color(40, 40, 40, 255);

            #region Textures

            //Backdrop
            int spriteWidth = 92;
            int spriteHeight = 92;

            for (int i = 0; i < (int)((Game.Window.ClientBounds.Width / spriteWidth) + 1); i++)
            {
                for (int j = 0; j < (int)((Game.Window.ClientBounds.Height / spriteHeight) + 1); j++)
                    spriteBatch.Draw(spriteSheet.Texture, new Vector2(spriteWidth * i, spriteHeight * j),
                    new Rectangle(0, 241, 92, 92), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            //Draw Black texture around planet
            spriteBatch.Draw(spriteSheet.Texture,
                             new Vector2(Game.Window.ClientBounds.Width / 2 - 2,
                                           0),
                             new Rectangle(241, 3, 400, 300),
                             Color.White,
                             0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width / Game.DefaultResolution.X,
                                 Game.Window.ClientBounds.Height / Game.DefaultResolution.Y),
                             SpriteEffects.None,
                             0.1f);

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

            //Draw lines
            //Vertical
            spriteBatch.Draw(lineTexture.Texture,
                             new Vector2(Game.Window.ClientBounds.Width / 2, 0),
                             lineTexture.SourceRectangle,
                             lineColor,
                             (float)(Math.PI * 90) / 180,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Height / 2, 2),
                             SpriteEffects.None,
                             .8f);

            //Vertical
            spriteBatch.Draw(lineTexture.Texture,
                             new Vector2(Game.Window.ClientBounds.Width / 3, Game.Window.ClientBounds.Height / 2),
                             lineTexture.SourceRectangle,
                             lineColor,
                             (float)(Math.PI * 90) / 180,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Height / 2, 2),
                             SpriteEffects.None,
                             .8f);

            //Horizontal
            spriteBatch.Draw(lineTexture.Texture,
                             new Vector2(0, Game.Window.ClientBounds.Height / 2),
                             lineTexture.SourceRectangle,
                             lineColor,
                             0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width, 2),
                             SpriteEffects.None,
                             .8f);

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
            spriteBatch.DrawString(Game.fontManager.GetFont(16),
                                   iconExpl,
                                   iconExplPos + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   iconExplOrigin,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);

            dataHeadStringOrigin = new Vector2(Game.fontManager.GetFont(16).MeasureString(dataHead).X / 2, 5);

            //Draw planet data head
            spriteBatch.DrawString(Game.fontManager.GetFont(16),
                                   dataHead,
                                   dataHeadStringPosition + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   dataHeadStringOrigin,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);

            //Draw planet data body
            spriteBatch.DrawString(Game.fontManager.GetFont(16),
                                   dataBody,
                                   dataBodyStringPosition + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   Vector2.Zero,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);            
            #endregion

            subStateManager.Draw(spriteBatch);
        }

    }
}
