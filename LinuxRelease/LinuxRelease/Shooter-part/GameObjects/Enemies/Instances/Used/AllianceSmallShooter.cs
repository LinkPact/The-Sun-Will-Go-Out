using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class AllianceSmallShooter : ShootingEnemyShip
    {
        public AllianceSmallShooter(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceSmallShooter(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, Movement movement) :
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

            AddPrimaryModule(1500, ShootingMode.Regular);
            primaryModule.SetRandomCharge(random);

            //Egenskaper
            SightRange = 400;
            HP = 100.0f;
            Damage = 60;
            Speed = 0.15f;

            movement = Movement.Following;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(435, 80, 25, 26)));

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
            laser1.Initialize();
            laser1.PositionX = PositionX + 4;
            laser1.PositionY = PositionY;
            laser1.Direction = new Vector2(0, 1.0f);
            laser1.Duration = 500;

            EnemyGreenBullet laser2 = new EnemyGreenBullet(Game, spriteSheet);
            laser2.Initialize();
            laser2.PositionX = PositionX - 4;
            laser2.PositionY = PositionY;
            laser2.Direction = new Vector2(0, 1.0f);
            laser2.Duration = 500;
            
            Game.stateManager.shooterState.gameObjects.Add(laser1);
            Game.stateManager.shooterState.gameObjects.Add(laser2);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
