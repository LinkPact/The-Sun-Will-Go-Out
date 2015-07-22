using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public enum AIAction
    {
        Idle,
        Attack,
        Avoid,
        Formation
    }

    public class Behaviour
    {
        #region Variables
        protected Game1 Game;
        protected AI ai;
        protected AlliedShip Ship;

        protected AIAction aIAction;
        public AIAction AIAction { get { return aIAction; } }

        public static List<GameObjectVertical> IgnoreList = new List<GameObjectVertical>(); //Objects for AI to ignore targeting
        public static List<GameObjectVertical> GarbageIgnoreList = new List<GameObjectVertical>();

        #endregion

        protected Behaviour(Game1 Game, AI ai, AlliedShip Ship)
        {
            this.Game = Game;
            this.ai = ai;
            this.Ship = Ship;
        }
        
        //Logic
        public virtual void Action()
        { 
            switch (aIAction)
            {
                case AIAction.Idle:
                    Ship.Stop();
                    break;

                case AIAction.Attack:
                    ai.Attack();
                    break;

                case AIAction.Avoid:
                    ai.Avoid();
                    break;

                case AIAction.Formation:
                    Ship.MoveToArea(ai.FormationArea);
                    break;
            }
        }

        //Sets the target to closest enemy by default, can be changed by overriding
        public virtual void SetTarget(GameObjectVertical closestEnemy)
        {
            if (closestEnemy != null && closestEnemy is VerticalShooterShip)
            {
                if (!IgnoreList.Contains(closestEnemy))
                {
                    if(Ship.BoundingY - closestEnemy.BoundingY + closestEnemy.BoundingWidth > -100)
                    ai.Target = (VerticalShooterShip)closestEnemy;
                    IgnoreList.Add(closestEnemy);
                }
            }

            foreach (GameObjectVertical obj in IgnoreList)
            {
                if (obj.IsKilled || obj.IsOutside)
                    GarbageIgnoreList.Add(obj);
            }

            UpdateIgnoreList();
        }

        public static void UpdateIgnoreList()
        {
            foreach (GameObjectVertical obj in GarbageIgnoreList)
                IgnoreList.Remove(obj);

            GarbageIgnoreList.Clear();
        }
    }
}
