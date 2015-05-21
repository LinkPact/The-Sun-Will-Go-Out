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

        private PlanetStateManager subStateManager;

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

        public PlanetStateManager SubStateManager { get { return subStateManager; } }

        #endregion

        public PlanetState(Game1 Game, string name) :
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

            planetTexturePosition = new Vector2(Game.Window.ClientBounds.Width / 2,
                                         Game.Window.ClientBounds.Height / 4);

            

            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/PlanetOverviewSpritesheet"),null);

            #endregion          

            subStateManager = new PlanetStateManager(this.Game);
            subStateManager.Initialize();

            PreviousPlanet = "";

        }

        //Method for loading data from the planet that the player has entered 
        public void LoadPlanetData(Planet planet)
       {
            planetSprite = planet.sprite;
            planetSprite.GetSubSprite(planet.sprite.SourceRectangle);

            planetName = planet.name;
            planetScale = planet.scale;

            planetTextureOrigin = new Vector2(planet.sprite.SourceRectangle.Value.Width / 2,
                                              planet.sprite.SourceRectangle.Value.Height / 2);

            nameStringOrigin = Game.fontManager.GetFont(14).MeasureString(planetName) / 2;

            this.planet = planet;
        }

        private float ScalePlanet(float diameter, float planetScale)
        {
            float preferredSize = Game.Window.ClientBounds.Width / 4f; // 3.25f
            float scale = planetScale;

            while ((diameter * scale) > preferredSize)
                scale -= 0.01f;

            return scale;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            subStateManager.OnEnter();

            iconExpl = "Colony";
        }

        public override void OnLeave()
        {
            PreviousPlanet = planet.name;
        }

        public override void Update(GameTime gameTime)
        {
            subStateManager.Update(gameTime);        

            if (subStateManager.ActiveButton != null)
            {
                iconExpl = subStateManager.ActiveButton.name;
                iconExplOrigin = Game.fontManager.GetFont(14).MeasureString(iconExpl) / 2;
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

            ////Draw border around planet namw
            //spriteBatch.Draw(spriteSheet.Texture,
            //                 new Rectangle((int)nameStringPosition.X - (int)fontBig.MeasureString(planetName).X / 2,
            //                               (int)nameStringPosition.Y - (int)fontBig.MeasureString(planetName).Y / 2 - 2,
            //                               (int)fontBig.MeasureString(planetName).X,
            //                               (int)fontBig.MeasureString(planetName).Y),
            //                 new Rectangle(4, 4, 1, 1),
            //                 Color.Gray,
            //                 0f,
            //                 Vector2.Zero,
            //                 SpriteEffects.None,
            //                 0.15f);

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

            #endregion

            #region strings

            //Draw planet name string
            spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                   planetName,
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

            dataHeadStringOrigin = new Vector2(Game.fontManager.GetFont(14).MeasureString(dataHead).X / 2, 5);
      
            #endregion

            subStateManager.Draw(spriteBatch);
        }

    }
}
