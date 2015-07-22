using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public enum OverlayType
    {
        Text,
        Portrait,
        MissionSelection,
        Response
    }

    public class BaseState : GameState
    {
        protected Sprite spriteSheet;

        protected Planet planet;
        protected Station station;
        protected BaseStateManager subStateManager;

        protected Vector2 nameStringPosition;
        protected Vector2 nameStringOrigin;

        public Sprite SpriteSheet { get { return spriteSheet; } }
        public OverlayType OverlayType { get { return overlayType; } }
        public bool IsOverlayDisplayed { get { return displayOverlay; } }

        private bool displayOverlay;
        private OverlayType overlayType;
        private Sprite overlay;
        private Sprite portraitOverlay;
        private Sprite textOverlay;
        private Sprite selectionOverlay;
        private Sprite responseOverlay;

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

            nameStringPosition = new Vector2(Game1.ScreenSize.X / 2, 50);

            portraitOverlay = spriteSheet.GetSubSprite(new Rectangle(450, 0, 567, 234));
            textOverlay = spriteSheet.GetSubSprite(new Rectangle(0, 0, 400, 183));
            selectionOverlay = spriteSheet.GetSubSprite(new Rectangle(450, 235, 567, 234));
            responseOverlay = spriteSheet.GetSubSprite(new Rectangle(1018, 0, 571, 309));
            overlay = textOverlay;
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
        }

        public void DisplayOverlay(OverlayType overlayType)
        {
            this.displayOverlay = true;
            this.overlayType = overlayType;

            switch (overlayType)
            {
                case OverlayType.Text:
                    overlay = textOverlay;
                    break;
                
                case OverlayType.Portrait:
                    overlay = portraitOverlay;
                    break;

                case OverlayType.MissionSelection:
                    overlay = selectionOverlay;
                    break;

                case OverlayType.Response:
                    overlay = responseOverlay;
                    break;
            }
        }

        public void HideOverlay()
        {
            displayOverlay = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet.Texture,
                             new Vector2(0, 0),
                             new Rectangle(0, 486, 1280, 720),
                             Color.White,
                             0f,
                             Vector2.Zero,
                             new Vector2(Game1.ScreenSize.X / 1280f,
                                         Game1.ScreenSize.Y / 720f),
                             SpriteEffects.None,
                             0.0f);

            if (displayOverlay)
            {
                spriteBatch.Draw(overlay.Texture,
                                 Game.ScreenCenter,
                                 overlay.SourceRectangle,
                                 new Color(255, 255, 255, 240),
                                 0f,
                                 overlay.CenterPoint,
                                 1f,
                                 SpriteEffects.None,
                                 0.96f);
            }

            base.Draw(spriteBatch);
        }
    }
}
