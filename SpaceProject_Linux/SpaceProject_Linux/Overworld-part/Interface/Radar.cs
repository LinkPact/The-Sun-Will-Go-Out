using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    /// <summary>
    /// Represents a radar object in the User HUD 
    /// </summary>
    public class Radar
    {
        private readonly Color ImmovableObjectColor = new Color(85, 85, 85);
        private readonly Color HostileShipColor = Color.Red;
        private readonly Color FriendlyShipColor = Color.Green;
        private readonly Color MainMissionColor = Color.Gold;
        private readonly Color SecondaryMissionColor = new Color(172, 172, 172);

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

        private List<DirectionArrow> missionArrows;
        private List<DirectionArrow> availableMainMissionArrows;
        private List<String> availableMainMissionLocationNames;

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

            missionArrows = new List<DirectionArrow>();
            availableMainMissionArrows = new List<DirectionArrow>();
            availableMainMissionLocationNames = new List<String>();
        }

        public void Update(GameTime gameTime, List<GameObjectOverworld> objectsInOverworld, Vector2 cameraPos)
        {
            objectsVisibleOnRadar.Clear();
            missionArrows.Clear();
            availableMainMissionArrows.Clear();
            availableMainMissionLocationNames.Clear();

            availableMainMissionLocationNames = MissionManager.GetAvailableMainMissionLocationNames();

            foreach (GameObjectOverworld obj in objectsInOverworld)
            {
                // Adds visible game objects
                if (CollisionDetection.IsPointInsideCircle(obj.position, game.player.position, viewRadius))
                {
                    objectsVisibleOnRadar.Add(obj);
                }

                // Adds mission arrows for non-visible mission coordinates
                else if (MissionManager.IsCurrentObjectiveDestination(obj))
                {
                    Boolean isMain = MissionManager.IsMainMissionDestination(obj);
                    missionArrows.Add(new DirectionArrow(spriteSheet, obj.position, playerpos, isMain));
                }

                else if (MissionManager.IsNoMainMissionActive())
                {
                    foreach (String str in availableMainMissionLocationNames)
                    {
                        if (obj.name.ToLower().Equals(str.ToLower()))
                        {
                            availableMainMissionArrows.Add(new DirectionArrow(spriteSheet, obj.position, playerpos, true));
                            break;
                        }
                    }
                }
            }

            playerpos = game.player.position;
            Origin = new Vector2(cameraPos.X + game.Window.ClientBounds.Width / 2 - background.SourceRectangle.Value.Width,
                cameraPos.Y + game.Window.ClientBounds.Height / 2 - background.SourceRectangle.Value.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
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

            DrawVisibleGameObjects(spriteBatch);

            foreach (DirectionArrow arrow in missionArrows)
            {
                arrow.Draw(spriteBatch, RadarCenterPos);
            }

            foreach (DirectionArrow arrow in availableMainMissionArrows)
            {
                arrow.Draw(spriteBatch, RadarCenterPos);
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

        private void DrawVisibleGameObjects(SpriteBatch spriteBatch)
        {
            float drawDistance = 0.91f;
            colorSwapCounter++;

            foreach (GameObjectOverworld obj in objectsVisibleOnRadar)
            {
                Color color = ImmovableObjectColor;
                float scale = 1f;
                ActiveSprite = ObjectSprite;
                drawDistance = 0.91f;

                if (MissionManager.IsCurrentObjectiveDestination(obj))
                {
                    drawDistance = 0.92f;
                    if (colorSwapCounter <= 25)
                    {
                        if (MissionManager.IsMainMissionDestination(obj))
                        {
                            color = MainMissionColor;
                        }
                        else
                        {
                            color = SecondaryMissionColor;
                        }
                        ActiveSprite = BlinkingSprite;
                        scale = 1.25f;
                    }

                    else
                    {
                        color = Color.White;
                        scale = 1f;
                    }

                    if (colorSwapCounter >= 50)
                    {
                        colorSwapCounter = 0;
                    }
                }
                else if (IsObjectAvailableMainMissionLocation(obj)
                    && MissionManager.IsNoMainMissionActive())
                {
                    drawDistance = 0.92f;
                    if (colorSwapCounter <= 25)
                    {
                        color = MainMissionColor;
                        ActiveSprite = BlinkingSprite;
                        scale = 1.25f;
                    }

                    else
                    {
                        color = Color.White;
                        scale = 1f;
                    }

                    if (colorSwapCounter >= 50)
                    {
                        colorSwapCounter = 0;
                    }
                }
                else if (obj is FreighterShip)
                {
                    color = FriendlyShipColor;
                }
                else if (obj is RebelShip)
                {
                    if (StatsManager.reputation >= 0)
                    {
                        color = HostileShipColor;
                    }
                    else
                    {
                        color = FriendlyShipColor;
                    }
                }
                else if (obj is AllianceShip
                    || obj is HangarShip)
                {
                    if (StatsManager.reputation < 0)
                    {
                        color = HostileShipColor;
                    }
                    else
                    {
                        color = FriendlyShipColor;
                    }
                }
                else if (obj is SystemStar)
                {
                    color = Color.White;
                    scale = 1.25f;
                }

                spriteBatch.Draw(ActiveSprite.Texture,
                        new Vector2(Origin.X + ((obj.position.X - View.X) / scaleX), Origin.Y + ((obj.position.Y - View.Y) / scaleY)),
                        ActiveSprite.SourceRectangle,
                        color,
                        0.0f,
                        new Vector2(ActiveSprite.SourceRectangle.Value.Width / 2, ActiveSprite.SourceRectangle.Value.Height / 2),
                        scale,
                        SpriteEffects.None,
                        drawDistance
                        );
            }
        }

        private bool IsObjectAvailableMainMissionLocation(GameObjectOverworld obj)
        {
            foreach (String str in availableMainMissionLocationNames)
            {
                if (obj.name.ToLower().Equals(str.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
