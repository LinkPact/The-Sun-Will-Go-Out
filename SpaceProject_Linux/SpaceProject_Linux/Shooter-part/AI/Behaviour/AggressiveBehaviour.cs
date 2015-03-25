using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    public class AggressiveBehaviour : Behaviour
    {
        public AggressiveBehaviour(Game1 Game, AI AI, AlliedShip Ship) :
            base(Game, AI, Ship)
        { }

        public override void Action()
        {

        }

        public override void SetTarget(GameObjectVertical closestEnemy)
        {
            base.SetTarget(closestEnemy);
        }
    }
}
