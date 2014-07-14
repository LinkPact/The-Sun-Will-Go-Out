using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class CircularAreaDamage : AreaDamage
    {
        private float radius;

        public CircularAreaDamage(Game1 game, AreaDamageType type, Vector2 position, float radius)
            : base(game, type, position)
        {
            this.radius = radius;
        }

        public override Boolean IsOverlapping(AnimatedGameObject obj)
        {
            Rectangle rect = new Rectangle((int)obj.PositionX, (int)obj.PositionY, obj.BoundingWidth, obj.BoundingHeight);

            if (CollisionDetection.IsCircleInRectangle(Position, radius, rect))
            {
                return true;
            }

            return false;
        }
    }
}
