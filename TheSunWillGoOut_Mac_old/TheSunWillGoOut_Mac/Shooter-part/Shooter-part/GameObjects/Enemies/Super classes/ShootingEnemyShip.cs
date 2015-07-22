using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public enum ShootingMode
    {
        Regular,
        Batches,
        Single
    }

    abstract class ShootingEnemyShip : EnemyShip
    {
        #region declaration

        protected ShootingModule primaryModule;
        public void AddPrimaryModule(float delay, ShootingMode shootingMode)
        {
            primaryModule = new ShootingModule(delay, shootingMode);
            shootingModules.Add(primaryModule);
        }
        
        protected ShootingModule secondaryModule;
        public void AddSecondaryModule(float delay, ShootingMode shootingMode)
        {
            secondaryModule = new ShootingModule(delay, shootingMode);
            shootingModules.Add(secondaryModule);
        }

        private List<ShootingModule> shootingModules = new List<ShootingModule>();

        private List<String> targetTypes = new List<String>();

        // Sound
        private Boolean isPrimarySoundAssigned = false;
        private SoundEffects primaryShootSoundID;
        protected SoundEffects PrimaryShootSoundID { 
            set {
                isPrimarySoundAssigned = true;
                primaryShootSoundID = value;
            } 
        }

        private Boolean isSecondarySoundAssigned = false;
        private SoundEffects secondaryShootSoundID;
        protected SoundEffects SecondaryShootSoundID { 
            set {
                isSecondarySoundAssigned = true;
                secondaryShootSoundID = value;
            }
        }

        #endregion

        protected ShootingEnemyShip(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ShootObjectTypes.Add("player");
            ShootObjectTypes.Add("ally");

            ObjectSubClass = "shooting";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!stealthOn && !stealthSwitching && !IsDisrupted)
                UpdateShooting(gameTime);
        }

        //'Shooting engine'
        private void UpdateShooting(GameTime gameTime)
        {
            FindAimObject();

            foreach (ShootingModule module in shootingModules)
            {
                if (primaryModule != null)
                {
                    primaryModule.Update(gameTime, ShootObject);
                    if (primaryModule.IsReady())
                    {
                        ShootingPattern(gameTime);

                        if (isPrimarySoundAssigned) 
                        {
                            Game.soundEffectsManager.PlaySoundEffect(primaryShootSoundID, soundPan);
                        }
                    }
                }

                if (secondaryModule != null)
                {
                    secondaryModule.Update(gameTime, ShootObject);
                    if (secondaryModule.IsReady())
                    {
                        SecondaryShootingPattern(gameTime);

                        if (isSecondarySoundAssigned) 
                        {
                            Game.soundEffectsManager.PlaySoundEffect(secondaryShootSoundID, soundPan);
                        }
                    }
                }
            }
        }

        protected abstract void ShootingPattern(GameTime gameTime);

        protected abstract void SecondaryShootingPattern(GameTime gameTime);
    }
}
