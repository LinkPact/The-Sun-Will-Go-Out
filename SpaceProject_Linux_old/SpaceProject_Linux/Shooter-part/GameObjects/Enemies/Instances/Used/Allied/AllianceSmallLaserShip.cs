﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    /**
     * Small, low-tier ship that shoot three lasers at different angles
     */

    class AllianceSmallLaserShip : ShootingEnemyShip
    {
        public AllianceSmallLaserShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        private void Setup()
        {
            fraction = Fraction.alliance;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.low;

            AddPrimaryModule(300, ShootingMode.Batches);
            primaryModule.SetRandomCharge(random);
            primaryModule.ShootsInBatchesSetup(2, 1000);

            Damage = (float)CollisionDamage.low;
            Speed = 0.17f;
            HP = 200;
            TurningSpeed = 2;

            movement = Movement.Following;
            PrimaryShootSoundID = SoundEffects.SmallLaser;
            SightRange = 400;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(435, 80, 25, 26)));
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            double width = Math.PI / 12;
            double numberOfShots = 3;
            
            foreach (double dir in MathFunctions.GetSpreadDirList(width, numberOfShots))
            {
                EnemyWeakBlueLaser laser1 = new EnemyWeakBlueLaser(Game, spriteSheet);
                laser1.PositionX = PositionX;
                laser1.PositionY = PositionY;
                laser1.Direction = MathFunctions.DirFromRadians(dir);
                laser1.Initialize();
                laser1.Speed *= 1.5f;
                laser1.Duration *= 2.0f;
            
                Game.stateManager.shooterState.gameObjects.Add(laser1);
            }
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
