using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class AllianceHeavyBeamer : ShootingEnemyShip
    {
        private HostileBeamModule beamModuleLeft;
        private HostileBeamModule beamModuleMiddle;
        private HostileBeamModule beamModuleRight;

        public AllianceHeavyBeamer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceHeavyBeamer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.alliance;
            ShieldSetup(CreatureShieldCapacity.high, CreatureShieldRegeneration.high);
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.veryHigh;

            //Egenskaper
            SightRange = 400;
            HP = 400.0f;
            Damage = 100.0f;
            Speed = 0.02f;

            AddPrimaryModule(10, ShootingMode.Regular);

            AddSecondaryModule(100, ShootingMode.Regular);
            secondaryModule.ShootsInBatchesSetup(3, 2000);

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(420, 340, 58, 58)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            float beamDamage = 6.0f;
            beamModuleLeft = new HostileBeamModule(Game, spriteSheet, beamDamage);
            beamModuleMiddle = new HostileBeamModule(Game, spriteSheet, beamDamage);
            beamModuleRight = new HostileBeamModule(Game, spriteSheet, beamDamage);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            float offset = 15;
            Vector2 leftBeamPos     = new Vector2(PositionX - offset, PositionY);
            Vector2 middleBeamPos   = new Vector2(PositionX, PositionY);
            Vector2 rightBeamPos    = new Vector2(PositionX + offset, PositionY);

            if (beamModuleLeft.HasTargetInLineOfSight(leftBeamPos))
            {
                beamModuleLeft.Activate(leftBeamPos, gameTime);
                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
            }

            if (beamModuleMiddle.HasTargetInLineOfSight(middleBeamPos))
            {
                beamModuleLeft.Activate(middleBeamPos, gameTime);
                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
            }

            if (beamModuleRight.HasTargetInLineOfSight(rightBeamPos))
            {
                beamModuleLeft.Activate(rightBeamPos, gameTime);
                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
            }
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        {
            EnemyWeakBlueLaser laser1 = new EnemyWeakBlueLaser(Game, spriteSheet);
            laser1.PositionX = PositionX;
            laser1.PositionY = PositionY;
            laser1.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
            laser1.Initialize();
            laser1.Speed *= 1.5f;
            laser1.Duration *= 7;

            Game.stateManager.shooterState.gameObjects.Add(laser1);
        }
    }
}
