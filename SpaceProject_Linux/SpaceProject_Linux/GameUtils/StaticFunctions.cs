using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class StaticFunctions
    {
        public StaticFunctions()
    {}
       
        //int IsPositionOutsideScreenX returns "1" if X-value of Vector2 "pos" is lower than left edge of screen,
        //returns "2" if X-value of "pos" is greater than right edge of screen
        //and returns "0" if none of the above is true
        public static int IsPositionOutsideScreenX(Vector2 pos, Game1 Game)
        {
            if (GameStateManager.currentState.ToLower().Equals("overworldstate"))
            {
                if (pos.X < Game.camera.cameraPos.X - (Game.ScreenSize.X / 2))
                    return 1;

                else if (pos.X > Game.camera.cameraPos.X + (Game.ScreenSize.X / 2))
                    return 2;

                else
                    return 0;
            }

            else
            {
                if (pos.X < 0)
                    return 1;

                else if (pos.X > Game.ScreenSize.X )
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
                if (pos.Y < Game.camera.cameraPos.Y - (Game.ScreenSize.Y / 2))
                    return 1;

                else if (pos.Y > Game.camera.cameraPos.Y + (Game.ScreenSize.Y / 2))
                    return 2;

                else
                    return 0;
            }

            else
            {
                if (pos.Y < 0)
                    return 1;

                else if (pos.Y > Game.ScreenSize.Y)
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
