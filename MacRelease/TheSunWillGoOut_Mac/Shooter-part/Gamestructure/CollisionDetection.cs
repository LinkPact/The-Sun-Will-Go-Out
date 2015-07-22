using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class CollisionDetection
    {
        public static Nullable<Rectangle> CurrentViewRectangle(Vector2 camPos, Game1 Game)
        {
            return new Rectangle((int)Game.camera.cameraPos.X - (Game1.ScreenSize.X / 2),
                (int)Game.camera.cameraPos.Y - (Game1.ScreenSize.Y / 2),
                (int)Game.camera.cameraPos.X - (Game1.ScreenSize.X / 2) + Game1.ScreenSize.X,
                (int)Game.camera.cameraPos.Y - (Game1.ScreenSize.Y / 2) + Game1.ScreenSize.Y);
        }
        
        public static bool IsPointInsideRectangle(Vector2 point, Rectangle rect)
        {
            Vector2 topLeft = new Vector2(rect.X, rect.Y);
            Vector2 bottomRight = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);

            if ((point.X >= topLeft.X && point.X <= bottomRight.X) && (point.Y >= topLeft.Y && point.Y <= bottomRight.Y))
            {
                return true;
            }

            return false;
        }

        public static bool IsPointInsideCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
        {
            float distance = (float)Math.Sqrt(Math.Pow(circleCenter.X - point.X, 2) + Math.Pow(circleCenter.Y - point.Y, 2));

            return (distance <= circleRadius);
        }

        public static bool IsRectInRect(Rectangle rect1, Rectangle rect2)
        {
            float left1 = rect1.X;
            float right1 = rect1.X + rect1.Width;
            float top1 = rect1.Y;
            float bottom1 = rect1.Y + rect1.Height;

            float left2 = rect2.X;
            float right2 = rect2.X + rect2.Width;
            float top2 = rect2.Y;
            float bottom2 = rect2.Y + rect2.Height;

            if (top1 > bottom2) return false;
            if (top2 > bottom1) return false;
            if (left1 > right2) return false;
            if (left2 > right1) return false;

            return true;
        }

        public static bool IsCircleInCircle(Vector2 center1, float radius1, Vector2 center2, float radius2)
        {
            float distance = (float)Math.Sqrt(Math.Pow(center1.X - center2.X, 2) + Math.Pow(center1.Y - center2.Y, 2));
            return (distance <= (radius1 + radius2));
        }

        public static bool IsCircleInRectangle(Vector2 circle, float radius, Rectangle rect)
        {
            Vector2 circleDistance;

            circleDistance.X = Math.Abs(circle.X - rect.Center.X);
            circleDistance.Y = Math.Abs(circle.Y - rect.Center.Y);

            if (circleDistance.X > (rect.Width / 2 + radius)) { return false; }
            if (circleDistance.Y > (rect.Height/ 2 + radius)) { return false; }

            if (circleDistance.X <= (rect.Width / 2)) { return true; }
            if (circleDistance.Y <= (rect.Height / 2)) { return true; }

            double cornerDistanceSquared = Math.Pow((circleDistance.X - rect.Width / 2), 2) +
                Math.Pow((circleDistance.Y - rect.Height / 2), 2);

            return (cornerDistanceSquared <= (radius * radius));
        }

        #region Per-Pixel Collision Detection

        public static Rectangle Normalize(Rectangle rect, Vector2 pos, Vector2 origin, Nullable<Rectangle> sourceRectangle )
        {
            return new Rectangle(rect.X - (int)pos.X + sourceRectangle.Value.X + (int)origin.X,
                                 rect.Y - (int)pos.Y + sourceRectangle.Value.Y + (int)origin.Y,
                                 rect.Width,
                                 rect.Height);
        }

        //Returns the rectangle where two other rectangles are intersecting
        public static Rectangle IntersectRectangle(Rectangle rect1, Rectangle rect2)
        {
            int x1 = Math.Max(rect1.Left, rect2.Left);
            int y1 = Math.Max(rect1.Top, rect2.Top);

            int x2 = Math.Min(rect1.Right, rect2.Right);
            int y2 = Math.Min(rect1.Bottom, rect2.Bottom);

            int width = x2 - x1;
            int height = y2 - y1;

            if (height > 0 && width > 0)
                return new Rectangle(x1, y1, width, height);

            else
                return Rectangle.Empty;
        }

        private const int ALPHA_THRESHOLD = 50;

        public static bool VisiblePixelsColliding(Rectangle bounds1, Rectangle bounds2,
                           Sprite sprite1, Sprite sprite2,
                           Vector2 origin1, Vector2 origin2)
        {
            Rectangle intersectRect = IntersectRectangle(bounds1, bounds2);

            if (intersectRect == Rectangle.Empty)
                return false;

            int pixelCount = intersectRect.Width * intersectRect.Height;

            Color[] pixels1 = new Color[pixelCount];
            Color[] pixels2 = new Color[pixelCount];

            sprite1.Texture.GetData<Color>(0, Normalize(intersectRect, new Vector2(bounds1.X, bounds1.Y),
                Vector2.Zero, sprite1.SourceRectangle), pixels1, 0, pixelCount);

            sprite2.Texture.GetData<Color>(0, Normalize(intersectRect, new Vector2(bounds2.X, bounds2.Y),
                Vector2.Zero, sprite2.SourceRectangle), pixels2, 0, pixelCount);

            for (int i = 0; i < pixelCount; i++)
            {
                if (pixels1[i].A > ALPHA_THRESHOLD && pixels2[i].A > ALPHA_THRESHOLD)
                    return true;
            }

            return false;
        }

        public static bool IsLinesIntersecting(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
            float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
            float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }

        public static bool IsLineInRect(Rectangle rect, Vector2 a, Vector2 b)
        {
            return IsLinesIntersecting(a, b, new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y)) ||
                IsLinesIntersecting(a, b, new Vector2(rect.X, rect.Y), new Vector2(rect.X, rect.Y + rect.Width)) ||
                IsLinesIntersecting(a, b, new Vector2(rect.X, rect.Y + rect.Height), new Vector2(rect.X + rect.Width, rect.Y + rect.Height)) ||
                IsLinesIntersecting(a, b, new Vector2(rect.X + rect.Width, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height));
        }

        #endregion

    }
}
