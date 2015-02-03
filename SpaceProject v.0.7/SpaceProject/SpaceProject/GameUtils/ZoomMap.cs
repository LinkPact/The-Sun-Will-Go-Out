using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        private const float ZoomRate = 0.0546875f;
        private const float ZoomedOutValue = 0.02f;

        private const float StarZoomScale = 0.15f;
        private const float PlanetZoomScale = 0.15f;
        private const float StationZoomScale = 0.2f;
        private const float PlayerZoomScale = 0.5f;

        private static MapState mapState = MapState.Off;
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
            // Scale up objects
            foreach (GameObjectOverworld obj in gameObjects)
            {
                if (obj is Planet
                    || obj.name.ToLower().Equals("soelara")
                    || obj.name.ToLower().Equals("fortrun station i")
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

            if (mapState == MapState.ZoomingOut)
            {
                camera.Zoom *= (ZoomRate * gameTime.ElapsedGameTime.Milliseconds);

                if (camera.Zoom <= ZoomedOutValue)
                {
                    Game1.Paused = true;
                    mapState = MapState.On;
                }
            }
            else if (mapState == MapState.ZoomingIn)
            {
                camera.Zoom *= (1 + (1 - (ZoomRate * gameTime.ElapsedGameTime.Milliseconds)));

                if (camera.Zoom >= 1f)
                {
                    HideMap(camera, gameObjects);
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
