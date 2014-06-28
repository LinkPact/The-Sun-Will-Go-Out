using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    abstract class ShootingEnemyShip : EnemyShip
    {
        #region declaration

        public enum ShootingMode
        {
            Regular,
            Batches,
            Single
        }

        protected ShootingMode shootingMode = ShootingMode.Regular;
        
        protected double lastTimeShot;

        protected double shootingDelay;
        public double ShootingDelay { get { return shootingDelay; } set { shootingDelay = value; } }
        
        private int shotsInBatch;
        private int batchSize;
        private double lastBatch;
        private double batchDelay;

        private bool hasShot;

        private List<String> targetTypes = new List<String>();

        // Sound
        protected SoundEffects shootSoundID = SoundEffects.BasicLaser;
        public SoundEffects getShootSoundID() { return shootSoundID; }

        #endregion

        protected ShootingEnemyShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ShootObjectTypes.Add("player");
            ShootObjectTypes.Add("ally");

            ObjectSubClass = "shooting";
        }

        public void setShootingDelay(int newDelay)
        {
            shootingDelay = newDelay;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!stealthOn && !stealthSwitching)
                UpdateShooting(gameTime);
        }

        protected void ShootsInBatches(int batchSize, double batchDelay)
        {
            shootingMode = ShootingMode.Batches;
            this.batchSize = batchSize;
            this.batchDelay = batchDelay;
        }

        protected void ShootsOnce(int delay)
        {
            shootingDelay = delay;
            shootingMode = ShootingMode.Single;        
        }

        //'Shooting engine'
        private void UpdateShooting(GameTime gameTime)
        {
            FindAimObject();

            if (shootingMode == ShootingMode.Regular)
            {
                if (ShootObject != null)
                {
                    lastTimeShot += gameTime.ElapsedGameTime.Milliseconds;

                    if (lastTimeShot >= shootingDelay)
                    {
                        ShootingPattern(gameTime);
                        lastTimeShot -= shootingDelay;

                        Game.soundEffectsManager.PlaySoundEffect(shootSoundID, soundPan);
                    }
                }
            }
            else if (shootingMode == ShootingMode.Batches)
            {
                if (shotsInBatch > 0)
                    lastTimeShot += gameTime.ElapsedGameTime.Milliseconds;
                else
                    lastBatch += gameTime.ElapsedGameTime.Milliseconds;

                if (lastBatch > batchDelay)
                {
                    shotsInBatch = batchSize;
                    lastBatch = 0;
                }

                if (lastTimeShot >= shootingDelay)
                {
                    shotsInBatch--;

                    ShootingPattern(gameTime);
                    lastTimeShot -= shootingDelay;

                    Game.soundEffectsManager.PlaySoundEffect(shootSoundID, soundPan);
                }
            }
            else if (shootingMode == ShootingMode.Single)
            {
                if (!hasShot)
                {
                    lastTimeShot += gameTime.ElapsedGameTime.Milliseconds;

                    if (lastTimeShot >= shootingDelay)
                    {
                        hasShot = true;
                        ShootingPattern(gameTime);
                        lastTimeShot -= shootingDelay;

                        Game.soundEffectsManager.PlaySoundEffect(shootSoundID, soundPan);
                    }
                }
            }
        }

        protected abstract void ShootingPattern(GameTime gameTime);
        
    }
}
