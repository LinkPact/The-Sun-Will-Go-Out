using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    public class AdvancedBeamWeapon : PlayerWeapon
    {
        private BeamModule beamModule1;
        private BeamModule beamModule2;

        public AdvancedBeamWeapon(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoots two solid high-damage beams at great range";
        }

        private void Setup()
        {
            Name = "Advanced Beam";
            Kind = "Primary";
            energyCostPerSecond = 14f;
            delay = 10;
            Weight = 200;
            damage = 8.0f;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(1100, 0, 100, 100));

            Value = 3200;
            Tier = TierType.Excellent;
            Color color = new Color(79, 255, 73);
            beamModule1 = new FriendlyBeamModule(Game, spriteSheet, damage, color);
            beamModule2 = new FriendlyBeamModule(Game, spriteSheet, damage, color);
            isBeam = true;
            numberOfShots = 2;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            beamModule1.Activate(new Vector2(player.PositionX - 4, player.PositionY), gameTime);
            beamModule2.Activate(new Vector2(player.PositionX + 4, player.PositionY), gameTime);
            return true;
        }
    }
}
