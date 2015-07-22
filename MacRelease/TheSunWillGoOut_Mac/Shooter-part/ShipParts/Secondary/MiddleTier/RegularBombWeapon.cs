using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class RegularBombWeapon : PlayerWeapon
    {
        public RegularBombWeapon(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoots a powerful short-range bomb which shatters and demolishes its surroundings";
        }

        private void Setup()
        {
            Name = "Bomb";
            Kind = "Secondary";
            energyCostPerSecond = 1.5f;
            delay = 1500;
            Weight = 400;
            ActivatedSoundID = SoundEffects.ClickLaser;

            bullet = new RegularBomb(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 200;
            Tier = TierType.Great;
            numberOfShots = 1;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            Vector2 centerDir = new Vector2(0, -1.0f);
            double dirRadians = MathFunctions.RadiansFromDir(centerDir);
            dirRadians += random.NextDouble() * Math.PI / 2 - Math.PI / 4;

            RegularBomb bomb = new RegularBomb(Game, spriteSheet);
            bomb.Position = player.Position;
            bomb.Direction = MathFunctions.DirFromRadians(dirRadians);
            bomb.Initialize();
            bomb.Damage = damage;

            Game.stateManager.shooterState.gameObjects.Add(bomb);
            return true;
        }
    }
}
