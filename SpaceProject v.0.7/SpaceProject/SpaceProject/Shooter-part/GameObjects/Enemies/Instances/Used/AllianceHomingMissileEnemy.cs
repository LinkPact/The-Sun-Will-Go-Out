using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class AllianceHomingMissileEnemy : ShootingEnemyShip
    {
        public AllianceHomingMissileEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceHomingMissileEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.alliance;            
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.medium;

            //Shooting
            SetShootingDelay(1000);
            ChargeWeapon(ChargeMode.randomCharge);
            
            //Egenskaper
            SightRange = 400;
            HP = 200.0f;
            Damage = 90;
            Speed = 0.15f;

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(410, 0, 28, 35)));
            
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyHomingMissileBullet bullet = new EnemyHomingMissileBullet(Game, spriteSheet, player);
            bullet.Position = Position;
            bullet.Direction = new Vector2(0, 1);
            bullet.Initialize();

            Game.stateManager.shooterState.gameObjects.Add(bullet);
        }
    }
}
