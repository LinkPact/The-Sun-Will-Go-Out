using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class DisruptorWeapon : PlayerWeapon
    {
        public DisruptorWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Fires a shot disrupting shields and weapons for a short period of time";
        }

        private void Setup()
        {
            Name = "Disruptor";
            Kind = "Secondary";
            energyCostPerSecond = 0f;
            delay = 2000;
            Weight = 400;
            ActivatedSoundID = SoundEffects.MuffledExplosion;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(100, 100, 100, 100));

            damage = 0;

            Value = 600;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            player.SightRange = 300;
            GameObjectVertical target = player.FindAimObject();
            if (target == null)
            {
                return false;
            }

            Vector2 dir = new Vector2(target.PositionX - player.PositionX, target.PositionY - player.PositionY);
            Vector2 scaledDir = MathFunctions.ScaleDirection(dir);

            DisruptorBullet disruptorBullet = new DisruptorBullet(Game, spriteSheet);
            BasicBulletSetup(disruptorBullet);
            disruptorBullet.PositionX = player.PositionX;
            disruptorBullet.PositionY = player.PositionY;
            disruptorBullet.Direction = scaledDir;

            Game.stateManager.shooterState.gameObjects.Add(disruptorBullet);
            return true;
        }
    }
}
