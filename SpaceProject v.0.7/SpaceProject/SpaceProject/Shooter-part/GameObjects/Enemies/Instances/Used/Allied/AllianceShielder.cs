using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class AllianceShielder : ShootingEnemyShip
    {
        public AllianceShielder(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ObjectName = "AllianceShielder";
            Setup();
        }

        private void Setup()
        {
            fraction = Fraction.alliance;

            ShieldSetup(CreatureShieldCapacity.high, CreatureShieldRegeneration.high);
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.high;

            shootingDelay = 3000;
            lastTimeShot = shootingDelay * random.NextDouble();

            Damage = 100;
            Speed = 0.06f;
            HP = 400;
            TurningSpeed = 2;

            movement = Movement.Following;
            SightRange = 400;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(460, 180, 53, 37)));
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            areaCollision = new AreaShieldCollision(Game, this, 150);
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
            EnemyWeakBlueLaser laser1 = new EnemyWeakBlueLaser(Game, spriteSheet);
            laser1.PositionX = PositionX;
            laser1.PositionY = PositionY;
            laser1.Direction = GlobalMathFunctions.ScaleDirection(ShootObject.Position - Position);
            laser1.Initialize();
            laser1.Duration *= 1.5f;

            Game.stateManager.shooterState.gameObjects.Add(laser1);

            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
        }
    }
}
