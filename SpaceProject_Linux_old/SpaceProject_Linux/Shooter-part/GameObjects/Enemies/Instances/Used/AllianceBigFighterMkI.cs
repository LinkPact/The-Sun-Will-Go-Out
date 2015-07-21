using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class AllianceBigFighterMkI : EnemyShip
    {
        private double lastTimeShot;
        private double shootingDelay;

        public AllianceBigFighterMkI(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            this.player = player;
            fraction = Fraction.alliance;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.high;

            //Shooting
            shootingDelay = 1000;
            lastTimeShot = shootingDelay * random.NextDouble();

            //Egenskaper
            SightRange = 500;
            HP = 900;
            Damage = 120;
            Speed = 0.07f;
            TurningSpeed = 10;

            ObjectClass = "enemy";
            ObjectName = "BigEnemy";

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(238, 0, 77, 96)));

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
            //if (ShootObject != null)
            HandleShooting(gameTime);

        }

        //Hanterar "shooting"
        int cannons;
        private void HandleShooting(GameTime gameTime)
        {
            lastTimeShot += gameTime.ElapsedGameTime.Milliseconds;

            if (lastTimeShot >= shootingDelay)
            {
                EnemyWeakRedLaser laser1 = new EnemyWeakRedLaser(Game, spriteSheet);
                EnemyWeakRedLaser laser2 = new EnemyWeakRedLaser(Game, spriteSheet);

                if (cannons == 0)
                {
                    laser1.PositionX = PositionX - 32;
                    laser1.PositionY = PositionY + 1;
                    laser1.Direction = new Vector2(0, 1.0f);
                    laser1.Initialize();
                    laser1.Duration = 500;

                    laser2.PositionX = PositionX + 32;
                    laser2.PositionY = PositionY + 1;
                    laser2.Direction = new Vector2(0, 1.0f);
                    laser2.Initialize();
                    laser2.Duration = 500;

                    cannons = 1;
                }

                else if (cannons == 1)
                {
                    laser1.PositionX = PositionX - 13;
                    laser1.PositionY = PositionY + 29;
                    laser1.Direction = new Vector2(0, 1.0f);
                    laser1.Initialize();
                    laser1.Duration = 500;

                    laser2.PositionX = PositionX + 13;
                    laser2.PositionY = PositionY + 29;
                    laser2.Direction = new Vector2(0, 1.0f);
                    laser2.Initialize();
                    laser2.Duration = 500;

                    cannons = 2;
                }

                else
                {
                    laser1.PositionX = PositionX - 2;
                    laser1.PositionY = PositionY + 1;
                    laser1.Direction = new Vector2(0, 1.0f);
                    laser1.Initialize();
                    laser1.Duration = 500;

                    laser2.PositionX = PositionX + 2;
                    laser2.PositionY = PositionY + 1;
                    laser2.Direction = new Vector2(0, 1.0f);
                    laser2.Initialize();
                    laser2.Duration = 500;

                    cannons = 0;
                }

                Game.stateManager.shooterState.gameObjects.Add(laser1);
                Game.stateManager.shooterState.gameObjects.Add(laser2);

                lastTimeShot -= shootingDelay;

                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.SmallLaser, soundPan);
            }
        }

    }
}
