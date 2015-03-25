using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
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
            energyCostPerSecond = 5f;
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
            FlameShot shot1_1 = new FlameShot(Game, spriteSheet);
            shot1_1.PositionX = player.PositionX;
            shot1_1.PositionY = player.PositionY;
            BasicBulletSetup(shot1_1);
            
            FlameShot shot1_2 = new FlameShot(Game, spriteSheet);
            shot1_2.PositionX = player.PositionX;
            shot1_2.PositionY = player.PositionY;
            BasicBulletSetup(shot1_2);
            shot1_2.Speed = Speed * 0.8f;

            FlameShot shot1_3 = new FlameShot(Game, spriteSheet);
            shot1_3.PositionX = player.PositionX;
            shot1_3.PositionY = player.PositionY;
            BasicBulletSetup(shot1_3);
            shot1_3.Speed = Speed * 0.6f;

            FlameShot shot2_1 = new FlameShot(Game, spriteSheet);
            shot2_1.PositionX = player.PositionX;
            shot2_1.PositionY = player.PositionY;
            BasicBulletSetup(shot2_1);
            shot2_1.Radians = MathFunctions.RadiansFromDir(shot2_1.Direction) - 3 * Math.PI / 16;
            shot2_1.Direction = MathFunctions.DirFromRadians(shot2_1.Radians);
            shot2_1.Speed = Speed * 0.3f;

            FlameShot shot2_2 = new FlameShot(Game, spriteSheet);
            shot2_2.PositionX = player.PositionX;
            shot2_2.PositionY = player.PositionY;
            BasicBulletSetup(shot2_2);
            shot2_2.Radians = MathFunctions.RadiansFromDir(shot2_2.Direction) - 2 * Math.PI / 16;
            shot2_2.Direction = MathFunctions.DirFromRadians(shot2_2.Radians);
            shot2_2.Speed = Speed * 0.3f;

            FlameShot shot2_3 = new FlameShot(Game, spriteSheet);
            shot2_3.PositionX = player.PositionX;
            shot2_3.PositionY = player.PositionY;
            BasicBulletSetup(shot2_3);
            shot2_3.Radians = MathFunctions.RadiansFromDir(shot2_3.Direction) - 1 * Math.PI / 16;
            shot2_3.Direction = MathFunctions.DirFromRadians(shot2_3.Radians);
            shot2_3.Speed = Speed * 0.3f;

            FlameShot shot2_4 = new FlameShot(Game, spriteSheet);
            shot2_4.PositionX = player.PositionX;
            shot2_4.PositionY = player.PositionY;
            BasicBulletSetup(shot2_4);
            shot2_4.Radians = MathFunctions.RadiansFromDir(shot2_4.Direction) + 1 * Math.PI / 16;
            shot2_4.Direction = MathFunctions.DirFromRadians(shot2_4.Radians);
            shot2_4.Speed = Speed * 0.3f;

            FlameShot shot2_5 = new FlameShot(Game, spriteSheet);
            shot2_5.PositionX = player.PositionX;
            shot2_5.PositionY = player.PositionY;
            BasicBulletSetup(shot2_5);
            shot2_5.Radians = MathFunctions.RadiansFromDir(shot2_5.Direction) + 2 * Math.PI / 16;
            shot2_5.Direction = MathFunctions.DirFromRadians(shot2_5.Radians);
            shot2_5.Speed = Speed * 0.3f;

            FlameShot shot2_6 = new FlameShot(Game, spriteSheet);
            shot2_6.PositionX = player.PositionX;
            shot2_6.PositionY = player.PositionY;
            BasicBulletSetup(shot2_6);
            shot2_6.Radians = MathFunctions.RadiansFromDir(shot2_6.Direction) + 3 * Math.PI / 16;
            shot2_6.Direction = MathFunctions.DirFromRadians(shot2_6.Radians);
            shot2_6.Speed = Speed * 0.3f;

            Game.stateManager.shooterState.gameObjects.Add(shot1_1);
            Game.stateManager.shooterState.gameObjects.Add(shot1_2);
            Game.stateManager.shooterState.gameObjects.Add(shot1_3);

            Game.stateManager.shooterState.gameObjects.Add(shot2_1);
            Game.stateManager.shooterState.gameObjects.Add(shot2_2);
            Game.stateManager.shooterState.gameObjects.Add(shot2_3);
            Game.stateManager.shooterState.gameObjects.Add(shot2_4);
            Game.stateManager.shooterState.gameObjects.Add(shot2_5);
            Game.stateManager.shooterState.gameObjects.Add(shot2_6);

            return true;
        }
    }
}
