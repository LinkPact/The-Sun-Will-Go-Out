using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class AreaShieldCollision : AreaObject
    {
        private float radius;
        
        private VerticalShooterShip sourceObject;
        public VerticalShooterShip SourceObject { get { return sourceObject; } }

        public AreaShieldCollision(Game1 game, VerticalShooterShip sourceObject, float radius)
            : base(game, sourceObject.Position)
        {
            this.sourceObject = sourceObject;
            this.radius = radius;
        }

        public override Boolean IsOverlapping(AnimatedGameObject obj)
        {
            Rectangle rect = new Rectangle((int)obj.PositionX, (int)obj.PositionY, 
                obj.BoundingWidth, obj.BoundingHeight);

            if (CollisionDetection.IsCircleInRectangle(sourceObject.Position, radius, rect))
            {
                return true;
            }

            return false;
        }

        public void InflictDamage(Bullet bullet)
        {
            sourceObject.InflictDamage(bullet);
            //bullet.InflictDamage(sourceObject);
        }

        public override void OnKilled()
        { }
    }
}
