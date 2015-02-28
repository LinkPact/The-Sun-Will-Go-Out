using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class DisruptorWeapon : PlayerWeapon
    {
        private float disruptionRadius;

        public DisruptorWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Emits a field disabling weapons, shields and stealth-technology";
        }

        private void Setup()
        {
            Name = "Disruptor";
            Kind = "Secondary";
            energyCostPerSecond = 2f;
            delay = 1200;
            Weight = 400;
            ActivatedSoundID = SoundEffects.MuffledExplosion;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(100, 100, 100, 100));

            bullet = new Mine(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 600;

            disruptionRadius = 80;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            AreaDisruptorCollision areaExpl = new AreaDisruptorCollision(Game, player, disruptionRadius);
            areaExpl.Initialize();

            Game.stateManager.shooterState.gameObjects.Add(areaExpl);
            
            return true;
        }
    }
}
