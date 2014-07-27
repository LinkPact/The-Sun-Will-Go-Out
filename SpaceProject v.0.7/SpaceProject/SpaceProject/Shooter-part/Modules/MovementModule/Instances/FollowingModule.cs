using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class FollowingModule : MovementModule
    {

        public FollowingModule(Game1 game)
            : base(game)
        { }

        public override void Setup(GameObjectVertical obj)
        {
            obj.FollowObjectTypes.Add("ally");
            obj.FollowObjectTypes.Add("player");

            if (obj.TurningSpeed == 0) 
                obj.TurningSpeed = 5;
        }

        public override void Update(GameTime gameTime, GameObjectVertical obj)
        {
            obj.FindFollowObject();


            if (obj.FollowObject != null && obj.DisableFollowObject <= 0)
            {
                obj.Direction = MathFunctions.ChangeDirection(obj.Direction, obj.Position, obj.FollowObject.Position, obj.TurningSpeed);
                obj.DirectionY = 1.0f;
                obj.Direction = MathFunctions.ScaleDirection(obj.Direction);
            }
            else
            {
                obj.DirectionX = 0;
                obj.DirectionY = 1;
            }
        }
    }
}
