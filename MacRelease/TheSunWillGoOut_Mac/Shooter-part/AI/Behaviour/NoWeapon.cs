using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class NoWeaponBehaviour : Behaviour
    {
        public NoWeaponBehaviour(Game1 Game, AI AI, AlliedShip Ship) :
            base(Game, AI, Ship)
        {
        }

        public override void Action()
        {
            if (ai.ClosestObject != null &&
                CollisionDetection.IsPointInsideCircle(ai.ClosestObject.Position, Ship.Position, ai.AvoidRadius))
                aIAction = AIAction.Avoid;

            else if (ai.FormationArea != null && !Ship.Bounding.Intersects(ai.FormationArea))
                aIAction = AIAction.Formation;

            else
                aIAction = AIAction.Idle;

            base.Action();
        }

        public override void SetTarget(GameObjectVertical closestEnemy)
        { }
    }
}