using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class FragmentMissileWeapon : PlayerWeapon
    {
        public FragmentMissileWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots a single missile which shatters into high-damaging fragments";
        }

        private void Setup()
        {
            Name = "Fragment Missile";
            Kind = "Primary";
            energyCostPerSecond = 9f;
            delay = 450;
            Weight = 500;
            ActivatedSoundID = SoundEffects.ClickLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(800, 0, 100, 100));

            bullet = new RegularMissile(Game, spriteSheet);
            bullet.Initialize();

            MissileFragment fragment = new MissileFragment(Game, spriteSheet);

            damage = fragment.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 1000;
            numberOfShots = 40;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            FragmentMissile missile1 = new FragmentMissile(Game, spriteSheet, numberOfShots);
            missile1.PositionX = player.PositionX;
            missile1.PositionY = player.PositionY;

            BasicBulletSetup(missile1);

            Game.stateManager.shooterState.gameObjects.Add(missile1);
            return true;
        }
    }
}
