using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class RebelBomber : ShootingEnemyShip
    {
        public RebelBomber(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public RebelBomber(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.pirate;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.high;

            //Egenskaper
            SightRange = 400;
            HP = 500.0f;
            Damage = 50;
            Speed = 0.04f;
            PrimaryShootSoundID = SoundEffects.ClickLaser;

            AddPrimaryModule(1500, ShootingMode.Regular);
            primaryModule.SetFullCharge();

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(534, 27, 42, 43)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            int activationTimeMilliseconds = 500;

            MineEnemy mine = new MineEnemy(Game, spriteSheet, player);
            mine.Initialize();
            mine.Position = Position;
            mine.SetActivationTime(activationTimeMilliseconds);
            mine.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
            mine.Speed = 0.10f;
            mine.Duration = 1000;

            Game.stateManager.shooterState.gameObjects.Add(mine);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
