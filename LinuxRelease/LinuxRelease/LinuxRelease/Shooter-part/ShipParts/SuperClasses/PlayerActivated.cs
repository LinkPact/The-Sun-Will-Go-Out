﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public abstract class PlayerActivated : ShipPart
    {
        protected Sprite spriteSheet;

        private Boolean isSoundAssigned = false;
        private SoundEffects activatedSoundID;
        protected SoundEffects ActivatedSoundID { 
            set 
            { 
                isSoundAssigned = true;
                activatedSoundID = value;
            } 
        }

        protected Boolean isBeam = false;

        protected float delay;
        public float Delay { 
            get 
            {
                if (!shootsInBatches)
                {
                    return delay;
                }
                else 
                {
                    return ShootCycleTime / batchSize;
                }
            } 
        }
        
        public float EnergyCost { 
            get 
            {
                if (!shootsInBatches)
                    return energyCostPerSecond * delay / 1000;
                else
                    return (energyCostPerSecond * ShootCycleTime) / (1000 * batchSize);
            } 
        }

        protected float ShootCycleTime
        {
            get
            {
                return delay * batchSize + interBatchDelay;
            }
        }

        // Batch-variables
        private Boolean shootsInBatches = false;
        protected Boolean ShootsInBatches { get { return shootsInBatches; } }

        private int shotsLeftInBatch;
        
        private int batchSize;
        protected int BatchSize { get { return batchSize; } }

        private int interBatchDelay;
        private int currentInterBatchDelay;

        protected float energyCostPerSecond;
        public float EnergyCostPerSecond { get { return energyCostPerSecond; } }

        private Boolean isReadyToUse;
        public Boolean IsReadyToUse { get { return isReadyToUse; } private set { } }

        protected Boolean isDisabled;

        private float elapsedTime;

        protected Boolean isActivatedThisTurn;

        protected PlayerActivated(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            this.spriteSheet = Game.spriteSheetVerticalShooter;
            random = new Random();
        }

        public virtual void Initialize()
        {
            elapsedTime = 0;
            isReadyToUse = false;
            isDisabled = false;
        }

        protected void ShootsInBatchesSetup(int batchSize, int interBatchDelay)
        {
            shootsInBatches = true;
            shotsLeftInBatch = 0;
            this.batchSize = batchSize;
            this.interBatchDelay = interBatchDelay;
        }

        public virtual void Update(PlayerVerticalShooter player, GameTime gameTime)
        {
            if (!isDisabled)
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > delay)
            {
                isReadyToUse = true;
            }

            if (shootsInBatches)
            {
                if (shotsLeftInBatch > 0)
                {
                    isDisabled = false;
                }
                else
                {
                    isDisabled = true;
                    currentInterBatchDelay -= gameTime.ElapsedGameTime.Milliseconds;
                }

                if (currentInterBatchDelay <= 0 && shotsLeftInBatch <= 0)
                {
                    shotsLeftInBatch = batchSize;
                    currentInterBatchDelay = interBatchDelay;
                }
            }
        }

        public virtual void Use(PlayerVerticalShooter player, GameTime gameTime)
        {
            isActivatedThisTurn = false;

            isReadyToUse = false;
            elapsedTime = 0;

            isActivatedThisTurn = Activate(player, gameTime);

            if (isActivatedThisTurn)
            {
                float activationCost = GetActivationCost(gameTime);
                player.MP -= activationCost;

                if (Level.IsLogging)
                {
                    Level.AddConsumedEnergy(activationCost);
                }

                ActivationSound();
            }

            if (isActivatedThisTurn && shootsInBatches)
            {
                shotsLeftInBatch--;
            }
        }

        private float GetActivationCost(GameTime gameTime)
        {
            float activationCost;
            if (!isBeam)
                activationCost = EnergyCost;
            else
                activationCost = energyCostPerSecond * gameTime.ElapsedGameTime.Milliseconds / 1000;
            return activationCost;
        }

        private void ActivationSound()
        {
            if (isSoundAssigned)
            {
                Game.soundEffectsManager.PlaySoundEffect(activatedSoundID);
            }
        }

        public abstract Boolean Activate(PlayerVerticalShooter player, GameTime gameTime);

        public override String RetrieveSaveData()
        {
            return "emptyitem";
        }
    }
}
