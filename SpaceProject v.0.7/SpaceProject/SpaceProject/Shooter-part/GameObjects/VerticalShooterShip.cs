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
        low         = 150,
        medium      = 300,
        high        = 800,
        extreme     = 1800
    }

    public enum CreatureShieldRegeneration
    {
        low         = 1,
        medium      = 5,
        high        = 10,
        extreme     = 100
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
        veryHigh    = 7,
        hangar      = 50,
        none        = 0
    }

    public abstract class VerticalShooterShip : CombatGameObject
    {
        #region declaration
        protected PlayerVerticalShooter player;
        
        protected Movement movement;

        private Boolean passedScreen;
        public Boolean PassedScreen { get { return passedScreen; } private set { } }

        protected Random random = new Random();

        protected LootValue lootValue = LootValue.low;

        protected int disruptionMilliseconds = 0;
        protected Boolean IsDisrupted { get { return disruptionMilliseconds > 0; } }

        private Boolean hasShield;
        public Boolean HasShield { get { return hasShield; } }

        private float shieldCapacity;

        private float currentShield;
        public Boolean ShieldCanTakeHit(double hitDamage) { return hitDamage <= currentShield; }

        private float shieldRegeneration;

        private MovementModule movementModule;

        protected Boolean stealthOn;
        protected Boolean stealthSwitching;
        protected readonly float stealthLevel = 0.035f;
        protected readonly float stealthSwitchSpeed = 0.015f;

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
            IsKilled = false;
            DrawLayer = 0.42f;
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
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
            return Game.random.Next((int)lootValue, 2*(int)lootValue);
        }

        public void SetBossMovement(float yPos)
        {
            movementModule = new StoppingModule(Game, true, yPos, 0);
            movementModule.Setup(this);
        }
        
        public void SetMovement(Movement movement)
        {
            this.movement = movement;
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
                    ExplosionGenerator.GenerateShipExplosion(Game, spriteSheet, this));
            }

            if (movement != Movement.None)
                movementModule.Update(gameTime, this);

            CheckWallCollision();

            if (hasShield)
            {
                currentShield += shieldRegeneration * MathFunctions.FPSSyncFactor(gameTime);
                if (currentShield > shieldCapacity)
                    currentShield = shieldCapacity;
            }

            DisruptionLogic(gameTime);

            if (!IsDisrupted)
            {
                ObjectColor = GetDamageTintColor();
            }
        }

        private void DisruptionLogic(GameTime gameTime)
        {
            if (IsDisrupted)
            {
                currentShield = 0;
                stealthOn = false;
                ObjectColor = Color.Green;

                disruptionMilliseconds -= gameTime.ElapsedGameTime.Milliseconds;
                if (disruptionMilliseconds < 0)
                {
                    disruptionMilliseconds = 0;
                }
            }
            else
            {
                ObjectColor = Color.White;
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
        
        public override void InflictDamage(GameObjectVertical obj)
        {
            if (obj.DisruptionTime > 0)
            {
                disruptionMilliseconds = obj.DisruptionTime;
            }

            float enemyDamage = obj.Damage;
            if (hasShield && currentShield > 0)
            {
                if (currentShield > enemyDamage)
                {
                    currentShield -= enemyDamage;
                    Game.AddGameObjToShooter(ShieldEffectGenerator.GenerateStandardShieldEffect(Game, spriteSheet, this));

                    if (!(obj is BeamBullet))
                    {
                        Game.soundEffectsManager.PlaySoundEffect(SoundEffects.ShieldHit);
                    }
                }
                else
                {
                    HP -= (obj.Damage - Shield);
                    Shield = 0;

                    if (!(obj is BeamBullet))
                    {
                        Game.soundEffectsManager.PlaySoundEffect(SoundEffects.MuffledExplosion);
                    }
                }
            }
            else
            {
                base.OnDamage();
                HP -= obj.Damage;
            }
        }

        protected float ShieldTransparency()
        {
            float fullChargeTransparency = 0.3f;
            float shieldChargeFraction = currentShield / shieldCapacity;
            return fullChargeTransparency * shieldChargeFraction;
        }

        public override void OnKilled()
        {
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.SmallExplosion);
        }
    }
}