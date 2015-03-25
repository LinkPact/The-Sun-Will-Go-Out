using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class AreaDisruptorCollision : CircularAreaDamage
    {
        private float radius;
        
        private GameObjectVertical sourceObject;
        public GameObjectVertical SourceObject { get { return sourceObject; } }

        public int DisruptionTimeMilliseconds = 2500;

        public AreaDisruptorCollision(Game1 game, GameObjectVertical sourceObject, float radius)
            : base(game, AreaDamageType.player, sourceObject.Position, 0, radius)
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
    }
}
