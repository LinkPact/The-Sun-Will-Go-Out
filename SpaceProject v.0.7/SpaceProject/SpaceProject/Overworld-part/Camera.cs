using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Camera
    {
        public Vector2 cameraPos;

        private const float CameraPanSpeed = 75f;
        private const float ZoomUpperLimit = 1.5f;
        private const float ZoomLowerLimit = 0.0001f;

        private Vector2 originalCameraPos;

        private float cameraZoom;
        private Matrix transform;
        private float cameraRotation;
        private int viewportWidth;
        private int viewportHeight;

        public int WorldWidth;
        public int WorldHeight;
 
        private float leftBarrier;
        private float rightBarrier;
        private float topBarrier;
        private float bottomBarrier;

        private Game1 Game;

        public Camera(int worldWidth2, int worldHeight2, float initialCameraZoom, Game1 Game)
        {
            cameraZoom = initialCameraZoom;
            cameraRotation = 0.0f;
            cameraPos = Game.player.position;
            viewportWidth = Game1.ScreenSize.X;
            viewportHeight = Game1.ScreenSize.Y;
            WorldWidth = worldWidth2;
            WorldHeight = worldHeight2;

            this.Game = Game;
        }

        public float Zoom
        {
            get { return cameraZoom; }
            set {
                cameraZoom = value;
                if (cameraZoom < ZoomLowerLimit)
                    cameraZoom = ZoomLowerLimit;

                if (cameraZoom > ZoomUpperLimit)
                    cameraZoom = ZoomUpperLimit;
                }

        }

        public float Rotation
        {
            get { return cameraZoom; }
            set { cameraZoom = value; }
        }

        public void Move(Vector2 amount)
        {
            cameraPos += amount;
        }

        //Clamps camera view to edges if world height and world width if the view is outside these
        public Vector2 Position
        {
            get { return cameraPos; }
            private set 
            {

                leftBarrier = (float)viewportWidth * 0.5f / cameraZoom;
                rightBarrier = WorldWidth - (float)viewportWidth * 0.5f / cameraZoom;
                topBarrier = WorldHeight - (float)viewportHeight * 0.5f / cameraZoom;
                bottomBarrier = (float)viewportHeight * 0.5f / cameraZoom;

                cameraPos = value;

                if (cameraPos.X > rightBarrier)
                    cameraPos.X = rightBarrier;

                if (cameraPos.X < leftBarrier)
                    cameraPos.X = leftBarrier;

                if (cameraPos.Y > topBarrier)
                    cameraPos.Y = topBarrier;

                if (cameraPos.Y < bottomBarrier)
                    cameraPos.Y = bottomBarrier;

            }
        }

        public Matrix GetTransformation()
        {
            transform =
                Matrix.CreateTranslation(new Vector3(-cameraPos.X, -cameraPos.Y, 0)) *
                Matrix.CreateRotationZ(cameraRotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(viewportWidth * 0.5f, viewportHeight * 0.5f, 0));

            return transform;
        }

        // Updates camera position relative to players position if camera is within bounds of screen 
        public void CameraUpdate(GameTime gameTime, PlayerOverworld player)
        {
            // Camera panning
            if (ZoomMap.MapState == MapState.On)
            {
                if (ControlManager.CheckHold(RebindableKeys.Right))
                {
                    cameraPos.X += CameraPanSpeed * MathFunctions.FPSSyncFactor(gameTime);
                }

                else if (ControlManager.CheckHold(RebindableKeys.Left))
                {
                    cameraPos.X -= CameraPanSpeed * MathFunctions.FPSSyncFactor(gameTime);
                }

                if (ControlManager.CheckHold(RebindableKeys.Up))
                {
                    cameraPos.Y -= CameraPanSpeed * MathFunctions.FPSSyncFactor(gameTime);
                }

                else if (ControlManager.CheckHold(RebindableKeys.Down))
                {
                    cameraPos.Y += CameraPanSpeed * MathFunctions.FPSSyncFactor(gameTime);
                }
            }

            else
            {
                originalCameraPos = cameraPos;
            }

            if (ZoomMap.MapState != MapState.On
                && ((cameraPos.X - (Game1.ScreenSize.X / 2) >= 0 && (cameraPos.X + Game1.ScreenSize.X / 2) <= WorldWidth)
                   || (cameraPos.Y - (Game1.ScreenSize.Y / 2) >= 0 && cameraPos.Y + (Game1.ScreenSize.Y / 2) <= WorldHeight)))
            {
                Position = player.position;
            }
        }
    }
}