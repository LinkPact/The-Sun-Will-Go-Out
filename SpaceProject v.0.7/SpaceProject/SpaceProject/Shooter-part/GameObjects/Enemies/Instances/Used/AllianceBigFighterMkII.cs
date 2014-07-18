﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.Shooter_part.GameObjects.Enemies.Instances.Used
{
    class AllianceBigFighterMkII : ShootingEnemyShip
    {
        public AllianceBigFighterMkII(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceBigFighterMkII(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.rebel;        
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.high;

            //Shooting
            shootingDelay = 3000;
            lastTimeShot = shootingDelay * random.NextDouble();

            //Egenskaper
            SightRange = 400;
            HP = 400.0f;
            Damage = 150;
            Speed = 0.05f;

            movement = Movement.Zigzag;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(92, 180, 69, 83)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
            {
                base.Draw(spriteBatch);
            }
        }

        protected override void ShootingPattern(GameTime gameTime)
        {

        }
    }
}
