using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    //TODO: Inherit from ShootingCreature?
    class AllianceHomingShotEnemy : EnemyShip
    {
        private double lastTimeShot;
        private double shootingDelay;

        public AllianceHomingShotEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceHomingShotEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
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

            lootValue = LootValue.medium;

            //Shooting
            shootingDelay = 2000;
            lastTimeShot = shootingDelay * random.NextDouble();

            //Egenskaper
            SightRange = 400;
            HP = 200.0f;
            Damage = 90;
            Speed = 0.15f;

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(410, 0, 28, 35)));
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            HandleShooting(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
            {
                base.Draw(spriteBatch);
            }
        }

        private void HandleShooting(GameTime gameTime)
        {
            lastTimeShot += gameTime.ElapsedGameTime.Milliseconds;

            if (lastTimeShot >= shootingDelay)
            {
                lastTimeShot -= shootingDelay;

                double width = Math.PI;
                int numberOfShots = 4;

                for (double dir = -width / 2 + Math.PI / 2; dir <= width / 2 + Math.PI / 2; dir += (width / numberOfShots))
                {
                    EnemyHomingBullet bullet = new EnemyHomingBullet(Game, spriteSheet, player);
                    bullet.Position = Position;
                    bullet.Direction = MathFunctions.DirFromRadians(dir);
                    bullet.Initialize();

                    Game.stateManager.shooterState.gameObjects.Add(bullet);
                    Game.soundEffectsManager.PlaySoundEffect(SoundEffects.SmallLaser, soundPan);
                }
            }
        }        
    }
}
