using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class AreaDisruptorCollision : AreaDamage
    {
        private float radius;
        
        private GameObjectVertical sourceObject;
        public GameObjectVertical SourceObject { get { return sourceObject; } }

        public AreaDisruptorCollision(Game1 game, GameObjectVertical sourceObject, float radius)
            : base(game, AreaDamageType.player, sourceObject.Position)
        {
            this.sourceObject = sourceObject;
            this.radius = radius;

            Damage = 10000;  // TODO: Remove, for testing purposes
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

        //public void InflictDamage(VerticalShooterShip ship)
        //{
        //    ship.InflictDamage(this);
        //    //sourceObject.InflictDamage(bullet);
        //    //bullet.InflictDamage(sourceObject);
        //}
    }
}
