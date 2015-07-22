using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class SearchAndLockOnModule : MovementModule
    {

        public SearchAndLockOnModule(Game1 game)
            : base(game)
        { }

        public override void Setup(GameObjectVertical obj)
        {
            obj.FollowObjectTypes.Add("ally");
            obj.FollowObjectTypes.Add("player");
        }

        public override void Update(GameTime gameTime, GameObjectVertical obj)
        {
            obj.FindFollowObject();

            if (obj.FollowObject != null && obj.DisableFollowObject <= 0)
            {
                obj.Direction = MathFunctions.ChangeDirection(gameTime, obj.Direction, obj.Position, obj.FollowObject.Position, obj.TurningSpeed);
                obj.Direction = MathFunctions.ScaleDirection(obj.Direction);
            }
        }
    }
}
