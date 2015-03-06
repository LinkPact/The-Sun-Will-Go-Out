using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class FieldDamageWeapon : PlayerWeapon
    {
        private float blastRadius = 200;

        public FieldDamageWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
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
            Name = "Field Damage";
            Kind = "Secondary";
            energyCostPerSecond = 1f;
            delay = 800;
            Weight = 400;
            ActivatedSoundID = SoundEffects.MuffledExplosion;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(200, 100, 100, 100));

            damage = 75;

            Value = 500;
            numberOfShots = 1;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            CircularAreaDamage areaExpl = new CircularAreaDamage(Game, AreaDamageType.player, player.Position, damage, blastRadius);
            areaExpl.Initialize();

            Game.stateManager.shooterState.gameObjects.Add(areaExpl);
                        
            return true;
        }
    }
}
