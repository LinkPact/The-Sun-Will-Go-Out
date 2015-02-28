using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class MineLayerWeapon : PlayerWeapon
    {
        public MineLayerWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Drops a mine which explodes over a middle size area upon collision";
        }

        private void Setup()
        {
            Name = "MineLayer";
            Kind = "Secondary";
            energyCostPerSecond = 1f;
            delay = 800;
            Weight = 400;
            ActivatedSoundID = SoundEffects.ClickLaser;

            bullet = new Mine(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 500;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            Vector2 centerDir = new Vector2(0, -1.0f);
            double dirRadians = MathFunctions.RadiansFromDir(centerDir);
            dirRadians += random.NextDouble() * Math.PI / 8 - Math.PI / 16;

            Mine mine = new Mine(Game, spriteSheet);
            mine.Position = player.Position;
            mine.Direction = MathFunctions.DirFromRadians(dirRadians);
            mine.Initialize();
            mine.Speed = 0.03f;
            mine.Duration = 20000;

            Game.stateManager.shooterState.gameObjects.Add(mine);
            return true;
        }
    }
}
