using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    /// <summary>
    /// Represents a radar object in the User HUD 
    /// </summary>
    public class Radar
    {
        private Game1 game;

        public Vector2 Origin;
        public int radarHeight;
        public int radarWidth;
        public float scaleX;
        public float scaleY;

        private Vector2 RadarCenterPos {
            get {
                return new Vector2(Origin.X + radarWidth / 2, Origin.Y + radarHeight / 2);
            }
        }

        private List<GameObjectOverworld> objectsVisibleOnRadar;
        private List<Vector2> objectsOnMap = null;

        private Vector2 playerpos;

        private Sprite ObjectSprite;
        private Sprite BlinkingSprite;

        private Sprite ActiveSprite; 
        
        protected Sprite spriteSheet;
        protected Sprite background;

        private DirectionArrow testArrow;

        private static int colorSwapCounter = 0;
        
        private int viewRadius;
        public Rectangle View
        {
            get { return new Rectangle((int)game.player.position.X - viewRadius, (int)game.player.position.Y - viewRadius, viewRadius * 2, viewRadius * 2); }
        }

        public Radar(Game1 game, Sprite spriteSheet)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;
        }

        public void Initialize(Vector2 Origin, int viewRadius)
        {
            objectsVisibleOnRadar = new List<GameObjectOverworld>();
            objectsOnMap = new List<Vector2>();
            this.Origin = Origin;
            this.viewRadius = viewRadius;
           // View = new Rectangle((int)game.player.position.X - viewRadius, (int)game.player.position.Y - viewRadius, viewRadius * 2, viewRadius * 2);
            radarHeight = 198;
            radarWidth = 198;
            scaleX = View.Width / radarWidth;
            scaleY = View.Height / radarHeight;
            
            ObjectSprite = spriteSheet.GetSubSprite(new Rectangle(42, 24, 6, 6));
            BlinkingSprite = spriteSheet.GetSubSprite(new Rectangle(49, 24, 6, 6));
            background = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites/radar"), new Rectangle(0, 0, 198, 198));

            testArrow = new DirectionArrow(game.spriteSheetVerticalShooter, new Vector2(118354, 98654));
        }

        public void Update(GameTime gameTime, List<GameObjectOverworld> objectsInOverworld, Vector2 cameraPos)
        {
            objectsVisibleOnRadar.Clear();
            foreach (GameObjectOverworld obj in objectsInOverworld)
            {
                if (CollisionDetection.IsPointInsideCircle(obj.position, game.player.position, viewRadius))
                {
                    objectsVisibleOnRadar.Add(obj);
                }
            }
            playerpos = game.player.position;
            Origin = new Vector2(cameraPos.X + game.Window.ClientBounds.Width / 2 - background.SourceRectangle.Value.Width,
                cameraPos.Y + game.Window.ClientBounds.Height / 2 - background.SourceRectangle.Value.Height);

            testArrow.Update(playerpos);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            testArrow.Draw(spriteBatch, RadarCenterPos, playerpos);

            spriteBatch.Draw(background.Texture,
                Origin,
                background.SourceRectangle,
                Color.White,
                0.0f,
                new Vector2(0, 0),
                1.0f,
                SpriteEffects.None,
                0.9f
                );

            float drawDistance = 0.91f;
            foreach (GameObjectOverworld obj in objectsVisibleOnRadar)
            {
                Color tempColor = Color.Gray;
                ActiveSprite = ObjectSprite;
                drawDistance = 0.91f;

                if (MissionManager.IsCurrentObjective(obj))
                {
                    drawDistance += 0.01f;
                    if (colorSwapCounter <= 25)
                    {
                        tempColor = Color.DarkOrange;
                        ActiveSprite = BlinkingSprite;
                    }

                    else
                    {
                        tempColor = Color.White;
                    }

                    colorSwapCounter++;
                    if (colorSwapCounter >= 50)
                    {
                        colorSwapCounter = 0;
                    }
                }
                else if (obj is RebelShip)
                {
                    tempColor = Color.Red;
                }
                else if (obj is FreighterShip || obj is AllianceShip || obj is HangarShip)
                {
                    tempColor = Color.Blue;
                }
                else if (obj is Station || obj is Planet)
                {
                    tempColor = Color.Yellow;
                }

                spriteBatch.Draw(ActiveSprite.Texture,
                        new Vector2(Origin.X + ((obj.position.X - View.X) / scaleX), Origin.Y + ((obj.position.Y - View.Y) / scaleY)),
                        ActiveSprite.SourceRectangle,
                        tempColor,
                        0.0f,
                        new Vector2(ActiveSprite.SourceRectangle.Value.Width / 2, ActiveSprite.SourceRectangle.Value.Height / 2),
                        1f,
                        SpriteEffects.None,
                        drawDistance
                        ); 
            }

            // Draw background sprite
            spriteBatch.Draw(ObjectSprite.Texture,
                new Vector2(Origin.X + ((playerpos.X - View.X) / scaleX), Origin.Y + ((playerpos.Y - View.Y) / scaleY)),
                ObjectSprite.SourceRectangle,
                Color.White,
                0.0f,
                new Vector2(ObjectSprite.SourceRectangle.Value.Width / 2, ObjectSprite.SourceRectangle.Value.Height / 2),
                1f,
                SpriteEffects.None,
                0.911f
                );
        }
    }
}
