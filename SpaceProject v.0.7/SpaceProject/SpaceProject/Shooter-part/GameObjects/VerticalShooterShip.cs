using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum CreatureShieldCapacity
    { 
        low         = 200,
        medium      = 500,
        high        = 800,
        extreme     = 1500
    }

    public enum CreatureShieldRegeneration
    {
        low         = 1,
        medium      = 5,
        high        = 10,
        extreme     = 30
    }

    public abstract class VerticalShooterShip : CombatGameObject
    {
        #region declaration
        protected PlayerVerticalShooter player;
        
        protected Movement movement;

        private Boolean passedScreen;
        public Boolean PassedScreen { get { return passedScreen; } private set { } }

        protected Random random = new Random();
        protected float Rotation;

        protected int lootRangeMin = 0;
        protected int lootRangeMax = 0;

        #region shield
        private Boolean hasShield;
        public Boolean HasShield { get { return hasShield; } }

        private float shieldCapacity;

        private float currentShield;
        public Boolean ShieldCanTakeHit(double hitDamage) { return hitDamage >= currentShield; }

        private float shieldRegeneration;
        #endregion

        #region movement
        //SlantingLine
        protected float directionRadians;

        //Zigzag
        protected float zigzagInterval;
        protected float zigzagXdir;
        protected bool zigzagDirRight;

        //CrossOver
        protected float endX;
        protected float xSpeed;
        protected bool endReached;
        protected bool isLeft;
        
        protected float glidingY;
        protected bool glideReached;

        //Stopping
        protected float stopY;
        protected float stopYVariation;
        protected int stopTime;
        protected int currentStopCount;
        #endregion

        #region stealth

        protected Boolean stealthOn;
        protected Boolean stealthSwitching;
        protected const float stealthLevel = 0.035f;
        protected const float stealthSwitchSpeed = 0.015f;
        
        #endregion

        #endregion

        protected VerticalShooterShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet)
        {
            this.player = player;
            movement = Movement.Line;
        }
        
        protected VerticalShooterShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, Movement movement) :
            base(Game, spriteSheet)
        {
            this.player = player;
            this.movement = movement;
            ObjectClass = "enemy";
        }
        
        public override void Initialize()
        {
            base.Initialize();

            MovementSetup();

            //Egenskaper
            IsKilled = false;

            //Startriktning
            Direction = new Vector2((float)random.NextDouble()*2-1, (float)random.NextDouble()*2-1);
            Direction = GlobalMathFunctions.ScaleDirection(Direction);

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
            Rotation = 0;

            useDeathAnim = true;
        }

        protected void ShieldSetup(CreatureShieldCapacity capacity, CreatureShieldRegeneration regeneration)
        {
            hasShield = true;
            this.shieldCapacity = (int)capacity;
            this.shieldRegeneration = (float)regeneration / 100;
            currentShield = shieldCapacity;
        }

        public int GetLoot()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return r.Next(lootRangeMin, lootRangeMax);
        }
        
        public void SetMovement(Movement movement)
        {
            this.movement = movement;

            MovementSetup();
        }
        
        public void MovementSetup()
        {
            switch (movement)
            {
                case Movement.Line:
                    {
                        break;
                    }
                case Movement.SlantingLine:
                    {
                        Vector2 centerDir = new Vector2(0, 1.0f);

                        double dirRadians = GlobalMathFunctions.RadiansFromDir(centerDir);
                        dirRadians += random.NextDouble() * 3 * Math.PI / 24 - Math.PI / 24;

                        Direction = GlobalMathFunctions.DirFromRadians(dirRadians);

                        break;
                    }

                case Movement.Zigzag:
                    {
                        zigzagInterval = 0.4f;
                        zigzagXdir = 0.0f;

                        if (random.NextDouble() > 0.5) zigzagDirRight = true;
                        else zigzagDirRight = false;

                        break;
                    }

                case Movement.Following:
                    {
                        //Follows = true;
                        SightRange = 400;
                        FollowObjectTypes.Add("ally");
                        FollowObjectTypes.Add("player");

                        if (TurningSpeed == 0) TurningSpeed = 5;
                        
                        break;
                    }

                case Movement.CrossOver:
                    {
                        float windowMiddlePos = Game.Window.ClientBounds.Width / 2;

                        if (PositionX < windowMiddlePos)
                        {
                            endX = windowMiddlePos + (windowMiddlePos - PositionX);
                            isLeft = true;
                        }
                        else
                        {
                            endX = windowMiddlePos - (PositionX - windowMiddlePos);
                            isLeft = false;
                        }
                        endReached = false;
                        break;
                    }
                case Movement.Stopping:
                    {
                        stopY = 250;
                        stopYVariation = 200;
                        stopY += random.Next((int)stopYVariation) - stopYVariation / 2;

                        stopTime = 5000;
                        currentStopCount = 0;
                        break;
                    }
                case Movement.AI:
                    {
                        break;
                    }
                default: 
                    {
                        throw new ArgumentException("Movement not found");
                    }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsKilled && useDeathAnim)
            {
                Game.stateManager.shooterState.backgroundObjects.Add(
                    ExplosionGenerator.GenerateRandomExplosion(Game, spriteSheet, this));
            }

            if (movement != Movement.None)
                UpdateMovement(gameTime);

            CheckWallCollision();

            if (hasShield)
            {
                currentShield += shieldRegeneration;
                if (currentShield > shieldCapacity)
                    currentShield = shieldCapacity;
            }
        }

        private void CheckWallCollision()
        {
            if (PositionX <= relativeOrigin - 50)
            {
                IsOutside = true;
            }

            if (PositionX > relativeOrigin + LevelWidth + 50)
            {
                IsOutside = true;
            }

            if (PositionY - anim.Height > windowHeight - Game.stateManager.shooterState.WindowHeightOffset)
            {
                passedScreen = true;
                IsOutside = true;
            }
        }
        
        private void UpdateMovement(GameTime gameTime)
        {

            switch (movement)
            {
                case Movement.Line:
                    {
                        DirectionY = 1.0f;
                        DirectionX = 0.0f;

                        break;
                    }
                case Movement.SlantingLine:
                    {
                        break;
                    }
                case Movement.Following:
                    {   
                        FindFollowObject();

                        if (FollowObject != null && DisableFollowObject <= 0)
                        {
                            Direction = ChangeDirection(Direction, Position, FollowObject.Position, TurningSpeed);
                            DirectionY = 1.0f;
                            Direction = GlobalMathFunctions.ScaleDirection(Direction);
                        }
                        else
                        {
                            DirectionX = 0;
                            DirectionY = 1;
                        }
                        break;
                    }
                case Movement.Zigzag:
                    {
                        if (zigzagDirRight)
                            zigzagXdir += (zigzagInterval / 60);
                        else
                            zigzagXdir -= (zigzagInterval / 60);

                        if (zigzagXdir > zigzagInterval)
                            zigzagDirRight = false;

                        if (zigzagXdir < -zigzagInterval)
                            zigzagDirRight = true;

                        DirectionX = zigzagXdir;

                        break;
                    }
                case Movement.CrossOver:
                    {
                        if (!endReached)
                        {
                            if (PositionY > 50)
                            {
                                if (isLeft)
                                {
                                    DirectionX = 1.2f;
                                }
                                else
                                {
                                    DirectionX = -1.2f;
                                }
                            }
                        }

                        if (isLeft && PositionX > endX) 
                        { 
                            endReached = true;
                            DirectionX = 0;
                        }
                        else if (!isLeft && PositionX < endX) 
                        { 
                            endReached = true;
                            DirectionX = 0;
                        }
                        break;
                    }
                case Movement.Stopping:
                    {
                        if (PositionY >= stopY && stopTime > 0)
                        {
                            stopTime -= gameTime.ElapsedGameTime.Milliseconds;
                            DirectionY = 0;
                        }
                        else
                        {
                            DirectionY = 1;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
            {
                base.Draw(spriteBatch);
            }
        }

        public override void InflictDamage(GameObjectVertical obj)
        {
            float enemyDamage = obj.Damage;

            if (hasShield && currentShield > 0)
            {
                if (currentShield > enemyDamage)
                {
                    currentShield -= enemyDamage;
                    Game.AddGameObjToShooter(ShieldEffectGenerator.GenerateStandardShieldEffect(Game, spriteSheet, this));
                }
                else
                {
                    HP -= (obj.Damage - Shield);
                    Shield = 0;
                }
            }
            else
            {
                base.OnDamage();
                HP -= obj.Damage;
            }
        }
    }
}
