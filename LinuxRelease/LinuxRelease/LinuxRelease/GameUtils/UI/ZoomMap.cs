using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private const float ZoomRate = 0.0546875f;
        private const float ZoomedOutValue = 0.02f;
        private const float StarZoomScale = 0.15f;
        private const float PlanetZoomScale = 0.15f;
        private const float StationZoomScale = 0.2f;
        private const float PlayerZoomScale = 0.5f;
        private const float HelpTextOffset = 12500;

        // variables
        private static MapState mapState = MapState.Off;
        private static Camera camera;

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
            ZoomMap.camera = camera;

            if (mapState == MapState.ZoomingOut)
            {
                camera.Zoom *= ZoomRate * gameTime.ElapsedGameTime.Milliseconds;

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

            else if (mapState == MapState.On 
                && (ControlManager.CheckPress(RebindableKeys.Action2) ||
                ControlManager.CheckKeyPress(Keys.Escape)))
            {
                mapState = MapState.ZoomingIn;
            }

            // Scale up objects
            foreach (GameObjectOverworld obj in gameObjects)
            {
                if ((obj is Planet
                    || obj.name.ToLower().Equals("soelara")
                    || obj.name.ToLower().Equals("lavis")
                    || obj.name.ToLower().Equals("fortrun"))
                    && camera.Zoom < PlanetZoomScale)
                {
                    obj.scale = PlanetZoomScale / camera.Zoom;
                }

                else if (obj is SystemStar
                    && camera.Zoom < StarZoomScale)
                {
                    obj.scale = StarZoomScale / camera.Zoom;
                }

                else if ((obj is Station
                    || obj.name.ToLower().Equals("fortrun station"))
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

        /// <summary>
        /// Draws names of all objects in gameObjects relative to their position
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameObjects">objects which names to be displayed</param>
        public static void DrawOverlay(SpriteBatch spriteBatch, List<GameObjectOverworld> gameObjects)
        {
            if (mapState == MapState.On)
            {
                foreach (GameObjectOverworld obj in gameObjects)
                {
                    spriteBatch.DrawString(FontManager.GetFontStatic(14), obj.name,
                        new Vector2(obj.position.X, obj.position.Y - ((obj.Bounds.Height * obj.scale) / 2) - 300),
                        Color.White, 0f,
                        FontManager.GetFontStatic(14).MeasureString(obj.name) / 2,
                        55, SpriteEffects.None, 1f);
                }

                spriteBatch.DrawString(FontManager.GetFontStatic(14), "Press '" + ControlManager.GetKeyName(RebindableKeys.Map) + "' to hide map..",
                        new Vector2(camera.cameraPos.X, camera.cameraPos.Y + HelpTextOffset),
                        Color.White, 0f,
                        FontManager.GetFontStatic(14).MeasureString("Press '" + ControlManager.GetKeyName(RebindableKeys.Map) + "' to hide map..") / 2,
                        80, SpriteEffects.None, 1f);
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

            if (PopupHandler.IsMessageQueueEmpty)
            {
                Game1.Paused = false;
            }
            camera.Zoom = 1f;
            mapState = MapState.Off;
        }
    }
}
