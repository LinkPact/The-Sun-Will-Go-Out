using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class RebelSmallAttackShip : ShootingEnemyShip
    {
        public RebelSmallAttackShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public RebelSmallAttackShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, Movement movement) :
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

            lootValue = LootValue.low;

            //Shooting
            //ShootsOnce(1500);
            AddPrimaryModule(100, ShootingMode.Regular);
            primaryModule.ShootsInBatchesSetup(2, 1000);
            primaryModule.SetFullCharge();

            //Egenskaper
            SightRange = 600;
            HP = 100.0f;
            HPmax = HP;
            Damage = (float)CollisionDamage.low;
            Speed = 0.25f;
            TurningSpeed *= 10f;

            movement = Movement.Following;
            PrimaryShootSoundID = SoundEffects.SmallLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(384, 80, 41, 34)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
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
            EnemyGreenBullet laser1 = new EnemyGreenBullet(Game, spriteSheet);
            laser1.PositionX = PositionX;
            laser1.PositionY = PositionY;
            laser1.Direction = new Vector2(0, 1.0f);
            laser1.Initialize();
            laser1.Duration = 500;
            laser1.Speed *= 1.5f;

            Game.stateManager.shooterState.gameObjects.Add(laser1);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
