using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    enum MapState
    {
        ZoomingOut,
        ZoomingIn,
        On,
        Off
    }
    
    class ZoomMap
    {
        // constants
        private const float ZoomDelta = 0.125f;
        private static float ZoomInFactor { get { return 1 + ZoomDelta; } }
        private static float ZoomOutFactor { get { return 1 - ZoomDelta; } }
        
        private const float ZoomedOutValue = 0.02f;
        private const float StarZoomScale = 0.15f;
        private const float PlanetZoomScale = 0.15f;
        private const float StationZoomScale = 0.2f;
        private const float PlayerZoomScale = 0.5f;

        // variables
        private static MapState mapState = MapState.Off;

        // properties
        public static bool IsMapOn { get { return mapState != MapState.Off; } private set { ; } }
        public static MapState MapState { get { return mapState; } private set { ; } }

        public static void ToggleMap()
        {
            if (IsMapOn)
            {
                mapState = MapState.ZoomingIn;
            }
            else
            {
                mapState = MapState.ZoomingOut;
            }
        }

        /// <summary>
        /// Updates map zooming
        /// </summary>
        /// <param name="gameObjects">Objects to be scaled up for visibility</param>
        public static void Update(GameTime gameTime, List<GameObjectOverworld> gameObjects, Camera camera)
        {
            if (mapState == MapState.ZoomingOut)
            {
                camera.Zoom *= ZoomOutFactor;

                if (Game1.Paused == false)
                    Game1.Paused = true;

                if (camera.Zoom <= ZoomedOutValue)
                {
                    mapState = MapState.On;
                    camera.Zoom = ZoomedOutValue;
                }
            }
            else if (mapState == MapState.ZoomingIn)
            {
                camera.Zoom *= ZoomInFactor;

                if (camera.Zoom >= 1f)
                {
                    HideMap(camera, gameObjects);
                }
            }

            // Scale up objects
            foreach (GameObjectOverworld obj in gameObjects)
            {
                if ((obj is Planet
                    || obj.name.ToLower().Equals("soelara")
                    || obj.name.ToLower().Equals("fortrun station i"))
                    && camera.Zoom < PlanetZoomScale)
                {
                    obj.scale = PlanetZoomScale / camera.Zoom;
                }

                else if (obj is SystemStar
                    && camera.Zoom < StarZoomScale)
                {
                    obj.scale = StarZoomScale / camera.Zoom;
                }

                else if (obj is Station
                    && camera.Zoom < StationZoomScale)
                {
                    obj.scale = StationZoomScale / camera.Zoom;
                }

                else if (obj is PlayerOverworld
                    && camera.Zoom < PlayerZoomScale)
                {
                    obj.scale = PlayerZoomScale / camera.Zoom;
                }
            }
        }

        public static void DrawOverlay(SpriteBatch spriteBatch, List<GameObjectOverworld> gameObjects)
        {
            if (mapState == MapState.On)
            {
                foreach (GameObjectOverworld obj in gameObjects)
                {
                    spriteBatch.DrawString(FontManager.GetFontStatic(14), obj.name, obj.position, Color.LightSeaGreen, 0f, Vector2.Zero, 50f, SpriteEffects.None, 1f);
                }
            }
        }

        /// <summary>
        /// Unpauses game and resets camera zoom and object scales
        /// </summary>
        private static void HideMap(Camera camera, List<GameObjectOverworld> gameObjects)
        {
            foreach (GameObjectOverworld obj in gameObjects)
            {
                obj.scale = 1;
            }

            Game1.Paused = false;
            camera.Zoom = 1f;
            mapState = MapState.Off;
        }
    }
}
