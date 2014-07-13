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

            ShieldSetup(CreatureShieldCapacity.medium, CreatureShieldRegeneration.medium);
        }

        public override void Initialize()
        {
            base.Initialize();

            lootRangeMin = 1;
            lootRangeMax = 3;

            shootingDelay = 2000;
            lastTimeShot = shootingDelay * random.NextDouble();

            Damage = 100;
            Speed = 0.06f;
            HP = 200;
            TurningSpeed = 2;

            movement = Movement.Following;
            SightRange = 400;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(380, 180, 26, 33)));
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
            double width = Math.PI / 12;
            double numberOfShots = 3;

            foreach (double dir in GlobalMathFunctions.GetSpreadDirList(width, numberOfShots))
            {
                EnemyWeakBlueLaser laser1 = new EnemyWeakBlueLaser(Game, spriteSheet);
                laser1.PositionX = PositionX;
                laser1.PositionY = PositionY;
                laser1.Direction = GlobalMathFunctions.DirFromRadians(dir);
                laser1.Initialize();
                laser1.Speed *= 1.5f;

                Game.stateManager.shooterState.gameObjects.Add(laser1);
            }


            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
        }
    }
}
