using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /**
     * Light, mid-range ship that toggles on a single laser firing straight over the screen
     */

    class AllianceLightBeamer : ShootingEnemyShip
    {
        private HostileBeamModule beamModule;

        private readonly float fireTime = 3000;
        private readonly float cooldownTime = 2000;
        private float currentTime;
        private Boolean IsFiring { get { return currentTime < fireTime; } }

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
        }

        public override void Initialize()
        {
            base.Initialize();

            currentTime = fireTime;
            lootValue = LootValue.medium;

            //Egenskaper
            SightRange = 600;
            HP = 500f;
            Damage = (float)CollisionDamage.medium;
            Speed = 0.05f;

            AddPrimaryModule(10, ShootingMode.Regular);
            movement = Movement.Following;
            //shootSoundID = SoundEffects.None;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(410, 0, 29, 36)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            float beamDamage = 12.0f;
            beamModule = new HostileBeamModule(Game, spriteSheet, beamDamage);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            currentTime += gameTime.ElapsedGameTime.Milliseconds;
            if (currentTime > fireTime + cooldownTime)
                currentTime -= (fireTime + cooldownTime);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            if (IsFiring)
            {
                beamModule.Activate(Position, gameTime);
            }
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
