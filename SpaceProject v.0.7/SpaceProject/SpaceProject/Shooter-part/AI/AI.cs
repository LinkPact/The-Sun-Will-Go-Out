using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Used when creating new AI to set behaviour
    public enum AIBehaviour
    { 
        Standard,
        Aggressive,
        NoWeapon
    }

    public class AI
    {
        #region Variables
        private Game1 Game;
        private Sprite spriteSheet;
        public AlliedShip ship;

        private Behaviour behaviour;

        //movement stuff
        private Vector2 prefferedDir;

        private bool switchDirX;
        private bool switchDirY;

        private bool accelerate;
        private bool deccelerate;

        //target and shooting stuff
        private VerticalShooterShip target;
        private GameObjectVertical closestObject;

        private string weapon;

        private float targetXDistance;
        private float targetYDistance;

        private float shotRange;

        public float AvoidRadius;   //Avoids objects that are closer than this value
        public List<GameObjectVertical> AvoidList;
        public List<GameObjectVertical> GarbageAvoidList;

        public Rectangle FormationArea;

        #endregion

        #region Properties

        public Behaviour Behaviour { get { return behaviour; } }

        public Vector2 PrefferedDir { get { return prefferedDir; } set { prefferedDir = value; } }

        public bool SwitchDirX { get { return switchDirX; } set { switchDirX = value; } }
        public bool SwitchDirY { get { return switchDirY; } set { switchDirY = value; } }

        public bool Accelerate { get { return accelerate; } set { accelerate = value; } }
        public bool Deccelerate { get { return deccelerate; } set { deccelerate = value; } }

        public VerticalShooterShip Target { get { return target; } set { target = value; } }
        public GameObjectVertical ClosestObject { get { return closestObject; } }

        public float TargetXDistance { get { return targetXDistance; } set { targetXDistance = value; } }
        public float TargetYDistance { get { return targetYDistance; } set { targetYDistance = value; } }

        public float ShotRange { get { return shotRange; } }

        #endregion

        public AI(Game1 Game, Sprite spriteSheet, AlliedShip ship, AIBehaviour behaviour, string weapon, float avoidRadius, Rectangle formationArea)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
            this.ship = ship;
            this.AvoidRadius = avoidRadius;
            this.FormationArea = formationArea;

            switch (behaviour)
            {
                case AIBehaviour.Standard:
                    this.behaviour = new StandardBehaviour(Game, this, ship);
                    break;

                case AIBehaviour.Aggressive:
                    throw new NotImplementedException("Not implemented!");

                case AIBehaviour.NoWeapon:
                    this.behaviour = new NoWeaponBehaviour(Game, this, ship);
                    break;

                default:
                    break;
            }

            switch (weapon.ToLower())
            {
                case "basiclaser":
                    shotRange = 300;
                    this.weapon = weapon;
                    break;

                case "none":
                case "":
                    shotRange = 0;
                    this.weapon = "none";
                    break;

                default:
                    shotRange = 0;
                    this.weapon = "none";
                    break;
            }

            AvoidList = new List<GameObjectVertical>();
            GarbageAvoidList = new List<GameObjectVertical>();
        }

        public void Initialize()
        {
        }

        #region AI

        public void Process(GameTime gameTime)
        {
            //Always updates the closest enemy, used for setting target
            closestObject = GlobalMathFunctions.ReturnClosestObject(ship, ship.SightRange,
                Game.stateManager.shooterState.gameObjects, new List<string>(){ "enemy", "enemyBullet"});

            if (target == null)
                behaviour.SetTarget(closestObject);

            else
            {
                targetYDistance = ship.BoundingY - (target.BoundingY + target.BoundingHeight);
                targetXDistance = ship.PositionX - (target.BoundingX + target.BoundingWidth);
                if(targetXDistance < 0) { targetXDistance *= -1; }

                //Removes target when dead or too far from AI-controlled ship
                if (target.IsOutside || target.IsKilled || targetYDistance < -100 || targetXDistance > 400)
                {
                    if (Behaviour.IgnoreList.Contains(target))
                    {
                        Behaviour.GarbageIgnoreList.Add(target);
                        Behaviour.UpdateIgnoreList();
                    }

                    target = null;
                }
            }

            //Calls the behaviour and asks which action to take
            behaviour.Action();
        }

        public void Attack()
        {
            #region Move to shooting position

            if (ship.BoundingX + ship.BoundingWidth < Game.Window.ClientBounds.Width - 20 &&
                targetYDistance > 0 && 
                ship.PositionX < target.BoundingX)
                ship.Move(new Vector2(1, ship.DirectionY));

            else if (ship.BoundingX > 20 &&
                targetYDistance > 0 &&
                ship.PositionX > target.BoundingX + target.BoundingWidth)
                ship.Move(new Vector2(-1, ship.DirectionY));

            else
                ship.Stop("x");

            if (ship.BoundingY > 20 &&
                targetYDistance > shotRange &&
                ship.BoundingY > 10)
                ship.Move(new Vector2(ship.DirectionX, -1));

            else if (ship.BoundingY + ship.BoundingHeight < (Game.Window.ClientBounds.Height - 600) / 2 + 600 &&
                targetYDistance > -20 && targetYDistance < 50 &&
                ship.BoundingY + ship.BoundingHeight < Game.Window.ClientBounds.Height - 10)
                ship.Move(new Vector2(ship.DirectionX, 1));

            else
                ship.Stop("y");

            #endregion

            #region Shoot

            if (ship.PositionX < target.BoundingX + target.BoundingWidth &&
                ship.PositionX > target.BoundingX &&
                targetYDistance < shotRange)
                ship.Shoot();

            #endregion
        }

        private GameObjectVertical objToAvoid;
        public void Avoid()
        {
            foreach (GameObjectVertical obj in Game.stateManager.shooterState.gameObjects)
            {
                if (obj.ObjectClass == "enemy" || obj.ObjectClass == "enemyBullet")
                {
                    if (CollisionDetection.IsPointInsideCircle(obj.Position, ship.Position, AvoidRadius))
                        AvoidList.Add(obj);
                }
            }

            if (AvoidList.Count > 0)
                objToAvoid = GlobalMathFunctions.ReturnClosestObject(ship, AvoidRadius, AvoidList);

            if (objToAvoid == target)
            {
                Behaviour.GarbageIgnoreList.Add(target);
                Behaviour.UpdateIgnoreList();
                target = null;
            }

            if(AvoidList.Count > 0 && objToAvoid != null)
            { 
                //enemy is to the right on AI-ship's center
                if (objToAvoid.BoundingX + objToAvoid.BoundingWidth >= ship.PositionX &&
                    objToAvoid.BoundingX - 20 < ship.PositionX + ship.CenterPointX)
                {
                    if(ship.BoundingX > (Game.Window.ClientBounds.Width - Game.stateManager.shooterState.CurrentLevel.LevelWidth) / 2 + 20)
                        ship.Move(new Vector2(-1, ship.DirectionY));
                }

                else if (objToAvoid.BoundingX < ship.PositionX
                    && objToAvoid.BoundingX + objToAvoid.BoundingWidth + 20 > ship.BoundingX)
                {
                    if(ship.BoundingX + ship.BoundingWidth < Game.stateManager.shooterState.CurrentLevel.LevelWidth - 20)
                        ship.Move(new Vector2(1, ship.DirectionY));
                }

                else
                    ship.Stop("x");

            }

            for (int i = 0; i < AvoidList.Count; i++ )
            {
                if (AvoidList[i].IsKilled || AvoidList[i].IsOutside ||
                    !CollisionDetection.IsPointInsideCircle(AvoidList[i].Position, ship.Position, AvoidRadius))
                {
                    GarbageAvoidList.Add(AvoidList[i]);

                    if(objToAvoid == AvoidList[i])
                        objToAvoid = null;
                }
            }

            foreach (GameObjectVertical obj in GarbageAvoidList)
                AvoidList.Remove(obj);

            GarbageAvoidList.Clear();
        }
        #endregion
    }
}
