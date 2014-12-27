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
        public StaticFunctions()
    {}
       
        //int IsPositionOutsideScreenX returns "1" if X-value of Vector2 "pos" is lower than left edge of screen,
        //returns "2" if X-value of "pos" is greater than right edge of screen
        //and returns "0" if none of the above is true
        public static int IsPositionOutsideScreenX(Vector2 pos, Game1 Game)
        {
            if (Game.camera != null)
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

        ////Call to pause
        //public static void Pause(Game1 Game)
        //{
        //    Game.player.IsUsed = false;
        //    Game.stateManager.ChangeState("PauseMenuState");
        //}

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

        private static double lastDouble;
        public static double GetRandomValue()
        {
            Random random = new Random();

        SetValue:
            double value = random.NextDouble();

            if (value == lastDouble)
                goto SetValue;

            lastDouble = value;

            return value;
        }

        private static int lastInt;
        public static int GetRandomValue(int maxValue)
        {
            Random random = new Random();

        SetValue:
            int value = random.Next(maxValue);

            if (value == lastInt)
                goto SetValue;

            lastInt = value;

            return value;
        }

        public static int GetRandomValue(int minValue, int maxValue)
        {
            Random random = new Random();

        SetValue:
            int value = random.Next(minValue, maxValue);

            if (value == lastInt)
                goto SetValue;

            lastInt = value;

            return value;
        }

        public static float GetRandomValue(float minValue, float maxValue)
        {
            Random random = new Random();
            float diff = maxValue - minValue;
            float value = (float)(random.NextDouble() * diff) + minValue;

            return value;
        }

        public static Vector2 NormalizePosition(Vector2 pos, Vector2 origin)
        {
            return new Vector2(pos.X - origin.X, pos.Y - origin.Y);
        }

        public static Vector2 PointToVector2(Point p)
        {
            return new Vector2(p.X, p.Y);
        }
    }

}
