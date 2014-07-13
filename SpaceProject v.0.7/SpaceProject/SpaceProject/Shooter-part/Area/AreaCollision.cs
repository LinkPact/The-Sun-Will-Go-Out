using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class AreaCollision : AreaObject
    {
        private float radius;
        private VerticalShooterShip sourceObject;

        public AreaCollision(Game1 game, VerticalShooterShip sourceObject, float radius)
            : base(game, sourceObject.Position)
        {
            this.sourceObject = sourceObject;
            this.radius = radius;
        }

        public override Boolean IsOverlapping(AnimatedGameObject obj)
        {
            Rectangle rect = new Rectangle((int)obj.PositionX, (int)obj.PositionY, obj.BoundingWidth, obj.BoundingHeight);

            if (CollisionDetection.IsCircleInRectangle(position, radius, rect))
            {
                return true;
            }

            return false;
        }

        public void InflictDamage(Bullet bullet)
        {
            if (sourceObject.ShieldCanTakeHit(bullet.Damage))
            {
                sourceObject.InflictDamage(bullet);
                bullet.InflictDamage(sourceObject);
            }
        }
    }
}
