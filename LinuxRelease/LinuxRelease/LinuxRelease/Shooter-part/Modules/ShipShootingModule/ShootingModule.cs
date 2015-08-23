using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class ShootingModule
    {
        private ShootingMode shootingMode;

        protected double lastTimeShot;

        private double shootingDelay;
        public double ShootingDelay { get { return shootingDelay; } }
        
        private int shotsInBatch;
        private int batchSize;
        private double lastBatch;
        private double batchDelay;
        
        private bool hasShot;

        public ShootingModule(float delay, ShootingMode shootingMode)
        {
            shootingDelay = delay;
            this.shootingMode = shootingMode;
        }

        public void ShootsInBatchesSetup(int batchSize, double batchDelay)
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

        public void SetFullCharge()
        {
            if (shootingMode == ShootingMode.Batches)
            {
                shotsInBatch = batchSize;
                lastBatch = 0;
            }
            else
            {
                lastTimeShot = shootingDelay;
            }
        }

        public void SetRandomCharge(Random random)
        {
            lastTimeShot = shootingDelay * random.NextDouble();        
        }

        private Boolean isReadyToShoot;
        public Boolean IsReady()
        {
            if (isReadyToShoot)
            {
                isReadyToShoot = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update(GameTime gameTime, GameObjectVertical shootObject)
        {
            if (shootObject == null)
                return;

            if (shootingMode == ShootingMode.Regular)
            {
                lastTimeShot += gameTime.ElapsedGameTime.Milliseconds;

                if (lastTimeShot >= shootingDelay)
                {
                    isReadyToShoot = true;
                    lastTimeShot -= shootingDelay;
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

                    //ShootingPattern(gameTime);
                    isReadyToShoot = true;
                    lastTimeShot -= shootingDelay;
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
                        //ShootingPattern(gameTime);
                        isReadyToShoot = true;
                        lastTimeShot -= shootingDelay;
                    }
                }
            }
        }

    }
}
