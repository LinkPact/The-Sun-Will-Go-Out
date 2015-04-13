using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class AllianceHangarAttackShip : ShootingEnemyShip
    {
        public AllianceHangarAttackShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceHangarAttackShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            movement = Movement.SearchAndLockOn;
            fraction = Fraction.alliance;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.veryLow;

            //Shooting
            AddPrimaryModule(100, ShootingMode.Regular);
            primaryModule.ShootsInBatchesSetup(2, 1000);
            primaryModule.SetFullCharge();

            //Egenskaper
            SightRange = 800;
            HP = 100.0f;
            Damage = 60;
            Speed = 0.25f;
            TurningSpeed = 2f;
            PrimaryShootSoundID = SoundEffects.SmallLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(435, 80, 25, 26)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
            {
                spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians - Math.PI / 2),
                    CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
            }
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyGreenBullet laser1 = new EnemyGreenBullet(Game, spriteSheet);
            laser1.PositionX = PositionX;
            laser1.PositionY = PositionY;
            laser1.Direction = Direction;
            laser1.Initialize();
            laser1.Duration = 500;
            laser1.Speed *= 1.5f;

            Game.stateManager.shooterState.gameObjects.Add(laser1);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        {

        }
    }
}
