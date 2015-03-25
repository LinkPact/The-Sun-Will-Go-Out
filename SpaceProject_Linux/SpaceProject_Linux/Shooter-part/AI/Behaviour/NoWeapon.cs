using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    public class NoWeaponBehaviour : Behaviour
    {
        public NoWeaponBehaviour(Game1 Game, AI AI, AlliedShip Ship) :
            base(Game, AI, Ship)
        {
        }

        public override void Action()
        {
            if (AI.ClosestObject != null &&
                CollisionDetection.IsPointInsideCircle(AI.ClosestObject.Position, Ship.Position, AI.AvoidRadius))
                aIAction = AIAction.Avoid;

            else if (AI.FormationArea != null && !Ship.Bounding.Intersects(AI.FormationArea))
                aIAction = AIAction.Formation;

            else
                aIAction = AIAction.Idle;

            base.Action();
        }

        public override void SetTarget(GameObjectVertical closestEnemy)
        { }
    }
}