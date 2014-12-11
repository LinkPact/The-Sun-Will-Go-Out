using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class AdvancedBeamWeapon : PlayerWeapon
    {
        private BeamModule beamModule;

        public AdvancedBeamWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public AdvancedBeamWeapon(Game1 Game, ItemVariety variety) :
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
            Name = "Advanced Beam";
            Kind = "Primary";
            energyCostPerSecond = 11f;
            delay = 10;
            Weight = 200;
            damage = 12.0f;

            Value = 2200;
            Color color = new Color(79, 255, 73);
            beamModule = new FriendlyBeamModule(Game, spriteSheet, damage, color);
            isBeam = true;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            beamModule.Activate(player.Position, gameTime);
            return true;
        }

        public override void PlaySound()
        {
            //Game.soundEffectsManager.PlaySoundEffect(SoundEffects.Test4, 0f);
        }
    }
}
