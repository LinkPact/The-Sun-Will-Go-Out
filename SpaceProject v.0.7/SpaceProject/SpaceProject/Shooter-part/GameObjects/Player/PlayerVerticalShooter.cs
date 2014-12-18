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
    //Spelarklassen, representerar det spelarstyrda objektet
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

        private float acceleration;
        private float deAcceleration;
        private float maxSpeed;

        public float AmassedCopper;
        public float AmassedGold;
        public float AmassedTitanium;

        //Weapon-handing class
        public PlayerShotHandler playerShotHandler;
        #endregion

        // Sound effects
        //SoundEffect testEffect1;

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

            //Characteristics
            Speed = 0.0f;
            Position = new Vector2(Game.Window.ClientBounds.Width / 2,
                Game.Window.ClientBounds.Height - Game.stateManager.shooterState.WindowHeightOffset - 75);
            IsKilled = false;
            Direction = Vector2.Zero;

            HPmax = StatsManager.Armor();

            // If hardcore the life is fixed. Else, life is set to the ships armor.
            if (StatsManager.gameMode != GameMode.hardcore)
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
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(213, 0, 23, 27)));

            BoundingSpace = 10;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            playerShotHandler.Draw(spriteBatch);

            if (IsKilled == false)
            {
                spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, 0.0f, CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
            }
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

            if (Shield < ShieldMax - shieldRegeneration)
            {
                Shield += shieldRegeneration;

                //Increase regeneration when energy is full
                if (MP == MPmax)
                {
                    Shield += shieldRegeneration;
                }
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

            if (Shield > damage)
            {
                Shield -= damage;
                Game.AddGameObjToShooter(ShieldEffectGenerator.GenerateStandardShieldEffect(Game, spriteSheet, this));
            }
            else if (Shield > obj.Damage)
            {
                Shield = 0;
            }
            else
            {
                HP -= (obj.Damage - Shield);
                Shield = 0;
            }
        }

        public override void OnKilled()
        { }
    }
}
