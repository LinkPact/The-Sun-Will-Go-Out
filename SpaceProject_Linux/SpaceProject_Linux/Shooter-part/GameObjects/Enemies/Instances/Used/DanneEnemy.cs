using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class DanneEnemy : EnemyShip
    {
        private double lastTimeShot;
        private double shootingDelay;

        public DanneEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        private void Setup()
        {
            this.player = player;
            fraction = Fraction.pirate;
        }

        public override void Initialize()
        {
            base.Initialize();

            //Shooting
            shootingDelay = 1500;
            lastTimeShot = shootingDelay * random.NextDouble();

            //Egenskaper
            SightRange = 500;
            HP = 40;
            Damage = 10;
            Speed = 0.07f;

            ObjectClass = "enemy";
            ObjectName = "DanneEnemy";

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(182, 16, 27, 31)));

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
                if (!this.IsKilled && obj.ObjectName.Equals("Player"))
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

                EnemyWeakRedLaser laser1 = new EnemyWeakRedLaser(Game, spriteSheet);
                laser1.PositionX = PositionX - 5;
                laser1.PositionY = PositionY + 8;
                laser1.Direction = new Vector2(0, 1.0f);
                laser1.Initialize();
                laser1.Duration = 500;

                EnemyWeakRedLaser laser2 = new EnemyWeakRedLaser(Game, spriteSheet);
                laser2.PositionX = PositionX + 5;
                laser2.PositionY = PositionY + 8;
                laser2.Direction = new Vector2(0, 1.0f);
                laser2.Initialize();
                laser2.Duration = 500;

                EnemyWeakRedLaser laser3 = new EnemyWeakRedLaser(Game, spriteSheet);
                laser3.PositionX = PositionX + 8;
                laser3.PositionY = PositionY + 5;
                laser3.Direction = new Vector2(0, 1.0f);
                laser3.Initialize();
                laser3.Duration = 500;

                EnemyWeakRedLaser laser4 = new EnemyWeakRedLaser(Game, spriteSheet);
                laser4.PositionX = PositionX - 8;
                laser4.PositionY = PositionY + 5;
                laser4.Direction = new Vector2(0, 1.0f);
                laser4.Initialize();
                laser4.Duration = 500;

                Game.stateManager.shooterState.gameObjects.Add(laser1);
                Game.stateManager.shooterState.gameObjects.Add(laser2);
                Game.stateManager.shooterState.gameObjects.Add(laser3);
                Game.stateManager.shooterState.gameObjects.Add(laser4);

                lastTimeShot -= shootingDelay;

                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.SmallLaser, soundPan);
            }
        }

    }
}
