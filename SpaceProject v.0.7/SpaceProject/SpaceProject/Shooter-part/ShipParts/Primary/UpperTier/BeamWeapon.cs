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

        public BeamWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoots a single solid beam at great range";
        }

        private void Setup()
        {
            Name = "Beam";
            Kind = "Primary";
            energyCostPerSecond = 13f;
            delay = 10;
            Weight = 200;
            damage = 8.0f;
            Value = 1100;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(400, 0, 100, 100));

            Color color = new Color(0, 0, 128);
            beamModule = new FriendlyBeamModule(Game, spriteSheet, damage, color);
            isBeam = true;
            numberOfShots = 1;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            beamModule.Activate(player.Position, gameTime);
            return true;
        }
    }
}
