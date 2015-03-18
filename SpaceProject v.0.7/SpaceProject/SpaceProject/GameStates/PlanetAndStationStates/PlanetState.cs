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

        private Sprite lineTexture;

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

            nameStringPosition = new Vector2(Game.Window.ClientBounds.Width * 3/4, 15);

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

            planetTexturePosition = new Vector2(Game.Window.ClientBounds.Width / 2 + Game.Window.ClientBounds.Width / 4,
                                         Game.Window.ClientBounds.Height / 4);

            

            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/PlanetOverviewSpritesheet"),null);

            lineTexture = spriteSheet.GetSubSprite(new Rectangle(240, 0, 1, 1));

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

            dataBody = TextUtils.FormatDataBody(Game.fontManager.GetFont(16),
                new List<string>(){"Surface:",
                                   "Mass:",
                                   "Temperature:",
                                   "Gravity:",
                                   "Orbit:",
                                   "Rotation:",
                                   "Habitable:"},
                new List<string>(){ planet.PlanetSurface,
                                    planet.PlanetMass.ToString() + " Earth masses",
                                    planet.PlanetTemp.ToString() + " Celsius",
                                    planet.PlanetGravity.ToString() + " Log G",
                                    planet.PlanetOrbit.ToString() + " Earth years",
                                    planet.PlanetRotation.ToString() + " Earth days" ,
                                    planet.Habitable(planet.PlanetTemp, planet.PlanetGravity)},
                                    383);

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

            Color lineColor = new Color(40, 40, 40, 255);

            #region Textures

            //Backdrop
            for (int i = 0; i < (int)((Game.Window.ClientBounds.Width / BG_WIDTH) + 1); i++)
            {
                for (int j = 0; j < (int)((Game.Window.ClientBounds.Height / BG_HEIGHT) + 1); j++)
                    spriteBatch.Draw(spriteSheet.Texture, new Vector2(BG_WIDTH * i, BG_HEIGHT * j),
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
