using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
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
            energyCostPerSecond = 10f;  // This does not seem to work perfectly? Burns less than 6 energy?
            delay = 10;
            Weight = 200;
            damage = 8.0f;
            Value = 600;
            ActivatedSoundID = SoundEffects.SmallLaser;

            Color color = new Color(0, 0, 128);
            beamModule = new FriendlyBeamModule(Game, spriteSheet, damage, color);
            isBeam = true;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            beamModule.Activate(player.Position, gameTime);
            return true;
        }
    }
}
