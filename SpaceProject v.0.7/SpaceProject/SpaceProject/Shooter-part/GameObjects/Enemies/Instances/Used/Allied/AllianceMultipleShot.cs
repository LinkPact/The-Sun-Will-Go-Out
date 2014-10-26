using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /*
     * Mid-tier ship that fires bullets in batches over a wide arc
     */

    class AllianceMultipleShot : ShootingEnemyShip
    {
        public AllianceMultipleShot(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceMultipleShot(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
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

            AddPrimaryModule(100, ShootingMode.Batches);
            primaryModule.SetFullCharge();
            primaryModule.ShootsInBatchesSetup(2, 2000);

            //Egenskaper
            SightRange = 400;
            HP = 400.0f;
            Damage = 90;
            Speed = 0.03f;

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(257, 379, 23, 34)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            double width = Math.PI / 4;
            int numberOfShots = 6;

            List<double> spreadDirections = MathFunctions.GetSpreadDirList(width, numberOfShots);
            foreach (double dir in spreadDirections)
            {
                EnemyWeakBlueLaser laser1 = new EnemyWeakBlueLaser(Game, spriteSheet);
                laser1.PositionX = PositionX;
                laser1.PositionY = PositionY;
                laser1.Direction = MathFunctions.DirFromRadians(dir);
                laser1.Initialize();
                laser1.Speed *= 1.0f;
                laser1.Duration *= 4.0f;

                Game.stateManager.shooterState.gameObjects.Add(laser1);
            }
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
