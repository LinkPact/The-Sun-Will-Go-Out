using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class CrossOverModule : MovementModule
    {
        protected float endX;
        protected bool endReached;
        protected bool isLeft;

        public CrossOverModule(Game1 game)
            : base(game)
        { 
            
        }

        public override void Setup(GameObjectVertical obj)
        {
            float windowMiddlePos = windowWidth / 2;

            if (obj.PositionX < windowMiddlePos)
            {
                endX = windowMiddlePos + (windowMiddlePos - obj.PositionX);
                isLeft = true;
            }
            else
            {
                endX = windowMiddlePos - (obj.PositionX - windowMiddlePos);
                isLeft = false;
            }
            endReached = false;
        }

        public override void Update(GameTime gameTime, GameObjectVertical obj)
        {
            if (!endReached)
            {
                if (obj.PositionY > 50)
                {
                    if (isLeft)
                    {
                        obj.DirectionX = 1f;
                    }
                    else
                    {
                        obj.DirectionX = -1f;
                    }
                }
            }

            if (isLeft && obj.PositionX > endX)
            {
                endReached = true;
                obj.DirectionX = 0;
            }
            else if (!isLeft && obj.PositionX < endX)
            {
                endReached = true;
                obj.DirectionX = 0;
            }
        }
    }
}
