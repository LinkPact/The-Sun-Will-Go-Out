﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class AllianceLightBeamer : ShootingEnemyShip
    {
        private HostileBeamModule beamModule;

        private float fireTime;
        private float cooldownTime;
        private float currentTime;
        private Boolean IsFiring { get { return currentTime < fireTime; } }

        public AllianceLightBeamer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceLightBeamer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.alliance;

            fireTime = 3000;
            cooldownTime = 1000;
            currentTime = 0;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.medium;

            //Egenskaper
            SightRange = 500;
            HP = 200f;
            Damage = 80.0f;
            Speed = 0.05f;

            AddPrimaryModule(10, ShootingMode.Regular);

            movement = Movement.Following;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(340, 340, 32, 38)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            float beamDamage = 6.0f;
            beamModule = new HostileBeamModule(Game, spriteSheet, beamDamage);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            currentTime += gameTime.ElapsedGameTime.Milliseconds;
            if (currentTime > fireTime + cooldownTime)
                currentTime -= (fireTime + cooldownTime);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            //if (beamModule.HasTargetInLineOfSight(Position))
            //{
            if (IsFiring)
            {
                beamModule.Activate(Position, gameTime);
                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
            }
            //}
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
