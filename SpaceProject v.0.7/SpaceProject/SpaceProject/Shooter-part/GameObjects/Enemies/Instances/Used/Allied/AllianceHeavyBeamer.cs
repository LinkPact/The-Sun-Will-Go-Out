using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /**
     * Heavy upper-tier ship firing three beams simultaneously
     * Also aims at the player and fires single lasers at a slow firing rate
     */

    class AllianceHeavyBeamer : ShootingEnemyShip
    {
        private HostileBeamModule beamModuleLeft;
        private HostileBeamModule beamModuleMiddle;
        private HostileBeamModule beamModuleRight;

        private readonly float fireTime = 4000;
        private readonly float cooldownTime = 3000;
        private float currentTime;
        private Boolean IsFiring { get { return currentTime < fireTime; } }

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

            currentTime = fireTime;
            lootValue = LootValue.veryHigh;

            //Egenskaper
            SightRange = 600;
            HP = 1000.0f;
            Damage = (float)CollisionDamage.high;
            Speed = 0.02f;

            AddPrimaryModule(10, ShootingMode.Regular);

            AddSecondaryModule(200, ShootingMode.Regular);
            secondaryModule.ShootsInBatchesSetup(3, 3000);

            movement = Movement.Line;
            SecondaryShootSoundID = SoundEffects.SmallLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(92, 180, 69, 83)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            float beamDamage = 12.0f;
            beamModuleLeft = new HostileBeamModule(Game, spriteSheet, beamDamage);
            beamModuleMiddle = new HostileBeamModule(Game, spriteSheet, beamDamage);
            beamModuleRight = new HostileBeamModule(Game, spriteSheet, beamDamage);
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
                float offset = 16;
                Vector2 leftBeamPos = new Vector2(PositionX - offset, PositionY);
                Vector2 middleBeamPos = new Vector2(PositionX, PositionY);
                Vector2 rightBeamPos = new Vector2(PositionX + offset, PositionY);

                beamModuleLeft.Activate(leftBeamPos, gameTime);
                beamModuleMiddle.Activate(middleBeamPos, gameTime);
                beamModuleRight.Activate(rightBeamPos, gameTime);
            }
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        {
            EnemyWeakBlueLaser laser1 = new EnemyWeakBlueLaser(Game, spriteSheet);
            laser1.PositionX = PositionX;
            laser1.PositionY = PositionY;
            laser1.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
            laser1.Initialize();
            laser1.Speed *= 1.0f;
            laser1.Duration *= 7;

            Game.stateManager.shooterState.gameObjects.Add(laser1);
        }
    }
}
