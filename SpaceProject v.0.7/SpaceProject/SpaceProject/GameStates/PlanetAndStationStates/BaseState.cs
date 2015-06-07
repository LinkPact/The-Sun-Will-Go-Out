using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BaseState : GameState
    {
        protected Sprite spriteSheet;

        protected Planet planet;
        protected Station station;
        protected BaseStateManager substateManager;

        protected Vector2 nameStringPosition;
        protected Vector2 nameStringOrigin;

        public Sprite SpriteSheet { get { return spriteSheet; } }

        private bool displayOverlay;
        private Sprite overlay;
        private Vector2 overlayPosition;

        public GameObjectOverworld GetBase()
        {
            if (this is PlanetState)
                return planet;
            else
                return station;
        }

        protected BaseState(Game1 Game, string Name) :
            base (Game, Name) 
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/PlanetOverviewSpritesheet"), null);    

            nameStringPosition = new Vector2(Game.Window.ClientBounds.Width / 2, 30);

            overlay = spriteSheet.GetSubSprite(new Rectangle(449, 0, 800, 400));
            overlayPosition = Game.ScreenCenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (substateManager.ButtonControl.Equals(ButtonControl.Menu))
            {
                displayOverlay = false;
            }
            else
            {
                displayOverlay = true;
            }
        }

        public void DisplayOverlay(bool displayOverlay)
        {
            this.displayOverlay = displayOverlay;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
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

            if (displayOverlay)
            {
                spriteBatch.Draw(overlay.Texture,
                                 overlayPosition,
                                 overlay.SourceRectangle,
                                 new Color(255, 255, 255, 240),
                                 0f,
                                 overlay.CenterPoint,
                                 new Vector2(Game.Window.ClientBounds.Width / 1280,
                                             Game.Window.ClientBounds.Height / 720),
                                 SpriteEffects.None,
                                 0.96f);
            }

            base.Draw(spriteBatch);
        }
    }
}
