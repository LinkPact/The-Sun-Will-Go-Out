using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public class StandardBehaviour : Behaviour
    {
        public StandardBehaviour(Game1 Game, AI AI, AlliedShip Ship) :
            base(Game, AI, Ship)
        { }

        public override void Action()
        {
            if (ai.Target != null && ai.TargetYDistance > 25)
                aIAction = AIAction.Attack;

            else if (ai.ClosestObject != null &&
                CollisionDetection.IsPointInsideCircle(ai.ClosestObject.Position, Ship.Position, ai.AvoidRadius))
                aIAction = AIAction.Avoid;

            else if (ai.FormationArea.X != -1 && !Ship.Bounding.Intersects(ai.FormationArea))
                aIAction = AIAction.Formation;

            else
                aIAction = AIAction.Idle;

            base.Action();
        }

        public override void SetTarget(GameObjectVertical closestEnemy)
        {
            if (closestEnemy is Meteorite15
                || closestEnemy is Meteorite20
                || closestEnemy is Meteorite25
                || closestEnemy is Meteorite30)
            {
                return;
            }

            base.SetTarget(closestEnemy);
        }
    }
}