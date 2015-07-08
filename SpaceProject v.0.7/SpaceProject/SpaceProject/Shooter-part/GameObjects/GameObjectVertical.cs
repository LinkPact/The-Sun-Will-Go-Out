using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Grundklassen for units
    public abstract class GameObjectVertical
    {
        #region declaration
        public ConfigFile config;

        public int windowWidth { get; set; }
        public int windowHeight { get; set; }
        public int relativeOrigin;
        public int LevelWidth; 

        public string ObjectClass;
        public string ObjectSubClass;
        public string ObjectName;

        private int degreeChange;
        public int DegreeChange { get { return degreeChange; } set { degreeChange = value; } }

        private double radians;
        public double Radians { get { return radians; } set { radians = value; } }

        private float hp;
        public float HP { 
            get {
                if (hp > 0)
                {
                    return hp;
                }
                else
                {
                    return 0;
                }
            } set { hp = value; } 
        }

        private float hpmax;
        public float HPmax { get { return hpmax; } set { hpmax = value; } }

        private float shield;
        public float Shield { get { return shield; } set { shield = value; } }

        private float shieldMax;
        public float ShieldMax { get { return shieldMax; } set { shieldMax = value; } }

        private float damage;
        public float Damage { get { return damage; } set { damage = value; } }

        private float duration;
        public float Duration { get { return duration; } set { duration = value; } }

        private float tempInvincibility;
        public float TempInvincibility { get { return tempInvincibility; } set { tempInvincibility = value; } }

        private float sightRange;
        public float SightRange { get { return sightRange; } set { sightRange = value; } }

        protected AreaShieldCollision areaCollision;
        public AreaShieldCollision AreaCollision { get { return areaCollision; } }
        public Boolean HasAreaCollision() { return areaCollision != null; }

        //Follow variables
        private bool follows;
        protected bool Follows { get { return follows; } set { follows = value; } }

        private GameObjectVertical followObject;
        public GameObjectVertical FollowObject { get { return followObject; } set { followObject = value; } }

        private List<String> followObjectTypes = new List<String>();
        public List<String> FollowObjectTypes { get { return followObjectTypes; } private set { } }

        private int disableFollowObject;
        public int DisableFollowObject { get { return disableFollowObject; } set { disableFollowObject = value; } }

        private float turningSpeed;
        public float TurningSpeed { get { return turningSpeed; } set { turningSpeed = value; } }

        //Aiming variables
        private GameObjectVertical shootObject;
        public GameObjectVertical ShootObject { get { return shootObject; } set { shootObject = value; } }

        private List<String> shootObjectTypes = new List<String>();
        public List<String> ShootObjectTypes { get { return shootObjectTypes; }}
        public void AssignShootObjectTypes(List<String> shootObjectTypes)
        {
            this.shootObjectTypes = shootObjectTypes;
        }

        //Position related
        private Vector2 _position;
        public Vector2 Position{ get { return _position; } set {  _position = value; } }
        public float PositionX{ get { return _position.X ; } set { _position.X = value; } }
        public float PositionY { get { return _position.Y; } set { _position.Y = value; } }

        private Vector2 _centerPoint;
        public Vector2 CenterPoint { get { return _centerPoint; } set { _centerPoint = value; } }
        public float CenterPointX { get { return _centerPoint.X; } set { _centerPoint.X = value; } }
        public float CenterPointY { get { return _centerPoint.Y; } set { _centerPoint.Y = value; } }

        private Vector2 _direction;
        public Vector2 Direction { get { return _direction; } set { _direction = value; } }
        public float DirectionX { get { return _direction.X; } set { _direction.X = value; } }
        public float DirectionY { get { return _direction.Y; } set { _direction.Y = value; } }

        public Rectangle bounding;
        public Rectangle Bounding { get { return bounding; } set { bounding = value; } }
        public int BoundingX { get { return bounding.X; } set { bounding.X = value; } }
        public int BoundingY { get { return bounding.Y; } set { bounding.Y = value; } }
        public int BoundingWidth { get { return bounding.Width; } set { bounding.Width = value; } }
        public int BoundingHeight { get { return bounding.Height; } set { bounding.Height = value; } }
        public int BoundingSpace { get; set; }

        public bool Enable { get; set; }
        public float Speed { get; set; }
        public bool IsKilled { get; set; }
        public bool IsOutside { get; set; }
        public float DrawLayer { get; set; }
        protected Game1 Game { get; private set; }

        // Sound
        protected SoundEffects deathSoundID = SoundEffects.SmallExplosion;
        public SoundEffects getDeathSoundID() { return deathSoundID; }
        protected float soundPan;
        public float SoundPan { get { return soundPan; } private set { ;} }

        // Misc
        protected Tracker lastHitBy = Tracker.None;
        public Tracker LastHitBy { get { return lastHitBy; } set { lastHitBy = value; } }

        protected int disruptionTime = 0;
        public int DisruptionTime { get { return disruptionTime; } }

        #endregion
        
        public GameObjectVertical(Game1 Game)
        {
            this.Game = Game;
            Enable = true;

            DrawLayer = 0.5f;
            
            BoundingSpace = 0;
            windowWidth = Game.Window.ClientBounds.Width;
            windowHeight = Game.Window.ClientBounds.Height;
            SetLevelWidth(800);

            ObjectClass = null;
            damage = 0;
            hp = 1;
            shield = 0;
            DegreeChange = 2;
        }
        
        public virtual void Initialize()
        {
            bounding = new Rectangle();
            followObject = null;

            if (ObjectName == null) ObjectName = "";
        }
        
        public virtual void DeInitialize() { }
        
        public virtual void Update(GameTime gameTime)
        {
            soundPan = (_position.X - Game.stateManager.shooterState.CurrentLevel.PlayerPosition.X) 
                / Game.stateManager.shooterState.CurrentLevel.LevelWidth;

            if (Enable)
            {
                Position += Direction * Speed * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (tempInvincibility > 0)
            {
                tempInvincibility -= gameTime.ElapsedGameTime.Milliseconds;
            }

            if (hp <= 0)
            {
                if (this is AllianceFighterAlly)
                    ((AllianceFighterAlly)this).OnKilled();

                IsKilled = true;
            }

            //Set degree
            if (DirectionY > 0)
                Radians = Math.Acos(DirectionX);
            else
                Radians = 2 * Math.PI - (Math.Acos(DirectionX));

            if (Radians >= 2 * Math.PI)
                Radians -= 2 * Math.PI;

            //Hantering av "Disable FollowObject"
            if (disableFollowObject > 0)
                disableFollowObject -= gameTime.ElapsedGameTime.Milliseconds;

            if (Follows)
            {
                FindFollowObject();

                if (FollowObject != null)    
                    UpdateFollowing();                
            }

            //Bullet-duration
            if (this is Bullet)
            {
                if (duration > 0) { duration -= gameTime.ElapsedGameTime.Milliseconds; }
                else { IsKilled = true;}
            }

            if (shootObject != null)
                if (shootObject.IsKilled || shootObject.IsOutside) 
                    shootObject = null;

            if (IsKilled)
                OnKilled();
        }
        
        public virtual void Draw(SpriteBatch spriteBatch)
        { }

        private void UpdateFollowing()
        {
            if (TurningSpeed == 0) TurningSpeed = 1;

            Direction = MathFunctions.ChangeDirection(Direction, Position, FollowObject.Position, TurningSpeed);
            Direction = MathFunctions.ScaleDirection(Direction);
        }
        
        public GameObjectVertical FindFollowObject()
        {
            if (FollowObject == null)
            {
                List<GameObjectVertical> objectList = Game.stateManager.shooterState.gameObjects;

                foreach (String type in FollowObjectTypes)
                {
                    foreach (GameObjectVertical obj in objectList)
                    {
                        if (obj.ObjectClass.Equals(type))
                        {
                            if (CollisionDetection.IsPointInsideCircle(obj.Position, Position, SightRange))
                            {
                                followObject = obj;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!CollisionDetection.IsPointInsideCircle(FollowObject.Position, Position, SightRange))
                {
                    FollowObject = null;
                }
            }
            return null;
        }

        public float FindClosestAimDistance()
        {
            GameObjectVertical aimObject = FindAimObject();

            float distance;
            if (aimObject != null)
                distance = MathFunctions.ObjectDistance(this, aimObject);
            else
                distance = 4000;

            return distance;
        }

        public GameObjectVertical FindAimObject()
        {
            if (ShootObject == null)
            {
                var targetsWithinRange = GetTargetsWithinRange(SightRange);

                if (targetsWithinRange.Count > 0)
                {
                    shootObject = targetsWithinRange[0];
                    foreach (GameObjectVertical obj in targetsWithinRange)
                    {
                        if (MathFunctions.ObjectDistance(this, obj) < MathFunctions.ObjectDistance(this, shootObject))
                        {
                            shootObject = obj;
                        }
                    }
                }
            }
            else
            {
                if (!CollisionDetection.IsPointInsideCircle(ShootObject.Position, Position, SightRange))
                {
                    ShootObject = null;
                }
            }
            return ShootObject;
        }

        public List<GameObjectVertical> GetTargetsWithinRange(float range)
        {
            var allGameObjects = Game.stateManager.shooterState.gameObjects;
            var targetsWithinRange = new List<GameObjectVertical>();

            foreach (String type in ShootObjectTypes)
            {
                foreach (GameObjectVertical obj in allGameObjects)
                {
                    if (obj.ObjectClass.Equals(type))
                    {
                        if (CollisionDetection.IsPointInsideCircle(obj.Position, Position, range))
                        {
                            if (CheckIfValidAimTarget(obj))
                                targetsWithinRange.Add(obj);
                        }
                    }
                }
            }

            return targetsWithinRange;
        }

        private Boolean CheckIfValidAimTarget(GameObjectVertical obj)
        {
            // Evaluates if input GameObjectVertical is valid aim-target for aiming weapons

            return !(obj is Meteorite || obj is MineEnemy);
        }

        public abstract void OnKilled();

        public virtual void OnDamage()
        { }

        public void SetLevelWidth(int levelWidth)
        {
            this.relativeOrigin = windowWidth / 2 - levelWidth / 2;
            this.LevelWidth = levelWidth;
        }

        public void SetStartPos(Vector2 pos)
        {
            _position = new Vector2(relativeOrigin + pos.X, pos.Y); 
        }
    }
}
