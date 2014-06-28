﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class StandardBehaviour : Behaviour
    {
        public StandardBehaviour(Game1 Game, AI AI, AlliedShip Ship) :
            base(Game, AI, Ship)
        { }

        public override void Action()
        {
            if (AI.Target != null && AI.TargetYDistance > 25)
                aIAction = AIAction.Attack;

            else if (AI.ClosestObject != null &&
                CollisionDetection.IsPointInsideCircle(AI.ClosestObject.Position, Ship.Position, AI.AvoidRadius))
                aIAction = AIAction.Avoid;

            else if (AI.FormationArea.X != -1 && !Ship.Bounding.Intersects(AI.FormationArea))
                aIAction = AIAction.Formation;

            else
                aIAction = AIAction.Idle;

            base.Action();
        }

        public override void SetTarget(GameObjectVertical closestEnemy)
        {
            base.SetTarget(closestEnemy);
        }
    }
}