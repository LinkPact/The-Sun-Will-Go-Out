using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Should maybe inherit from SHootingCreature? TODO: Check this!
    class RebelMediumSizedEnemy : EnemyShip
    {
        private double lastTimeShot;
        private double shootingDelay;

        public RebelMediumSizedEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        private void Setup()
        {
            fraction = Fraction.rebel;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootRangeMin = 3;
            lootRangeMax = 6;

            //Shooting
            shootingDelay = 1000;
            lastTimeShot = shootingDelay * random.NextDouble();

            //Egenskaper
            SightRange = 500;
            HP = 400; 
            Damage = 120;
            Speed = 0.15f;

            movement = Movement.Zigzag;
            zigzagInterval = 0.4f;
            zigzagXdir = 0.0f;

            ObjectClass = "enemy";
            ObjectName = "RebelMediumEnemy";

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(317, 0, 38, 53)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

            UpdateShooting(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
                base.Draw(spriteBatch);
        }

        //Kontrollerar "shooting"
        private void UpdateShooting(GameTime gameTime)
        {
            //Slutar skjuta mot objekt utanfor SightRange
            if (ShootObject != null)
                if (!CollisionDetection.IsPointInsideCircle(ShootObject.Position, Position, SightRange))
                {
                    ShootObject = null;
                }

            //Styr via prioritetsordningen i CheckPriority vilket objekt som ska laggas in i ShootObject. 
            foreach (GameObjectVertical obj in Game.stateManager.shooterState.gameObjects)
            {
                if (!this.IsKilled && obj.ObjectName != null && obj.ObjectName.Equals("Player"))
                {
                    if (CollisionDetection.IsPointInsideCircle(obj.Position, Position, SightRange))
                    {
                        ShootObject = obj;
                    }
                }
            }

            //Skjuter om det finns att skjuta pa
            if (ShootObject != null)
                HandleShooting(gameTime);

        }

        //Hanterar "shooting"
        private void HandleShooting(GameTime gameTime)
        {
            lastTimeShot += gameTime.ElapsedGameTime.Milliseconds;

            if (lastTimeShot >= shootingDelay)
            {
                EnemyGreenBullet bullet1 = new EnemyGreenBullet(Game, spriteSheet);
                EnemyGreenBullet bullet2 = new EnemyGreenBullet(Game, spriteSheet);

                bullet1.PositionX = PositionX - 10;
                bullet1.PositionY = PositionY + 16;
                bullet1.Direction = GlobalMathFunctions.ScaleDirection(ShootObject.Position - Position);
                bullet1.Initialize();
                bullet1.Duration = 500;

                bullet2.PositionX = PositionX + 10;
                bullet2.PositionY = PositionY + 16;
                bullet2.Direction = GlobalMathFunctions.ScaleDirection(ShootObject.Position - Position);
                bullet2.Initialize();
                bullet2.Duration = 500;


                Game.stateManager.shooterState.gameObjects.Add(bullet1);
                Game.stateManager.shooterState.gameObjects.Add(bullet2);

                lastTimeShot -= shootingDelay;

                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
            }
        }
    }
}
