using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class WaveBeamWeapon : PlayerWeapon
    {
        public WaveBeamWeapon(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoots waves of energy able to pulverize enemies and bullets alike";
        }

        private void Setup()
        {
            Name = "Wave Beam";
            Kind = "Primary";
            energyCostPerSecond = 9f;
            delay = 40;
            Weight = 130;
            Value = 550;
            Tier = TierType.Good;
            ActivatedSoundID = SoundEffects.SmallLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(500, 0, 100, 100));

            bullet = new GreenBullet(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            int shotsPerBatch = 2;
            int timeBetweenBatches = 250;
            ShootsInBatchesSetup(shotsPerBatch, timeBetweenBatches);
            numberOfShots = 1;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            WaveBeam bul = new WaveBeam(Game, spriteSheet);
            bul.PositionX = player.PositionX;
            bul.PositionY = player.PositionY;
            BasicBulletSetup(bul);
            bul.Direction = new Vector2(0,-1);
            
            Game.stateManager.shooterState.gameObjects.Add(bul);
            return true;
        }
    }
}
