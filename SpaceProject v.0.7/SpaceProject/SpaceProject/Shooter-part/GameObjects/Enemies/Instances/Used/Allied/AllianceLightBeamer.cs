using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class AllianceLightBeamer : ShootingEnemyShip
    {
        private BeamModule beamModule;

        public AllianceLightBeamer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceLightBeamer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.alliance;

            beamModule = new BeamModule(Game, spriteSheet, false, false);
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.medium;

            //Egenskaper
            SightRange = 400;
            HP = 200f;
            Damage = 2.0f;
            Speed = 0.05f;

            movement = Movement.Following;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(380, 340, 38, 58)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            beamModule.Activate(this, gameTime);

            if (beamModule.HasTarget())
            {
                GameObjectVertical target = beamModule.GetTarget();

                // CHANGE THIS BIT!!!
                target.HP -= Damage;
            }

            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
        }
    }
}
