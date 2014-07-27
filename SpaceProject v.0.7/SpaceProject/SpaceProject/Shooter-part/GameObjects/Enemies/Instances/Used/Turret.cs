using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Turret : EnemyShip
    {
        //Shooting variables
        private double lastTimeShot;
        private double shootingDelay;
        private double shootingDelay2;

        private Sprite gunSprite;
        //private float idealGunAngle;
        private float gunAngle;
        private Vector2 gunCenter;
        private Sprite laserSprite;

        //Idle movement
        int stepsToRotate;
        int rotationDir;
        int wait = 50;

        public Turret(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        private void Setup()
        {
            fraction = Fraction.other;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.high;

            shootingDelay = 3000;
            shootingDelay2 = -1;
            lastTimeShot = shootingDelay * random.NextDouble();

            HP = 500;
            Damage = 150;
            Speed = 0.02f;

            movement = Movement.Line;
            SightRange = 300;
            DrawLayer = 0.6f;

            ObjectClass = "enemy";
            ObjectName = "Turret";

            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(360, 0, 43, 43)));
            gunSprite = spriteSheet.GetSubSprite(new Rectangle(360, 45, 39, 21));
            gunCenter = new Vector2(10, 10);
            laserSprite = spriteSheet.GetSubSprite(new Rectangle(360, 68, 40, 1));
            
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateShooting(gameTime);

            if(ShootObject != null)
                HandleShooting(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
            {
                base.Draw(spriteBatch);

                spriteBatch.Draw(gunSprite.Texture, new Vector2(this.PositionX, this.PositionY), gunSprite.SourceRectangle,
                    Color.White, gunAngle, gunCenter, 1f, SpriteEffects.None, DrawLayer + 0.01f);
            
                spriteBatch.Draw(laserSprite.Texture, new Vector2(this.PositionX, this.PositionY), laserSprite.SourceRectangle,
                    Color.White, gunAngle, Vector2.Zero, 1f, SpriteEffects.None, DrawLayer + 0.02f);
            }
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
                        gunAngle = (float)Math.Atan2((double)(ShootObject.Position.Y - PositionY),
                            (double)(ShootObject.Position.X - PositionX));
                    }
                }
            }


            //Skjuter om det finns att skjuta pa
            if (ShootObject != null)
                HandleShooting(gameTime);

            else
                IdleMovement();
        }

        private void IdleMovement()
        {
            if (stepsToRotate <= 0)
            {
                wait--;
                if (wait <= 0)
                {
                    stepsToRotate = random.Next(45, 180);
                    rotationDir = (int)Math.Round(random.NextDouble(),0);

                    wait = 50;
                }
            }

            else
            {
                if (rotationDir == 0)
                    gunAngle += (float)Math.PI / 180;
                else
                    gunAngle -= (float)Math.PI / 180;

                stepsToRotate--;
            }

            if (gunAngle > (float)(Math.PI * 359) / 180)
                gunAngle = 0;
            else if (gunAngle < 0)
                gunAngle = (float)(Math.PI * 359) / 180; 
        }

        //Kollar prioritetsordningen
        private int CheckPriority(GameObjectVertical obj)
        {
            int tempPriority = 0;

            if (obj.ObjectName == "FriendCreatureNest")
                tempPriority = 1;

            if (obj.ObjectName == "FriendCreatureRegular")
                tempPriority = 2;

            if (obj.ObjectName == "FriendCreatureMagic")
                tempPriority = 3;

            if (obj.ObjectName == "FriendCreatureKiller")
                tempPriority = 4;

            if (obj.ObjectName == "FriendCreaturePeasant")
                tempPriority = 5;

            if (obj.ObjectName == "Player")
                tempPriority = 6;

            return tempPriority;
        }

        //Hanterar "shooting"
        private void HandleShooting(GameTime gameTime)
        {
            lastTimeShot += gameTime.ElapsedGameTime.Milliseconds;

            if (lastTimeShot >= shootingDelay)
            {
                lastTimeShot -= shootingDelay;

                EnemyGreenBullet laser1 = new EnemyGreenBullet(Game, spriteSheet);
                laser1.PositionX = PositionX - 2;
                laser1.PositionY = PositionY - 2;
                laser1.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
                laser1.Initialize();
                laser1.Duration = 500;
                laser1.DrawLayer = .5f;

                EnemyGreenBullet laser2 = new EnemyGreenBullet(Game, spriteSheet);
                laser2.PositionX = PositionX + 2;
                laser2.PositionY = PositionY + 2;
                laser2.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
                laser2.Initialize();
                laser2.Duration = 500;
                laser2.DrawLayer = .5f;

                Game.stateManager.shooterState.gameObjects.Add(laser1);
                Game.stateManager.shooterState.gameObjects.Add(laser2);

                shootingDelay2 = 350;
                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
            }

            if (shootingDelay2 != -1 && lastTimeShot >= shootingDelay2)
            {
                lastTimeShot -= shootingDelay2;

                EnemyGreenBullet laser1 = new EnemyGreenBullet(Game, spriteSheet);
                laser1.PositionX = PositionX - 2;
                laser1.PositionY = PositionY - 2;
                laser1.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
                laser1.Initialize();
                laser1.Duration = 500;
                laser1.DrawLayer = .5f;

                EnemyGreenBullet laser2 = new EnemyGreenBullet(Game, spriteSheet);
                laser2.PositionX = PositionX + 2;
                laser2.PositionY = PositionY + 2;
                laser2.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
                laser2.Initialize();
                laser2.Duration = 500;
                laser2.DrawLayer = .5f;

                Game.stateManager.shooterState.gameObjects.Add(laser1);
                Game.stateManager.shooterState.gameObjects.Add(laser2);

                shootingDelay2 = -1;

                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
            }
        }
    }
}
