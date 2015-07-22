using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class HorizontalModule : MovementModule
    {
        private Boolean leftToRight;

        public HorizontalModule(Game1 game, Boolean leftToRight)
            : base(game)
        {
            this.leftToRight = leftToRight;
        }

        public override void Setup(GameObjectVertical obj)
        {
            if (leftToRight)
            {
                obj.DirectionX = 1f;
            }
            else
            {
                obj.DirectionX = -1;
            }
            obj.DirectionY = 0.2f;
            
            obj.Direction = MathFunctions.ScaleDirection(obj.Direction);
        }

        public override void Update(GameTime gameTime, GameObjectVertical obj)
        { 
            
        }
    }
}
