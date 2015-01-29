using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class ZoomMap
    {
        private const float ZOOM_SPEED = 0.0546875f;
        private const float ZOOMED_OUT_VALUE = 0.025f;
        private const float ZOOM_PLAYER_SCALE = 0.25f;

        private static bool zoomingMap;
        private static bool zoomingOut;
        public static bool ZoomingMap { get { return zoomingMap; } private set { ; } }

        public static void ToggleMap()
        {
            zoomingMap = true;
            zoomingOut = !zoomingOut;
            //Game1.Paused = !Game1.Paused;
        }

        public static void Update(GameTime gameTime, List<GameObjectOverworld> gameObjects, Camera camera)
        {
            if (camera.Zoom < ZOOM_PLAYER_SCALE)
            {
                foreach (GameObjectOverworld obj in gameObjects)
                {
                    obj.scale = ZOOM_PLAYER_SCALE / camera.Zoom;
                }
            }

            if (zoomingOut)
            {
                camera.Zoom *= (ZOOM_SPEED * gameTime.ElapsedGameTime.Milliseconds);

                if (camera.Zoom <= ZOOMED_OUT_VALUE)
                {
                    //Game1.Paused = true;
                    zoomingMap = false;
                }
            }
            else
            {
                camera.Zoom *= (1 + (1 - (ZOOM_SPEED * gameTime.ElapsedGameTime.Milliseconds)));

                if (camera.Zoom >= 1f)
                {
                    camera.Zoom = 1f;
                    foreach (GameObjectOverworld obj in gameObjects)
                    {
                        obj.scale = 1;
                    }
                    zoomingMap = false;
                }
            }
        }
    }
}
