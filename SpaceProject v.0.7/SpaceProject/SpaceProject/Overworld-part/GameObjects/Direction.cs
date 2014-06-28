﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class Direction
    {
        private Vector2 scaledDirection;

        public Direction Zero
        {
            get { return new Direction(Vector2.Zero); }
        }

        public Direction() { }

        public Direction(Vector2 direction)
        {
            scaledDirection = direction;
        }

        // Set direction from vector
        public void SetDirection(Vector2 direction)
        {
            scaledDirection = GlobalFunctions.ScaleDirection(direction);
        }

        // Set direction from radians
        public void SetDirection(double radians)
        {
            SetDirection(new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians)));
        }

        // Set direction from degrees
        public void SetDirection(float degrees)
        {
            SetDirection(new Vector2((float)Math.Cos(degrees * (Math.PI / 180)), (float)Math.Sin(degrees * (Math.PI / 180))));
        }

        public Vector2 GetDirectionAsVector()
        {
            return scaledDirection;
        }

        public double GetDirectionAsRadian()
        {
            return Math.Atan2(scaledDirection.Y, scaledDirection.X);
        }

        public float GetDirectionAsDegree()
        {
            return (float)(Math.Atan2(scaledDirection.Y, scaledDirection.X) * (180 / Math.PI));
        }

        public void RotateTowardsPoint(Vector2 startingPoint, Vector2 preferredPoint, float rotateSpeed)
        {
            SetDirection(scaledDirection + (preferredPoint - startingPoint) * rotateSpeed);
        }
    }
}
