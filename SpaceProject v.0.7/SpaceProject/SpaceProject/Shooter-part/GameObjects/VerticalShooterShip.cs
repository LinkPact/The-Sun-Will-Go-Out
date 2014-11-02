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

    public enum SpeedEnum
    { 
        low         = 1,
        low_med     = 2,
        med         = 3,
        med_high    = 4,
        high        = 5
    }

    public enum LootValue
    { 
        veryLow     = 1,
        low         = 2,
        medium      = 3,
        high        = 5,
        veryHigh    = 7
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

        protected LootValue lootValue = LootValue.low;

        #region shield
        private Boolean hasShield;
        public Boolean HasShield { get { return hasShield; } }

        private float shieldCapacity;

        private float currentShield;
        public Boolean ShieldCanTakeHit(double hitDamage) { return hitDamage <= currentShield; }

        private float shieldRegeneration;
        #endregion

        private MovementModule movementModule;

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
            Direction = MathFunctions.ScaleDirection(Direction);

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
            return r.Next((int)lootValue, 2*(int)lootValue);
        }

        public void SetBossMovement(float yPos)
        {
            movementModule = new StoppingModule(Game, true, yPos, 0);
            movementModule.Setup(this);
        }
        
        public void SetMovement(Movement movement)
        {
            this.movement = movement;
            //MovementSetup();
        }
        
        public void MovementSetup()
        {
            switch (movement)
            {
                case Movement.Line:
                        movementModule = new LinearModule(Game);
                        break;
                case Movement.SlantingLine:
                        movementModule = new SlantingLineModule(Game);
                        break;
                case Movement.SmallZigzag:
                        movementModule = new SmallZigZagModule(Game);
                        break;
                case Movement.MediumZigzag:
                        movementModule = new MediumZigZagModule(Game);
                        break;
                case Movement.BigZigzag:
                        movementModule = new BigZigZagModule(Game);
                        break;
                case Movement.Following:
                        movementModule = new FollowingModule(Game);
                        break;
                case Movement.CrossOver:
                        movementModule = new CrossOverModule(Game);
                        break;
                case Movement.Stopping:
                        movementModule = new StoppingModule(Game, false);
                        break;
                case Movement.FullStop:
                        movementModule = new StoppingModule(Game, true);
                        break;
                case Movement.RightHorizontal:
                        movementModule = new HorizontalModule(Game, true);
                        break;
                case Movement.LeftHorizontal:
                        movementModule = new HorizontalModule(Game, false);
                        break;
                case Movement.SearchAndLockOn:
                        movementModule = new SearchAndLockOnModule(Game);
                        break;
                case Movement.AI:
                        break;
                default: 
                        throw new ArgumentException("Movement not found");
            }
            movementModule.Setup(this);
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
                movementModule.Update(gameTime, this);

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
