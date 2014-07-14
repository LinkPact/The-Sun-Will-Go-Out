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

            ShieldSetup(CreatureShieldCapacity.high, CreatureShieldRegeneration.high);
        }

        public override void Initialize()
        {
            base.Initialize();

            lootRangeMin = 3;
            lootRangeMax = 6;

            //Shooting
            shootingDelay = 250;

            //Egenskaper
            SightRange = 4000;
            HP = 400.0f;
            Damage = 150;
            Speed = 0.09f;
            movement = Movement.Following;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(420, 180, 26, 33)));
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            tempCounter = 0;
        }

        public override void Update(GameTime gameTime)
        {
            tempCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (tempCounter > 2000)
            {
                ToggleStealth();
                tempCounter = -100000;
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
            double width = Math.PI / 6;
            int numberOfShots = 3;

            for (double dir = Math.PI / 2 - width / 2; dir <= Math.PI / 2 + width / 2; dir += (width / (numberOfShots - 1)))
            {
                EnemyWeakBlueLaser laser1 = new EnemyWeakBlueLaser(Game, spriteSheet);
                laser1.Position = Position;
                laser1.Direction = GlobalMathFunctions.DirFromRadians(dir);
                laser1.Initialize();
                laser1.Duration *= 3;
                laser1.DrawLayer = this.DrawLayer - 0.01f;

                Game.AddGameObjToShooter(laser1);
            }

            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
        }
    }
}