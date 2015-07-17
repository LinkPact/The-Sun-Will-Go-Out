using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{

    public class PlanetState : BaseState
    {
        public static string PreviousPlanet;

        private const int BG_WIDTH = 92;
        private const int BG_HEIGHT = 92;

        #region Texture Fields

        private Vector2 planetTexturePosition;
        private Vector2 planetTextureOrigin;

        #endregion

        #region Planet Data Fields

        //planet data variables
        private Sprite planetSprite;
        private string planetName;
        private float planetScale;

        #endregion

        #region Properties
        public Planet Planet { get { return planet; } }

        public BaseStateManager SubStateManager { get { return subStateManager; } }

        #endregion

        public PlanetState(Game1 Game, string name) :
            base(Game, name)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            planetTexturePosition = new Vector2(Game.Window.ClientBounds.Width / 2,
                                         Game.Window.ClientBounds.Height / 4);

            subStateManager = new PlanetStateManager(this.Game);
            subStateManager.Initialize();

            PreviousPlanet = "";

            ActiveSong = Music.SpaceStation;
        }

        //Method for loading data from the planet that the player has entered 
        public void LoadPlanetData(Planet planet)
       {
            this.planet = planet;

            planetSprite = planet.sprite;
            planetSprite.GetSubSprite(planet.sprite.SourceRectangle);

            planetName = planet.name;
            planetScale = planet.scale;

            planetTextureOrigin = new Vector2(planet.sprite.SourceRectangle.Value.Width / 2,
                                              planet.sprite.SourceRectangle.Value.Height / 2);

            nameStringOrigin = Game.fontManager.GetFont(18).MeasureString(planetName) / 2;

            subStateManager.RumorsMenuState.LoadRumors(planet);
            subStateManager.OverviewMenuState.SetButtons();
        }

        private float ScalePlanet(float diameter, float planetScale)
        {
            float preferredSize = Game.Window.ClientBounds.Width / 5f;
            float scale = planetScale;

            while ((diameter * scale) > preferredSize)
                scale -= 0.01f;

            return scale;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            subStateManager.ActiveButtonIndexX = 0;
            subStateManager.ActiveButtonIndexY = 0;

            subStateManager.OnEnter();
        }

        public override void OnLeave()
        {
            PreviousPlanet = planet.name;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            subStateManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //Draw planet texture
            spriteBatch.Draw(planetSprite.Texture,
                             planetTexturePosition,
                             planetSprite.SourceRectangle,
                             Color.White,
                             .0f,
                             planetTextureOrigin,
                             ScalePlanet(planetSprite.SourceRectangle.Value.Width, planetScale),
                             SpriteEffects.None,
                             .5f);

            //Draw planet name string
            spriteBatch.DrawString(Game.fontManager.GetFont(18),
                                   planetName,
                                   nameStringPosition + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   nameStringOrigin,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);

            subStateManager.Draw(spriteBatch);
        }

    }
}
