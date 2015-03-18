using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    /**
     * A slow mid-tier ship that has an enourmous shield, able to protect nearby ships
     * Also has a weak laser with short range
     */

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

            ShieldSetup(CreatureShieldCapacity.extreme, CreatureShieldRegeneration.extreme);
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.high;

            AddPrimaryModule(3000, ShootingMode.Regular);
            primaryModule.SetRandomCharge(random);

            Damage = (float)CollisionDamage.high;
            Speed = 0.035f;
            HP = 400;
            TurningSpeed = 2;

            movement = Movement.Following;
            SightRange = 400;
            PrimaryShootSoundID = SoundEffects.SmallLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(217, 315, 38, 53)));
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            int shieldRadius = 150;
            areaCollision = new AreaShieldCollision(Game, this, shieldRadius);
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
            laser1.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
            laser1.Initialize();
            laser1.Duration *= 1.5f;

            Game.stateManager.shooterState.gameObjects.Add(laser1);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
