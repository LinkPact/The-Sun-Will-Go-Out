using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    /**
     * Advanced weapon class containing all logic for beam weapons.
     * 
     * TODO: Generalize through moving out the beam logic to a separate class.
     */
    public class BeamWeapon : PlayerWeapon
    {
        private BeamModule beamModule;

        public BeamWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public BeamWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots a single solid beam at great range";
        }

        private void Setup()
        {
            Name = "Beam";
            Kind = "Primary";
            energyCostPerSecond = 0.15f;
            delay = 10;
            Weight = 200;

            damage = 6.0f;

            Value = 700;

            beamModule = new BeamModule(Game, spriteSheet, true, true);
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            beamModule.Activate(player, gameTime);

            if (beamModule.HasTarget())
            {
                EnemyShip target = (EnemyShip)beamModule.GetTarget();
                target.InflictDamage(beamModule.GetBullet());
            }

            return true;
        }

        public override void PlaySound()
        {
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.Test4, 0f);
        }
    }
}
