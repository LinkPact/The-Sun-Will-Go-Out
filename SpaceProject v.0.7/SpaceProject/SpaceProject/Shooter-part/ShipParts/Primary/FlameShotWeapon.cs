using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class FlameShotWeapon : PlayerWeapon
    {
        public FlameShotWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots a concentrated line of fire straight forward and close-range at a wider angle";
        }

        private void Setup()
        {
            Name = "Flame Shot";
            Kind = "Primary";
            energyCostPerSecond = 10f;
            delay = 500;
            Weight = 100;
            ActivatedSoundID = SoundEffects.MuffledExplosion;

            bullet = new FlameShot(Game, spriteSheet);
            bullet.Initialize();

            damage = 60;
            duration = 800;
            speed = 0.4f;

            Value = 800;
        }

        public override void Initialize()
        { }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            Vector2 centerDir = new Vector2(0, -1.0f);

            int longShotCount = 7;
            double longSpread = Math.PI / 16;

            int shortShotCount = 15;
            double shortSpread = Math.PI / 3;

            for (int n = 0; n < longShotCount; n++)
            {
                FlameShot shot = new FlameShot(Game, spriteSheet);
                shot.PositionX = player.PositionX;
                shot.PositionY = player.PositionY;

                shot.Direction = MathFunctions.SpreadDir(centerDir, longSpread);
                shot.Initialize();
                shot.SetSpreadSpeed(random, 0.2f);
                Game.stateManager.shooterState.gameObjects.Add(shot);
            }

            for (int n = 0; n < shortShotCount; n++)
            {
                FlameShot shot = new FlameShot(Game, spriteSheet);
                shot.PositionX = player.PositionX;
                shot.PositionY = player.PositionY;

                shot.Direction = MathFunctions.SpreadDir(centerDir, shortSpread);
                shot.Initialize();
                shot.Speed *= 0.3f;
                shot.SetSpreadSpeed(random, 0.2f);
                Game.stateManager.shooterState.gameObjects.Add(shot);
            }

            return true;
        }
    }
}
