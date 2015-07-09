using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpaceProject
{
    public class PlayerVerticalShooter : CombatGameObject
    {
        #region declaration
        //Playercontrol
        public int lastTimeShotPrimary;
        public int lastTimeShotSecondary;
        public int shootingDelay;

        public float MPmax;
        public float MP;
        public float MPtimer;
        public float MPgainTime;

        public float MPgainedSec;

        public float conversionFactor;
        public float shieldRegeneration;
        public float ShieldRegeneration
        {
            get
            {
                if (MP == MPmax)
                {
                    return shieldRegeneration * 1.5f;
                }
                else
                {
                    return shieldRegeneration;
                }
            }
        }

        private Sprite shieldSprite;
        
        private float redLevel;
        private readonly float redShiftTimeDamaged = 1000;
        private readonly float redShiftTimeBadlyDamaged = 300;
        private Boolean redToningIn;

        private float acceleration;
        private float deAcceleration;
        private float maxSpeed;

        public float AmassedCopper;
        public float AmassedGold;
        public float AmassedTitanium;

        //Weapon-handing class
        public PlayerShotHandler playerShotHandler;
        #endregion

        public PlayerVerticalShooter(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            ShootObjectTypes.Add("enemy");
        }
        
        public override void Initialize()
        {
            base.Initialize();

            acceleration = StatsManager.Acceleration();
            deAcceleration = StatsManager.Acceleration();

            maxSpeed = ShipInventoryManager.equippedPlating.Speed;

            playerShotHandler = new PlayerShotHandler(this, Game, spriteSheet);
            playerShotHandler.Initialize();

            Speed = 0.0f;
            Position = new Vector2(Game.Window.ClientBounds.Width / 2,
                Game.Window.ClientBounds.Height - Game.stateManager.shooterState.WindowHeightOffset - 75);
            IsKilled = false;
            Direction = Vector2.Zero;

            HPmax = StatsManager.Armor();

            // If hardcore the life is fixed. Else, life is set to the ships armor.
            if (StatsManager.gameMode != GameMode.Hardcore)
                HP = StatsManager.Armor();
            else
                HP = StatsManager.GetShipLife();

            ShieldMax = ShipInventoryManager.equippedShield.Capacity;
            Shield = ShieldMax;
            conversionFactor = ShipInventoryManager.equippedShield.ConversionFactor;
            shieldRegeneration = StatsManager.GetShieldRegeneration();

            MPmax = ShipInventoryManager.equippedEnergyCell.Capacity;
            MP = MPmax;
            MPtimer = 0;
            MPgainedSec = StatsManager.GetEnergyRegeneration();

            ObjectClass = "player";
            ObjectName = "Player";
            TempInvincibility = 0;
            Damage = 2000;
            DrawLayer = 0.6f;

            lastTimeShotPrimary = 10000;
            lastTimeShotSecondary = 10000;
            shootingDelay = 300;

            //Animation
            anim.LoopTime = 1000;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(216, 1, 23, 27)));

            shieldSprite = spriteSheet.GetSubSprite(new Rectangle(260, 100, 37, 37));
            
            BoundingSpace = 10;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
            angle = (float)(Math.PI / 180) * 180;

            redLevel = 0;
            redToningIn = true;
        }

        public override void Update(GameTime gameTime)
        {
            anim.Update(gameTime);
            base.Update(gameTime);

            //Skotthantering
            lastTimeShotPrimary += gameTime.ElapsedGameTime.Milliseconds;
            lastTimeShotSecondary += gameTime.ElapsedGameTime.Milliseconds;
            playerShotHandler.Update(gameTime);

            PlayerMovementControl();
            CheckCollisionsEdges();
            UpdateRuntimeStats(gameTime);

            if (HP < HPmax / 5)
            {
                UpdateRedTint(gameTime, redShiftTimeBadlyDamaged);
            }
            if (HP < HPmax / 3)
            {
                UpdateRedTint(gameTime, redShiftTimeDamaged);
            }
        }

        private void UpdateRedTint(GameTime gameTime, float redShiftTime)
        {
            float redShiftAmount = gameTime.ElapsedGameTime.Milliseconds / redShiftTime * 255;

            if (redToningIn)
            {
                redLevel += redShiftAmount;
                if (redLevel >= 255)
                {
                    redLevel = 255;
                    redToningIn = false;
                }
            }
            else
            {
                redLevel -= redShiftAmount;
                if (redLevel <= 0)
                {
                    redLevel = 0;
                    redToningIn = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsKilled == false)
            {
                spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, GetTintColor(), angle, CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
                spriteBatch.Draw(shieldSprite.Texture, Position, shieldSprite.SourceRectangle, Color.White * ShieldTransparency(), 0.0f, new Vector2(shieldSprite.CenterPoint.X+1, shieldSprite.CenterPoint.Y), 1.0f, SpriteEffects.None, DrawLayer);
            }
        }

        private float ShieldTransparency()
        {
            float fullChargeTransparency = 0.3f;
            float shieldChargeFraction = Shield / ShieldMax;
            return fullChargeTransparency * shieldChargeFraction;
        }

        private Color GetTintColor()
        {
            int shiftLevel = (int)(255 - redLevel);
            return new Color(255, shiftLevel, shiftLevel);
        }
        
        private void UpdateRuntimeStats(GameTime gameTime)
        {
            float MPgain = MPgainedSec * gameTime.ElapsedGameTime.Milliseconds / 1000;
            
            if (MP < MPmax - MPgain)
                MP += MPgain;
            else
            {
                MP = MPmax;
            }

            float shieldGain = ShieldRegeneration * MathFunctions.FPSSyncFactor(gameTime);

            if (Shield < ShieldMax - shieldGain)
            {
                Shield += shieldGain;
            }
            else
            {
                Shield = ShieldMax;
            }
        }
        
        private void PlayerMovementControl()
        {
            if (Speed > maxSpeed)
                Speed = maxSpeed;

            bool moveKeyPressed = false;

            if (ControlManager.CheckHold(RebindableKeys.Left) || ControlManager.CheckHold(RebindableKeys.Up) || 
                ControlManager.CheckHold(RebindableKeys.Right) || ControlManager.CheckHold(RebindableKeys.Down))
                moveKeyPressed = true;

            if (!moveKeyPressed)
                Speed -= 0.02f;

            if (Speed < deAcceleration)
                Speed = 0;

            if (moveKeyPressed)
                PlayerMovement();
        }
        
        private void PlayerMovement()
        {
            if (ControlManager.CheckHold(RebindableKeys.Left) && !ControlManager.CheckHold(RebindableKeys.Up) &&
                !ControlManager.CheckHold(RebindableKeys.Right) && !ControlManager.CheckHold(RebindableKeys.Down))
            {
                DirectionX = -1.0f;
                DirectionY = 0;
                Speed += acceleration;
            }
            
            if (!ControlManager.CheckHold(RebindableKeys.Left) && ControlManager.CheckHold(RebindableKeys.Up) &&
                !ControlManager.CheckHold(RebindableKeys.Right) && !ControlManager.CheckHold(RebindableKeys.Down))
            {
                DirectionX = 0;
                DirectionY = -1.0f;
                Speed += acceleration;
            }
            
            if (!ControlManager.CheckHold(RebindableKeys.Left) && !ControlManager.CheckHold(RebindableKeys.Up) &&
                ControlManager.CheckHold(RebindableKeys.Right) && !ControlManager.CheckHold(RebindableKeys.Down))
            {
                DirectionX = 1.0f;
                DirectionY = 0;
                Speed += acceleration;
            }
            
            if (!ControlManager.CheckHold(RebindableKeys.Left) && !ControlManager.CheckHold(RebindableKeys.Up) &&
                !ControlManager.CheckHold(RebindableKeys.Right) && ControlManager.CheckHold(RebindableKeys.Down))
            {
                DirectionX = 0;
                DirectionY = 1.0f;
                Speed += acceleration;
            }
            
            if (ControlManager.CheckHold(RebindableKeys.Left) && ControlManager.CheckHold(RebindableKeys.Up) &&
                !ControlManager.CheckHold(RebindableKeys.Right) && !ControlManager.CheckHold(RebindableKeys.Down))
            {
                DirectionX = -1.0f;
                DirectionY = -1.0f;
                Speed += acceleration;
            }
            
            if (ControlManager.CheckHold(RebindableKeys.Left) && !ControlManager.CheckHold(RebindableKeys.Up) &&
                !ControlManager.CheckHold(RebindableKeys.Right) && ControlManager.CheckHold(RebindableKeys.Down))
            {
                DirectionX = -1.0f;
                DirectionY = 1.0f;
                Speed += acceleration;
            }
            
            if (!ControlManager.CheckHold(RebindableKeys.Left) && ControlManager.CheckHold(RebindableKeys.Up) &&
                ControlManager.CheckHold(RebindableKeys.Right) && !ControlManager.CheckHold(RebindableKeys.Down))
            {
                DirectionX = 1.0f;
                DirectionY = -1.0f;
                Speed += acceleration;
            }
            
            if (!ControlManager.CheckHold(RebindableKeys.Left) && !ControlManager.CheckHold(RebindableKeys.Up) &&
                ControlManager.CheckHold(RebindableKeys.Right) && ControlManager.CheckHold(RebindableKeys.Down))
            {
                DirectionX = 1.0f;
                DirectionY = 1.0f;
                Speed += acceleration;
            }
        }

        private void CheckCollisionsEdges()
        {
            //Kollision mot kanterna
            if (PositionX - anim.Width/2 <= relativeOrigin)
                PositionX = relativeOrigin + anim.Width / 2;

            if (PositionX + anim.Width/2 > relativeOrigin + LevelWidth)
                PositionX = relativeOrigin + LevelWidth - anim.Width / 2;

            if (PositionY - anim.Height/2 <= 0 + Game.stateManager.shooterState.WindowHeightOffset)
                PositionY = (anim.Height / 2) + Game.stateManager.shooterState.WindowHeightOffset;

            if (PositionY + anim.Height / 2 > windowHeight - Game.stateManager.shooterState.WindowHeightOffset)
                PositionY = windowHeight - Game.stateManager.shooterState.WindowHeightOffset - anim.Height / 2;
        }

        public override bool CheckOutside()
        {
            if (PositionX + anim.Width < 0 || PositionX - anim.Width > windowWidth
                || PositionY + anim.Width < 0 || PositionY - anim.Height > windowHeight)
            {
                return true;
            }
            else
                return false;
        }

        public override void InflictDamage(GameObjectVertical obj)
        {
            if (TempInvincibility > 0)
                return;

            base.OnDamage();
            ApplyDamage(obj);
            TempInvincibility = CollisionHandlingVerticalShooter.TEMP_INVINCIBILITY;
        }

        private void ApplyDamage(GameObjectVertical obj)
        {
            float damage = ShipInventoryManager.equippedShield.GetShieldDamage(obj) * StatsManager.damageFactor;
            float HPdamage = 0;
            float shieldDamage = 0;

            if (Shield > damage)
            {
                shieldDamage = damage;
                Shield -= shieldDamage;
                Game.AddGameObjToShooter(ShieldEffectGenerator.GenerateStandardShieldEffect(Game, spriteSheet, this));

                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.ShieldHit);
            }
            else
            {
                HPdamage = damage - Shield;
                shieldDamage = Shield;
                HP -= HPdamage;
                Shield = 0;

                Game.soundEffectsManager.PlaySoundEffect(obj.getDeathSoundID());
            }

            if (Level.IsLogging)
            {
                Level.AddShipDamage(HPdamage);
                Level.AddShieldDamage(shieldDamage);
            }
        }

        public override void OnKilled()
        { }
    }
}
