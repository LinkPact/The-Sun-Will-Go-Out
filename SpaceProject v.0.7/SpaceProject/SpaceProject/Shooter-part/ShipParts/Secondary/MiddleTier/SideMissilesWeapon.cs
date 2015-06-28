using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class SideMissilesWeapon : PlayerWeapon
    {
        public SideMissilesWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoots four short range missiles protecting the flanks of the ship";
        }

        private void Setup()
        {
            Name = "SideMissiles";
            Kind = "Secondary";
            energyCostPerSecond = 0f;
            delay = 2000;
            Weight = 500;
            ActivatedSoundID = SoundEffects.ClickLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(400, 100, 100, 100));

            bullet = new RegularMissile(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage * 1.6f;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 600;
            numberOfShots = 4;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            Vector2 dir1 = MathFunctions.DirFromRadians(-Math.PI / 2 - (6 * Math.PI / 32));
            Vector2 dir2 = MathFunctions.DirFromRadians(-Math.PI / 2 - (4 * Math.PI / 32));
            Vector2 dir3 = MathFunctions.DirFromRadians(-Math.PI / 2 - (2 * Math.PI / 32));
            Vector2 dir4 = MathFunctions.DirFromRadians(-Math.PI / 2 + (2 * Math.PI / 32));
            Vector2 dir5 = MathFunctions.DirFromRadians(-Math.PI / 2 + (4 * Math.PI / 32));
            Vector2 dir6 = MathFunctions.DirFromRadians(-Math.PI / 2 + (6 * Math.PI / 32));

            //CreateMissile(player, -4, dir1);
            CreateMissile(player, -3, dir2, speedFactor: 0.8f);
            CreateMissile(player, -2, dir3, speedFactor: 1.5f);
            CreateMissile(player, 2, dir4, speedFactor: 1.5f);
            CreateMissile(player, 3, dir5, speedFactor: 0.8f);
            //CreateMissile(player, 4, dir6);

            return true;
        }

        // Used to create a missile according to given parameters, and add it to the game
        private void CreateMissile(PlayerVerticalShooter player, float xDiff, Vector2 direction, float speedFactor = 1)
        {
            RegularMissile missile = new RegularMissile(Game, spriteSheet);
            missile.PositionX = player.PositionX + xDiff;
            missile.PositionY = player.PositionY;
            missile.Direction = direction;
            missile.Radians = MathFunctions.RadiansFromDir(missile.Direction);
            missile.Initialize();
            missile.Speed *= speedFactor;
            missile.Damage = damage;

            Game.stateManager.shooterState.gameObjects.Add(missile);
        }
    }
}
