using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class AllianceHomingBullets : ShootingEnemyShip
    {
        public AllianceHomingBullets(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceHomingBullets(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.alliance;
            ShieldSetup(CreatureShieldCapacity.medium, CreatureShieldRegeneration.medium);
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.medium;

            SetShootingDelay(2000);
            ChargeWeapon(ChargeMode.fullCharge);

            //Egenskaper
            SightRange = 400;
            HP = 400.0f;
            Damage = 90;
            Speed = 0.04f;

            movement = Movement.Following;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(380, 340, 38, 58)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            double width = 2 * Math.PI;
            int numberOfShots = 6;

            List<double> spreadDirections = GlobalMathFunctions.GetSpreadDirList(width, numberOfShots);

            foreach (double dir in spreadDirections)
            {
                EnemyHomingBullet bullet = new EnemyHomingBullet(Game, spriteSheet, player);
                bullet.Position = Position;
                bullet.Direction = GlobalMathFunctions.DirFromRadians(dir);
                bullet.Initialize();
                bullet.Speed *= 0.8f;

                Game.stateManager.shooterState.gameObjects.Add(bullet);
                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
            }
        }
    }
}
