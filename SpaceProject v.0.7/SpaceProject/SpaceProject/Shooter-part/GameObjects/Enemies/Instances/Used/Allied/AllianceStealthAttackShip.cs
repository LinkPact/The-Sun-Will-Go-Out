﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    class AllianceStealthAttackShip : ShootingEnemyShip
    {
        private int tempCounter;
        private int stealthDelay;

        private Boolean stealthToggled;

        public AllianceStealthAttackShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceStealthAttackShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, Movement movement) :
            base(Game, spriteSheet, player)
        {
            this.movement = movement;
            Setup();
        }

        private void Setup()
        {
            fraction = Fraction.alliance;
            stealthOn = true;
            transparency = stealthLevel;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.medium;

            //Shooting
            SetShootingDelay(1500);
            ChargeWeapon(ChargeMode.fullCharge);

            //Egenskaper
            SightRange = 300;
            HP = 200.0f;
            Damage = 50;
            Speed = 0.14f;
            TurningSpeed *= 3;
            movement = Movement.Following;

            stealthDelay = 1500;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(420, 180, 26, 33)));
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            tempCounter = 0;
            stealthToggled = false;
        }

        public override void Update(GameTime gameTime)
        {
            tempCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (FollowObject != null && !stealthToggled)
            {
                ToggleStealth();
                stealthToggled = true;
            }

            UpdateStealth();
            base.Update(gameTime);
        }

        private void ToggleStealth()
        {
            if (!stealthSwitching)
                stealthSwitching = true;
        }

        private void UpdateStealth()
        {
            if (stealthSwitching)
            {
                if (!stealthOn)
                {
                    transparency -= stealthSwitchSpeed;

                    if (transparency < stealthLevel)
                    {
                        transparency = stealthLevel;
                        stealthSwitching = false;
                        stealthOn = true;
                    }
                }
                else
                {
                    transparency += stealthSwitchSpeed;

                    if (transparency > 1)
                    {
                        transparency = 1;
                        stealthSwitching = false;
                        stealthOn = false;
                    }
                }
            }
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            int numberOfShots = 10;

            for (int n = 0; n < numberOfShots; n++)
            {
                EnemyWeakBlueLaser laser1 = new EnemyWeakBlueLaser(Game, spriteSheet);
                laser1.Position = Position;
                laser1.Direction = new Vector2(0, 1);
                laser1.Direction = GlobalMathFunctions.SpreadDir(laser1.Direction, Math.PI / 8);
                laser1.Initialize();
                laser1.DrawLayer = this.DrawLayer - 0.01f;
                laser1.SetSpreadSpeed(random);
                laser1.Duration *= 2;

                Game.AddGameObjToShooter(laser1);
            }

            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
        }
    }
}