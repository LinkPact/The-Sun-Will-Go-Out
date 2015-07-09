using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class StaticFunctions
    {
        public enum CoordinateLocation
        { 
            above,
            below,
            left,
            right,
            inside
        }

        public StaticFunctions()
        {}

        private static Vector2 windowDimensions;
        private static Boolean isWindowSizeInitiated = false;

        private static void InitiateWindowValues(Game1 game)
        {
            float windowHeight = game.Window.ClientBounds.Height;
            float windowWidth = game.Window.ClientBounds.Width;
            windowDimensions = new Vector2(windowWidth, windowHeight);
        }

        private static Vector2 GetWindowDimensions(Game1 Game) 
        { 
            if (!isWindowSizeInitiated)
            {
                InitiateWindowValues(Game);
                isWindowSizeInitiated = true;
            }
            return windowDimensions;
        }

        public static Boolean IsPositionOutsideScreen(Vector2 pos, Game1 Game)
        {
            Vector2 cameraPos = Game.camera.cameraPos;
            var coordLocation = GetCoordinateLocation(pos, Game, cameraPos);
            return coordLocation != CoordinateLocation.inside;
        }

        public static Vector2 GetCoordinateInsideScreen(Vector2 pos, Game1 Game)
        {
            Vector2 windowDimensions = GetWindowDimensions(Game);
            Vector2 cameraPos = Game.camera.cameraPos;

            var coordLocation = GetCoordinateLocation(pos, Game, cameraPos);

            float newXPos;
            float newYPos;

            if (coordLocation == CoordinateLocation.left || coordLocation == CoordinateLocation.right)
            {
                int lowerYBound = (int)(cameraPos.Y - (windowDimensions.Y / 2));
                int upperYBound = (int)(Game.camera.cameraPos.Y + (windowDimensions.Y / 2));

                newYPos = MathFunctions.GetExternalRandomInt(lowerYBound, upperYBound);

                if (coordLocation == CoordinateLocation.left)
                {
                    newXPos = pos.X + windowDimensions.X;
                }
                else
                {
                    newXPos = pos.X - windowDimensions.X;                
                }
            }
            else if (coordLocation == CoordinateLocation.above || coordLocation == CoordinateLocation.below)
            {
                int lowerXBound = (int)(cameraPos.X - (windowDimensions.X / 2));
                int upperXBound = (int)(Game.camera.cameraPos.X + (windowDimensions.X / 2));

                newXPos = MathFunctions.GetExternalRandomInt(lowerXBound, upperXBound);

                if (coordLocation == CoordinateLocation.above)
                {
                    newYPos = pos.Y + windowDimensions.Y;
                }
                else
                {
                    newYPos = pos.Y - windowDimensions.Y;
                }
            }
            else if (coordLocation == CoordinateLocation.inside)
            {
                newXPos = pos.X;
                newYPos = pos.Y;
            }
            else
            {
                throw new ArgumentException("Non-valid situation encountered!");
            }
            return new Vector2(newXPos, newYPos);
        }

        private static CoordinateLocation GetCoordinateLocation(Vector2 pos, Game1 Game, Vector2 cameraPos)
        {
            Vector2 windowDimensions = GetWindowDimensions(Game);

            if (pos.X < cameraPos.X - (windowDimensions.X / 2))
            {
                return CoordinateLocation.left;
            }
            else if (pos.X > cameraPos.X + (windowDimensions.X / 2))
            {
                return CoordinateLocation.right;
            }
            else if (pos.Y < cameraPos.Y - (windowDimensions.Y / 2))
            {
                return CoordinateLocation.above;
            }
            else if (pos.Y > cameraPos.Y + (windowDimensions.Y / 2))
            {
                return CoordinateLocation.below;
            }
            else
            {
                return CoordinateLocation.inside;
            }
        }

        //int IsPositionOutsideScreenX returns "1" if X-value of Vector2 "pos" is lower than left edge of screen,
        //returns "2" if X-value of "pos" is greater than right edge of screen
        //and returns "0" if none of the above is true
        public static int IsPositionOutsideScreenX(Vector2 pos, Game1 Game)
        {
            if (GameStateManager.currentState.ToLower().Equals("overworldstate"))
            {
                if (pos.X < Game.camera.cameraPos.X - (Game.Window.ClientBounds.Width / 2))
                    return 1;
        
                else if (pos.X > Game.camera.cameraPos.X + (Game.Window.ClientBounds.Width / 2))
                    return 2;
        
                else
                    return 0;
            }
        
            else
            {
                if (pos.X < 0)
                    return 1;
        
                else if (pos.X > Game.Window.ClientBounds.Width )
                    return 2;
        
                else
                    return 0;
            }
        }
        
        //int IsPositionOutsideScreenY returns "1" if Y-value of Vector2 "pos" is lower than top edge of screen,
        //returns "2" if Y-value of "pos" is greater than bottom edge of screen
        //and returns "0" if none of the above is true
        public static int IsPositionOutsideScreenY(Vector2 pos, Game1 Game)
        {
            if (Game.camera != null)
            {
                if (pos.Y < Game.camera.cameraPos.Y - (Game.Window.ClientBounds.Height / 2))
                    return 1;
        
                else if (pos.Y > Game.camera.cameraPos.Y + (Game.Window.ClientBounds.Height / 2))
                    return 2;
        
                else
                    return 0;
            }
        
            else
            {
                if (pos.Y < 0)
                    return 1;
        
                else if (pos.Y > Game.Window.ClientBounds.Height)
                    return 2;
        
                else
                    return 0;
            }
        }

        //Checks if the systemsprites in a list should be drawn to the screen
        public static void CheckObjectUsage(Game1 Game, List<GameObjectOverworld> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Nullable<Rectangle> cameraView = CollisionDetection.CurrentViewRectangle(Game.camera.cameraPos, Game);

                if (list[i].position.X
                    - list[i].sprite.SourceRectangle.Value.Width / 2 <= cameraView.Value.Width &&
                    list[i].position.X
                    + list[i].sprite.SourceRectangle.Value.Width / 2 >= cameraView.Value.X &&
                    list[i].position.Y
                    - list[i].sprite.SourceRectangle.Value.Height / 2 <= cameraView.Value.Height &&
                    list[i].position.Y
                    + list[i].sprite.SourceRectangle.Value.Height / 2 >= cameraView.Value.Y)
                {
                    list[i].IsUsed = true;
                }
                else
                {
                    list[i].IsUsed = false;
                }
            }
        }

        //Checks if Object is out of screen
        public static bool OutOfScreen(GameObjectOverworld Object, Vector2 systemSize)
        {
            return ((Object.position.X + Object.sprite.SourceRectangle.Value.Width / 2) < 0
            || (Object.position.X - Object.sprite.SourceRectangle.Value.Width / 2) > systemSize.X
            || (Object.position.Y + Object.sprite.SourceRectangle.Value.Height / 2) < 0
            || (Object.position.Y - Object.sprite.SourceRectangle.Value.Height / 2) > systemSize.Y);
        }

        //public static SpriteFont GetFont(Game1 Game, bool isBig)
        //{ 
        //    SpriteFont font;
        //
        //    if (isBig)
        //        font = Game.Content.Load<SpriteFont>("Overworld-Sprites/bigFont");
        //
        //    else
        //        font = Game.Content.Load<SpriteFont>("Overworld-Sprites/smallFont");
        //
        //    return font;
        //}

        public static Vector2 NormalizePosition(Vector2 pos, Vector2 origin)
        {
            return new Vector2(pos.X - origin.X, pos.Y - origin.Y);
        }

        public static Vector2 PointToVector2(Point p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static string GetOSName()
        {
            System.OperatingSystem oSInfo =
                System.Environment.OSVersion;

            switch (oSInfo.Version.Major)
            {
                case 5:
                    switch (oSInfo.Version.Minor)
                    {
                        case 0:
                            return "Windows 2000";

                        case 1:
                        case 2:
                            return "Windows XP";

                        default:
                            return "Unknown";
                    }

                case 6:
                    switch (oSInfo.Version.Minor)
                    {
                        case 0:
                            return "Windows Vista";

                        case 1:
                            return "Windows 7";

                        case 2:
                            return "Windows 8";

                        case 3:
                            return "Windows 8.1";

                        default:
                            return "Unknown";
                    }

                case 10:
                    return "Windows 10";

                default:
                    return "Unknown";
            }
        }
    }

}
