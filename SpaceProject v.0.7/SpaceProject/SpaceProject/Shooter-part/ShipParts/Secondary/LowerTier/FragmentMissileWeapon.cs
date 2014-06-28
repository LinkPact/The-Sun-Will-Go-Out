using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class FragmentMissileWeapon : PlayerWeapon
    {

        public FragmentMissileWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public FragmentMissileWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots a single powerful missile which shatters and impacts a small area";
        }

        private void Setup()
        {
            Name = "FragmentMissile";
            Kind = "Secondary";
            energyCostPerSecond = 2f;
            delay = 800;
            Weight = 500;

            bullet = new RegularMissile(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 300;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            FragmentMissile missile1 = new FragmentMissile(Game, spriteSheet);
            missile1.PositionX = player.PositionX;
            missile1.PositionY = player.PositionY;

            BasicBulletSetup(missile1);
            Game.stateManager.shooterState.gameObjects.Add(missile1);
            return true;
        }

    }
}
